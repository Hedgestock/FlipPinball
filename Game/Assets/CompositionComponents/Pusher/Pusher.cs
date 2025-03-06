using Godot;
using System;

public partial class Pusher : Node
{
    [Export]
    uint Strength;

    private void Push(Ball ball, Vector2 direction)
    {
        ball.LinearVelocity += direction * Strength;
    }
}
