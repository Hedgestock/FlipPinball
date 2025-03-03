using Godot;
using System;

public partial class RoundTarget : Target
{
    public override void CollideWithBall(Ball ball)
    {
        if (!IsOn) return;
        base.CollideWithBall(ball);

        Vector2 direction = Vector2.Down.Rotated(TargetElement.GlobalRotation);

        ball.LinearVelocity += direction * Strength;
    }
}
