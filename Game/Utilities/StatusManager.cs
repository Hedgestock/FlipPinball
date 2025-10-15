using Godot;
using System;

public partial class StatusManager : Node
{
    [Signal]
    public delegate void StatusChangedEventHandler(string Status, bool translate);

    protected static StatusManager _instance;
    public static StatusManager Instance { get { return _instance; } }

    public StatusManager()
    {
        if (_instance != null)
            return;
        _instance = this;
    }
}
