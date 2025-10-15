using Godot;
using System;

public partial class Home : CanvasLayer
{
    [Export]
    Label ScoreLabel;
    public override void _Ready()
    {
        base._Ready();
        ScoreLabel.Text = Tr("MENU_SCORE").Replace("{score}", $"{ScoreManager.TotalScoreValue:N0}").Replace("{level}", $"{GameManager.CurrentLevel}");
    }
}
