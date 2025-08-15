using Godot;
using System;

public partial class SizeChange : PhysicsEffect
{
    public override string Description
    {
        get
        {
            return $"Size {(IsMultiplier ? 'x' : Value > 0 ? '+' : '-')}{Math.Abs(Value)}";
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
            ball.Size *= Value;
        else
            ball.Size += Value;

        ball.Size = Math.Clamp(0.5f, 2f, ball.Size);
    }
}