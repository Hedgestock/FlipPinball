using Godot;
using System;
using System.Linq;

public partial class KickBack : Node2D
{
    [Signal]
    public delegate void KickingBallEventHandler(Ball ball, Vector2 direction); 

    [Export]
    Area2D DetectionZone;
    [Export]
    Timer Delay;

    public override void _Ready()
    {
        base._Ready();
    }

    void OnBodyEntered(Node2D body) {
        if (body is Ball)
        {
            Delay.Start();
        }
    }

    void KickBall()
    {
        foreach (RigidBody2D ball in DetectionZone.GetOverlappingBodies().Where(b => b is Ball))
        {
            EmitSignal(SignalName.KickingBall, ball, Vector2.Up);
        }
    }
}
