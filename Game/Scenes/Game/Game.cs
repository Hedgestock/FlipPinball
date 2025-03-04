using Godot;

public partial class Game : Node
{
    [Export]
    private GridContainer MainContainer;

    [Export]
    private Label Score;

    [Export]
    PackedScene ScoreBubbleScene;
    public override void _Ready()
    {
        GetTree().Root.SizeChanged += OnScreenResize;
        ScoreManager.Instance.Scoring += UpdateScore;
        base._Ready();
    }

    private void UpdateScore(long totalScoreValue, int currentlyScoring)
    {
        PhysicsScoreBubble scoreBubble = ScoreBubbleScene.Instantiate<PhysicsScoreBubble>();
        scoreBubble.Label.Text = currentlyScoring.ToString("+0;-#");
        scoreBubble.GlobalPosition = Score.GlobalPosition + (Score.Size / 2);
        AddChild(scoreBubble);
        Score.Text = $"Score : {totalScoreValue}";
    }

    private void OnScreenResize()
    {
        GD.Print(GetViewport().GetVisibleRect().Size);
        //MainContainer.Size = GetViewport().GetVisibleRect().Size;
    }
}
