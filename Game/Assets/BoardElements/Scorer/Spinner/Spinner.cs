using Godot;
using System;

public partial class Spinner : Scorer
{
    private double _initialSpinSpeed = 0;
    private double _lastSpinSpeed = 0;
    private double _iterations = -1;

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        if (_iterations < 0) return;

        _iterations += delta;
        double currentSpinSpeed = _initialSpinSpeed * Math.Pow(0.6, _iterations);
        int numberOfTurns = (int)(_lastSpinSpeed - currentSpinSpeed);
        if (numberOfTurns > 0)
        {
            for (int i = 0; i < numberOfTurns; i++)
            {
                Score();
            }
            _lastSpinSpeed = currentSpinSpeed;
        }

        if (currentSpinSpeed < 1)
        {
            _iterations = -1;
        }
    }

    private void OnSpinnerBodyEnter(Node2D body)
    {
        if (body is Ball ball)
        {
            _iterations = 1;
            _initialSpinSpeed = Math.Abs(Math.Cos(ball.LinearVelocity.AngleTo(Vector2.Down.Rotated(Rotation))) * ball.LinearVelocity.Length()) / 50;
            _lastSpinSpeed = _initialSpinSpeed;
        }
    }
}
