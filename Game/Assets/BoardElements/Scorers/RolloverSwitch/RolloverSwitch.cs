using Godot;
using System;

public partial class RolloverSwitch : Node2D
{
    [Signal]
    public delegate void LanePassedEventHandler(int mult);
    [Signal]
    public delegate void DeactivatingEventHandler(int mult);

    [Export]
    Area2D NorthZone;
    [Export]
    Area2D SouthZone;

    [Export]
    OnOffLight OnOffLight;

    [Export]
    bool SelfActivated = true;
    [Export]
    int OnMult = 1;

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
        EmitSignal(SignalName.LanePassed, OnOffLight.IsOn ? OnMult : 1);
        if (!SelfActivated && !OnOffLight.IsOn) return;
        OnOffLight.IsOn = !OnOffLight.IsOn;
        if (!OnOffLight.IsOn) EmitSignal(SignalName.Deactivating);
    }
}
