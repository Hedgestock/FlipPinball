using Godot;
using System;

public partial class Target : Collidable
{
    [Export]
    protected StaticBody2D TargetElement;

    [Export]
    Timer ResetDelay;

    public override void CollideWithBall(Ball ball)
    {
        if (!IsOn) return;
        base.CollideWithBall(ball);

        Score();

        IsOn = !IsOn;
    }

    protected override void SetOnValue(bool value)
    {
        base.SetOnValue(value);
        if (value)
            ResetDelay.Stop();
        else
            ResetDelay.Start();
    }
}
