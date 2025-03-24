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
            return $"{string.Join(Restrictive ? " and " : " or ", GetGroups())} score {((int)Prio <= 1 ? '+' : 'x')}{Value} {((int)Prio % 2 == 1 ? 'S' : "")}{(Restrictive ? 'R' : "")}";
        }
    }

    public override float AnalogRarity
    {
        get
        {
            float rarity = 0;
            if ((int)Prio <= 1)
            {
                rarity += Value / 700;
            }
            else
            {
                rarity += Value / 1.7f;
            }

            if ((int)Prio % 2 == 1)
                rarity += 0.5f;

            if (GetGroups().Contains("Global"))
                rarity++;
            else if (Restrictive)
                rarity -= GetGroups().Count / 4f;
            else
                rarity += Math.Min(GetGroups().Count / 4f, 1f);
            //GD.Print((GetGroups(), Prio, Value, rarity));

            return rarity;
        }
    }

    [Export]
    public Priority Prio = Priority.Adder;

    [Export]
    public bool Restrictive = false;

    [Export]
    public float Value = 1;


    static StringName[] ScoringGroups = ["Bumpers", "Global", "Rollovers", "Slingshots", "Spinners", "Spitters", "Targets", "ShapeRound", "ShapeSquare"];

    public static ScoreModifier CreateRandomSimple()
    {
        ScoreModifier sm = new();

        sm.Prio = (Priority)GD.RandRange(0, 3);
        sm.Value = (float)((int)sm.Prio <= 1 ? GD.RandRange(100, 2000) : Mathf.Snapped(GD.RandRange(1.1, 5), 0.1));
        sm.AddToGroup(ScoringGroups[GD.RandRange(0, ScoringGroups.Length - 1)]);

        return sm;
    }

    public static ScoreModifier CreateRandom()
    {
        ScoreModifier sm = new();

        sm.Prio = (Priority)GD.RandRange(0, 3);
        sm.Value = (float)((int)sm.Prio <= 1 ? GD.RandRange(100, 2000) : Mathf.Snapped(GD.RandRange(1.1, 5), 0.1));
        sm.Restrictive = GD.Randi() % 2 == 0;
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
        return sm;
    }
}
