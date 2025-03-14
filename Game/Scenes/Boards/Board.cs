using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Board : Node2D
{
    [Signal]
    public delegate void BoardTiltedEventHandler();

    [Export]
    Plunger Plunger;
    [Export]
    public Array<Paddle> PaddlesLeft = new();
    [Export]
    public Array<Paddle> PaddlesRight = new();
    [Export]
    OnOffLight SaveBallLight;
    [Export]
    SkillShot SkillShot;


    List<Ball> LiveBalls = new();

    Ball LoadedBall = null;

    // Debug code
    Vector2 LaunchPos;

    public override void _Ready()
    {
        base._Ready();
        ScoreManager.BoardScore = Score;

        if (OS.GetName() == "Android" || OS.GetName() == "iOS")
            lastAccel = Enumerable.Repeat(Input.GetAccelerometer().Length(), 50).ToArray();

        Callable.From(() =>
        {
            LoadedBall = GameManager.GetNextBall();
            LoadBall(LoadedBall, Plunger.GlobalPosition);
            GameManager.Instance.EmitSignal(GameManager.SignalName.LoadedBall, LoadedBall);
        }).CallDeferred();
    }

    public override void _Input(InputEvent @event)
    {
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
            if (@event.IsActionPressed("screen_tap") || @event.IsActionPressed("screen_tap_secondary"))
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
            else if (@event.IsActionReleased("screen_tap_secondary"))
            {
                Ball ball = GD.Load<PackedScene>("res://Game/Assets/Ball/Ball.tscn").Instantiate<Ball>();
                ball.GlobalPosition = LaunchPos;
                ball.LinearVelocity = (@eventMouseButton.Position - LaunchPos) * 10;
                //ball.AddChild(GD.Load<PackedScene>("res://Game/Assets/Ballterations/ScoreModifier/BumperAdder.tscn").Instantiate<ScoreModifier>());
                //ball.AddChild(GD.Load<PackedScene>("res://Game/Assets/Ballterations/ScoreModifier/BumperMultiplier.tscn").Instantiate<ScoreModifier>());
                //ball.AddChild(GD.Load<PackedScene>("res://Game/Assets/Ballterations/ScoreModifier/BumperSuperMultiplier.tscn").Instantiate<ScoreModifier>());
                //ball.AddChild(GD.Load<PackedScene>("res://Game/Assets/Ballterations/ScoreModifier/BumperSuperAdder.tscn").Instantiate<ScoreModifier>());
                //ball.AddChild(GD.Load<PackedScene>("res://Game/Assets/Ballterations/ScoreModifier/SlingshotAdder.tscn").Instantiate<ScoreModifier>());
                //ball.AddChild(GD.Load<PackedScene>("res://Game/Assets/Ballterations/ScoreModifier/BumperSlingshotAdder.tscn").Instantiate<ScoreModifier>());
                //ball.AddChild(GD.Load<PackedScene>("res://Game/Assets/Ballterations/ScoreModifier/RoundOrTargetAdder.tscn").Instantiate<ScoreModifier>());
                ball.AddChild(GD.Load<PackedScene>("res://Game/Assets/Ballterations/ScoreModifier/RoundAndTargetAdder.tscn").Instantiate<ScoreModifier>());
                AddLiveBall(ball);
            }
        }
    }

    float[] lastAccel;
    int tiltDisabled = 0;

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        if (Input.IsActionPressed("paddle_left"))
            foreach (var paddle in PaddlesLeft)
                paddle.Rotate(delta, Mathf.DegToRad(-60));
        else foreach (var paddle in PaddlesLeft)
                paddle.Rotate(delta, 0);

        if (Input.IsActionPressed("paddle_right"))
            foreach (var paddle in PaddlesRight)
                paddle.Rotate(delta, Mathf.DegToRad(60));
        else foreach (var paddle in PaddlesRight)
                paddle.Rotate(delta, 0);

        if (OS.GetName() == "Android" || OS.GetName() == "iOS")
        {
            float[] tmp = lastAccel;
            System.Array.Copy(tmp, 1, lastAccel, 0, lastAccel.Length - 1);
            lastAccel[lastAccel.Length - 1] = Input.GetAccelerometer().Length();

            if (tiltDisabled <= 0)
            {
                if (lastAccel.Max() - lastAccel.Min() > 10)
                {
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
    protected virtual int Score(int score)
    {
        return ScoreManager.Score(score);
    }

    protected virtual void PaddleAdditionnalBehaviour(bool left)
    {
        Array<Paddle> paddles = left ? PaddlesLeft : PaddlesRight;
        foreach (var paddle in paddles)
        {
            paddle.SoundPlayer.Play();
        }
    }

    void AddLiveBall(Ball ball)
    {
        LiveBalls.Add(ball);
        GameManager.Instance.EmitSignal(GameManager.SignalName.LiveBallsChanged, LiveBalls.ToArray());
        ball.SelfDestruct += () => DespawnBall(ball, DespawnType.SelfDestruct);
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
        GameManager.HeldBalls.Add(ball);
        RemoveLiveBall(ball);
        GameManager.Instance.EmitSignal(GameManager.SignalName.HeldBallsChanged, GameManager.HeldBalls.ToArray());
        CallDeferred(MethodName.LoadBall, ball.Duplicate(), Plunger.GlobalPosition);
        Plunger.AutoFire = true;
    }

    protected void ClearHeldBalls()
    {
        if (GameManager.HeldBalls.Count == 0) return;
        GameManager.HeldBalls.Clear();
        GameManager.Instance.EmitSignal(GameManager.SignalName.HeldBallsChanged, GameManager.HeldBalls.ToArray());
    }

    public enum DespawnType
    {
        Drain,
        OOB,
        SelfDestruct
    }

    void DespawnBall(Node2D body, DespawnType type)
    {
        if (body is Ball ball)
        {
            if (type == DespawnType.OOB) { GD.PrintErr($"Ball {ball} OOB"); }

            RemoveLiveBall(ball);

            if (LiveBalls.Count != 0) return;

            if (type == DespawnType.Drain && SaveBallLight.IsOnOrBlinking)
            {
                CallDeferred(MethodName.LoadBall, ball.Duplicate(), Plunger.GlobalPosition);
                SaveBallLight.TurnOff();
            }
            else
            {
                GameManager.Instance.EmitSignal(GameManager.SignalName.BoardReset);
            }
        }
    }

    [Export]
    Pusher TiltPusher;

    void Tilt()
    {
        float tiltAngle = (float)GD.RandRange(-MathF.PI / 4, MathF.PI / 4);
        Vector2 tiltDirection = Vector2.Down;

        if (Input.IsActionPressed("paddle_left") && Input.IsActionPressed("paddle_right"))
            tiltDirection = Vector2.Up;
        else if (Input.IsActionPressed("paddle_left"))
            tiltDirection = Vector2.Right;
        else if (Input.IsActionPressed("paddle_right"))
            tiltDirection = Vector2.Left;

        LiveBalls.ForEach(b => TiltPusher.Push(b, tiltDirection.Rotated(tiltAngle)));

        EmitSignal(SignalName.BoardTilted);
    }
}
