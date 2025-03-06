using Godot;
using System;

public partial class RolloverSwitch : Node2D
{
    [Signal]
    public delegate void ToggledEventHandler(bool IsOn);

    [Signal]
    public delegate void LanePassedEventHandler(bool IsOn);

    [Export]
    Area2D NorthZone;
    [Export]
    Area2D SouthZone;
    [Export]
    OnOffLight OnOffLight;
    [Export]
    bool SelfActivated = true;

    void OnNorthZoneEnter(Node2D body)
    {
        if (SouthZone.OverlapsBody(body))
        {
            Toggle();
        }
    }

    void OnSouthZoneEnter(Node2D body)
    {
        if (NorthZone.OverlapsBody(body))
        {
            Toggle();
        }
    }

    void LanePass()
    {

    }

    void Toggle()
    {
        if (!SelfActivated && !OnOffLight.IsOn) return;
        OnOffLight.IsOn = !OnOffLight.IsOn;
        EmitSignal(SignalName.Toggled, OnOffLight.IsOn);
    }
}
