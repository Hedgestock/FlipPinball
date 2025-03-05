using Godot;
using System;

public partial class Paddle : CharacterBody2D
{
    [Export]
    Area2D UnstuckingArea;

    [Export]
    public AudioStreamPlayer SoundPlayer;

    private void Unstuck(Node2D body)
    {
        if (body is Ball ball)
        {
            ball.LinearVelocity += Vector2.Down;
        }
    }
}
