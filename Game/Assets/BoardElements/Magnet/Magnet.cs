using Godot;
using System;
using System.Linq;

public partial class Magnet : Node2D
{
    [Signal]
    public delegate void MagnetEjectEventHandler(Ball ball, Vector2 direction);

    [Export]
    Area2D EffectZone;

    [Export]
    Area2D MagnetZone;

    [Export]
    Timer MagnetEndTimer;

    [Export]
    int MagnetStrength = 6000;

    bool IsOn = false;

    public override void _Ready()
    {
        base._Ready();
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

        if (MagnetEndTimer.IsStopped() && MagnetZone.GetOverlappingBodies().Where(b => b is Ball).Count() > 0)
        {
            MagnetEndTimer.Start();
        }
        else if (!MagnetEndTimer.IsStopped() && MagnetZone.GetOverlappingBodies().Where(b => b is Ball).Count() <= 0)
        {
            MagnetEndTimer.Stop();
        }
    }

    private void TurnOn()
    {
        IsOn = true;
        EffectZone.GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred(CollisionShape2D.PropertyName.Disabled, false);
        MagnetZone.GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred(CollisionShape2D.PropertyName.Disabled, false);
    }

    private void TurnOff()
    {
        IsOn = false;
        EffectZone.GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
        MagnetZone.GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
    }

    private void OnEffectZoneBodyEnter(Node2D body)
    {
        if (body is Ball ball)
        {
            ball.LinearDamp = 2f;
        }
    }

    private void OnEffectZoneBodyExit(Node2D body)
    {
        if (body is Ball ball)
        {
            ball.LinearDamp = 0;
        }
    }

    private void MagnetEnd()
    {
        TurnOff();
        foreach (Ball ball in MagnetZone.GetOverlappingBodies().Where(b => b is Ball))
        {
            ball.LinearDamp = 0;
            EmitSignal(SignalName.MagnetEject, ball, Vector2.Left.Rotated((float)GD.RandRange(0, MathF.PI)));
        }
    }
}
