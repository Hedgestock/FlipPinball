using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class TargetGroup : Node
{
    [Signal]
    public delegate void AllOffEventHandler();

    List<Node> Targets;

    public override void _Ready()
    {
        base._Ready();
        Targets = GetChildren().Where(c => c.IsInGroup("Targets")).ToList();
        Targets.ForEach(s => s.GetNode<OnOffLight>("Hitbox/OnOffLight").Toggled += (on) => { if (!on) CheckGroupStatus(); });
    }

    private void CheckGroupStatus()
    {
        foreach (var target in Targets)
            if (target.GetNode<OnOffLight>("Hitbox/OnOffLight").IsOn) return;

        EmitSignal(SignalName.AllOff);
    }
}
