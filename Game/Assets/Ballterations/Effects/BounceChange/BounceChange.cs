using Godot;
using System;

public partial class BounceChange : PhysicsEffect
{
    public override string Description
    {
        get
        {
            return $"Bounciness {(IsMultiplier ? 'x' : Value > 0 ? '+' : '-')}{Math.Abs(Value)}";
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
            ball.PhysicsMaterialOverride.Bounce *= Value;
        else
            ball.PhysicsMaterialOverride.Bounce += Value;
    }
}
