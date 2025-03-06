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

    public int CurrentLevel;

    public override void _Ready()
    {
        base._Ready();
        CurrentLevel = MinLevel;
    }

    void LevelUp()
    {
        if (CurrentLevel >= MaxLevel)
            EmitSignal(SignalName.OnLevelOverflow);
        else
            EmitSignal(SignalName.OnLevelUp, ++CurrentLevel);
    }

    void LevelDown()
    {
        if (CurrentLevel <= MinLevel) return;
        EmitSignal(SignalName.OnLevelDown, --CurrentLevel);
    }

    void SetLevel(int level)
    {
        if (level < MinLevel || level > MaxLevel || level == CurrentLevel) return;
        if (level > CurrentLevel) EmitSignal(SignalName.OnLevelUp, level);
        else EmitSignal(SignalName.OnLevelDown, level);
        CurrentLevel = level;
    }

    void MaximizeLevel()
    {
        SetLevel(MaxLevel);
    }

    void MinimizeLevel()
    {
        SetLevel(MinLevel);
    }
}
