using Godot;
using Godot.FlipPinball;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class BallterationGenerator : Node
{
    protected static BallterationGenerator _instance;
    public static BallterationGenerator Instance { get { return _instance; } }

    public BallterationGenerator()
    {
        if (_instance != null)
            return;
        _instance = this;
    }

    [Export]
    Curve RarityCurve;

    public override void _Ready()
    {
        base._Ready();
        //for (int i = 0; i < 2500; i++)
        //{
        //    float source = GD.Randf();
        //    GD.Print($"{GD.RandRange(0,RarityCurve.Sample(source)):N10}");
        //    //GD.Print($"{RarityCurve.Sample(source):N10}");
        //}
    }

    static WeightedItem<Func<Ballteration>>[] WeightedGeneratorsBase = [
        new(CreateNewBall, 10),
        new(CreateScoreModifier),
        new(CreateSimpleScoreModifier),
        new(CreateChaosScoreModifier),
        ];

    public static Ballteration GenerateToRarityCurve(List<WeightedItem<Func<Ballteration>>> WeightedGenerators = null)
    {
        // TODO: Maybe optimise that with a queue instead of doing .Except
        // not critical for now

        // Use this one with ConstrainRarity, might fail less but less close to the curve.
        //float targetRarity = Instance.RarityCurve.Sample(GD.Randf());

        // Use this one with EnsureRarity, prone to failing but sticks to the curve
        // TODO: Change Ballteration.Ameliorate() to a clamping system, idem for effects
        float targetRarity = (float)GD.RandRange(0, Instance.RarityCurve.Sample(GD.Randf()));

        GD.Print($"Targetting {targetRarity}");

        WeightedGenerators ??= WeightedGeneratorsBase.ToList();

        WeightedItem<Func<Ballteration>> WeightedGenerator = WeightedItem<Func<Ballteration>>.GetFrom(WeightedGenerators);
        WeightedGenerators.Remove(WeightedGenerator);
        Ballteration ballteration = WeightedGenerator.Item();
        GD.Print($"Generating {ballteration.GetType()}");

        while (!(EnsureRarity(ballteration, out Ballteration betterFit, targetRarity) || WeightedGenerators.Count() == 0))
        {
            WeightedGenerator = WeightedItem<Func<Ballteration>>.GetFrom(WeightedGenerators);
            WeightedGenerators.Remove(WeightedGenerator);
            ballteration = WeightedGenerator.Item();
            GD.Print($"Generating {ballteration.DisplayName}");
        }

        return ballteration;
    }


    #region 
    const int defaultRetries = 10;

    public static bool ConstrainRarity(Ballteration ballteration, float targetRarity, bool max = true, int retries = defaultRetries)
    {
        for (int i = 0; i < retries; i++)
        {
            if ((max && ballteration.AnalogRarity <= targetRarity) || (!max && ballteration.AnalogRarity >= targetRarity))
            {
                GD.PrintRich($"[color=GREEN]Ballteration fits in {i} retries[/color] (max: {max}, targetRarity: {targetRarity}, ballteration {ballteration.AnalogRarity})");
                return true;
            }
            GD.PrintRich($"[color=ORANGE]Not fitting[/color] (generated {ballteration.AnalogRarity}, max: {max}, targetRarity: {targetRarity})\nRetrying {i}...");
            if (max)
                ballteration.Worsen();
            else
                ballteration.Ameliorate();
        }

        GD.PrintRich($"[color=RED]Failed to fit target rarity[/color](generated {ballteration.AnalogRarity}, max: {max}, targetRarity: {targetRarity})");
        return false;
    }

    const float rarityVariance = 0.5f;

    public static bool EnsureRarity(Ballteration ballteration, out Ballteration betterFit, float targetRarity, int retries = defaultRetries)
    {
        float minAllowedRarity = targetRarity - rarityVariance;
        float maxAllowedRarity = targetRarity + rarityVariance;

        betterFit = (Ballteration)ballteration.Duplicate();

        for (int i = 0; i < retries; i++)
        {
            // In case we find a closer ballteration, we keep it in memory
            if (Math.Abs(ballteration.AnalogRarity - targetRarity) < Math.Abs(betterFit.AnalogRarity - targetRarity))
            {
                betterFit = (Ballteration)ballteration.Duplicate();
            }
            if (ballteration.AnalogRarity >= maxAllowedRarity)
            {
                GD.PrintRich($"[color=GREEN]Above target[/color] (ballteration {ballteration.AnalogRarity}, targetRarity: {targetRarity}+-0.5)\nWorsening {i}...");
                ballteration.Worsen();
            }
            else if (ballteration.AnalogRarity <= minAllowedRarity)
            {
                GD.PrintRich($"[color=RED]Below target[/color] target (ballteration {ballteration.AnalogRarity}, targetRarity: {targetRarity}+-0.5)\nImproving {i}...");
                ballteration.Ameliorate();
            }
            else
            {
                GD.PrintRich($"[color=GREEN]Ballteration fits in {i} retries[/color] (targetRarity: {targetRarity}, ballteration {ballteration.AnalogRarity})");
                // Ballteration fits the requirements, and is already stored in betterFit, we say that we are happy
                return true;
            }

        }

        GD.PrintRich($"[color=RED]Failed to fit target rarity[/color](ballteration {ballteration.AnalogRarity}, targetRarity: {targetRarity} +-0.5)");
        // Ballteration does not fit the requirements, but we have the betterFit, we say that we are unhappy
        return false;
    }
    #endregion

    #region generators
    #region score_modifiers
    public static Ballteration CreateSimpleScoreModifier()
    {
        Ballteration ballteration = new();
        ScoreModifier modifier = ScoreModifier.CreateRandomSimple();

        ballteration.AddChild(modifier);

        ballteration.Kind = Ballteration.Type.Score;
        ballteration.DisplayName = $"{modifier.GetGroups().First()} {((int)modifier.Prio % 2 == 1 ? "super " : "")}{((int)modifier.Prio <= 1 ? "adder" : "multiplier")}";

        return ballteration;
    }
    public static Ballteration CreateScoreModifier()
    {
        Ballteration ballteration = new();
        ScoreModifier modifier = ScoreModifier.CreateRandom();

        ballteration.AddChild(modifier);

        ballteration.Kind = Ballteration.Type.Score;
        ballteration.DisplayName = $"Score {((int)modifier.Prio % 2 == 1 ? "super " : "")}{((int)modifier.Prio <= 1 ? "adder" : "multiplier")}";

        return ballteration;
    }
    public static Ballteration CreateChaosScoreModifier()
    {
        Ballteration ballteration = new();

        for (int i = 0; i < GD.RandRange(1, 4); i++)
        {
            ScoreModifier modifier = ScoreModifier.CreateRandom();
            ballteration.AddChild(modifier);
        }

        ballteration.Kind = Ballteration.Type.Score;
        ballteration.DisplayName = $"Chaotic score modifier";

        return ballteration;
    }
    #endregion

    public static Ballteration CreateNewBall()
    {
        switch (GD.RandRange(1, 3))
        {
            default:
                return GD.Load<PackedScene>("res://Game/Assets/Ballterations/Pool0Yellow/NewBall.tscn").Instantiate<Ballteration>();
            case 2:
                return GD.Load<PackedScene>("res://Game/Assets/Ballterations/Pool0Yellow/ExtraBall.tscn").Instantiate<Ballteration>();
            case 3:
                return GD.Load<PackedScene>("res://Game/Assets/Ballterations/Pool0Yellow/ReplayBall.tscn").Instantiate<Ballteration>();
        }
    }
    #endregion
}
