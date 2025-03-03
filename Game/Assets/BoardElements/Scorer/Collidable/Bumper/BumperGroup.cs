using Godot;
using System.Collections.Generic;
using System.Linq;

public partial class BumperGroup : Node2D
{
    int _level = 1;

    public int Level
    {
        get { return _level; }
        set {
            Bumpers.ForEach(b => b.Level = value);
            _level = value;
        }
    }

    List<Bumper> Bumpers;

    public override void _Ready()
    {
        base._Ready();
        Bumpers = GetChildren().Where(c => c is Bumper).Cast<Bumper>().ToList();
    }
}
