using Godot;
using System.Linq;

public partial class Ball : RigidBody2D
{
    [Signal]
    public delegate void SelfDestructEventHandler();

    [Export]
    Line2D Trail;
    [Export]
    Area2D Center;

    public override void _Ready()
    {
        base._Ready();

        LastPosition = GlobalPosition;
        if (ProcessMode != ProcessModeEnum.Disabled)
        {
            ResetTrail();
        }
        else
        {
            Trail.Points = [new(0, 0), new(20, 20)];
        }
    }

    Vector2 LastVelocity;
    Vector2 LastCollisionNormal;
    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        var collisionInfo = MoveAndCollide(LinearVelocity * (float)delta, true);
        if (collisionInfo != null)
        {
            LastCollisionNormal = collisionInfo.GetNormal();
        }

        LastVelocity = LinearVelocity;

        //TrailProcessing(delta);
    }


    Vector2[] LastPoints;
    Vector2 LastPosition;
    Vector2 LastProcessedPosition;
    double TimeSinceTrailProcessing = 0;
    double TrailTime = 1;
    int TrailDetail = 60;
    void TrailProcessing(double delta)
    {
        Trail.GlobalRotation = 0;
        LastPoints[0] = Vector2.Zero;
        TimeSinceTrailProcessing += delta;

        if (TimeSinceTrailProcessing >= TrailTime / Trail.Points.Length)
        {
            for (int i = 1; i < Trail.Points.Length; i++)
            {
                LastPoints[i] = Trail.Points[i - 1];
            }
            TimeSinceTrailProcessing -= TrailTime / Trail.Points.Length;
        }

        for (int i = 1; i < Trail.Points.Length; i++)
        {
            LastPoints[i] += (LastPosition - GlobalPosition);
        }

        LastPosition = GlobalPosition;
        Trail.Points = LastPoints;
        LastPoints = Trail.Points;
    }

    public void OnCollision(Node body)
    {
        if (body is Hitbox hitbox) hitbox.CollideWithBall(this, LastVelocity, LastCollisionNormal);
    }

    public void SetCollision(int layer, bool value)
    {
        SetCollisionLayerValue(layer, value);
        SetCollisionMaskValue(layer, value);
        Center.SetCollisionLayerValue(layer, value);


        // Kinda magic but I look at the possition of the most significant bit to get the "height" of the ball
        int highestSetLayer = 32 - System.Numerics.BitOperations.LeadingZeroCount(CollisionLayer);
        // -2 because the lowest board layer is 2
        ZIndex = (highestSetLayer - 2) * 10 + 3;

        //GD.Print($"ZIndex:{ZIndex} {(value? "set": "unset")}layer:{layer} hsl:{highestSetLayer} cl:{CollisionLayer}");
    }

    public void ResetTrail()
    {
        Trail.Points = Enumerable.Repeat(Vector2.Zero, TrailDetail).ToArray();
        LastPoints = Trail.Points;
    }

    public static Ball GetFreshBall()
    {
        Ball ball = GD.Load<PackedScene>("res://Game/Assets/Ball/Ball.tscn").Instantiate<Ball>();
        ball.GetNode<Sprite2D>("Sprite2D").Modulate = new(new Color(GD.Randi()), 1);
        ball.GetNode<Line2D>("Trail").Modulate = new(new Color(GD.Randi()), 1);
        return ball;
    }
}
