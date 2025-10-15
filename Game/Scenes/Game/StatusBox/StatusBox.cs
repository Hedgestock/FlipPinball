using Godot;
using System;

public partial class StatusBox : VBoxContainer
{
    [Export]
    Label FPS;
    [Export]
    Label GameTimerLabel;
    [Export]
    Label StatusLabel;

    Label BallTimerLabel;
    DateTime GameStart;
    DateTime BallStart;

    public override void _Ready()
    {
        base._Ready();
        StatusManager.Instance.Connect(StatusManager.SignalName.StatusChanged, new Callable(this, MethodName.UpdateStatus));
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        //GameTimerLabel.Text = $"Game time: {DateTime.Now - GameStart:mm\\:ss}";
        //if (BallTimerLabel != null)
        //{
        //    BallTimerLabel.Text = $"Ball time: {DateTime.Now - BallStart:mm\\:ss}";
        //}
        FPS.Text = Tr("FPS_COUNTER").Replace("{fps}", $"{Engine.GetFramesPerSecond()}");
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

    private void UpdateStatus(string status)
    {
        StatusLabel.Text = Tr(status);
    }
}
