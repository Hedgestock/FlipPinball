using Godot;
using System;
using System.Linq;

public partial class BallViewer : TextureRect
{

    [Export]
    PackedScene BallterationViewerScene;

    [Export]
    Window Tooltip;
    [Export]
    SubViewport SubViewport;

    private Ball _ball = null;

    public Ball Ball
    {
        set
        {   
            if (_ball != null)
                _ball.QueueFree();
            _ball = value;

            value.ProcessMode = ProcessModeEnum.Disabled;
            value.GlobalPosition = Vector2.One * 12;
            SubViewport.AddChild(value);

            VBoxContainer ballterationDisplayer = (VBoxContainer)Tooltip.FindChild("VBoxContainer");

            foreach(var ballteration in value.GetChildren().OfType<Ballteration>())
            {
                BallterationViewer viewer = BallterationViewerScene.Instantiate<BallterationViewer>();
                viewer.Ballteration = ballteration;
                ballterationDisplayer.AddChild(viewer);
            }
        }
    }

    void OnMouseEnter()
    {
        Tooltip.Show();
    }
}
