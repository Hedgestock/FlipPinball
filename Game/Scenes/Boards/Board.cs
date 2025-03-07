using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public partial class Board : Node2D
{
    [Export]
    float PaddleSpeed = 25;

    [Export]
    Node2D Plunger;
    [Export]
    public Array<Paddle> PaddlesLeft = new Array<Paddle>();
    [Export]
    public Array<Paddle> PaddlesRight = new Array<Paddle>();
    [Export]
    OnOffLight SaveBallLight;


    private List<Ball> LiveBalls = new List<Ball>();

    private Ball LoadedBall = null;

    // Debug code
    private Vector2 LaunchPos;

    public override void _Ready()
    {
        base._Ready();
        GetTree().CreateTimer(1).Timeout += () =>
        {
            GameManager.SetGame();
            LoadedBall = GameManager.GetNextBall();
        };
    }

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
                Ball ball = GD.Load<PackedScene>("res://Game/Assets/Ball/Ball.tscn").Instantiate<Ball>();
                ball.Position = LaunchPos;
                ball.LinearVelocity = (@eventMouseButton.Position - LaunchPos) * 10;
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
        LoadedBall.Position = Plunger.Position;
        LiveBalls.Add(LoadedBall);
        AddChild(LoadedBall);
    }

    protected void AddExtraBall()
    {
        GameManager.AddExtraBall(LoadedBall.Duplicate() as Ball);
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
        if (body is Ball ball)
        {
            if (oob) { GD.PrintErr($"Ball {ball} OOB"); }
            LiveBalls.Remove(ball);
            RemoveChild(ball);
            if (LiveBalls.Count != 0) return;
            if (SaveBallLight.IsOnOrBlinking)
            {
                CallDeferred(MethodName.LoadBall);
                SaveBallLight.TurnOff();
            }
            else
            {
                ball.QueueFree();
                LoadedBall = GameManager.GetNextBall();
            }

        }
    }

    private void Tilt()
    {
        Vector2 tiltDirection = Vector2.Up.Rotated((float)GD.RandRange(0, 2 * MathF.PI)) * 70;
        LiveBalls.ForEach(b => b.LinearVelocity += tiltDirection);
    }
}
