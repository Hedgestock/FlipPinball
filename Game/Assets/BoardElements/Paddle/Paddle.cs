using Godot;
using System;

public partial class Paddle : CharacterBody2D
{
    [Export]
    Area2D UnstuckingArea;

    private void Unstuck(Node2D body)
    {
        if (!(body is Ball)) return;
        (body as Ball).LinearVelocity += Vector2.Down;
    }
}
