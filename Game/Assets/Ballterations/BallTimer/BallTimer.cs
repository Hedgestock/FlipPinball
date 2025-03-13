using Godot;
using System;

public partial class BallTimer : Node
{
    [Export]
    Timer Timer;
    [Export]
    Label Label;

    // This is only Exported to keep state when duplicating
    [Export]
    double timeleft = 15;

    public override void _Ready()
    {
        base._Ready();

        Timer.Start(timeleft);
        Timer.Timeout += () =>
            {
                GD.Print("LOL");
                GetParent().EmitSignal(Ball.SignalName.SelfDestruct);
            };
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        Label.Text = Timer.TimeLeft.ToString("0.00");
        timeleft = Timer.TimeLeft;
    }
}
