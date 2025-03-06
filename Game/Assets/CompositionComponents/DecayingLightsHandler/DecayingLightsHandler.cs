using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class DecayingLightsHandler : Node
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
        Leveler.CurrentLevel = 0;
    }

    private void SetLevel(int level)
    {
        for (int i = 0; i < Lights.Count; i++)
        {
            Lights[i].IsOn = i < level;
        }
    }
}
