using Godot;
using System;
using System.Collections.Generic;
public partial class Bumper : Node2D
{
    [Signal]
    public delegate void BumpingEventHandler(int level);

    [Export]
    Sprite2D Sprite;

    OnOffLight OnOffLight;

    public override void _Ready()
    {
        base._Ready();
        OnOffLight = GetNode<OnOffLight>("OnOffLight");
        OnOffLight.Modulate = Colors.LightGray;
    }

    int Level = 0;

    private void SetLevel(int value)
    {
        Level = value;
        switch (value)
        {
            default:
                OnOffLight.Modulate = Colors.LightGray;
                break;
            case 1:
                OnOffLight.Modulate = Colors.Green;
                break;
            case 2:
                OnOffLight.Modulate = Colors.Blue;
                break;
            case 3:
                OnOffLight.Modulate = Colors.Red;
                break;
            case 4:
                OnOffLight.Modulate = Colors.DarkViolet;
                break;
        }
    }


    void Bump()
    {
        Tween tween = GetTree().CreateTween();
        tween.TweenProperty(Sprite, "scale", Vector2.One * 1.2f, .05)
            .SetEase(Tween.EaseType.Out)
            .SetTrans(Tween.TransitionType.Elastic);
        tween.TweenProperty(Sprite, "scale", Vector2.One, .05)
           .SetEase(Tween.EaseType.In)
           .SetTrans(Tween.TransitionType.Elastic);

        EmitSignal(SignalName.Bumping, Level + 1);
    }
}
