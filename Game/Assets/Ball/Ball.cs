using Godot;
using System;
using System.Linq;

public partial class Ball : RigidBody2D
{
    [Export]
    Line2D Trail;

    Vector2 LastPosition;
    Vector2 LastVelocity;
    Vector2 LastCollisionNormal;

    public override void _Ready()
    {
        base._Ready();
        LastPosition = GlobalPosition;
        Trail.Points = Enumerable.Repeat(Vector2.Zero, 50).ToArray();
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        var collisionInfo = MoveAndCollide(LinearVelocity * (float)delta, true);
        if (collisionInfo != null)
        {
            LastCollisionNormal = collisionInfo.GetNormal();
        }
        Vector2[] tmp = new Vector2[Trail.Points.Length];
        tmp[0] = Vector2.Zero;
        for (int i = 1; i < Trail.Points.Length; i++)
        {
            tmp[i] = Trail.Points[i - 1] + (LastPosition - GlobalPosition);
        }
        Trail.Points = tmp;
        LastPosition = GlobalPosition;
        LastVelocity = LinearVelocity;
    }

    public void OnCollision(Node body)
    {
        if (body is Hitbox hitbox) hitbox.CollideWithBall(this, LastVelocity, LastCollisionNormal);
    }
}
