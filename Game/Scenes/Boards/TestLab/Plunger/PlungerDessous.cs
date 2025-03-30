using Godot;
using System;

public partial class PlungerDessous : Sprite2D
{
    float startPos;

    public override void _Ready()
    {
        base._Ready();
        startPos = Position.Y;
    }

    void SetPosition(float value)
    {
        Position = new Vector2(Position.X, startPos + value / 5000 * 65);
    }
}
