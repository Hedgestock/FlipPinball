using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class InfoBox : VBoxContainer
{
    [Export]
    PackedScene BallViewerScene;

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

    //[Export]
    //PackedScene ScoreBubbleScene;

    public override void _Ready()
    {
        base._Ready();

        ScoreManager.Instance.Connect(ScoreManager.SignalName.Scoring, new Callable(this, MethodName.UpdateScore));

        GameManager.Instance.Connect(GameManager.SignalName.CreditsChanged, new Callable(this, MethodName.UpdateCredits));
        GameManager.Instance.Connect(GameManager.SignalName.LoadedBall, new Callable(this, MethodName.UpdateLoadedBall));
        GameManager.Instance.Connect(GameManager.SignalName.BallQueueChanged, new Callable(this, MethodName.UpdateBallQueue));
        GameManager.Instance.Connect(GameManager.SignalName.HeldBallsChanged, new Callable(this, MethodName.UpdateHeldBalls));
        GameManager.Instance.Connect(GameManager.SignalName.LiveBallsChanged, new Callable(this, MethodName.UpdateLiveBalls));
    }

    public void Reset()
    {
        TargetScore.Text = $"Target score: {GameManager.TargetScore:N0} ({GameManager.CurrentLevel})";
        Score.Text = $"Score: {ScoreManager.ScoreValue:N0}";
    }

    void UpdateScore(int currentlyScoring)
    {
        if (currentlyScoring == 0) return;
        //PhysicsScoreBubble scoreBubble = ScoreBubbleScene.Instantiate<PhysicsScoreBubble>();
        //scoreBubble.Label.Text = currentlyScoring.ToString("+0;-#");
        //scoreBubble.GlobalPosition = Score.GlobalPosition + (Score.Size / 2);
        //GD.Print(scoreBubble.ProcessMode);
        //AddChild(scoreBubble);
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
        //BallTimerLabel = new Label();
        //StatusBox.AddChild(BallTimerLabel);
        //BallStart = DateTime.Now;
    }
}
