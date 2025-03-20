using Godot;
using System;

public partial class ScrollContainerMax : ScrollContainer
{

    [Export]
    Vector2 CustomMaximumSize;

    Control Child;

    public override void _Ready()
    {
        base._Ready();
        Child = GetChild<Control>(0);
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        // TODO: Is expensive for somethig that only runs once, but in _Ready() the size
        // of the balletration effects container in the ballteration card is wrong...
        CustomMinimumSize = new Vector2(Math.Min(CustomMaximumSize.X, Child.Size.X), Math.Min(CustomMaximumSize.Y, Child.Size.Y));
    }


}
