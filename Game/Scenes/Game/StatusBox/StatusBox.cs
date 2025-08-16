using Godot;
using System;

public partial class StatusBox : VBoxContainer
{
    [Export]
    Label FPS;

    [Export]
    Label GameTimerLabel;

    Label BallTimerLabel;
    DateTime GameStart;
    DateTime BallStart;


    public override void _Process(double delta)
    {
        base._Process(delta);
        //GameTimerLabel.Text = $"Game time: {DateTime.Now - GameStart:mm\\:ss}";
        //if (BallTimerLabel != null)
        //{
        //    BallTimerLabel.Text = $"Ball time: {DateTime.Now - BallStart:mm\\:ss}";
        //}
        FPS.Text = $"{Engine.GetFramesPerSecond()} FPS";
    }

    private void Pause()
    {
        InputEventAction pauseEvent = new();
        pauseEvent.Action = "pause";
        pauseEvent.Pressed = true;
        Input.ParseInputEvent(pauseEvent);
    }

    private void Delete()
    {
        InputEventAction deleteEvent = new();
        deleteEvent.Action = "delete";
        deleteEvent.Pressed = true;
        Input.ParseInputEvent(deleteEvent);
    }
}
