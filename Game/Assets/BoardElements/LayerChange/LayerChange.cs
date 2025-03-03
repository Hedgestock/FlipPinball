using Godot;
using System;
using System.Reflection.Emit;

public partial class LayerChange : Node2D
{
    [Export]
    int Layer1;
    [Export]
    int Layer2;

    [Export]
    Area2D Gate1;
    [Export]
    Area2D Gate2;

    public override void _Ready()
    {
        base._Ready();
        Gate1.CollisionLayer = (uint)(1 << Layer1 | 1 << Layer2);
        Gate1.CollisionMask = Gate1.CollisionLayer;
        Gate2.CollisionLayer = Gate1.CollisionLayer;
        Gate2.CollisionMask = Gate1.CollisionLayer;
    }

    void OnBodyEnterGate1 (Node2D body)
    {
        ChangeLayer(body, Layer1, true, Gate2);
    }

    void OnBodyEnterGate2(Node2D body)
    {
        ChangeLayer(body, Layer2, true, Gate1);
    }

    void OnBodyExitGate1(Node2D body)
    {
        ChangeLayer(body, Layer1, false, Gate2);
    }

    void OnBodyExitGate2(Node2D body)
    {
        ChangeLayer(body, Layer2, false, Gate1);
    }

    void ChangeLayer(Node2D body, int layer, bool active, Area2D otherGate)
    {
        if (!(body is Ball)) return;
        Ball ball = (Ball)body;
        if (otherGate.OverlapsBody(ball))
        {
            ball.SetCollisionLayerValue(1 << layer, active);
            ball.SetCollisionMaskValue(1 << layer, active);
        }
    }
}
