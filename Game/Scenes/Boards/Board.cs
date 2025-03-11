using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using static System.Net.Mime.MediaTypeNames;

public partial class Board : Node2D
{
    [Signal]
    public delegate void BoardTiltedEventHandler();

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


    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("load_ball") && LiveBalls.Count == 0)
        {
            LoadedBall = GameManager.GetNextBall();
            GameManager.Instance.EmitSignal(GameManager.SignalName.LoadedBall, LoadedBall);
            LoadBall(LoadedBall, Plunger.GlobalPosition);
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

        //return;
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
                ball.GlobalPosition = LaunchPos;
                ball.LinearVelocity = (@eventMouseButton.Position - LaunchPos) * 10;
                AddLiveBall(ball);
            }
        }
    }

    float[] lastAccel = new float[50];
    int tiltDisabled = 0;

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        if (Input.IsActionPressed("paddle_left")) { RotatePaddle(delta, Mathf.DegToRad(-60), true); }
        else { RotatePaddle(delta, 0, true); }

        if (Input.IsActionPressed("paddle_right")) { RotatePaddle(delta, Mathf.DegToRad(60), false); }
        else { RotatePaddle(delta, 0, false); }

        if (OS.GetName() == "Android" || OS.GetName() == "iOS")
        {
            float[] tmp = lastAccel;
            System.Array.Copy(tmp, 1, lastAccel, 0, lastAccel.Length - 1);
            lastAccel[lastAccel.Length - 1] = Input.GetAccelerometer().Length();

            if (tiltDisabled <= 0)
            {
                if (lastAccel.Max() - lastAccel.Min() > 10)
                {
                    GD.Print("tilting");
                    Tilt();
                }
                tiltDisabled = 240;
            }
            else
            {
                tiltDisabled--;
            }
        }


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

    void TelportLiveBall(Ball ball, Vector2 destination)
    {
        RemoveLiveBall(ball);
        CallDeferred(MethodName.LoadBall, ball.Duplicate(), destination);
    }

    void LoadBall(Ball ball, Vector2 position)
    {
        ball.GlobalPosition = position;
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
        CallDeferred(MethodName.LoadBall, ball.Duplicate(), Plunger.GlobalPosition);
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
                CallDeferred(MethodName.LoadBall, ball.Duplicate(), Plunger.GlobalPosition);
                Plunger.AutoFire = true;
                SaveBallLight.TurnOff();
            }
            else
            {
                LoadedBall = GameManager.GetNextBall();
                GameManager.Instance.EmitSignal(GameManager.SignalName.LoadedBall, LoadedBall);
                CallDeferred(MethodName.LoadBall, LoadedBall, Plunger.GlobalPosition);
            }
        }
    }

    private void Tilt()
    {
        float tiltAngle = (float)GD.RandRange(-MathF.PI / 4, MathF.PI / 4);
        Vector2 tiltDirection = Vector2.Down;

        if (Input.IsActionPressed("paddle_left") && Input.IsActionPressed("paddle_right"))
            tiltDirection = Vector2.Up;
        else if (Input.IsActionPressed("paddle_left"))
            tiltDirection = Vector2.Right;
        else if (Input.IsActionPressed("paddle_right"))
            tiltDirection = Vector2.Left;

        LiveBalls.ForEach(b => b.LinearVelocity += tiltDirection.Rotated(tiltAngle) * 70);

        EmitSignal(SignalName.BoardTilted);
    }
}
