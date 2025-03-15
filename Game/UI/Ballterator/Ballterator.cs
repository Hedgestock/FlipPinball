using Godot;
using System;

public partial class Ballterator : ScrollContainer
{
    [Export]
    FlowContainer Container;
    public override void _Ready()
    {
        base._Ready();
        CallDeferred(MethodName.OnScreenResize);
        GetTree().Root.Connect(Viewport.SignalName.SizeChanged, new Callable(this, MethodName.OnScreenResize));
    }

    void OnScreenResize()
    {
        Vector2 screenSize = GetViewport().GetVisibleRect().Size;

        //Container.Vertical = screenSize.Y < screenSize.X;
    }
}
