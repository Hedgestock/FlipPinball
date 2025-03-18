using Godot;
using System;

public partial class ScrollContainerMax : ScrollContainer
{

    [Export]
    Vector2 CustomMaximumSize;

    public override void _Ready()
    {
        base._Ready();
        Control child = GetChild<Control>(0);
        CustomMinimumSize = new Vector2(Math.Min(CustomMaximumSize.X, child.Size.X), Math.Min(CustomMaximumSize.Y, child.Size.Y));
    }
}
