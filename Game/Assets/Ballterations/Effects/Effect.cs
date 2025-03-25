using Godot;
using System;

public abstract partial class Effect : Node
{
    public abstract string Description { get; }

    public abstract float AnalogRarity { get; }

    public abstract Effect Refine(Effect minimum = null, Effect maximum = null);
}
