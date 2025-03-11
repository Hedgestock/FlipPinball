using Godot;
using System;

public partial class Pusher : Node
{
    [Export]
    uint Strength;

    public void Push(Ball ball, Vector2 direction)
    {
        ball.ApplyCentralImpulse(direction * Strength);
    }
}
