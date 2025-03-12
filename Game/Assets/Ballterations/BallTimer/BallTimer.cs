using Godot;
using System;

public partial class BallTimer : Node2D
{
    [Export]
    Timer Timer;
    [Export]
    Label Label;

    double timeleft;

    public override void _Ready()
    {
        base._Ready();
        //Timer.Autostart = true;

        GD.Print(timeleft, "Left on ready ", Timer.TimeLeft.ToString("0.00"));
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        Label.Text = Timer.TimeLeft.ToString("0.00");
        timeleft = Timer.TimeLeft;
    }
    void Destroy()
    {
        GetParent().QueueFree();
    }
}
