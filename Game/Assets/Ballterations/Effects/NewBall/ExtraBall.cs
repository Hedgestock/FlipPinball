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

    public override float AnalogRarity { get { return (float)Ballteration.Rarity.Purple; } }

    public override void Activate()
    {
        GameManager.AddExtraBall(GameManager.CurrentBoard.LoadedBall, true);
    }

    public override Effect Ameliorate()
    {
        return new ReplayBall();
    }

    public override Effect Worsen()
    {
        return new NewBall();
    }
}
