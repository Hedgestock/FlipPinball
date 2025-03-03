using Godot;

public partial class Game : Node
{
    [Export]
    private GridContainer MainContainer;

    [Export]
    private Label Score;
    public override void _Ready()
    {
        GetTree().Root.SizeChanged += OnScreenResize;
        ScoreManager.Instance.Connect(ScoreManager.SignalName.Scoring, new Callable(this, MethodName.UpdateScore));
        //ScoreManager.Instance.Scoring +=UpdateScore; Apparently I should be able to do that
        base._Ready();
    }

    private void UpdateScore(long totalScoreValue, int currentlyScoring)
    {
        Score.Text = $"Score : {totalScoreValue}, {currentlyScoring}";
    }

    private void OnScreenResize()
    {
        GD.Print(GetViewport().GetVisibleRect().Size);
        //MainContainer.Size = GetViewport().GetVisibleRect().Size;
    }
}
