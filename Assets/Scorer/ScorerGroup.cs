using Godot;
using System.Collections.Generic;
using System;
using System.Linq;

public partial class ScorerGroup : Node2D
{
    List<Scorer> Scorers;

    public override void _Ready()
    {
        base._Ready();
        Scorers = GetChildren().Where(c => c is Scorer).Cast<Scorer>().ToList();
    }

    public void RotateStatus(int direction)
    {

        bool[] statuses = Scorers.Select(s => s.IsOn).ToArray();

        foreach (var (scorer, i) in Scorers.Select((scorer, i) => (scorer, i)))
        {
            scorer.IsOn = statuses[Mathf.PosMod(i + direction, statuses.Length)];
        }
    }
}