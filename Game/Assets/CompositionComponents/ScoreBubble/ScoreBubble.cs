using Godot;
using System;

public partial class ScoreBubble : Label
{
    int CachedScore = 0;

    Tween Tween;
    public void DisplayScore(int score)
    {
        if (Tween != null) { Tween.Kill(); }

        Position = Vector2.Zero;
        Text = score.ToString("+0;-#");

        Show();
        Tween = GetTree().CreateTween();
        Tween.TweenProperty(this, "position", Vector2.Up * 20, .5f)
            .SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Back);
        Tween.TweenCallback(Callable.From(Hide));
    }
}
