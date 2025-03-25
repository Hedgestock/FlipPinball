using Godot;
using System;

public partial class NewBall : MetaEffect
{
    public override string Description
    {
        get
        {
            return $"Adds a new ball to your ball queue.";
        }
    }

    public override float AnalogRarity { get { return (float)Ballteration.RarityColor.Green; } }

    public override void Activate()
    {
        GameManager.AddExtraBall(Ball.GetFreshBall(), true);
    }

    public override Effect Refine(Effect minimum, Effect maximum)
    {
        // A new ball has no "worse" version, so if we are not worsening it, nothing changes
        // TODO: Maybe look into timed balls
        if (minimum != null && AnalogRarity <= minimum.AnalogRarity) return new ExtraBall();
        return new NewBall();
    }
}
