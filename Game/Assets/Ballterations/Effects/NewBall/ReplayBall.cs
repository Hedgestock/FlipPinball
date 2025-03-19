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

    public override float AnalogRarity { get { return (float)Ballteration.Rarity.Purple; } }

    public override void Activate()
    {
        GameManager.AddExtraBall(GameManager.CurrentBoard.LoadedBall, true);
    }
}
