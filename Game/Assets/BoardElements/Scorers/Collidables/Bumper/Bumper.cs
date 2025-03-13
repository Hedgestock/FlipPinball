using Godot;
using System;
using System.Collections.Generic;
public partial class Bumper : Node2D
{
    [Export]
    Sprite2D Sprite;
    [Export]
    OnOffLight OnOffLight;
    [Export]
    Scorer Scorer;

    public override void _Ready()
    {
        base._Ready();
        OnOffLight.Modulate = Colors.LightGray;
    }

    int Level = 1;

    private void SetLevel(int value)
    {
        Level = value;
        switch (value)
        {
            default:
                OnOffLight.Modulate = Colors.LightGray;
                break;
            case 2:
                OnOffLight.Modulate = Colors.Green;
                break;
            case 3:
                OnOffLight.Modulate = Colors.Blue;
                break;
            case 4:
                OnOffLight.Modulate = Colors.Red;
                break;
            case 5:
                OnOffLight.Modulate = Colors.DarkViolet;
                break;
        }
    }


    void Bump(Ball ball)
    {
        Scorer.Score(ball, Scorer.Value * Level);

        Tween tween = GetTree().CreateTween();
        tween.TweenProperty(Sprite, "scale", Vector2.One * 1.2f, .05)
            .SetEase(Tween.EaseType.Out)
            .SetTrans(Tween.TransitionType.Elastic);
        tween.TweenProperty(Sprite, "scale", Vector2.One, .05)
           .SetEase(Tween.EaseType.In)
           .SetTrans(Tween.TransitionType.Elastic);
    }
}
