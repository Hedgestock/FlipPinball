using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Tunnel : Node2D
{
    [Export]
    Leveler Leveler;

    List<OnOffLight> Lights;

    public override void _Ready()
    {
        base._Ready();
        Lights = GetChildren().Where(c => c is OnOffLight).Cast<OnOffLight>().ToList();
        Leveler.MaxLevel = Lights.Count;
        Leveler.MinLevel = 0;
        Leveler.Level = 0;
    }

    private void SetLevel(int level)
    {
        Lights[level - 1].TurnOn();
    }
}
