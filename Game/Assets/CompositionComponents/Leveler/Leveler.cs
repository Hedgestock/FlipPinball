using Godot;
using System;
using System.Reflection.Emit;

public partial class Leveler : Node2D
{
    [Signal]
    public delegate void OnLevelOverflowEventHandler();
    [Signal]
    public delegate void OnLevelUnderflowEventHandler();

    [Signal]
    public delegate void OnLevelUpEventHandler(int level);
    [Signal]
    public delegate void OnLevelDownEventHandler(int level);

    [Signal]
    public delegate void OnLevelChangeEventHandler(int level);

    [Export]
    public int MaxLevel;
    [Export]
    public int MinLevel;

    private int _currentLevel;
    public int CurrentLevel
    {
        get { return _currentLevel; }
        set
        {
            if (value == _currentLevel) return;
            else if (value < MinLevel)
            {
                EmitSignal(SignalName.OnLevelUnderflow);
                return;
            }
            else if (value > MaxLevel)
            {
                EmitSignal(SignalName.OnLevelOverflow);
                return;
            }
            else if (value > _currentLevel)
                EmitSignal(SignalName.OnLevelUp, value);
            else
                EmitSignal(SignalName.OnLevelDown, value);
            EmitSignal(SignalName.OnLevelChange, value);
            _currentLevel = value;
        }
    }


    public override void _Ready()
    {
        base._Ready();
        _currentLevel = MinLevel;
    }

    public void LevelUp()
    {
        CurrentLevel++;
    }

    public void LevelDown()
    {
        CurrentLevel--;
    }

    public void SetLevel(int level)
    {
        CurrentLevel = level;
    }

    public void BumpLevel(int level)
    {
        if (_currentLevel >= level) return;
        CurrentLevel = level;
    }

    public void BonkLevel(int level)
    {
        if (_currentLevel <= level) return;
        CurrentLevel = level;
    }

    public void MaximizeLevel()
    {
        CurrentLevel = MaxLevel;
    }

    public void MinimizeLevel()
    {
        CurrentLevel = MinLevel;
    }
}
