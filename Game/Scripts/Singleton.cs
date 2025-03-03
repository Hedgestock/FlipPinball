using Godot;
using System;

public partial class Singleton : Node
{
    protected static Singleton _instance;
    public static Singleton Instance { get { return _instance; } }

    public Singleton()
    {
        if (_instance != null)
            return;
        _instance = this;
    }
}
