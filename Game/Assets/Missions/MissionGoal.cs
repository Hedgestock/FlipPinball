using Godot;
using Godot.Collections;
using System;
using System.Text.RegularExpressions;

public partial class MissionGoal : Node
{
    [Signal]
    public delegate void UpdatedEventHandler();
    [Signal]
    public delegate void CompletedEventHandler();

    [Export]
    int Remains = 1;
    int _remains = 1;

     string StatusRunning;
     string StatusCompleted;


    public string Status
    {
        get { return Tr(IsComplete ? StatusCompleted : Tr(StatusRunning).Replace("{remains}", $"{_remains}")); }
    }

    public bool IsComplete = false;
    bool IsActive = false;

    public override void _Ready()
    {
        base._Ready();
        string tmp = Regex.Replace(Name + GameManager.CurrentBoard.Name, "(?<!^)([A-Z])", "_$1").ToUpperInvariant();
        StatusRunning = "MISSION_GOAL_" + tmp;
        StatusCompleted = "MISSION_GOAL_COMPLETE_" + tmp;
    }


    public void Init()
    {
        IsActive = true;
        IsComplete = false;
        _remains = Remains;
    }

    private void Update()
    {

        if (!IsActive) return;
        _remains--;
        if (_remains <= 0)
            Complete();
        EmitSignalUpdated();
    }

    private void Complete()
    {
        if (!IsActive) return;
        IsActive = false;
        IsComplete = true;
        EmitSignalCompleted();
    }
}
