using Godot;
using System;

public partial class ShapeShift : PhysicsEffect
{
    [Export]
    Texture2D Texture;
    [Export]
    Shape2D Shape;
    [Export]
    PhysicsMaterial PhysicsMaterial;
    [Export]
    string ShapeName;

    public override string Description
    {
        get
        {
            return $"Shapeshift the ball into a {ShapeName}";
        }
    }

    public override float AnalogRarity => 0;

    public override Effect Refine(Effect minimum = null, Effect maximum = null)
    {
        return this;
    }

    override public void Affect(Ball ball)
    {
        ball.GetNode<Sprite2D>("Sprite2D").Texture = Texture;
        ball.GetNode<CollisionShape2D>("CollisionShape2D").Shape = Shape;
        ball.PhysicsMaterialOverride.Friction = PhysicsMaterial.Friction;
    }
}
