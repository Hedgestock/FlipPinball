using Godot;
using System;

public partial class Target : Node2D
{

    [Export]
    uint Strength = 500;

    [Export]
    Hitbox Hitbox;

    [Export]
    protected Timer ResetDelay;

    public void BumpBall(Ball ball)
    {
        Vector2 direction = Vector2.Down.Rotated(Hitbox.GlobalRotation);

        ball.LinearVelocity += direction * Strength;

        ResetDelay.Start();
    }
}
