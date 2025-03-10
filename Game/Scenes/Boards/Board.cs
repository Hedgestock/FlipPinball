using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public partial class Board : Node2D
{
    [Export]
    float PaddleSpeed = 25;

    [Export]
    Plunger Plunger;
    [Export]
    public Array<Paddle> PaddlesLeft = new();
    [Export]
    public Array<Paddle> PaddlesRight = new();
    [Export]
    OnOffLight SaveBallLight;


    private List<Ball> LiveBalls = new();

    private List<Ball> HeldBalls = new();

    private Ball LoadedBall = null;

    // Debug code
    private Vector2 LaunchPos;

    public override void _Ready()
    {
        base._Ready();
        GetTree().CreateTimer(1).Timeout += () =>
        {
            GameManager.SetGame();
        };
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("load_ball") && LiveBalls.Count == 0)
        {
            LoadedBall = GameManager.GetNextBall();
            GameManager.Instance.EmitSignal(GameManager.SignalName.LoadedBall, LoadedBall);
            LoadBall(LoadedBall, Plunger.Position);
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
                AddLiveBall(ball);
            }
        }
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

    public virtual void ResetBoard()
    {

    }

    void AddLiveBall(Ball ball)
    {
        LiveBalls.Add(ball);
        GameManager.Instance.EmitSignal(GameManager.SignalName.LiveBallsChanged, LiveBalls.ToArray());
        AddChild(ball);
    }

    void RemoveLiveBall(Ball ball)
    {
        LiveBalls.Remove(ball);
        GameManager.Instance.EmitSignal(GameManager.SignalName.LiveBallsChanged, LiveBalls.ToArray());
        RemoveChild(ball);
    }

    void LoadBall(Ball ball, Vector2 position)
    {
        if (HeldBalls.Contains(ball))
        {
            HeldBalls.Remove(ball);
            GameManager.Instance.EmitSignal(GameManager.SignalName.HeldBallsChanged, HeldBalls.ToArray());
        }
        ball.Position = position;
        ball.LinearVelocity = Vector2.Zero;
        AddLiveBall(ball);
    }

    protected void GiveExtraBall()
    {
        GameManager.AddExtraBall((Ball)LoadedBall.Duplicate());
    }


    protected void HoldBall(Ball ball)
    {
        HeldBalls.Add(ball);
        RemoveLiveBall(ball);
        GameManager.Instance.EmitSignal(GameManager.SignalName.HeldBallsChanged, HeldBalls.ToArray());
        CallDeferred(MethodName.LoadBall, (Ball)ball.Duplicate(), Plunger.Position);
        Plunger.AutoFire = true;
    }

    protected void ClearHeldBalls()
    {
        if (HeldBalls.Count == 0) return;
        HeldBalls.Clear();
        GameManager.Instance.EmitSignal(GameManager.SignalName.HeldBallsChanged, HeldBalls.ToArray());
    }

    private void OnEnterDrain(Node2D body, bool oob)
    {
        if (body is Ball ball)
        {
            if (oob) { GD.PrintErr($"Ball {ball} OOB"); }
            RemoveLiveBall(ball);
            if (LiveBalls.Count != 0) return;
            if (SaveBallLight.IsOnOrBlinking)
            {
                CallDeferred(MethodName.LoadBall, (Ball)ball.Duplicate(), Plunger.Position);
                Plunger.AutoFire = true;
                SaveBallLight.TurnOff();
            }
            else
            {
                ball.Remove();
                LoadedBall = GameManager.GetNextBall();
                GameManager.Instance.EmitSignal(GameManager.SignalName.LoadedBall, LoadedBall);
                LoadBall(LoadedBall, Plunger.Position);
            }
        }
    }

    private void Tilt()
    {
        Vector2 tiltDirection = Vector2.Up.Rotated((float)GD.RandRange(0, 2 * MathF.PI)) * 70;
        LiveBalls.ForEach(b => b.LinearVelocity += tiltDirection);
    }
}
