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

    public override float AnalogRarity { get { return (float)Ballteration.Rarity.Green; } }

    public override void Activate()
    {
        GameManager.AddExtraBall(Ball.GetFreshBall(), true);
    }

    public override Effect Ameliorate()
    {
        return new ExtraBall();
    }

    public override Effect Worsen()
    {
        return this;
    }
}
