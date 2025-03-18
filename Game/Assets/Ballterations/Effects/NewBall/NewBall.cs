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

    public override float AnalogRarity => throw new NotImplementedException();

    public override void Activate()
    {
        GameManager.AddExtraBall(Ball.GetFreshBall(), true);
    }
}
