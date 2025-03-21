using Godot;
using System;

public partial class BallTooltip : Window
{
    [Export]
    Container Content;
    void OnShow()
    {
        Position = (Vector2I)((Control)GetParent()).GlobalPosition;
    }
}
