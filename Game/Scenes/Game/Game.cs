using Godot;
using Godot.Collections;
using System;
using System.Linq;

public partial class Game : Node
{
    [Export]
    private GridContainer MainContainer;

    [Export]
    private HFlowContainer BallsList;

    [Export]
    private BallViewer CurrentBallViewer;

    [Export]
    private PackedScene BallViewerScene;

    [Export]
    private Label Score;

    [Export]
    private Label History;

    [Export]
    VBoxContainer StatusBox;

    [Export]
    PackedScene ScoreBubbleScene;
    public override void _Ready()
    {
        GetTree().Root.SizeChanged += OnScreenResize;
        ScoreManager.Instance.Scoring += UpdateScore;
        GameManager.Instance.NextBall += UpdateCurrentBall;
        GameManager.Instance.BallQueueChanged += UpdateStatus;

        GameStart = DateTime.Now;

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
    }

    private void UpdateScore(long totalScoreValue, int currentlyScoring)
    {
        PhysicsScoreBubble scoreBubble = ScoreBubbleScene.Instantiate<PhysicsScoreBubble>();
        scoreBubble.Label.Text = currentlyScoring.ToString("+0;-#");
        scoreBubble.GlobalPosition = Score.GlobalPosition + (Score.Size / 2);
        AddChild(scoreBubble);
        Score.Text = $"Score: {totalScoreValue}";
        History.Text = $"Scored: {currentlyScoring}\n{History.Text}";
    }

    private void UpdateStatus(Array<Ball> balls)
    {
        foreach (var child in BallsList.GetChildren())
        {
            child.QueueFree();
        }
        foreach (var ball in balls)
        {
            BallViewer viewer = BallViewerScene.Instantiate<BallViewer>();
            viewer.Ball = ball.Duplicate() as Ball;
            BallsList.AddChild(viewer);
        }
    }

    private void UpdateCurrentBall(Ball ball)
    {
        CurrentBallViewer.Ball = ball.Duplicate() as Ball;
        BallTimerLabel = new Label();
        StatusBox.AddChild(BallTimerLabel);
        BallStart = DateTime.Now;
    }

    private void OnScreenResize()
    {
        GD.Print(GetViewport().GetVisibleRect().Size);
        //MainContainer.Size = GetViewport().GetVisibleRect().Size;
    }
}
