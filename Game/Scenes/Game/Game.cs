using Godot;
using Godot.Collections;
using System;
using System.Linq;

public partial class Game : Node
{
    [Export]
    private PackedScene BallViewerScene;

    [Export]
    BoxContainer MainContainer;

    [Export]
    private HFlowContainer BallQueue;
    [Export]
    private HFlowContainer HeldBalls;
    [Export]
    private HFlowContainer LiveBalls;
    [Export]
    private BallViewer LoadedBallViewer;


    [Export]
    private Label Score;
    [Export]
    private Label History;
    [Export]
    VBoxContainer StatusBox;
    [Export]
    private Label FPS;

    [Export]
    PackedScene ScoreBubbleScene;
    public override void _Ready()
    {
        GetTree().Root.SizeChanged += OnScreenResize;
        ScoreManager.Instance.Scoring += UpdateScore;
        //GameManager.Instance.LoadedBall += UpdateLoadedBall;
        //GameManager.Instance.BallQueueChanged += (balls) => UpdateBallList(balls, BallQueue);
        //GameManager.Instance.HeldBallsChanged += (balls) => UpdateBallList(balls, HeldBalls);
        //GameManager.Instance.LiveBallsChanged += (balls) => UpdateBallList(balls, LiveBalls);

        GameStart = DateTime.Now;

        //OnScreenResize();
        //GetTree().Root.SizeChanged += OnScreenResize;

        base._Ready();
    }

    [Export]
    private Label GameTimerLabel;
    private Label BallTimerLabel;
    private DateTime GameStart;
    private DateTime BallStart;

    public override void _Process(double delta)
    {
        base._Process(delta);
        GameTimerLabel.Text = $"Game time: {DateTime.Now - GameStart:mm\\:ss}";
        if (BallTimerLabel != null)
        {
            BallTimerLabel.Text = $"Ball time: {DateTime.Now - BallStart:mm\\:ss}";
        }
        FPS.Text = $"{Engine.GetFramesPerSecond()} FPS";
    }

    private void UpdateScore(long totalScoreValue, int currentlyScoring)
    {
        if (currentlyScoring == 0) return;
        PhysicsScoreBubble scoreBubble = ScoreBubbleScene.Instantiate<PhysicsScoreBubble>();
        scoreBubble.Label.Text = currentlyScoring.ToString("+0;-#");
        scoreBubble.GlobalPosition = Score.GlobalPosition + (Score.Size / 2);
        AddChild(scoreBubble);
        Score.Text = $"Score: {totalScoreValue}";
        //History.Text = $"Scored: {currentlyScoring}\n{History.Text}";
    }

    private void UpdateBallList(Array<Ball> balls, Container ballsViewer)
    {
        foreach (var child in ballsViewer.GetChildren())
        {
            child.QueueFree();
        }
        foreach (var ball in balls)
        {
            BallViewer viewer = BallViewerScene.Instantiate<BallViewer>();
            viewer.Ball = (Ball)ball.Duplicate();
            ballsViewer.AddChild(viewer);
        }
    }

    private void UpdateLoadedBall(Ball ball)
    {
        LoadedBallViewer.Ball = (Ball)ball.Duplicate();
        BallTimerLabel = new Label();
        StatusBox.AddChild(BallTimerLabel);
        BallStart = DateTime.Now;
    }

    private void OnScreenResize()
    {
        Vector2 screenSize = GetViewport().GetVisibleRect().Size;
        GD.Print(screenSize);


        MainContainer.Vertical = screenSize.X < screenSize.Y;

    }

    void LeftPress()
    {
        Input.ActionPress("paddle_left");
    }

    void RightPress()
    {
        Input.ActionPress("paddle_right");
    }

    void PlungerPress()
    {
        Input.ActionPress("plunger");
        Input.ActionPress("load_ball");
    }

    void TiltPress()
    {
        Input.ActionPress("tilt");
    }

    void LeftRelease()
    {
        Input.ActionRelease("paddle_left");
    }

    void RightRelease()
    {
        Input.ActionRelease("paddle_right");
    }

    void PlungerRelease()
    {
        Input.ActionRelease("plunger");
    }

    void TiltRelease()
    {
        Input.ActionRelease("tilt");
    }
}
