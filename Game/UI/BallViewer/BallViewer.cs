using Godot;
using System;

public partial class BallViewer : TextureRect
{
    [Export]
    SubViewport SubViewport;

    private Ball _ball = null;

    public Ball Ball
    {
        set
        {   
            if (_ball != null)
                _ball.QueueFree();
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
