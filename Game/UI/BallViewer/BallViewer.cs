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
            var ballterations = value.GetChildren().OfType<Ballteration>();

            if (ballterations.Any())
            {
                foreach (var ballteration in ballterations)
                {
                    BallterationViewer viewer = BallterationViewerScene.Instantiate<BallterationViewer>();
                    viewer.Ballteration = ballteration;
                    ballterationDisplayer.AddChild(viewer);
                }
            }
            else
            {
                Label noBallterations = new ();
                noBallterations.Text = "Ball hasn't been altered, no ballterations yet.";
                noBallterations.AutowrapMode = TextServer.AutowrapMode.WordSmart;
                ballterationDisplayer.AddChild(noBallterations);
            }
        }
    }

    void OnMouseEnter()
    {
        Tooltip.Show();
    }
}
