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

    [Signal]
    public delegate void TeleportingEventHandler(Ball ball, Vector2 position);

    public override void _Ready()
    {
        Group = "Spitters";
        base._Ready();
        foreach (var node in Nodes)
            ((Spitter)node.node).SwallowingBall += (ball) => SpitFromActive(ball, node.light);
        GameManager.Instance.Connect(GameManager.SignalName.HeldBallsChanged, new Callable(this, MethodName.SpitFromAll));
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
        StatusManager.Instance.EmitSignal(StatusManager.SignalName.StatusChanged, "STATUS_QUANTUM_TUNNELING_ACTIVATED_TESTLAB");
    }

    void SpitFromActive(Ball ball, OnOffLight light)
    {
        if (!Activated) return;
        if (light.IsOn)
        {
            EmitSignalShouldHold(ball);
            light.TurnOff();
        }
        else
        {
            var active = Nodes.First(n => n.light.IsOn);
            EmitSignalTeleporting(ball, ((Node2D)active.node).GlobalPosition);
            active.light.TurnBlinking();
        }
    }

    void SpitFromAll(Array<Ball> HeldBalls)
    {
        if (HeldBalls.Count != Nodes.Count) return;
        for (int i = 0; i < HeldBalls.Count; i++)
        {
            CallDeferred(GodotObject.MethodName.EmitSignal, SignalName.AddingBall, HeldBalls[i].Duplicate(), ((Node2D)Nodes[i].node).GlobalPosition);
        }
    }
}
