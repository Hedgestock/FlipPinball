using Godot;
using System;
using System.Linq;

public partial class KickBack : Node2D
{
    [Export]
    uint Strength = 4000;

    [Export]
    Area2D DetectionZone;

    [Export]
    StaticBody2D OneWayGate;
    [Export]
    Timer Delay;

    public override void _Ready()
    {
        base._Ready();
        OneWayGate.ProcessMode = ProcessModeEnum.Disabled;
    }

    private void OnBodyEntered(Node2D body) {
        if (body is Ball)
        {
            Delay.Start();
        }
    }

    private void KickBall()
    {
        foreach (RigidBody2D ball in DetectionZone.GetOverlappingBodies().Where(b => b is Ball))
        {
            ball.LinearVelocity = Vector2.Up * Strength;
        }
        OneWayGate.ProcessMode = ProcessModeEnum.Inherit;
    }
}
