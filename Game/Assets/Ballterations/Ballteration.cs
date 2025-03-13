using Godot;
using System;

public partial class Ballteration : Node
{
    public enum Type
    {
        Score,
        Physics,
        Meta,
        Other
    }

    protected Type Kind = Type.Other;
}