using Godot;
using System;

public partial class RolloverSwitch : Scorer
{
    [Export]
    Area2D NorthZone;
    [Export]
    Area2D SouthZone;

    public override void _Ready()
    {
        base._Ready();
        IsOn = false;
    }

    void OnNorthZoneEnter(Node2D body)
    {
        if (!(body is Ball)) return;
        if (SouthZone.OverlapsBody(body))
        {
            Toggle();
        }
    }

    void OnSouthZoneEnter(Node2D body)
    {
        if (!(body is Ball)) return;
        if (NorthZone.OverlapsBody(body))
        {
            Toggle();
        }
    }

    private void Toggle()
    {
        IsOn = !IsOn;

        Score();
    }
}
