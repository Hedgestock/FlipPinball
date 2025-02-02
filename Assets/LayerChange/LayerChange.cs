using Godot;
using System;

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

    void OnBodyEnterGate1 (Node2D body)
    {
        ChangeLayer(body, Layer1, true, Gate2);

    }

    void OnBodyEnterGate2(Node2D body)
    {
        ChangeLayer(body, Layer2, false, Gate1);
    }

    void OnBodyExitGate1(Node2D body)
    {
        ChangeLayer(body, Layer1, false, Gate2);

    }

    void OnBodyExitGate2(Node2D body)
    {
        ChangeLayer(body, Layer2, true, Gate1);
    }

    void ChangeLayer(Node2D body, int layer, bool active, Area2D otherGate)
    {
        if (!(body is Ball)) return;
        Ball ball = (Ball)body;
        if (otherGate.OverlapsBody(ball))
        {
            ball.SetCollisionLayerValue(layer, active);
            ball.SetCollisionMaskValue(layer, active);
        }
    }
}
