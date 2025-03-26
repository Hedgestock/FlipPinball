using Godot;
using Godot.FlipPinball;
using System;
using System.Collections.Generic;
using System.Linq;
using static Ballteration;

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

    //public override void _Ready()
    //{
    //    base._Ready();
    //    //for (int i = 0; i < 2500; i++)
    //    //{
    //    //    float source = GD.Randf();
    //    //    GD.Print($"{GD.RandRange(0,RarityCurve.Sample(source)):N10}");
    //    //    //GD.Print($"{RarityCurve.Sample(source):N10}");
    //    //}
    //}

    static WeightedItem<Func<float, Ballteration>>[] WeightedPickersBase = [
        new((float targetRarity) => GeneratorWrapper(targetRarity, CreateNewBall), 10),
        new((float targetRarity) => GeneratorWrapper(targetRarity, CreateScoreModifier)),
        new((float targetRarity) => GeneratorWrapper(targetRarity, CreateSimpleScoreModifier)),
        new((float targetRarity) => GeneratorWrapper(targetRarity, CreateChaosScoreModifier)),
        new(GetFromPool,50),
        ];

    public static Ballteration Generate(List<WeightedItem<Func<float, Ballteration>>> WeightedPickers = null)
    {
        // TODO: Maybe optimise that with a queue instead of doing .Except
        // not critical for now

        // Use this one with ConstrainRarity, might fail less but less close to the curve.
        //float targetRarity = Instance.RarityCurve.Sample(GD.Randf());

        // Use this one with EnsureRarity, prone to failing but sticks to the curve
        // TODO: Change Ballteration.Ameliorate() to a clamping system, idem for effects
        float targetRarity = (float)GD.RandRange(0, Instance.RarityCurve.Sample(GD.Randf()));

        GD.Print($"Targetting {targetRarity}");

        return Generate(targetRarity, WeightedPickers);
    }

    public static Ballteration Generate(float targetRarity, List<WeightedItem<Func<float, Ballteration>>> WeightedPickers = null)
    {
        // TODO: Use best fit instead of last fit
        WeightedPickers ??= WeightedPickersBase.ToList();

        WeightedItem<Func<float, Ballteration>> WeightedPicker = WeightedItem<Func<float, Ballteration>>.GetFrom(WeightedPickers);
        WeightedPickers.Remove(WeightedPicker);
        Ballteration ballteration = WeightedPicker.Item(targetRarity);

        GD.Print($"Generating {ballteration.GetType()}");

        float minAllowedRarity = targetRarity - rarityVariance;
        float maxAllowedRarity = targetRarity + rarityVariance;

        while ((ballteration.Rarity < minAllowedRarity || maxAllowedRarity < ballteration.Rarity) && WeightedPickers.Count() != 0)
        {
            WeightedPicker = WeightedItem<Func<float, Ballteration>>.GetFrom(WeightedPickers);
            WeightedPickers.Remove(WeightedPicker);
            ballteration = WeightedPicker.Item(targetRarity);
            GD.Print($"Generating {ballteration.DisplayName}");
        }

        return ballteration;
    }

    #region constrainers
    const int defaultRetries = 10;

    public static bool TryConstrainRarity(Ballteration ballteration, float targetRarity, bool targetIsMax = true, int retries = defaultRetries)
    {
        for (int i = 0; i < retries; i++)
        {
            if ((targetIsMax && ballteration.Rarity <= targetRarity) || (!targetIsMax && ballteration.Rarity >= targetRarity))
            {
                GD.PrintRich($"[color=GREEN]Ballteration fits in {i} retries[/color] (max: {targetIsMax}, targetRarity: {targetRarity}, ballteration {ballteration.Rarity})");
                return true;
            }
            GD.PrintRich($"[color=ORANGE]Not fitting[/color] (generated {ballteration.Rarity}, max: {targetIsMax}, targetRarity: {targetRarity})\nRetrying {i}...");
            if (targetIsMax)
                ballteration.Refine(maximum: ballteration);
            else
                ballteration.Refine(minimum: ballteration);
        }

        GD.PrintRich($"[color=RED]Failed to fit target rarity[/color](generated {ballteration.Rarity}, max: {targetIsMax}, targetRarity: {targetRarity})");
        return false;
    }

    const float rarityVariance = 0.5f;

    public static bool TryEnsureRarity(Ballteration ballteration, out Ballteration bestFit, float targetRarity, int retries = defaultRetries)
    {
        float minAllowedRarity = targetRarity - rarityVariance;
        float maxAllowedRarity = targetRarity + rarityVariance;

        bestFit = (Ballteration)ballteration.Duplicate();

        for (int i = 0; i < retries; i++)
        {
            // In case we find a closer ballteration, we keep it in memory
            if (Math.Abs(ballteration.Rarity - targetRarity) < Math.Abs(bestFit.Rarity - targetRarity))
            {
                bestFit = (Ballteration)ballteration.Duplicate();
            }
            if (ballteration.Rarity >= maxAllowedRarity)
            {
                GD.PrintRich($"[color=GREEN]Above target[/color] (ballteration {ballteration.Rarity}, targetRarity: {targetRarity}+-0.5)\nWorsening {i}...");
                ballteration.Refine(maximum: ballteration);
            }
            else if (ballteration.Rarity <= minAllowedRarity)
            {
                GD.PrintRich($"[color=RED]Below target[/color] target (ballteration {ballteration.Rarity}, targetRarity: {targetRarity}+-0.5)\nImproving {i}...");
                ballteration.Refine(minimum: ballteration);
            }
            else
            {
                GD.PrintRich($"[color=GREEN]Ballteration fits in {i} retries[/color] (targetRarity: {targetRarity}, ballteration {ballteration.Rarity})");
                // Ballteration fits the requirements, and is already stored in bestFit, we say that we are happy
                return true;
            }

        }

        GD.PrintRich($"[color=RED]Failed to fit target rarity[/color](ballteration {ballteration.Rarity}, targetRarity: {targetRarity} +-0.5)");
        // Ballteration does not fit the requirements, but we have the bestFit, we say that we are unhappy
        return false;
    }
    #endregion

    #region generators
    public static Ballteration GeneratorWrapper(float targetRarity, Func<Ballteration> Generator)
    {
        Ballteration ballteration = Generator();

        // TODO: Stop ignoring the return value and then re-testing for it
        TryEnsureRarity(ballteration, out Ballteration bestFit, targetRarity);

        return bestFit;
    }

    #region score_modifiers
    public static Ballteration CreateSimpleScoreModifier()
    {
        Ballteration ballteration = new();
        ScoreModifier modifier = ScoreModifier.CreateRandomSimple();

        ballteration.AddChild(modifier);

        ballteration.DisplayName = $"{modifier.GetGroups().First()} {((int)modifier.Prio % 2 == 1 ? "super " : "")}{((int)modifier.Prio <= 1 ? "adder" : "multiplier")}";

        return ballteration;
    }
    public static Ballteration CreateScoreModifier()
    {
        Ballteration ballteration = new();
        ScoreModifier modifier = ScoreModifier.CreateRandom();

        ballteration.AddChild(modifier);

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

        ballteration.DisplayName = $"Chaotic score modifier";

        return ballteration;
    }
    #endregion

    public static Ballteration CreateNewBall()
    {
        Ballteration ballteration = new();
        switch (GD.RandRange(1, 3))
        {
            default:
                ballteration.AddChild(new NewBall());
                break;
            case 2:
                ballteration.AddChild(new ExtraBall());
                break;
            case 3:
                ballteration.AddChild(new ReplayBall());
                break;
        }

        ballteration.DisplayName = "Ball UP";
        return ballteration;
    }

    #endregion

    const string BallterationsPath = "res://Game/Assets/Ballterations/";

    public static Ballteration GetFromPool(float targetRarity)
    {
        int clampedRarity = (int)Math.Round(targetRarity);

        //TODO: tell that we failed
        if (clampedRarity < (int)RarityColor.Gray)
            clampedRarity = (int)RarityColor.Gray;
        else if (clampedRarity > (int)RarityColor.Red)
            clampedRarity = (int)RarityColor.Red;


        var dir = DirAccess.Open($"{BallterationsPath}Pool{clampedRarity}{(RarityColor)clampedRarity}");
        List<WeightedItem<string>> validBallterationsPaths = new();
        dir.ListDirBegin();

        foreach (var fileName in dir.GetFiles())
        {
            if (!dir.CurrentIsDir() && fileName.GetExtension() == "tscn")
                validBallterationsPaths.Add(new WeightedItem<string>(dir.GetCurrentDir() + "/" + fileName));
        }

        Ballteration ballteration = GD.Load<PackedScene>(WeightedItem<string>.ChooseFrom(validBallterationsPaths)).Instantiate<Ballteration>();
        ballteration.Rarity = clampedRarity;
        return ballteration;
    }
}
