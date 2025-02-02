using Godot;
using System;

public partial class Ball : RigidBody2D
{
    public void OnCollision(Node body)
    {
        if (body.GetParent() is Collidable) (body.GetParent() as Collidable).CollideWithBall(this);
    }
}
