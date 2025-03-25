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
                rarity += Value / 700;
            }
            else
            {
                rarity += Value / 1.7f;
            }

            if (IsSuper)
                rarity += 0.5f;

            if (GetGroups().Contains("Global"))
                rarity++;
            else if (Restrictive)
                rarity -= Math.Max(GetGroups().Count / 4f, 1f);
            else
                rarity += Math.Min(GetGroups().Count / 4f, 1f);

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

    const int minAdderValue = 100;
    const int maxAdderValue = 2000;
    const float minMultiplierValue = 1.1f;
    const float maxMultiplierValue = 5f;

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

    public override ScoreModifier Ameliorate()
    {
        if (!IsSuper)
            Prio = (Priority)((int)Prio + (GD.Randi() % 2));

        Value = (float)((int)Prio <= 1 ? GD.RandRange((int)Value, maxAdderValue) : Mathf.Snapped(GD.RandRange(Value, maxMultiplierValue), 0.1));

        //if (Restrictive)
        //    Restrictive = false;
        //    Restrictive = GD.Randi() % 2 == 0;

        return this;
    }

    public override ScoreModifier Worsen()
    {
        if (IsSuper)
            Prio = (Priority)((int)Prio - (GD.Randi() % 2));

        Value = (float)((int)Prio <= 1 ? GD.RandRange(minAdderValue, (int)Value) : Mathf.Snapped(GD.RandRange(minMultiplierValue, Value), 0.1));

        //if (!Restrictive && GetGroups().Count > 1)
        //    Restrictive = true;
        //    Restrictive = GD.Randi() % 2 == 0;

        return this;
    }
}
