using Godot;
using Godot.FlipPinball;
using System;
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

     static WeightedItem<Func<Ballteration>>[] WeightedGenerators = [
        new(CreateNewBall, 10),
        new(CreateScoreModifier),
        new(CreateSimpleScoreModifier),
        new(CreateChaosScoreModifier),
        ];

    public static Ballteration GenerateToRarityCurve(Func<Ballteration> Creator)
    {
        // Use this one with EnsureRarity, but more prone to failure.
        //float targetRarity = (float)GD.RandRange(0, Instance.RarityCurve.Sample(GD.Randf()));
        float targetRarity = Instance.RarityCurve.Sample(GD.Randf());

        Ballteration ballteration = WeightedItem<Func<Ballteration>>.ChooseFrom(WeightedGenerators)();

        ConstrainRarity(ballteration, targetRarity);

        return ballteration;
    }


    #region constrainers
    public static bool ConstrainRarity(Ballteration ballteration, float targetRarity, bool max = true, int retries = 20)
    {
        for (int i = 0; i < retries; i++)
        {
            if ((max && ballteration.AnalogRarity <= targetRarity) || (!max && ballteration.AnalogRarity >= targetRarity))
            {
                GD.Print($"Ballteration fits in {i} retries (max: {max}, targetRarity: {targetRarity}, ballteration {ballteration.AnalogRarity})");
                return true;
            }
            GD.Print($"(generated {ballteration.AnalogRarity}, max: {max}, targetRarity: {targetRarity})\nRetrying {i}...");
            if (max)
                ballteration.Worsen();
            else
                ballteration.Ameliorate();
        }

        GD.Print($"(generated {ballteration.AnalogRarity}, max: {max}, targetRarity: {targetRarity})");
        return false;
    }

    public static bool EnsureRarity(Ballteration ballteration, float targetRarity, int retries = 20)
    {
        for (int i = 0; i < retries; i++)
        {
            if (ballteration.AnalogRarity >= targetRarity - 0.5 && ballteration.AnalogRarity <= targetRarity + 0.5)
            {
                GD.Print($"Ballteration fits in {i} retries (targetRarity: {targetRarity}, ballteration {ballteration.AnalogRarity})");
                return true;
            }
            GD.Print($"(ballteration {ballteration.AnalogRarity}, targetRarity: {targetRarity} +-0.5)\nImproving {i}...");
            ballteration.Ameliorate();
        }

        GD.Print($"(ballteration {ballteration.AnalogRarity}, targetRarity: {targetRarity} +-0.5)");
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
