using Godot;
using System;

public partial class SceneChanger : Button
{
    [Export]
    string ScenePath;
    public override void _Ready()
    {
        base._Ready();
        Pressed += () => GameManager.Instance.GetTree().ChangeSceneToFile(ScenePath);
    }

}
