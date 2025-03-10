using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class DecayingLeveler : Node2D
{
    [Export]
    Leveler Leveler;

    List<OnOffLight> Lights;

    public override void _Ready()
    {
        base._Ready();
        Lights = FindChildren("*").Where(c => c is OnOffLight).Cast<OnOffLight>().ToList();
        if (Lights.Count == 0) return;
        Leveler.MaxLevel = Lights.Count;
        Leveler.MinLevel = 0;
        Leveler.CurrentLevel = 0;
    }

    private void HandleLights(int level)
    {
        for (int i = 0; i < Lights.Count; i++)
        {
            Lights[i].IsOn = i < level;
        }
    }
}
