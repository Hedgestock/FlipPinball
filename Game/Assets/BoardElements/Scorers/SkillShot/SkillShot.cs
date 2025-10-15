using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class SkillShot : Node2D
{
    [Signal]
    public delegate void ValidateEventHandler();
    [Signal]
    public delegate void CancelEventHandler();

    [Export]
    Area2D CancellationZone;
    [Export]
    Area2D ValidationZone;
    [Export]
    Area2D InboundZone;
    [Export]
    Scorer Scorer;

    List<SkillShotZone> Zones;

    int Multiplier = 1;

    public override void _Ready()
    {
        base._Ready();
        Zones = GetChildren().Where(c => c is SkillShotZone).Cast<SkillShotZone>().ToList();
        Zones.ForEach(z => z.BallEntered += () =>
        {
            CancellationZone.GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred(CollisionShape2D.PropertyName.Disabled, false);
            ValidationZone.GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred(CollisionShape2D.PropertyName.Disabled, false);
            Multiplier = z.Multiplier;
        });
    }

    void Cancellation(Node2D body)
    {
        if (!(body is Ball) || InboundZone.OverlapsBody(body)) return;

        EmitSignalCancel();

        TurnOff();
    }

    void Validation(Node2D body)
    {
        if (!(body is Ball) || InboundZone.OverlapsBody(body)) return;

        Scorer.Score((Ball)body, Scorer.Value * Multiplier);
        EmitSignalValidate();

        TurnOff();
    }

    public void TurnOff()
    {
        CancellationZone.GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
        ValidationZone.GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred(CollisionShape2D.PropertyName.Disabled, true);

        Zones.ForEach(z =>
        {
            z.GetNode<OnOffLight>("OnOffLight").TurnOff();
            z.GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
        });
    }

    public void TurnOn()
    {
        CancellationZone.GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
        ValidationZone.GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred(CollisionShape2D.PropertyName.Disabled, true);

        Zones.ForEach(z =>
        {
            z.GetNode<OnOffLight>("OnOffLight").TurnOff();
            z.GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred(CollisionShape2D.PropertyName.Disabled, false);
        });
    }
}