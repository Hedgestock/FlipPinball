using Godot;
using System;
using System.Reflection.Metadata;

public partial class Paddle : CharacterBody2D
{

    [Export]
    int RotationSpeed;
    [Export]
    int AngleRange;

    [Export]
    public AudioStreamPlayer2D SoundPlayer;

    float RestRotation;

    public override void _Ready()
    {
        base._Ready();
        RestRotation = Rotation;
    }

    public void Flip(double delta)
    {
        Rotation = (float)Mathf.RotateToward(Rotation, RestRotation + Mathf.DegToRad(Scale.X * AngleRange), delta * RotationSpeed);
    }

    public void Return(double delta)
    {
        Rotation = (float)Mathf.RotateToward(Rotation, RestRotation, delta * RotationSpeed);
    }
}
