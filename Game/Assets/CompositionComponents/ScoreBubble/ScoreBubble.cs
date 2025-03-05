using Godot;
using System;

public partial class ScoreBubble : Label
{
    public override void _Ready()
    {
        base._Ready();
        Tween tween = GetTree().CreateTween();
        tween.TweenProperty(this, "position", Position + Vector2.Up * 20, .5f)
            .SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Back);
        tween.TweenCallback(Callable.From(QueueFree));
    }
}
