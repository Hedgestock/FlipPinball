using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

public partial class TargetGroup : Node2D
{
    [Signal]
    public delegate void AllOffEventHandler();

    List<Target> Targets;

    public override void _Ready()
    {
        base._Ready();
        Targets = GetChildren().Where(c => c is Target).Cast<Target>().ToList();
        Targets.ForEach(s => s.ChangedState += CheckGroupStatus);
    }

    private void CheckGroupStatus()
    {
        foreach (var target in Targets)
            if (target.IsOn) return;

        EmitSignal(SignalName.AllOff);
    }
}
