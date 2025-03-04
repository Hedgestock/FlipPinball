using Godot;
using System;

public partial class Spitter : Scorer
{
    [Export]
    uint Strength = 1000;
    [Export]
    Timer SpitDelay;
    [Export]
    RayCast2D SpitDirection;
    [Export]
    AudioStreamPlayer EntrySoundPlayer;

    Ball StoredBall;

    private void OnBodyEnter(Node2D body)
    {
        if (!IsOn || !(body is Ball)) return;
        StoredBall = (Ball)body;
        Tween tween = GetTree().CreateTween();
        tween.TweenProperty(StoredBall, "global_position", GlobalPosition, .5f)
            .SetEase(Tween.EaseType.Out)
            .SetTrans(Tween.TransitionType.Elastic);
        StoredBall.SetDeferred(RigidBody2D.PropertyName.Freeze, true);
        SpitDelay.Start();
        IsOn = false;
        EntrySoundPlayer.Play();
    }

    private void SpitBall()
    {
        if (StoredBall == null)
        {
            GD.PrintErr($"Spitter {this} tried to spit while empty");
            return;
        }
        StoredBall.SetDeferred(RigidBody2D.PropertyName.Freeze, false);
        StoredBall.LinearVelocity = (SpitDirection.TargetPosition - SpitDirection.Position).Normalized() * Strength;
        StoredBall = null;
        IsOn = true;
        Score();
    }
}
