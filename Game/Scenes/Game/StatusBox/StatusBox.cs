using Godot;
using System;
using static System.Net.Mime.MediaTypeNames;

public partial class StatusBox : VBoxContainer
{
    [Export]
    Label FPS;
    [Export]
    Label GameTimerLabel;
    [Export]
    Label StatusLabel;
    [Export]
    Label CurrentMissionLabel;
    [Export]
    RichTextLabel MissionStatusLabel;
    [Export]
    Timer MissionResetTimer;
    [Export]
    Timer StatusResetTimer;

    Label BallTimerLabel;
    DateTime GameStart;
    DateTime BallStart;


    public override void _Ready()
    {
        base._Ready();
        MissionResetTimer.Connect(Timer.SignalName.Timeout, Callable.From(ResetMission));
        StatusResetTimer.Connect(Timer.SignalName.Timeout, Callable.From(ResetStatus));
        StatusManager.Instance.Connect(StatusManager.SignalName.StatusChanged, new Callable(this, MethodName.UpdateStatus));
        StatusManager.Instance.Connect(StatusManager.SignalName.MissionChanged, new Callable(this, MethodName.UpdateMissionTitle));
        StatusManager.Instance.Connect(StatusManager.SignalName.MissionStatusChanged, new Callable(this, MethodName.UpdateMissionStatus));
        StatusManager.Instance.Connect(StatusManager.SignalName.ResetMission, Callable.From(() => MissionResetTimer.Start()));
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

    public void Reset()
    {
        ResetStatus();
        ResetMission();
    }

    public void ResetStatus()
    {
        UpdateStatus("STATUS_PLACEHOLDER");
    }

    public void ResetMission()
    {
        UpdateMissionTitle("STATUS_MISSION_PLACEHOLDER");
        UpdateMissionStatus("STATUS_MISSION_GOALS_PLACEHOLDER");
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
        deleteEvent.Action = "delete_live_balls";
        deleteEvent.Pressed = true;
        Input.ParseInputEvent(deleteEvent);
    }

    public void UpdateStatus(string status)
    {
        GD.Print("up status");
        StatusResetTimer.Stop();
        StatusLabel.Text = Tr(status);
        if (status != "STATUS_PLACEHOLDER")
            StatusResetTimer.Start();
    }

    public void UpdateMissionTitle(string title)
    {
        MissionResetTimer.Stop();
        CurrentMissionLabel.Text = Tr(title);
    }

    public void UpdateMissionStatus(string status)
    {
        MissionResetTimer.Stop();
        MissionStatusLabel.Text = Tr(status);
    }
}
