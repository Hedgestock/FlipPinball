using Godot;
using System;

public partial class ReplayBall : MetaEffect
{
    public override string Description
    {
        get
        {
            return $"Puts the last played ball to the front of the ball queue.";
        }
    }

    public override float AnalogRarity { get { return (float)Ballteration.RarityColor.Red; } }

    public override void Activate()
    {
        GameManager.AddExtraBall(GameManager.CurrentBoard.LoadedBall);
    }

    public override Effect Refine(Effect minimum, Effect maximum)
    {
        // A replay ball has no "better" version, so if we are not worsening it, nothing changes
        if (maximum != null && AnalogRarity >= maximum.AnalogRarity) return new ExtraBall();
        return new ReplayBall();
    }
}
