using Godot;
using Godot.Collections;
using System.Collections.Generic;

public partial class Board : Node2D
{
    [Export]
    PackedScene Ball;

    [Export]
    float PaddleSpeed = 25;

    [Export]
    Node2D Plunger;
    [Export]
    public Array<CharacterBody2D> PaddlesLeft = new Array<CharacterBody2D>();
    [Export]
    public Array<CharacterBody2D> PaddlesRight = new Array<CharacterBody2D>();

    private List<RigidBody2D> LiveBalls = new List<RigidBody2D>();

    private Vector2 LaunchPos;

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("load_ball"))
        {
            LoadBall();
        }
        if (@event.IsActionPressed("paddle_left"))
        {
            PaddleAdditionnalBehaviour(true);
        }
        if (@event.IsActionPressed("paddle_right"))
        {
            PaddleAdditionnalBehaviour(false);
        }

        // This is for testing purposes
        if (@event is InputEventMouseButton eventMouseButton)
        {
            if (@event.IsActionPressed("screen_tap"))
            {
                LaunchPos = @eventMouseButton.Position;
            }
            if (@event.IsActionReleased("screen_tap"))
            {
                RigidBody2D ball = Ball.Instantiate<RigidBody2D>();
                ball.Position = LaunchPos;
                ball.LinearVelocity = (@eventMouseButton.Position - LaunchPos) * 10;
                LiveBalls.Add(ball);
                AddChild(ball);
            }
        }
    }

    public virtual void ResetBoard()
    {

    }

    private void LoadBall()
    {
        if (LiveBalls.Count != 0) return;
        RigidBody2D ball = Ball.Instantiate<RigidBody2D>();
        ball.Position = Plunger.Position;
        LiveBalls.Add(ball);
        AddChild(ball);
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        if (Input.IsActionPressed("paddle_left")) { RotatePaddle(delta, Mathf.DegToRad(-60), true); }
        else { RotatePaddle(delta, 0, true); }

        if (Input.IsActionPressed("paddle_right")) { RotatePaddle(delta, Mathf.DegToRad(60), false); }
        else { RotatePaddle(delta, 0, false); }
    }

    private void RotatePaddle(double delta, double angle, bool left)
    {
        Array<CharacterBody2D> paddles = left ? PaddlesLeft : PaddlesRight;

        GD.Print("test ,", paddles.Count);

        foreach (var paddle in paddles)
        {
            paddle.Rotation = (float)Mathf.RotateToward(paddle.Rotation, angle, delta * PaddleSpeed);
            GD.Print(angle);
        }
    }

    protected virtual void PaddleAdditionnalBehaviour(bool left) { }

    private void OnEnterDrain(Node2D body, bool oob)
    {
        GD.Print($"draining {body.GetType()}");
        if (!(body is Ball)) return;
        GD.Print($"OOB: {oob}");

        if (oob) { GD.PrintErr($"Ball {body} OOB"); }
        LiveBalls.Remove(body as Ball);
        body.QueueFree();
    }
}
