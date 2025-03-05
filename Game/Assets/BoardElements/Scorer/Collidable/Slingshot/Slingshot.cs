using Godot;
using System;

public partial class Slingshot : Node2D
{
    [Export]
    private uint Strength = 1000;
    [Export]
    Hitbox Hitbox;

    public void BumpBall(Ball ball)
    {
        //if (!IsOn) return;

        Vector2 direction = Vector2.Up.Rotated(Hitbox.GlobalRotation);

        ball.LinearVelocity += direction * Strength;
    }
}
