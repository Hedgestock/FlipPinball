using Godot;
using System;

public partial class Rollover : Node2D
{
    [Signal]
    public delegate void RolledOverEventHandler();

    [Export]
    OnOffLight OnOffLight;
    [Export]
    Scorer Scorer;

    [Export]
    int OnMultiplier = 1;

    private void OnAreaBodyEnter(Node2D body)
    {
        if (body is Ball ball)
        {
            if (OnOffLight.IsOn)
                Scorer.Score(ball);
            else
                Scorer.Score(ball, Scorer.Value * OnMultiplier);
            EmitSignal(SignalName.RolledOver);
        }
    }
}
