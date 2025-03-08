using Godot;
using System;
using System.Linq;

public partial class TeleportGroup : BoardElementsGroup
{
    public override void _Ready()
    {
        Group = "Spitters";
        base._Ready();
        foreach (var node in Nodes)
            ((Spitter)node.node).SwallowingBall += SpitFromActive;
    }

    bool Activated
    {
        get
        {
            foreach (var node in Nodes)
                if (node.light.IsOn) return true;
            return false;
        }
    }

    void Activate()
    {
        if (Activated) return;
        Nodes[GD.RandRange(0, Nodes.Count - 1)].light.TurnOn();
    }

    void SpitFromActive(Ball ball)
    {
        if (!Activated) return;
        ball.Position = ((Node2D)Nodes.First(n => n.light.IsOn).node).Position;
        SetAllOff();
    }
}
