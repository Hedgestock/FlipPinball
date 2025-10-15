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
    OnOffLight OnOffLight;

    [Export]
    Timer MagnetEndTimer;

    [Export]
    int MagnetStrength = 6000;

    public override void _Ready()
    {
        base._Ready();
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        if (!OnOffLight.IsOn) return;
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

    private void OnMagnetZoneAreaEnter(Area2D area)
    {
        if (area.GetParent() is Ball ball)
        {
            ball.LinearVelocity = Vector2.Zero;
        }
    }

    private void MagnetEnd()
    {
        OnOffLight.TurnOff();
        foreach (Ball ball in EffectZone.GetOverlappingBodies().Where(b => b is Ball))
            ball.LinearDamp = 0;

        foreach (Ball ball in MagnetZone.GetOverlappingBodies().Where(b => b is Ball))
            EmitSignalMagnetEject(ball, Vector2.Left.Rotated((float)GD.RandRange(0, MathF.PI)));
    }
}
