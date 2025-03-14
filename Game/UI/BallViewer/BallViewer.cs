using Godot;
using System;

public partial class BallViewer : SubViewportContainer
{
    [Export]
    SubViewport SubViewport;

    private Ball _ball = null;

    public Ball Ball
    {
        get { return _ball; }
        set
        {   
            if (_ball != null)
                Ball.QueueFree();
            value.ProcessMode = ProcessModeEnum.Disabled;
            value.GlobalPosition = Vector2.One * 12;
            SubViewport.AddChild(value);
            _ball = value;
        }
    }

    public override void _Ready()
    {
        base._Ready();
    }
}
