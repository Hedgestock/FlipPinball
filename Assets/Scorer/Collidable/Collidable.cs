using Godot;
using System;
using System.Collections.Generic;

public abstract partial class Collidable : Scorer
{
    [Export]
    protected uint TriggerSpeed = 50;
    [Export]
    protected uint Strength = 1000;
    [Export]
    uint HitsOverheat = 10;
    [Export]
    uint MSToOverheat = 1200;
    [Export]
    double OverheatDuration = 1;

    Queue<DateTime> LastHits = new Queue<DateTime>();

    public override void _Ready()
    {
        base._Ready();
    }
    public virtual void CollideWithBall(Ball ball)
    {
        if (!IsOn) return;
        AddHit();
    }

    // This system ensures that no ball gets softlocked between 2 Collidables or a Collidable and a wall
    protected virtual void AddHit()
    {
        LastHits.Enqueue(DateTime.Now);

        if (LastHits.Count <= HitsOverheat) return;

        DateTime OldestValue = LastHits.Dequeue();

        if ((DateTime.Now - OldestValue).TotalMilliseconds > MSToOverheat) return;

        IsOn = false;

        GetTree().CreateTimer(OverheatDuration).Timeout += () => SetOnValue(true);
    }
}
