using Godot;
using System;

public partial class Spinner : Node2D
{
    [Signal]
    public delegate void CompleteRotationEventHandler(int level);

    int Level = 1;

    double InitialSpinSpeed = 0;
    double LastSpinSpeed = 0;
    double Iterations = -1;

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        if (Iterations < 0) return;

        Iterations += delta;
        double currentSpinSpeed = InitialSpinSpeed * Math.Pow(0.6, Iterations);
        int numberOfTurns = (int)(LastSpinSpeed - currentSpinSpeed);
        if (numberOfTurns > 0)
        {
            for (int i = 0; i < numberOfTurns; i++)
            {
                EmitSignal(SignalName.CompleteRotation, Level);
            }
            LastSpinSpeed = currentSpinSpeed;
        }

        if (currentSpinSpeed < 1)
        {
            Iterations = -1;
        }
    }

    private void OnSpinnerBodyEnter(Node2D body)
    {
        if (body is Ball ball)
        {
            Iterations = 1;
            InitialSpinSpeed = Math.Abs(Math.Cos(ball.LinearVelocity.AngleTo(Vector2.Down.Rotated(Rotation))) * ball.LinearVelocity.Length()) / 50;
            LastSpinSpeed = InitialSpinSpeed;
        }
    }

    private void SetLevel(int value)
    {
        Level = value;
        //switch (value)
        //{
        //    default:
        //        Sprite.Modulate = Colors.White;
        //        break;
        //    case 2:
        //        Sprite.Modulate = Colors.ForestGreen;
        //        break;
        //    case 3:
        //        Sprite.Modulate = Colors.MediumBlue;
        //        break;
        //    case 4:
        //        Sprite.Modulate = Colors.Red;
        //        break;
        //    case 5:
        //        Sprite.Modulate = Colors.Black;
        //        break;
        //}
    }
}
