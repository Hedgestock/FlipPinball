using Godot;
using System;

public partial class Rollover : Node2D
{
    [Signal]
    public delegate void RolledOverEventHandler();
    private void OnAreaBodyEnter(Node2D body)
    {
        EmitSignal(SignalName.RolledOver);
    }
}
