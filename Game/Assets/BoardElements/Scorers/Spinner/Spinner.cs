using Godot;
using System;

public partial class Spinner : Node2D
{
    [Signal]
    public delegate void CompleteRotationEventHandler(int level);

    double InitialSpinSpeed = 0;
    double LastSpinSpeed = 0;
    double Iterations = -1;

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        if (Iterations < 0) return;

        Iterations += delta;
        double currentSpinSpeed = InitialSpinSpeed * Math.Pow(0.3, Iterations) - 0.7 ;
        int numberOfTurns = (int)(LastSpinSpeed - currentSpinSpeed);
        if (numberOfTurns >= 1)
        {
            for (int i = 0; i < numberOfTurns; i++)
            {
                EmitSignal(SignalName.CompleteRotation, ((Leveler)GetParent()).CurrentLevel);
            }

            LastSpinSpeed = currentSpinSpeed;
        }

        if (currentSpinSpeed < 0)
        {
            Iterations = -1;
        }
    }

    private void OnSpinnerBodyEnter(Node2D body)
    {
        if (body is Ball ball)
        {
            Iterations = 0;
            InitialSpinSpeed = Math.Abs(Math.Cos(ball.LinearVelocity.AngleTo(Vector2.Down.Rotated(Rotation))) * ball.LinearVelocity.Length()) / 200;
            LastSpinSpeed = InitialSpinSpeed;
        }
    }
}
