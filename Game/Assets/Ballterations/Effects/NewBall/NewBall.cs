using Godot;
using System;

public partial class NewBall : MetaEffect
{
    public override string Description
    {
        get
        {
            return $"Adds a new ball to your ball queue.";
        }
    }

    public override void Activate()
    {
        GameManager.AddExtraBall(GD.Load<PackedScene>("res://Game/Assets/Ball/Ball.tscn").Instantiate<Ball>(), true);
    }
}
