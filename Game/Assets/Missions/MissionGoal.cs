using Godot;
using Godot.Collections;
using System;

public partial class MissionGoal : Node
{
    [Signal]
    public delegate void UpdatedEventHandler();
    [Signal]
    public delegate void CompletedEventHandler();

    [Export]
    private string StatusRunning;
    [Export]
    private string StatusCompleted;

    [Export]
    int Remains = 1;
    int _remains = 1;

    public string Status
    {
        get { return Tr(IsComplete ? StatusCompleted : Tr(StatusRunning).Replace("{remains}", $"{_remains}")); }
    }

    public bool IsComplete = false;
    bool IsActive = false;

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
