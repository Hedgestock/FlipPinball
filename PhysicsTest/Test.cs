using Godot;
using System;

public partial class Test : Node2D
{
    [Export]
    Godot.Collections.Array<Paddle> Paddles;
    public override void _Input(InputEvent @event)
    {
        base._Input(@event);
        if (@event is InputEventMouseButton eventMouseButton)
        {
            if (@event.IsActionPressed("screen_tap"))
            {
                Ball ball = GD.Load<PackedScene>("res://Game/Assets/Ball/Ball.tscn").Instantiate<Ball>();
                ball.GlobalPosition = @eventMouseButton.Position;
                AddChild(ball);
            }
            else if (@event.IsActionReleased("screen_tap"))
            {

                Ball ball = GD.Load<PackedScene>("res://Game/Assets/Ball/Ball.tscn").Instantiate<Ball>();
                foreach (Node2D child in ball.GetChildren())
                {
                    child.Scale /= 2;
                }
                ball.GetNode<Line2D>("Trail").Scale = Vector2.One;
                ball.GetNode<Line2D>("Trail").Width = 10;
                ball.Mass = .5f;
                ball.GlobalPosition = @eventMouseButton.Position;
                AddChild(ball);
            }
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        if (Input.IsActionPressed("paddle_left")) { RotatePaddle(delta, Mathf.DegToRad(-60)); }
        else { RotatePaddle(delta, 0); }


        
    }

    private void RotatePaddle(double delta, double angle)
    {

        foreach (var paddle in Paddles)
        {
            paddle.Rotation = (float)Mathf.RotateToward(paddle.Rotation, angle, delta * 25);
        }
    }
}
