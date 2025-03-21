using Godot;
using System;
using System.Collections.Generic;

public partial class TutorialPopup : Window
{
    public override void _Ready()
    {
        base._Ready();
        SizeChanged += () => Position = new Vector2I(Position.X, (1080 - Size.Y)/2);
    }
}
