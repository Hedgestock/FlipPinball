using Godot;
using System.Linq;

public partial class Plunger : Node2D
{
    [Signal]
    public delegate void WindingUpEventHandler();
    [Signal]
    public delegate void ReleasingEventHandler();

    [Export]
    uint MaxStrength = 5000;

    [Export]
    TextureProgressBar PlungerProgress;
    [Export]
    Area2D DetectionZone;

    public bool AutoFire = false;

    public override void _Ready()
    {
        base._Ready();
        PlungerProgress.MaxValue = MaxStrength;
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        if (AutoFire)
        {
            PlungerProgress.Value = MaxStrength / 2;
        }
        else if (Input.IsActionPressed("plunger"))
        {
            PlungerProgress.Value += MaxStrength / 2 * delta;
        }
    }

    public override void _Input(InputEvent @event)
    {
        if (AutoFire) return;
        if (@event.IsActionPressed("plunger"))
        {
            EmitSignal(SignalName.WindingUp);
        }
        if (@event.IsActionReleased("plunger"))
        {
            ReleasePlunger();
        }
    }

    void OnDetectionZoneBodyEnter(Node2D body)
    {
        if (body is Ball && AutoFire)
        {
            AutoFire = false;
            ReleasePlunger();
        }
    }

    void ReleasePlunger()
    {
        EmitSignal(SignalName.Releasing);
        foreach (RigidBody2D ball in DetectionZone.GetOverlappingBodies().Where(b => b is Ball))
        {
            ball.LinearVelocity = Vector2.Up * (int)PlungerProgress.Value;
        }
        PlungerProgress.Value = 0;
    }
}
