using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;

public partial class Board : Node2D
{
    [Signal]
    public delegate void BoardTiltedEventHandler();

    [Export]
    private AudioStream Music;

    [Export]
    public Array<Paddle> PaddlesLeft = new();
    [Export]
    public Array<Paddle> PaddlesRight = new();
    public CanvasLayer Tutorial;
    Plunger Plunger;
    OnOffLight SaveBallLight;
    OnOffLight ReplayBallLight;
    SkillShot SkillShot;

    List<Ball> LiveBalls = new();

    public Ball LoadedBall = null;

    // Debug code
    Vector2 LaunchPos;

    public override void _Ready()
    {
        base._Ready();

        ScoreManager.BoardScore = Score;

        Tutorial = (CanvasLayer)FindChild(nameof(Tutorial));
        Plunger = (Plunger)FindChild(nameof(Plunger));
        SaveBallLight = (OnOffLight)FindChild(nameof(SaveBallLight));
        ReplayBallLight = (OnOffLight)FindChild(nameof(ReplayBallLight));
        SkillShot = (SkillShot)FindChild(nameof(SkillShot));

        TiltPusher = (Pusher)FindChild(nameof(TiltPusher));

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
        if (@event.IsActionPressed("delete_live_balls"))
        {
            DeleteAllLiveBalls(DespawnType.SoftLockSafeGuard);
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

    protected void HoldLoadedBall()
    {
        GameManager.HeldBalls.Add((Ball)LoadedBall.Duplicate());
        GameManager.Instance.EmitSignal(GameManager.SignalName.HeldBallsChanged, GameManager.HeldBalls.ToArray());
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
        SelfDestruct,
        SoftLockSafeGuard
    }

    void DeleteAllLiveBalls(DespawnType type)
    {
        List<Ball> tmp = new(LiveBalls);
        foreach (Ball ball in tmp)
        {
            DespawnBall(ball, type);
        }
    }

    void DespawnBall(Node2D body, DespawnType type)
    {
        if (body is Ball ball)
        {
            if (type == DespawnType.OOB) { GD.PrintErr($"Ball {ball} OOB"); }

            RemoveLiveBall(ball);

            if (LiveBalls.Count != 0) return;

            if (type == DespawnType.Drain && (SaveBallLight.IsOnOrBlinking || ReplayBallLight.IsOnOrBlinking))
            {
                CallDeferred(MethodName.AddLiveBall, ball.Duplicate(), Plunger.GlobalPosition, true);
                SaveBallLight.TurnOff();
                ReplayBallLight.TurnOff();
            }
            else
            {
                GameManager.BallDiedHandler();
            }
        }
    }

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

    string MissionSelectMessage;
    string MissionSelectionFailedMessage;
    string MissionFailedMessage;

    protected Mission[] Missions;

    void InitMissions()
    {
        Missions = FindChild(nameof(Missions)).GetChildren().OfType<Mission>().ToArray();

        string tmp = Regex.Replace(GameManager.CurrentBoard.Name, "(?<!^)([A-Z])", "_$1").ToUpperInvariant();
        MissionSelectMessage = "MISSION_SELECT_" + tmp;
        MissionSelectionFailedMessage = "MISSION_SELECTION_FAILED_" + tmp;
        MissionFailedMessage = "MISSION_FAILED_" + tmp;



        foreach (Mission mission in Missions)
        {
            mission.Connect(Mission.SignalName.Completed, Callable.From(EndMission));
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

    protected void FailMission()
    {
        if (CurrentMission == null) return;
        if (!IsMissionActive)
        {
            StatusManager.Instance.EmitSignal(StatusManager.SignalName.MissionStatusChanged, MissionSelectionFailedMessage);
            CurrentMission = null;
            return;
        }
        StatusManager.Instance.EmitSignal(StatusManager.SignalName.MissionStatusChanged, MissionFailedMessage);
        EndMission();
    }

    protected virtual void EndMission()
    {
        if (CurrentMission == null || !IsMissionActive) return;
        CurrentMission = null;
        IsMissionActive = false;
    }
}
