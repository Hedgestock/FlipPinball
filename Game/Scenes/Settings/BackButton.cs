using Godot;
using System;

public partial class BackButton : Button
{
    public override void _Ready()
    {
        base._Ready();
        Pressed += () => SceneManager.ChangeSceneToFile("res://Game/Scenes/Home.tscn");
    }
}
