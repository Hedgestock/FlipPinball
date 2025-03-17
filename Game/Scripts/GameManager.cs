using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public partial class GameManager : Node
{
    protected static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    public GameManager()
    {
        if (_instance != null)
            return;
        _instance = this;
    }

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

    public static int CurrentLevel;
    public static long TargetScore { get { return 200000 * (long)Math.Pow((CurrentLevel + 1), (CurrentLevel + 1f) / 2); } }
    public static PackedScene CurrentBoard;
    public static LinkedList<Ball> BallQueue;
    public static List<Ball> HeldBalls;

    public static void SetGame()
    {
        BallQueue = new();
        HeldBalls = new();
        CurrentLevel = 1;
        CurrentBoard = GD.Load<PackedScene>("res://Game/Scenes/Boards/TestLab/TestLab.tscn");

        ScoreManager.ScoreValue = 0;
        ScoreManager.TotalScoreValue = 0;

        for (int i = 0; i < 3; i++)
        {
            Ball ball = GD.Load<PackedScene>("res://Game/Assets/Ball/Ball.tscn").Instantiate<Ball>();
            ball.GetNode<Sprite2D>("Sprite2D").Modulate = new(new Color(GD.Randi()), 1);
            ball.GetNode<Line2D>("Trail").Modulate = new(new Color(GD.Randi()), 1);
            BallQueue.AddLast(ball);
        }
    }

    public static void BallDiedHandler()
    {
        if (ScoreManager.ScoreValue < TargetScore)
            Instance.EmitSignal(SignalName.NewBall);

        else
        {
            CurrentLevel++;
            ScoreManager.ScoreValue = 0;
            Instance.EmitSignal(SignalName.LevelCleared);
        }
    }

    public static Ball GetNextBall()
    {
        if (BallQueue.Count == 0)
        {
            Instance.EmitSignal(SignalName.GameOver);
            SceneManager.Instance.CallDeferred(SceneManager.MethodName.ChangeSceneToFile, "res://Game/Scenes/Home.tscn");
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

    protected void ClearHeldBalls()
    {
        if (HeldBalls.Count == 0) return;
        HeldBalls.Clear();
        Instance.EmitSignal(SignalName.HeldBallsChanged, HeldBalls.ToArray());
    }
}
