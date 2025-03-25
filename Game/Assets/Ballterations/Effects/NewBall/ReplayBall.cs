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

    public override float AnalogRarity { get { return (float)Ballteration.Rarity.Red; } }

    public override void Activate()
    {
        GameManager.AddExtraBall(GameManager.CurrentBoard.LoadedBall);
    }

    public override Effect Ameliorate()
    {
        return this;
    }

    public override Effect Worsen()
    {
        return new ReplayBall();
    }
}
