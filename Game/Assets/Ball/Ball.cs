using Godot;
using System;
using System.Linq;

public partial class Ball : RigidBody2D
{
    [Export]
    Line2D Trail;
    [Export]
    Area2D Center;

    Vector2[] LastPoints;
    Vector2 LastPosition;
    Vector2 LastVelocity;
    Vector2 LastCollisionNormal;

    public override void _Ready()
    {
        base._Ready();

        LastPosition = GlobalPosition;
        if (ProcessMode != ProcessModeEnum.Disabled)
        {
            ResetTrail();
        }
        else
        {
            Trail.Points = [new(0, 0), new(20, 20)];
        }
    }


    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        var collisionInfo = MoveAndCollide(LinearVelocity * (float)delta, true);
        if (collisionInfo != null)
        {
            LastCollisionNormal = collisionInfo.GetNormal();
        }

        LastVelocity = LinearVelocity;

        LastPoints[0] = Vector2.Zero;
        for (int i = 1; i < Trail.Points.Length; i++)
        {
            LastPoints[i] = Trail.Points[i - 1] + (LastPosition - GlobalPosition);
        }
        Trail.Points = LastPoints;
        LastPoints = Trail.Points;
        LastPosition = GlobalPosition;

        GD.Print(GlobalPosition);
    }

    public void OnCollision(Node body)
    {
        if (body is Hitbox hitbox) hitbox.CollideWithBall(this, LastVelocity, LastCollisionNormal);
    }

    public void SetCollision(int layer, bool value)
    {
        SetCollisionLayerValue(layer, value);
        SetCollisionMaskValue(layer, value);
        Center.SetCollisionLayerValue(layer, value);
        if (value)
            ZIndex = (layer - 2) * 10 + 4;
        GD.Print(ZIndex);
    }

    public void ResetTrail()
    {
        Trail.Points = Enumerable.Repeat(Vector2.Zero, 50).ToArray();
        LastPoints = Trail.Points;
    }
}
