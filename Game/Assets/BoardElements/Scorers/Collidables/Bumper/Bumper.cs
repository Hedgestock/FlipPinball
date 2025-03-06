using Godot;
using System;
using System.Collections.Generic;
public partial class Bumper : Node2D
{
    [Signal]
    public delegate void BumpingEventHandler(int level);

    [Export]
    Sprite2D Sprite;

    int Level = 0;

    private void SetLevel(int value)
    {
        Level = value;
        switch (value)
        {
            default:
                Sprite.Modulate = Colors.White;
                break;
            case 1:
                Sprite.Modulate = Colors.ForestGreen;
                break;
            case 2:
                Sprite.Modulate = Colors.MediumBlue;
                break;
            case 3:
                Sprite.Modulate = Colors.Red;
                break;
            case 4:
                Sprite.Modulate = Colors.Black;
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
