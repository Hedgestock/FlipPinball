using Godot;
using System;
using System.Collections.Generic;
public partial class Bumper : Collidable
{
    public override void CollideWithBall(Ball ball)
    {
        if (!IsOn) return;
        base.CollideWithBall(ball);

        Vector2 direction = (ball.GlobalPosition - this.GlobalPosition).Normalized();

        // Here we estimate the force of the impact by projecting the speed of the ball on the direction vector
        // It might not work that well if the ball hits almost tangentially to the bumper but it should be good enough
        if ((ball.LinearVelocity.Dot(direction) * direction).Length() < TriggerSpeed) return;
        ball.LinearVelocity += direction * Strength;

        Score();
    }
}
