using Godot;
using System;

public partial class SkillShotZone : Area2D
{
    [Signal]
    public delegate void BallEnteredEventHandler();

    [Export]
    public int Multiplier;

    private void OnBodyEntered(Node2D body)
    {
        if (body is Ball)
        {
            GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
            EmitSignal(SignalName.BallEntered);
        }
    }
}
