using Godot;
using System;

public partial class PhysicsScoreBubble : RigidBody2D
{
    [Export]
    public Label Label;

    [Export]
    VisibleOnScreenNotifier2D visibleOnScreenNotifier;

    public override void _Ready()
    {
        base._Ready();
        visibleOnScreenNotifier.Rect = new Rect2(Vector2.Zero, Label.Size);
        visibleOnScreenNotifier.ScreenExited += QueueFree;
        AngularVelocity = (float)GD.RandRange(-1d, 1d);
        LinearVelocity = (Vector2.Up * 200).Rotated(AngularVelocity);
    }
}
