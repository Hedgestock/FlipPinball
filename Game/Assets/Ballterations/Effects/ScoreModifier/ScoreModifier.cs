using Godot;
using System;

public partial class ScoreModifier : Effect
{
    public enum Priority
    {
        Adder,
        SuperAdder,
        Multiplier,
        SuperMultiplier,
    }

    public override string Description
    {
        get
        {
            return $"{string.Join(Restrictive ? " and " : " or ", GetGroups())} score {(Prio <= Priority.SuperAdder ? '+' : 'x')}{Value} {(IsSuper ? 'S' : "")}{(Restrictive ? 'R' : "")}";
        }
    }

    public override float AnalogRarity
    {
        get
        {
            float rarity = 0;
            if (Prio <= Priority.SuperAdder)
            {
                rarity += Value / 800;
            }
            else
            {
                rarity += Value / 1.6f;
            }

            if (IsSuper)
                rarity += 0.3f;

            if (GetGroups().Contains("Global"))
                rarity++;
            else if (Restrictive)
                rarity -= Math.Max(GetGroups().Count / (ScoringGroups.Length / 2), 1f);
            else
                rarity += Math.Min(GetGroups().Count / (ScoringGroups.Length / 2), 1f);

            return rarity;
        }
    }

    public bool IsSuper
    {
        get { return (int)Prio % 2 == 1; }
    }

    [Export]
    public Priority Prio = Priority.Adder;

    [Export]
    public bool Restrictive = false;

    [Export]
    public float Value = 1;


    static StringName[] ScoringGroups = ["Bumpers", "Global", "Rollovers", "Slingshots", "Spinners", "Spitters", "Targets", "Shape Round", "Shape Square"];

    protected const int minAdderValue = 100;
    protected const int maxAdderValue = 2000;
    protected const float minMultiplierValue = 1.1f;
    protected const float maxMultiplierValue = 5f;

    public static ScoreModifier CreateRandomSimple()
    {
        ScoreModifier sm = new();

        sm.Prio = (Priority)GD.RandRange(0, 3);
        sm.Value = (float)((int)sm.Prio <= 1 ? GD.RandRange(minAdderValue, maxAdderValue) : Mathf.Snapped(GD.RandRange(minMultiplierValue, maxMultiplierValue), 0.1));
        sm.AddToGroup(ScoringGroups[GD.RandRange(0, ScoringGroups.Length - 1)]);

        return sm;
    }

    public static ScoreModifier CreateRandom()
    {
        ScoreModifier sm = new();

        sm.Prio = (Priority)GD.RandRange(0, 3);
        sm.Value = (float)((int)sm.Prio <= 1 ? GD.RandRange(minAdderValue, maxAdderValue) : Mathf.Snapped(GD.RandRange(minMultiplierValue, maxMultiplierValue), 0.1));

        for (int i = 0; i < GD.RandRange(1, ScoringGroups.Length); i++)
            sm.AddToGroup(ScoringGroups[GD.RandRange(0, ScoringGroups.Length - 1)]);

        // Maybe optimise that at some point, not critical though
        if (sm.GetGroups().Contains("Global"))
        {
            foreach (var group in sm.GetGroups())
            {
                sm.RemoveFromGroup(group);
            }
            sm.AddToGroup("Global");
        }

        //if (sm.GetGroups().Count > 1)
        //    sm.Restrictive = GD.Randi() % 2 == 0;

        return sm;
    }

    public override ScoreModifier Refine(Effect minimum, Effect maximum)
    {
        ScoreModifier minimumSM = minimum as ScoreModifier;
        ScoreModifier maximumSM = maximum as ScoreModifier;
        ScoreModifier refined = (ScoreModifier)Duplicate();
        //if (!refined.IsSuper && maximumSM != null && maximumSM.IsSuper)
        //    refined.Prio = (Priority)((int)refined.Prio + (GD.Randi() % 2));


        // We clamp the refined value between the minimum and maximum effect values, and use defaults if null
        refined.Value = (float)((int)refined.Prio <= 1 ?
            GD.RandRange((int?)minimumSM?.Value ?? minAdderValue, (int?)maximumSM?.Value ?? maxAdderValue) :
            Mathf.Snapped(GD.RandRange(minimumSM?.Value ?? minMultiplierValue, maximumSM?.Value ?? maxMultiplierValue), 0.1));

        //if (refined.Restrictive)
        //    refined.Restrictive = GD.Randi() % 2 == 0;

        return refined;
    }
}
