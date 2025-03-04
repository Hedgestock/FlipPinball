using Godot;
using System.Linq;

public partial class Plunger : Node2D
{
    [Export]
    uint MaxStrength = 5000;

    [Export]
    TextureProgressBar PlungerProgress;
    [Export]
    Area2D DetectionZone;

    [Export]
    AudioStreamPlayer ReleaseSoundPlayer;
    [Export]
    AudioStreamPlayer WindUpSoundPlayer;
    public override void _Ready()
    {
        base._Ready();
        PlungerProgress.MaxValue = MaxStrength;
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        if (Input.IsActionPressed("plunger"))
        {
            WindPlunger();
        }
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("plunger"))
        {

            WindUpSoundPlayer.Play();
        }
        if (@event.IsActionReleased("plunger"))
        {

            ReleasePlunger();
        }
    }

    private void WindPlunger()
    {
        PlungerProgress.Value += 10;
    }

    private void ReleasePlunger()
    {

        foreach (RigidBody2D ball in DetectionZone.GetOverlappingBodies().Where(b => b is Ball))
        {
            ball.LinearVelocity = Vector2.Up * (int)PlungerProgress.Value;
        }
        PlungerProgress.Value = 0;
        WindUpSoundPlayer.Stop();
        ReleaseSoundPlayer.Play();
    }
}
