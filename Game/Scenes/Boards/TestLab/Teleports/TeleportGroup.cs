using Godot;
using Godot.Collections;
using System;
using System.Linq;

public partial class TeleportGroup : BoardElementsGroup
{
    [Signal]
    public delegate void ShouldHoldEventHandler(Ball ball);

    [Signal]
    public delegate void AddingBallEventHandler(Ball ball, Vector2 position);

    public override void _Ready()
    {
        Group = "Spitters";
        base._Ready();
        foreach (var node in Nodes)
            ((Spitter)node.node).SwallowingBall += (ball) => SpitFromActive(ball, node.light);
        GameManager.Instance.HeldBallsChanged += SpitFromAll;
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

    void SpitFromActive(Ball ball, OnOffLight light)
    {
        if (!Activated) return;
        if (light.IsOn)
        {
            EmitSignal(SignalName.ShouldHold, ball);
        }
        else
        {
            ball.GlobalPosition = ((Node2D)Nodes.First(n => n.light.IsOn).node).GlobalPosition;
        }
        SetAllOff();
    }

    void SpitFromAll(Array<Ball> HeldBalls)
    {
        if (HeldBalls.Count != Nodes.Count) return;
        for (int i = 0; i < HeldBalls.Count; i++)
        {
            EmitSignal(SignalName.AddingBall, HeldBalls[i], ((Node2D)Nodes[i].node).GlobalPosition);
        }
        SetAllOff();
    }
}
