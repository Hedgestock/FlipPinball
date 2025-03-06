using Godot;
using System;
using System.Linq;

public partial class Spitter : Node2D
{
    [Signal]
    public delegate void SwallowingBallEventHandler(Ball ball);

    [Signal]
    public delegate void SpittingBallEventHandler(Ball ball);

    [Export]
    uint Strength = 1000;
    [Export]
    Timer SpitDelay;
    [Export]
    RayCast2D SpitDirection;
    [Export]
    Area2D DetectionZone;


    private void OnBodyEnter(Node2D body)
    {
        //if (!IsOn || !(body is Ball)) return;
        if (body is Ball ball)
        {
            Tween tween = GetTree().CreateTween();
            tween.TweenProperty(ball, "global_position", GlobalPosition, .5f)
                .SetEase(Tween.EaseType.Out)
                .SetTrans(Tween.TransitionType.Elastic);
            ball.SetDeferred(RigidBody2D.PropertyName.Freeze, true);
            SpitDelay.Start();
            //IsOn = false;
            EmitSignal(SignalName.SwallowingBall, ball);
        }
    }

    private void SpitBall()
    {
        foreach (RigidBody2D ball in DetectionZone.GetOverlappingBodies().Where(b => b is Ball))
        {
            ball.SetDeferred(RigidBody2D.PropertyName.Freeze, false);
            ball.LinearVelocity = (SpitDirection.TargetPosition - SpitDirection.Position).Normalized() * Strength;

            EmitSignal(SignalName.SpittingBall, ball);
        }
        //IsOn = true;
    }
}
