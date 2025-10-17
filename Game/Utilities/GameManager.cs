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

    [Signal]
    public delegate void CreditsChangedEventHandler();

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
    static long _credits = 0;
    public static long Credits
    {
        get { return _credits; }
        set
        {
            _credits = value;
            Instance.EmitSignalCreditsChanged();
        }
    }
    public static long TargetScore { get { return 200_000 * (long)Math.Pow((CurrentLevel + 1), (CurrentLevel + 1f) / 2f) - Math.Min(_credits, 0); } }
    public static Board CurrentBoard;
    public static LinkedList<Ball> BallQueue;
    public static List<Ball> HeldBalls;

    public static void SetGame()
    {
        BallQueue = new();
        HeldBalls = new();
        CurrentLevel = 1;
        Credits = 0;

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
            GetTree().Paused = !GetTree().Paused;
            if (CurrentBoard != null)
                CurrentBoard.Tutorial.Visible = !CurrentBoard.Tutorial.Visible;
        }
    }

    public static void BallDiedHandler()
    {
        if (ScoreManager.ScoreValue < TargetScore)
            Instance.EmitSignalNewBall();
        else
            Instance.EmitSignalLevelCleared();
    }

    public static void AdvanceLevel()
    {
        CurrentLevel++;
        ScoreManager.ScoreValue = 0;
    }

    public static Ball GetNextBall()
    {
        if (BallQueue.Count == 0)
        {
            Instance.EmitSignalGameOver();
            AudioManager.StopMusic();
            SceneManager.Instance.CallDeferred(SceneManager.MethodName.ChangeSceneToFile, "uid://b8iu65a2xswru");
            return null;
        }
        Ball ball = BallQueue.First.Value;
        BallQueue.RemoveFirst();
        Instance.EmitSignalBallQueueChanged();
        return ball;
    }

    public static void AddExtraBall(Ball ball, bool enqueue = false)
    {
        if (enqueue)
            BallQueue.AddLast(ball);
        else
            BallQueue.AddFirst(ball);
        Instance.EmitSignalBallQueueChanged();
    }
}
