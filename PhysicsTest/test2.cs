using Godot;
using System;

public partial class test2 : Area2D
{
    public override void _Ready()
    {
        base._Ready();
        GetNode<Label>("Label").GlobalPosition = GlobalPosition;
    }
    public override void _Input(InputEvent @event)
    {
        base._Input(@event);
        if (@event.IsActionPressed("plunger"))
        {
            foreach (Ball ball in GetOverlappingBodies())
            {
                ball.LinearVelocity += Vector2.Up * 1000;
                GD.Print(ball.LinearVelocity);
            }
        }
    }
}
