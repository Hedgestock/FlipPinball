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
    private AudioStream Music;

    [Export]
    public CanvasLayer Tutorial;

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

    public Ball LoadedBall = null;

    // Debug code
    Vector2 LaunchPos;

    public override void _Ready()
    {
        base._Ready();

        ScoreManager.BoardScore = Score;

        InitMissions();

        if (OS.GetName() == "Android" || OS.GetName() == "iOS")
            lastAccel = Enumerable.Repeat(Input.GetAccelerometer().Length(), 50).ToArray();

        AudioManager.PlayMusic(Music);

        CallDeferred(MethodName.LoadBall);
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
                ball.Modulate = Colors.White;
                ball.GetNode<Line2D>("Trail").Modulate = Colors.White;
                AddLiveBall(ball, LaunchPos, false);
            }
            else if (@event.IsActionReleased("screen_tap_secondary"))
            {
                Ball ball = GD.Load<PackedScene>("res://Game/Assets/Ball/Ball.tscn").Instantiate<Ball>();
                ball.GlobalPosition = LaunchPos;
                ball.LinearVelocity = (@eventMouseButton.Position - LaunchPos) * 10;
                Ballteration bt = new();
                bt.AddChild(GD.Load<PackedScene>("res://Game/Assets/Ballterations/Effects/ScoreModifier/Tests/GlobalAdder.tscn").Instantiate<ScoreModifier>());
                ball.AddChild(bt);
                AddLiveBall(ball, LaunchPos, false);
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
                paddle.Flip(delta);
        else foreach (var paddle in PaddlesLeft)
                paddle.Return(delta);

        if (Input.IsActionPressed("paddle_right"))
            foreach (var paddle in PaddlesRight)
                paddle.Flip(delta);
        else foreach (var paddle in PaddlesRight)
                paddle.Return(delta);

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

    void LoadBall()
    {
        LoadedBall = GameManager.GetNextBall();
        if (LoadedBall == null) return;

        AddLiveBall((Ball)LoadedBall.Duplicate(), Plunger.GlobalPosition);
        GameManager.Instance.EmitSignal(GameManager.SignalName.LoadedBall, LoadedBall);
    }

    void AddLiveBall(Ball ball, Vector2 position, bool inert = true)
    {
        if (inert)
        {
            ball.LinearVelocity = Vector2.Zero;
            ball.AngularVelocity = 0;
        }

        ball.GlobalPosition = position;

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

    void TeleportLiveBall(Ball ball, Vector2 destination, bool inert = true)
    {
        RemoveLiveBall(ball);
        CallDeferred(MethodName.AddLiveBall, ball.Duplicate(), destination, inert);
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
        CallDeferred(MethodName.AddLiveBall, ball.Duplicate(), Plunger.GlobalPosition, true);
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
                CallDeferred(MethodName.AddLiveBall, ball.Duplicate(), Plunger.GlobalPosition, true);
                SaveBallLight.TurnOff();
            }
            else
            {
                GameManager.BallDiedHandler();
            }
        }
    }

    [Export]
    Pusher TiltPusher;

    void Tilt()
    {
        float tiltAngle = (float)GD.RandRange(-MathF.PI / 4, MathF.PI / 4);
        Vector2 tiltDirection = Vector2.Up;

        //if (Input.IsActionPressed("paddle_left") && Input.IsActionPressed("paddle_right"))
        //    tiltDirection = Vector2.Up;
        //else
        if (Input.IsActionPressed("paddle_left"))
            tiltDirection = Vector2.Right;
        else if (Input.IsActionPressed("paddle_right"))
            tiltDirection = Vector2.Left;

        LiveBalls.ForEach(b => TiltPusher.Push(b, tiltDirection.Rotated(tiltAngle)));

        EmitSignalBoardTilted();
    }

    [Export]
    string MissionSelectMessage;
    [Export]
    Node MissionContainer;

    protected Mission[] Missions;

    void InitMissions()
    {
        Missions = MissionContainer.GetChildren().OfType<Mission>().ToArray();

        foreach (Mission mission in Missions)
        {
            mission.Connect(Mission.SignalName.Completed, Callable.From(EndMission));
            foreach (MissionGoal goal in mission.AllGoals)
            {
                goal.Connect(MissionGoal.SignalName.Updated, Callable.From(mission.GoalUpdated));
                goal.Connect(MissionGoal.SignalName.Completed, Callable.From(mission.GoalCompleted));
            }
        }
    }

    protected bool IsMissionActive = false;
    protected Mission CurrentMission = null;
    protected virtual void SelectMission()
    {
        if (CurrentMission == null || IsMissionActive) return;
        StatusManager.Instance.EmitSignal(StatusManager.SignalName.MissionChanged, CurrentMission.MissionName);
        StatusManager.Instance.EmitSignal(StatusManager.SignalName.MissionStatusChanged, MissionSelectMessage);
    }

    protected virtual void AcceptMission()
    {
        if (CurrentMission == null || IsMissionActive) return;
        IsMissionActive = true;
        CurrentMission.Init();
    }

    protected virtual void EndMission()
    {
        if (CurrentMission == null || !IsMissionActive) return;
        CurrentMission = null;
        IsMissionActive = false;
    }
}
