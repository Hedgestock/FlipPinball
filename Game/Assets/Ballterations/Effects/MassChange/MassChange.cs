using Godot;
using System;

public partial class MassChange : PhysicsEffect
{
    public override string Description
    {
        get
        {
            return $"Mass {(IsMultiplier ? 'x' : Value > 0 ? '+' : '-')}{Math.Abs(Value)}";
        }
    }

    public override float AnalogRarity => 0;

    [Export]
    private bool IsMultiplier;

    [Export]
    private float Value;

    public override Effect Refine(Effect minimum = null, Effect maximum = null)
    {
        return this;
    }

    override public void Affect(Ball ball)
    {
        if (IsMultiplier)
            ball.Mass *= Value;
        else
            ball.Mass += Value;

        ball.Mass = Math.Clamp(0.5f, 2f, ball.Mass);
    }
}
