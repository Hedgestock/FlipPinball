using Godot;
using Godot.Collections;
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
    public delegate void NewBallEventHandler();
    [Signal]
    public delegate void BallQueueChangedEventHandler(Array<Ball> balls);
    [Signal]
    public delegate void HeldBallsChangedEventHandler(Array<Ball> balls);
    [Signal]
    public delegate void LiveBallsChangedEventHandler(Array<Ball> balls);
    [Signal]
    public delegate void LoadedBallEventHandler(Ball ball);

    static int CurrentLevel;
    public static PackedScene CurrentBoard;
    //static int 
    //public static int CurrentLevel { get { return _currentLevel; } }
    public static LinkedList<Ball> BallQueue;
    public static List<Ball> HeldBalls = new();

    public static void SetGame()
    {
        BallQueue = new();
        CurrentLevel = 1;
        CurrentBoard = GD.Load<PackedScene>("res://Game/Scenes/Boards/TestLab/TestLab.tscn");

        ScoreManager.ScoreValue = 0;

        for (int i = 0; i < 3; i++)
        {
            Ball ball = GD.Load<PackedScene>("res://Game/Assets/Ball/Ball.tscn").Instantiate<Ball>();
            ball.GetNode<Sprite2D>("Sprite2D").Modulate = new(new Color(GD.Randi()), 1);
            ball.GetNode<Line2D>("Trail").Modulate = new(new Color(GD.Randi()), 1);
            BallQueue.AddLast(ball);
        }
    }

    public static Ball GetNextBall()
    {
        //if (BallQueue.Count == 0)
        //{
        //    Instance.EmitSignal(SignalName.GameOver);
        //}
        Ball ball = BallQueue.First.Value;
        BallQueue.RemoveFirst();
        Instance.EmitSignal(SignalName.BallQueueChanged, new Array<Ball>(BallQueue));
        return ball;
    }

    public static void AddExtraBall(Ball ball)
    {
        BallQueue.AddFirst(ball);
        Instance.EmitSignal(SignalName.BallQueueChanged, new Array<Ball>(BallQueue));
    }

    protected void ClearHeldBalls()
    {
        if (HeldBalls.Count == 0) return;
        HeldBalls.Clear();
        Instance.EmitSignal(SignalName.HeldBallsChanged, HeldBalls.ToArray());
    }
}
