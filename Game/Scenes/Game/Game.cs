using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Game : Node
{
    [Export]
    Viewport BoardViewport;

    [Export]
    PackedScene BallViewerScene;

    [Export]
    BoxContainer MainContainer;

    [Export]
    Container BallQueue;
    [Export]
    Container HeldBalls;
    [Export]
    Container LiveBalls;
    [Export]
    BallViewer LoadedBallViewer;



    [Export]
    Label Score;
    [Export]
    Label TargetScore;
    [Export]
    Label Credits;
    [Export]
    Label TotalScore;

    [Export]
    VBoxContainer InfoBox;
    [Export]
    Control Placeholder;
    [Export]
    Container StatusScrollContainer;
    [Export]
    VBoxContainer StatusBox;
    [Export]
    Label FPS;

    [Export]
    Container Ballterator;

    [Export]
    PackedScene ScoreBubbleScene;

    public override void _Ready()
    {
        CallDeferred(MethodName.OnScreenResize);
        GetTree().Root.Connect(Viewport.SignalName.SizeChanged, new Callable(this, MethodName.OnScreenResize));

        ScoreManager.Instance.Connect(ScoreManager.SignalName.Scoring, new Callable(this, MethodName.UpdateScore));

        GameManager.Instance.Connect(GameManager.SignalName.CreditsChanged, new Callable(this, MethodName.UpdateCredits));
        GameManager.Instance.Connect(GameManager.SignalName.NewBall, new Callable(this, MethodName.ResetBoard));
        GameManager.Instance.Connect(GameManager.SignalName.LevelCleared, new Callable(this, MethodName.OpenBallterator));
        GameManager.Instance.Connect(GameManager.SignalName.LoadedBall, new Callable(this, MethodName.UpdateLoadedBall));
        GameManager.Instance.Connect(GameManager.SignalName.BallQueueChanged, new Callable(this, MethodName.UpdateBallQueue));
        GameManager.Instance.Connect(GameManager.SignalName.HeldBallsChanged, new Callable(this, MethodName.UpdateHeldBalls));
        GameManager.Instance.Connect(GameManager.SignalName.LiveBallsChanged, new Callable(this, MethodName.UpdateLiveBalls));

        GameStart = DateTime.Now;

        GameManager.SetGame();

        ResetBoard();

        base._Ready();

        //OpenBallterator(); //debug only
    }

    [Export]
    Label GameTimerLabel;
    Label BallTimerLabel;
    DateTime GameStart;
    DateTime BallStart;

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

    void OpenBallterator()
    {
        GetTree().Paused = true;
        Ballterator.Show();
    }

    void ResetBoard()
    {
        TargetScore.Text = $"Target score: {GameManager.TargetScore:N0} ({GameManager.CurrentLevel})";
        Score.Text = $"Score: {ScoreManager.ScoreValue:N0}";
        foreach (var child in BoardViewport.GetChildren())
        {
            child.QueueFree();
        }
        GameManager.CurrentBoard = GD.Load<PackedScene>("res://Game/Scenes/Boards/TestLab/TestLab.tscn").Instantiate<Board>();
        BoardViewport.CallDeferred(Node.MethodName.AddChild, GameManager.CurrentBoard);
    }

    void UpdateScore(int currentlyScoring)
    {
        if (currentlyScoring == 0) return;
        PhysicsScoreBubble scoreBubble = ScoreBubbleScene.Instantiate<PhysicsScoreBubble>();
        scoreBubble.Label.Text = currentlyScoring.ToString("+0;-#");
        scoreBubble.GlobalPosition = Score.GlobalPosition + (Score.Size / 2);
        //GD.Print(scoreBubble.ProcessMode);
        AddChild(scoreBubble);
        Score.Text = $"Score: {ScoreManager.ScoreValue:N0}";
        TotalScore.Text = $"Total Score: {ScoreManager.TotalScoreValue:N0}";
    }

    void UpdateCredits()
    {
        Credits.Text = $"Credits Left: {Math.Max(GameManager.Credits, 0):N0}";
    }

    void UpdateBallQueue()
    {
        UpdateBallList(GameManager.BallQueue.ToArray(), BallQueue);
    }
    void UpdateHeldBalls(Array<Ball> balls)
    {
        UpdateBallList(balls, HeldBalls);
    }

    void UpdateLiveBalls(Array<Ball> balls)
    {
        UpdateBallList(balls, LiveBalls);
    }

    void UpdateBallList(IEnumerable<Ball> balls, Container ballsViewer)
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

    void UpdateLoadedBall(Ball ball)
    {
        LoadedBallViewer.Ball = (Ball)ball.Duplicate();
        BallTimerLabel = new Label();
        StatusBox.AddChild(BallTimerLabel);
        BallStart = DateTime.Now;
    }


    void OnScreenResize()
    {
        Vector2 screenSize = GetViewport().GetVisibleRect().Size;

        if (screenSize.X == 600)
        {
            MainContainer.CustomMinimumSize = new Vector2(screenSize.X, screenSize.Y - 1080);
            MainContainer.Size = new Vector2(screenSize.X, screenSize.Y - 1080);
            MainContainer.Position = new Vector2(0, 1080);
            MainContainer.Position = new Vector2(0, 1080);

            StatusScrollContainer.Hide();
            Placeholder.Hide();
        }
        else
        {
            MainContainer.CustomMinimumSize = Vector2.Zero;
            MainContainer.Position = Vector2.Zero;
            StatusScrollContainer.Show();
            Placeholder.Show();
        }

        GD.Print("Game.cs -> Game resizing: ", screenSize);

        TouchInputSetup();
    }

    [ExportGroup("TouchInputs")]
    [Export]
    TouchScreenButton LeftPaddleButton { get; set; }
    [Export]
    TouchScreenButton RightPaddleButton { get; set; }
    [Export]
    TouchScreenButton PlungerButton { get; set; }

    void TouchInputSetup()
    {
        Vector2 screenSize = GetViewport().GetVisibleRect().Size;

        LeftPaddleButton.Position = new(screenSize.X / 4, screenSize.Y / 2);
        RightPaddleButton.Position = new((screenSize.X / 4) * 3, screenSize.Y / 2);
        ((RectangleShape2D)LeftPaddleButton.Shape).Size = new(screenSize.X / 2, screenSize.Y);
        ((RectangleShape2D)RightPaddleButton.Shape).Size = new(screenSize.X / 2, screenSize.Y);
    }
}
