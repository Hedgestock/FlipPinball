using Godot;
using System;

public partial class Home : CanvasLayer
{
    [Export]
    Label ScoreLabel;
    public override void _Ready()
    {
        base._Ready();
        ScoreLabel.Text = $"Score: {ScoreManager.ScoreValue:N0}";
    }

    void StartGame()
    {
        GameManager.SetGame();
    }
}
