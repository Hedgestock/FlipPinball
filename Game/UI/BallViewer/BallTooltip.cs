using Godot;
using System;

public partial class BallTooltip : Window
{
    [Export]
    Container Content;
    void OnShow()
    {
        Position = new Vector2I(
            (int)Math.Min(((Control)GetParent()).GlobalPosition.X, GetTree().Root.GetViewport().GetVisibleRect().Size.X - Size.X),
            (int)Math.Min(((Control)GetParent()).GlobalPosition.Y, GetTree().Root.GetViewport().GetVisibleRect().Size.Y - Size.Y));
    }
}
