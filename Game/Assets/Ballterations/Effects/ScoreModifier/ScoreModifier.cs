using Godot;
using System;
using System.Linq;

public partial class ScoreModifier : Effect
{
    public enum Priority
    {
        Adder,
        SuperAdder,
        Multiplier,
        SuperMultiplier,
    }


    [Export]
    public Priority Prio = Priority.Adder;

    [Export]
    public bool Restrictive = false;

    [Export]
    public float Value = 1;

    public override string Description
    {
        get
        {
            return $"{GetGroups()} score {((int)Prio <= 1 ? '+' : 'x')}{Value} {((int)Prio % 2 == 1 ? 'S' : "")}{(Restrictive ? 'R' : "")}";
        }
    }

    static StringName[] ScoringGroups = ["Bumpers", "Global", "Rollovers", "Slingshots", "Spinners", "Spitters", "Targets", "ShapeRound", "ShapeSquare"];

    public static Ballteration CreateRandomSimple()
    {
        Ballteration b = new();
        ScoreModifier sc = new();

        sc.Prio = (Priority)GD.RandRange(0, 3);
        sc.Value = (float)((int)sc.Prio <= 1 ? GD.RandRange(100, 2000) : Mathf.Snapped(GD.RandRange(1.1, 5), 0.1));
        sc.AddToGroup(ScoringGroups[GD.RandRange(0, ScoringGroups.Length - 1)]);
        b.AddChild(sc);

        b.DisplayName = $"{sc.GetGroups().First()}{((int)sc.Prio % 2 == 1 ? " super" : "")} {((int)sc.Prio <= 1 ? "adder" : "multiplier")}";

        return b;
    }
}
