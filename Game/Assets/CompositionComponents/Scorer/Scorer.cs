using Godot;
using System;
using System.Linq;

public partial class Scorer : Node2D
{
    [Export]
    public int Value;

    [Export]
    ScoreBubble ScoreBubbleScene;

    public override void _Ready()
    {
        base._Ready();
        foreach (var group in GetParent().GetGroups())
            AddToGroup(group);
    }

    public void Score(Ball ball)
    {
        Score(ball, Value);
    }

    bool CheckEligibility(ScoreModifier modifier)
    {
        var groups = modifier.GetGroups();

        if (groups.Contains("Global")) return true;

        var intersection = groups.Intersect(GetGroups());
        if (modifier.Restrictive)
            return intersection.Count() == groups.Count();
        return intersection.Any();
    }

    public void Score(Ball ball, int value)
    {
        float superAdder = 0;
        float multiplier = 1;
        float adder = 0;
        float superMultiplier = 1;

        foreach (ScoreModifier scoreModifier in ball.GetChildren().OfType<Ballteration>().SelectMany(p => p.GetChildren().Where(c => c is ScoreModifier sm && CheckEligibility(sm))))
        {
            switch (scoreModifier.Prio)
            {
                case ScoreModifier.Priority.SuperAdder:
                    superAdder += scoreModifier.Value;
                    break;
                case ScoreModifier.Priority.Multiplier:
                    multiplier *= scoreModifier.Value;
                    break;
                case ScoreModifier.Priority.Adder:
                    adder += scoreModifier.Value;
                    break;
                case ScoreModifier.Priority.SuperMultiplier:
                    superMultiplier *= scoreModifier.Value;
                    break;
            }
        }
        Score((int)((((value + superAdder) * multiplier) + adder) * superMultiplier));
    }

    public void Score(int score)
    {
        if (score == 0) return;

        int actualScore = ScoreManager.BoardScore(score);

        if (actualScore == 0) return;

        ScoreBubbleScene.DisplayScore(actualScore);
    }
}
