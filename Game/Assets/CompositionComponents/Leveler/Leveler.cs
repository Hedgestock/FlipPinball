using Godot;
using System;

public partial class Leveler : Node
{
    [Signal]
    public delegate void OnLevelOverflowEventHandler();

    [Signal]
    public delegate void OnLevelUpEventHandler(int level);
    [Signal]
    public delegate void OnLevelDownEventHandler(int level);

    [Export]
    public int MaxLevel;
    [Export]
    public int MinLevel;

    public int Level;

    public override void _Ready()
    {
        base._Ready();
        Level = MinLevel;
    }

    private void LevelUp()
    {
        if (Level >= MaxLevel)
            EmitSignal(SignalName.OnLevelOverflow);
        else
            EmitSignal(SignalName.OnLevelUp, ++Level);
    }

    private void LevelDown()
    {
        if (Level <= MinLevel) return;
        EmitSignal(SignalName.OnLevelDown, --Level);
    }
}
