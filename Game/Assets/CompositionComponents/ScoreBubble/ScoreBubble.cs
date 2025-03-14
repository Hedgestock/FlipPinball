using Godot;
using System;

public partial class ScoreBubble : Label
{
    public void Display(string text)
    {
        Position = Vector2.Zero;
        Text = text;
        Show();
        Tween tween = GetTree().CreateTween();
        tween.TweenProperty(this, "position", Vector2.Up * 20, .5f)
            .SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Back);
        tween.TweenCallback(Callable.From(Hide));
    }
}
