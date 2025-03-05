using Godot;
using System;

public partial class RolloverSwitch : Node2D
{
    [Signal]
    public delegate void ToggledEventHandler(bool IsOn);

    [Export]
    Area2D NorthZone;
    [Export]
    Area2D SouthZone;
    [Export]
    OnOffLight OnOffLight;

    void OnNorthZoneEnter(Node2D body)
    {
        if (SouthZone.OverlapsBody(body))
        {
            GD.Print(body.Name);
            Toggle();
        }
    }

    void OnSouthZoneEnter(Node2D body)
    {
        if (NorthZone.OverlapsBody(body))
        {
            GD.Print(body);
            Toggle();
        }
    }

    private void Toggle()
    {
        OnOffLight.IsOn = !OnOffLight.IsOn;
        EmitSignal(SignalName.Toggled, OnOffLight.IsOn);
    }
}
