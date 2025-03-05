using Godot;
using System;
using System.Linq;

public partial class Ball : RigidBody2D
{
    [Export]
    Line2D Trail;

    Vector2 LastPosition;

    public override void _Ready()
    {
        base._Ready();
        LastPosition = GlobalPosition;
        Trail.Points = Enumerable.Repeat(Vector2.Zero, 50).ToArray();
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        Vector2[] tmp = new Vector2[Trail.Points.Length];
        tmp[0] = Vector2.Zero;
        for (int i = 1; i < Trail.Points.Length; i++)
        {
            tmp[i] = Trail.Points[i - 1] + (LastPosition - GlobalPosition);
        }
        Trail.Points = tmp;
        LastPosition = GlobalPosition;
    }

    public void OnCollision(Node body)
    {
        if (body is Hitbox hitbox) hitbox.CollideWithBall(this);
    }
}
