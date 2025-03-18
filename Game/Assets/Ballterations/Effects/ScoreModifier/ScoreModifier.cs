using Godot;
using System;
using System.Linq;
using static Ballteration;

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
            return $"{GetGroups()} score {((int)Prio <= 1 ? '+' : 'x')}{Value} {((int)Prio % 2 == 1 ? 'S' : "")}{(Restrictive ? 'R' : "")}";
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
                rarity -= GetGroups().Count / 4;
            else
                rarity += GetGroups().Count / ScoringGroups.Count();
            GD.Print((GetGroups(), Prio, Value, rarity));

            return rarity;
            //return (Ballteration.Rarity)Math.Clamp(rarity, 1, 5);
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
}
