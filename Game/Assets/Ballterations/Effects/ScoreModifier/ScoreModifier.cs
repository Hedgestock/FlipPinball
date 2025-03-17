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

    public override string Description
    {
        get
        {
            return $"{GetGroups()} score {((int)Prio <= 1 ? '+' : 'x')}{Value} {((int)Prio % 2 == 1 ? 'S' : "")}{(Restrictive ? 'R' : "")}";
        }
    }
    public Ballteration.Rarity Rarity
    {
        get
        {
            int rarity = 0;
            if ((int)Prio <= 1)
            {
                if (Value <= 200)
                    rarity = 1;
                else if (Value <= 500)
                    rarity = 2;
                else if (Value <= 1000)
                    rarity = 3;
                else
                    rarity = 4;
            }
            else
            {
                if (Value <= 1.5)
                    rarity = 1;
                else if (Value <= 2)
                    rarity = 2;
                else if (Value <= 3)
                    rarity = 3;
                else
                    rarity = 4;
            }

            if ((int)Prio % 2 == 1)
                rarity++;

            if (GetGroups().Count > 3)
            {
                if (Restrictive)
                    rarity--;
                else
                    rarity++;
            }

            return (Ballteration.Rarity)Math.Clamp(rarity, 1, 5);
        }
    }



    [Export]
    public Priority Prio = Priority.Adder;

    [Export]
    public bool Restrictive = false;

    [Export]
    public float Value = 1;


    static StringName[] ScoringGroups = ["Bumpers", "Global", "Rollovers", "Slingshots", "Spinners", "Spitters", "Targets", "ShapeRound", "ShapeSquare"];

    public static Ballteration CreateRandomSimple()
    {
        Ballteration b = new();
        ScoreModifier sm = new();

        sm.Prio = (Priority)GD.RandRange(0, 3);
        sm.Value = (float)((int)sm.Prio <= 1 ? GD.RandRange(100, 2000) : Mathf.Snapped(GD.RandRange(1.1, 5), 0.1));
        sm.AddToGroup(ScoringGroups[GD.RandRange(0, ScoringGroups.Length - 1)]);
        b.AddChild(sm);

        b.Kind = Ballteration.Type.Score;
        b.DisplayName = $"{sm.GetGroups().First()}{((int)sm.Prio % 2 == 1 ? " super" : "")} {((int)sm.Prio <= 1 ? "adder" : "multiplier")}";
        b.Color = sm.Rarity;

        return b;
    }
}
