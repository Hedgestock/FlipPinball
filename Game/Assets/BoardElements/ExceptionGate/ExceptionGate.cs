using Godot;
using System;

public partial class ExceptionGate : StaticBody2D
{
    private void AddException(Ball ball)
    {
        ball.AddCollisionExceptionWith(this);
    }

    private void RemoveException(Ball ball)
    {
        ball.RemoveCollisionExceptionWith(this);
    }
}
