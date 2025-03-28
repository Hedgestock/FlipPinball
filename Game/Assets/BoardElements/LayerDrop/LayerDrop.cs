using Godot;
using System;
using System.Reflection.Emit;

public partial class LayerDrop : Node2D
{
    [Export]
    int LayerFrom;
    [Export]
    int LayerTo;

    SceneTreeTimer Timer;

    Ball BallIn;

    private void OnDropBodyEntered(Node2D body)
    {
        if (body is Ball ball)
        {
            BallIn = ball;
            Timer = GetTree().CreateTimer(1);
            Timer.Timeout +=  DropBall;
        }
    }

    private void OnDropBodyExited(Node2D body)
    {
        if (body == BallIn)
        {
            Timer.Timeout -=  DropBall;
            BallIn = null;
        }
    }

    private void DropBall()
    {
        BallIn.SetCollision(LayerTo, true);
        BallIn.SetCollision(LayerFrom, false);
    }
}
