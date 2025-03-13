using Godot;
using System;

public partial class ScoreModifier : Node
{
    public enum Priority
    {
        SuperAdder,
        Multiplier,
        Adder,
        SuperMultiplier,
    }


    [Export]
    public Priority Prio = Priority.Adder;

    [Export]
    public bool Restrictive = false;

    [Export]
    public int Value = 1;
}
