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


    //[Export]
    //Container bars;
    //ProgressBar[] test = new ProgressBar[100];
    public override void _Ready()
    {
        OnScreenResize();

        GetTree().Root.SizeChanged += OnScreenResize;
        ScoreManager.Instance.Scoring += UpdateScore;
        GameManager.Instance.LoadedBall += UpdateLoadedBall;
        GameManager.Instance.BallQueueChanged += (balls) => UpdateBallList(balls, BallQueue);
        GameManager.Instance.HeldBallsChanged += (balls) => UpdateBallList(balls, HeldBalls);
        GameManager.Instance.LiveBallsChanged += (balls) => UpdateBallList(balls, LiveBalls);

        GameStart = DateTime.Now;

        base._Ready();

        //for (int i = 0; i < test.Length; i++)
        //{
        //    var bar = new ProgressBar();
        //    bar.FillMode = (int)ProgressBar.FillModeEnum.BottomToTop;
        //    bar.ShowPercentage = false;
        //    bar.SizeFlagsVertical = Control.SizeFlags.Fill;
        //    bar.MaxValue = 20;
        //    bar.Value = 10;
        //    test[i] = bar;
        //    bars.AddChild(bar);
        //}
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

    //Vector3[] lastAccel = new Vector3[100];

    //public override void _PhysicsProcess(double delta)
    //{
    //    Vector3[] tmp = lastAccel;
    //    System.Array.Copy(tmp, 1, lastAccel, 0, lastAccel.Length - 1);
    //    base._PhysicsProcess(delta);

    //    lastAccel[test.Length-1] = Input.GetAccelerometer();
    //    for (int i = 0; i < test.Length; i++)
    //    {
    //        test[i].Value = lastAccel[i].Length();
    //    }
    //}

    private void UpdateScore(long totalScoreValue, int currentlyScoring)
    {
        if (currentlyScoring == 0) return;
        //PhysicsScoreBubble scoreBubble = ScoreBubbleScene.Instantiate<PhysicsScoreBubble>();
        //scoreBubble.Label.Text = currentlyScoring.ToString("+0;-#");
        //scoreBubble.GlobalPosition = Score.GlobalPosition + (Score.Size / 2);
        //AddChild(scoreBubble);
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


    void OnScreenResize()
    {
        Vector2 screenSize = GetViewport().GetVisibleRect().Size;

        MainContainer.CustomMinimumSize = new Vector2(screenSize.X, screenSize.Y - 1080);

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
