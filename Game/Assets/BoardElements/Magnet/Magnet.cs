using Godot;
using System;
using System.Linq;

public partial class Magnet : Node2D
{
    [Export]
    Area2D EffectZone;
    CircleShape2D EffectShape;

    [Export]
    Area2D MagnetZone;

    [Export]
    Timer MagnetEnd;

    [Export]
    int MagnetStrength = 6000;
    [Export]
    int EjectionStrength = 2000;

    public bool IsOn = true;

    public override void _Ready()
    {
        base._Ready();
        EffectShape = EffectZone.GetNode<CollisionShape2D>("CollisionShape2D").Shape as CircleShape2D;
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        if (!IsOn) return;
        foreach (Ball ball in EffectZone.GetOverlappingBodies().Where(b => b is Ball))
        {
            Vector2 vecToCenter = GlobalPosition - ball.GlobalPosition;
            Vector2 pullForce = vecToCenter.Normalized() * MagnetStrength;
            ball.LinearVelocity += pullForce * (float)delta;
        }

        if (MagnetEnd.IsStopped() && MagnetZone.GetOverlappingBodies().Where(b => b is Ball).Count() > 0)
        {
            MagnetEnd.Start();
        }
        else if (!MagnetEnd.IsStopped() && MagnetZone.GetOverlappingBodies().Where(b => b is Ball).Count() <= 0)
        {
            MagnetEnd.Stop();
        }
    }

    private void OnEffectZoneBodyEnter(Node2D body)
    {
        if (IsOn && body is Ball ball)
        {
            ball.LinearDamp = 2f;
        }
    }

    private void OnEffectZoneBodyExit(Node2D body)
    {
        if (IsOn && body is Ball ball)
        {
            ball.LinearDamp = 0;
        }
    }

    private void OnMagnetEnd()
    {
        if (!IsOn) return;
        IsOn = false;
        foreach (Ball ball in MagnetZone.GetOverlappingBodies().Where(b => b is Ball))
        {
            Vector2 ejection = Vector2.Left.Rotated((float)GD.RandRange(0, MathF.PI)) * EjectionStrength;
            ball.LinearVelocity = ejection;
        }
    }
}
