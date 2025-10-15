using Godot;
using System;

public partial class BallSelector : PanelContainer
{
    [Signal]
    public delegate void BallSelectedEventHandler(Ball ball);

    [Export]
    BallViewer BallViewer;

    Ball _ball;

    public Ball Ball
    {
        set
        {
            _ball = value;
            BallViewer.Ball = (Ball)value.Duplicate();
        }
    }

    void OnClick()
    {
        EmitSignalBallSelected(_ball);
    }
}
