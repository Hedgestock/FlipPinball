using Godot;
using System;

public partial class Pusher : Node
{
    [Export]
    public uint Strength;

    [Export]
    public int PushVariation;

    public void Push(Ball ball, Vector2 direction)
    {
        if (PushVariation != 0)
            ball.ApplyCentralImpulse(direction * (Strength + GD.RandRange(-PushVariation, PushVariation)));
        else
            ball.ApplyCentralImpulse(direction * Strength);
    }
}
