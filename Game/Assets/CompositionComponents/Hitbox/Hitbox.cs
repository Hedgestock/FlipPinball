using Godot;
using System;
using System.Collections.Generic;

public partial class Hitbox : StaticBody2D
{
    [Signal]
    public delegate void HitEventHandler(Ball ball, Vector2 CollisionNormal);

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

    public virtual void CollideWithBall(Ball ball, Vector2 ballVelocity, Vector2 collisionNormal)
    {
        if (!_isActive) return;

        // Here we estimate the force of the impact by projecting the speed of the ball
        // on the normal of the collided shape (which should be the hitbox itself) at point of impact.
        if (ballVelocity.Dot(collisionNormal) > TriggerSpeed) return;

        AddHit();

        EmitSignal(SignalName.Hit, ball, collisionNormal);
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
