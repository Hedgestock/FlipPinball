using Godot;
using System;

public partial class RolloverSwitch : Node2D
{
    [Signal]
    public delegate void LanePassedEventHandler();
    [Signal]
    public delegate void DeactivatingEventHandler();

    [Export]
    Area2D NorthZone;
    [Export]
    Area2D SouthZone;

    [Export]
    OnOffLight OnOffLight;
    [Export]
    Scorer Scorer;

    [Export]
    bool SelfActivated = true;
    [Export]
    int OnMultiplier = 1;

    void OnNorthZoneEnter(Node2D body)
    {
        if (body is Ball ball && SouthZone.OverlapsBody(body))
        {
            LanePass(ball);
        }
    }

    void OnSouthZoneEnter(Node2D body)
    {
        if (body is Ball ball && NorthZone.OverlapsBody(body))
        {
            LanePass(ball);
        }
    }

    void LanePass(Ball ball)
    {
        if (OnOffLight.IsOn)
            Scorer.Score(ball, Scorer.Value * OnMultiplier);
        else
            Scorer.Score(ball);

        EmitSignal(SignalName.LanePassed);
        if (!SelfActivated && !OnOffLight.IsOn) return;
        OnOffLight.IsOn = !OnOffLight.IsOn;
        if (!OnOffLight.IsOn) EmitSignal(SignalName.Deactivating);
    }
}
