using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

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
    public delegate void BallQueueChangedEventHandler(Array<Ball> balls);

    [Signal]
    public delegate void HeldBallsChangedEventHandler(Array<Ball> balls);

    [Signal]
    public delegate void LiveBallsChangedEventHandler(Array<Ball> balls);

    [Signal]
    public delegate void LoadedBallEventHandler(Ball ball);

    private static LinkedList<Ball> BallQueue = new();

    public static void SetGame()
    {
        for (int i = 0; i < 3; i++)
        {
            Ball ball = GD.Load<PackedScene>("res://Game/Assets/Ball/Ball.tscn").Instantiate<Ball>();
            ball.GetNode<Sprite2D>("Sprite2D").Modulate = new(new Color(GD.Randi()), 1);
            //ball.GetNode<Line2D>("Trail").Modulate = new(new Color(GD.Randi()), 1);
            BallQueue.AddLast(ball);
        }
    }

    public static Ball GetNextBall()
    {
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

}
