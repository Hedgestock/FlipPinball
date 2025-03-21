using Godot;
using System;
using System.Linq;

public partial class BallViewer : TextureRect
{

    [Export]
    PackedScene BallterationViewerScene;

    BallTooltip Tooltip;

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

            Tooltip = GD.Load<PackedScene>("res://Game/UI/BallViewer/BallTooltip.tscn").Instantiate<BallTooltip>();

            ViewportTexture ballMirror = new();
            ballMirror.ViewportPath = "SubViewport";
            Tooltip.BallMirror.Texture = ballMirror;

            var ballterations = value.GetChildren().OfType<Ballteration>();
            if (ballterations.Any())
            {
                foreach (var ballteration in ballterations)
                {
                    BallterationViewer viewer = BallterationViewerScene.Instantiate<BallterationViewer>();
                    viewer.Ballteration = ballteration;
                    Tooltip.Content.AddChild(viewer);
                }
            }
            else
            {
                Label noBallterations = new();
                noBallterations.Text = "Ball hasn't been altered, no ballterations yet.";
                noBallterations.AutowrapMode = TextServer.AutowrapMode.WordSmart;
                Tooltip.Content.AddChild(noBallterations);
            }
        }
    }

    void TooltipHandler(InputEvent @event)
    {
        if (@event is InputEventMouseButton eventMouseButton)
        {
            if (@event.IsActionPressed("screen_tap"))
            {
                AddChild(Tooltip);
                Tooltip.Show();
            }
        }
    }
}
