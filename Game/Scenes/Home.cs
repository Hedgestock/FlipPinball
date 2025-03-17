using Godot;
using System;

public partial class Home : CanvasLayer
{
    [Export]
    Label ScoreLabel;
    public override void _Ready()
    {
        base._Ready();
        ScoreLabel.Text = $"Score: {ScoreManager.TotalScoreValue:N0}\nLost on level: {GameManager.CurrentLevel}";
    }
}
