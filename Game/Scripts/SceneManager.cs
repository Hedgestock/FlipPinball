using Godot;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;

public partial class SceneManager : Node
{

    protected static SceneManager _instance;
    public static SceneManager Instance { get { return _instance; } }

    public SceneManager()
    {
        if (_instance != null)
            return;
        _instance = this;
    }

    public static string PrevScene = "";

    static public void ChangeSceneToFile(string file)
    {
        PrevScene = _instance.GetTree().CurrentScene.SceneFilePath;
        _instance.GetTree().ChangeSceneToFile(file);
    }

    static public void ChangeSceneToPacked(PackedScene scene)
    {
        PrevScene = _instance.GetTree().CurrentScene.SceneFilePath;
        _instance.GetTree().ChangeSceneToPacked(scene);
    }

    static public void GoToPreviousScene()
    {
        var _tmpPrevScene = _instance.GetTree().CurrentScene.SceneFilePath;
        _instance.GetTree().ChangeSceneToFile(PrevScene);
        PrevScene = _tmpPrevScene;
    }
       }
