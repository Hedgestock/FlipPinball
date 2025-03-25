using Godot;
using System;

public partial class ExtraBall : MetaEffect
{
    public override string Description
    {
        get
        {
            return $"Adds a copy of the currently loaded ball to your ball queue.";
        }
    }

    public override float AnalogRarity { get { return (float)Ballteration.RarityColor.Purple; } }

    public override void Activate()
    {
        GameManager.AddExtraBall(GameManager.CurrentBoard.LoadedBall, true);
    }

    public override Effect Refine(Effect minimum, Effect maximum)
    {
        if (minimum != null && AnalogRarity <= minimum.AnalogRarity) return new ReplayBall();
        if (maximum != null && AnalogRarity >= maximum.AnalogRarity) return new NewBall();
        return new ExtraBall();
    }
}
