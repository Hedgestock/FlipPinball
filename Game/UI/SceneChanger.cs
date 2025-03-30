using Godot;
using System;

public partial class SceneChanger : Button
{
    [Export]
    PackedScene Scene;
    public override void _Ready()
    {
        base._Ready();
        Pressed += () => SceneManager.ChangeSceneToPacked(Scene);
    }
}
