using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public partial class GameManager : Node
{
    [Signal]
    public delegate void GameOverEventHandler();
    [Signal]
    public delegate void LevelClearedEventHandler();
    [Signal]
    public delegate void NewBallEventHandler();
    [Signal]
    public delegate void BallQueueChangedEventHandler();
    [Signal]
    public delegate void HeldBallsChangedEventHandler(Array<Ball> balls);
    [Signal]
    public delegate void LiveBallsChangedEventHandler(Array<Ball> balls);
    [Signal]
    public delegate void LoadedBallEventHandler(Ball ball);

    protected static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    public GameManager()
    {
        if (_instance != null)
            return;
        _instance = this;
    }

    public override void _Ready()
    {
        base._Ready();
        ProcessMode = ProcessModeEnum.Always;
    }

    public static int CurrentLevel;
    public static long Debt = 0;
    public static long TargetScore { get { return 20/*0000*/ * (long)Math.Pow((CurrentLevel + 1), (CurrentLevel + 1f) / 2) - Debt; } }
    public static Board CurrentBoard;
    public static LinkedList<Ball> BallQueue;
    public static List<Ball> HeldBalls;

    public static void SetGame()
    {
        BallQueue = new();
        HeldBalls = new();
        CurrentLevel = 1;

        ScoreManager.ScoreValue = 0;
        ScoreManager.TotalScoreValue = 0;

        for (int i = 0; i < 3; i++)
        {
            AddExtraBall(Ball.GetFreshBall());
        }
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("pause"))
        {
            //GetTree().Paused = !GetTree().Paused;
            if (CurrentBoard != null)
                CurrentBoard.Tutorial.Visible = !CurrentBoard.Tutorial.Visible;
        }
    }

    public static void BallDiedHandler()
    {
        if (ScoreManager.ScoreValue < TargetScore)
            Instance.EmitSignal(SignalName.NewBall);
        else
        {
            Instance.EmitSignal(SignalName.LevelCleared);
            CurrentLevel++;
            ScoreManager.ScoreValue = 0;
        }
    }

    public static Ball GetNextBall()
    {
        if (BallQueue.Count == 0)
        {
            Instance.EmitSignal(SignalName.GameOver);
            SceneManager.Instance.CallDeferred(SceneManager.MethodName.ChangeSceneToFile, "res://Game/Scenes/Home.tscn");
            return null;
        }
        Ball ball = BallQueue.First.Value;
        BallQueue.RemoveFirst();
        Instance.EmitSignal(SignalName.BallQueueChanged);
        return ball;
    }

    public static void AddExtraBall(Ball ball, bool enqueue = false)
    {
        if (enqueue)
            BallQueue.AddLast(ball);
        else
            BallQueue.AddFirst(ball);
        Instance.EmitSignal(SignalName.BallQueueChanged);
    }

    void ClearHeldBalls()
    {
        if (HeldBalls.Count == 0) return;
        HeldBalls.Clear();
        Instance.EmitSignal(SignalName.HeldBallsChanged, HeldBalls.ToArray());
    }
}
