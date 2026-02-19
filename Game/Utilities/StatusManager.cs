using Godot;
using System;

public partial class StatusManager : Node
{
    [Signal]
    public delegate void StatusChangedEventHandler(string status);
 
    [Signal]
    public delegate void MissionChangedEventHandler(string status);

    [Signal]
    public delegate void MissionStatusChangedEventHandler(string status);

    [Signal]
    public delegate void ResetMissionEventHandler();

    protected static StatusManager _instance;
    public static StatusManager Instance { get { return _instance; } }

    public StatusManager()
    {
        if (_instance != null)
            return;
        _instance = this;
    }
}
