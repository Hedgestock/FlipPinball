using Godot;
using System;

public partial class SplashScreen : ColorRect
{
    [Export]
    Label Wafflestock;

    public override void _Ready()
    {
        Tween tween = GetTree().CreateTween();
        tween.TweenProperty(Wafflestock, "modulate", new Color("ffffff"), 1);
        tween.TweenProperty(this, "modulate", new Color("ffffff"), 1);
        tween.TweenProperty(this, "modulate", new Color("ffffff00"), 1);
        tween.TweenCallback(Callable.From(QueueFree));
    }
}
