using Godot;
using System;
using System.Collections.Generic;
public partial class Bumper : Node2D
{
    [Signal]
    public delegate void BumpEventHandler(int level);

    [Export]
    Sprite2D Sprite;

    [Export]
    uint Strength = 1000;

    int Level = 1;

    public bool LevelUp()
    {
        if (Level >= 5) return false;

        Level++;

        switch (Level)
        {
            default:
                Sprite.Modulate = Colors.White;
                break;
            case 2:
                Sprite.Modulate = Colors.ForestGreen;
                break;
            case 3:
                Sprite.Modulate = Colors.MediumBlue;
                break;
            case 4:
                Sprite.Modulate = Colors.Red;
                break;
            case 5:
                Sprite.Modulate = Colors.Black;
                break;
        }

        return true;
    }

    void BumpBall(Ball ball)
    {
        Vector2 direction = (ball.GlobalPosition - this.GlobalPosition).Normalized();

        ball.LinearVelocity += direction * Strength;

        Tween tween = GetTree().CreateTween();
        tween.TweenProperty(Sprite, "scale", Vector2.One * 1.2f, .05)
            .SetEase(Tween.EaseType.Out)
            .SetTrans(Tween.TransitionType.Elastic);
        tween.TweenProperty(Sprite, "scale", Vector2.One, .05)
           .SetEase(Tween.EaseType.In)
           .SetTrans(Tween.TransitionType.Elastic);

        EmitSignal(SignalName.Bump, Level);
    }
}
