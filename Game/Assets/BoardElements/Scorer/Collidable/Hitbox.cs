using Godot;
using System;
using System.Collections.Generic;

public partial class Hitbox : StaticBody2D
{
    [Signal]
    public delegate void HitEventHandler(Ball ball);

    [Signal]
    public delegate void OverheatEventHandler();

    [Signal]
    public delegate void OverheatEndEventHandler();

    [Export]
    private uint TriggerSpeed;
    [Export]
    uint HitsOverheat;
    [Export]
    uint MSToOverheat;
    [Export]
    double OverheatDuration;

    private bool _isActive = true;

    public void Activate(bool value)
    {
        _isActive = value;
    }

    Queue<DateTime> LastHits = new Queue<DateTime>();

    public virtual void CollideWithBall(Ball ball)
    {
        if (!_isActive) return;
        Vector2 direction = (ball.GlobalPosition - this.GlobalPosition).Normalized();

        // Here we estimate the force of the impact by projecting the speed of the ball on the direction vector
        // It might not work that well if the ball hits almost tangentially to the bumper but it should be good enough
        if ((ball.LinearVelocity.Dot(direction) * direction).Length() < TriggerSpeed) return;

        AddHit();
        EmitSignal(SignalName.Hit, ball);
    }

    // This system ensures that no ball gets softlocked between Hitboxes and/or walls
    protected virtual void AddHit()
    {
        LastHits.Enqueue(DateTime.Now);

        if (LastHits.Count <= HitsOverheat) return;

        DateTime OldestValue = LastHits.Dequeue();

        if ((DateTime.Now - OldestValue).TotalMilliseconds > MSToOverheat) return;

        EmitSignal(SignalName.Overheat);
        _isActive = false;

        GetTree().CreateTimer(OverheatDuration).Timeout += () => { _isActive = true; EmitSignal(SignalName.OverheatEnd); };
    }
}
