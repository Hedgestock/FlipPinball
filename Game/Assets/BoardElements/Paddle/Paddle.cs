using Godot;
using System;
using System.Reflection.Metadata;

public partial class Paddle : CharacterBody2D
{

    [Export]
    float RotationSpeed;

    [Export]
    public AudioStreamPlayer2D SoundPlayer;
    public void Rotate(double delta, double angle)
    {
        Rotation = (float)Mathf.RotateToward(Rotation, angle, delta * RotationSpeed);
    }
}
