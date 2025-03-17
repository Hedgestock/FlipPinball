using Godot;
using System;

public partial class SplashScreen : ColorRect
{
    [Export]
    Label Wafflestock;
    [Export]
    VideoStreamPlayer Logo;

    public override void _Ready()
    {
        if (SceneManager.PrevScene != "") return;
        this.Visible = true;
        Logo.Play();
        Tween tween = GetTree().CreateTween();
        tween.TweenProperty(Wafflestock, "modulate", new Color("ffffff"), .5);
        tween.TweenProperty(this, "modulate", new Color("ffffff"), 1);
        tween.TweenProperty(this, "modulate", new Color("ffffff00"), .5);
        tween.TweenCallback(Callable.From(QueueFree));
    }
}
