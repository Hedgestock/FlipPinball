using Godot;
using System;

public partial class RolloverSwitch : Node2D
{
    [Signal]
    public delegate void LanePassedEventHandler(int mult);

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
            LanePass();
        }
    }

    void OnSouthZoneEnter(Node2D body)
    {
        if (NorthZone.OverlapsBody(body))
        {
            LanePass();
        }
    }

    void LanePass()
    {
        if (!SelfActivated && !OnOffLight.IsOn) return;
        OnOffLight.IsOn = !OnOffLight.IsOn;
        EmitSignal(SignalName.LanePassed, SelfActivated ? 1 : 5);
    }
}
