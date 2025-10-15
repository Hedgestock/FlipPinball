using Godot;
using System;
using System.Linq;

public partial class Spitter : Node2D
{
    [Signal]
    public delegate void SwallowingBallEventHandler(Ball ball);

    [Signal]
    public delegate void SpittingBallEventHandler(Ball ball, Vector2 direction);

    [Export]
    Timer SpitDelay;
    [Export]
    RayCast2D SpitDirection;
    [Export]
    Area2D DetectionZone;

    Ball StoredBall;


    private void OnAreaEnter(Area2D area)
    {
        if (area.GetParent() is Ball ball && ball != StoredBall)
        {
            Tween tween = GetTree().CreateTween();
            tween.TweenProperty(ball, "global_position", GlobalPosition, .5f)
                .SetEase(Tween.EaseType.Out)
                .SetTrans(Tween.TransitionType.Elastic);
            ball.SetDeferred(RigidBody2D.PropertyName.Freeze, true);
            SpitDelay.Start();
            EmitSignalSwallowingBall(ball);
            StoredBall = ball;
        }
    }

    private void SpitBall()
    {
        foreach (Ball ball in DetectionZone.GetOverlappingBodies().Where(b => b is Ball))
        {
            ball.SetDeferred(RigidBody2D.PropertyName.Freeze, false);
            ball.LinearVelocity = Vector2.Zero;
            CallDeferred(GodotObject.MethodName.EmitSignal, SignalName.SpittingBall, ball, SpitDirection.TargetPosition.Normalized());
        }
        StoredBall = null;
    }
}
