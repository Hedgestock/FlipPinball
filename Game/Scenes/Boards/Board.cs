using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public partial class Board : Node2D
{
    [Export]
    PackedScene Ball;

    [Export]
    float PaddleSpeed = 25;

    [Export]
    Node2D Plunger;
    [Export]
    public Array<Paddle> PaddlesLeft = new Array<Paddle>();
    [Export]
    public Array<Paddle> PaddlesRight = new Array<Paddle>();

    private List<RigidBody2D> LiveBalls = new List<RigidBody2D>();

    private Vector2 LaunchPos;

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("load_ball"))
        {
            LoadBall();
        }
        if (@event.IsActionPressed("paddle_left"))
        {
            PaddleAdditionnalBehaviour(true);
        }
        if (@event.IsActionPressed("paddle_right"))
        {
            PaddleAdditionnalBehaviour(false);
        }
        if (@event.IsActionPressed("tilt"))
        {
            Tilt();
        }

        // This is for testing purposes
        if (@event is InputEventMouseButton eventMouseButton)
        {
            if (@event.IsActionPressed("screen_tap"))
            {
                LaunchPos = @eventMouseButton.Position;
            }
            else if (@event.IsActionReleased("screen_tap"))
            {
                RigidBody2D ball = Ball.Instantiate<RigidBody2D>();
                ball.Position = LaunchPos;
                ball.LinearVelocity = (@eventMouseButton.Position - LaunchPos) * 10;
                LiveBalls.Add(ball);
                AddChild(ball);
            }
            else
            {
                RigidBody2D ball = Ball.Instantiate<RigidBody2D>();
                ball.Position = @eventMouseButton.Position;
                ball.SetCollisionLayerValue(3, true);
                ball.SetCollisionMaskValue(3, true);
                ball.SetCollisionLayerValue(2, false);
                ball.SetCollisionMaskValue(2, false);
                LiveBalls.Add(ball);
                AddChild(ball);
            }
        }
    }

    public virtual void ResetBoard()
    {

    }

    private void LoadBall()
    {
        if (LiveBalls.Count != 0) return;
        RigidBody2D ball = Ball.Instantiate<RigidBody2D>();
        ball.Position = Plunger.Position;
        LiveBalls.Add(ball);
        AddChild(ball);
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        if (Input.IsActionPressed("paddle_left")) { RotatePaddle(delta, Mathf.DegToRad(-60), true); }
        else { RotatePaddle(delta, 0, true); }

        if (Input.IsActionPressed("paddle_right")) { RotatePaddle(delta, Mathf.DegToRad(60), false); }
        else { RotatePaddle(delta, 0, false); }
    }

    private void RotatePaddle(double delta, double angle, bool left)
    {
        Array<Paddle> paddles = left ? PaddlesLeft : PaddlesRight;

        foreach (var paddle in paddles)
        {
            paddle.Rotation = (float)Mathf.RotateToward(paddle.Rotation, angle, delta * PaddleSpeed);
        }
    }

    protected virtual void PaddleAdditionnalBehaviour(bool left)
    {
        Array<Paddle> paddles = left ? PaddlesLeft : PaddlesRight;
        foreach (var paddle in paddles)
        {
            paddle.SoundPlayer.Play();
        }
    }

    private void OnEnterDrain(Node2D body, bool oob)
    {
        if (!(body is Ball)) return;

        if (oob) { GD.PrintErr($"Ball {body} OOB"); }
        LiveBalls.Remove(body as Ball);
        body.QueueFree();
    }

    private void Tilt()
    {
        Vector2 tiltDirection = Vector2.Up.Rotated((float)GD.RandRange(0, 2 * MathF.PI)) * 70;
        LiveBalls.ForEach(b => b.LinearVelocity += tiltDirection);
    }
}
