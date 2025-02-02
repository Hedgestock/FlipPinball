using Godot;
using System;

public partial class Slingshot : Collidable
{
    [Export]
    Area2D Rubber;


    public override void CollideWithBall(Ball ball)
    {
        if (!IsOn || !Rubber.OverlapsBody(ball)) return;
        base.CollideWithBall(ball);

        Vector2 direction = Vector2.Up.Rotated(Rubber.GlobalRotation);
        GD.Print(Rubber.GlobalRotationDegrees);

        // Here we estimate the force of the impact by projecting the speed of the ball on the direction vector
        // It might not work that well if the ball hits almost tangentially to the bumper but it should be good enough
        if ((ball.LinearVelocity.Dot(direction) * direction).Length() < TriggerSpeed) return;
        ball.LinearVelocity += direction * Strength;

        Score();
    }
}
