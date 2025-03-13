using Godot;
using System;
using System.Linq;

public partial class Scorer : Node2D
{
    [Export]
    public int Value;

    [Export]
    PackedScene ScoreBubbleScene;

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
        var intersection = modifier.GetGroups().Intersect(GetGroups());
        if (modifier.Restrictive)
            return intersection.Count() == modifier.GetGroups().Count();
        return intersection.Any();
    }

    public void Score(Ball ball, int value)
    {
        int superAdder = 0;
        int multiplier = 1;
        int adder = 0;
        int superMultiplier = 1;

        foreach (ScoreModifier ballteration in ball.GetChildren().Where(c => c is ScoreModifier sm && CheckEligibility(sm)))
        {
            switch (ballteration.Prio)
            {
                case ScoreModifier.Priority.SuperAdder:
                    superAdder += ballteration.Value;
                    break;
                case ScoreModifier.Priority.Multiplier:
                    multiplier *= ballteration.Value;
                    break;
                case ScoreModifier.Priority.Adder:
                    adder += ballteration.Value;
                    break;
                case ScoreModifier.Priority.SuperMultiplier:
                    superMultiplier *= ballteration.Value;
                    break;
            }
        }
        Score((((value + superAdder) * multiplier) + adder) * superMultiplier);
    }

    public void Score(int score)
    {
        if (score == 0) return;

        int actualScore = ScoreManager.BoardScore(score);

        if (actualScore == 0) return;

        ScoreBubble bubble = ScoreBubbleScene.Instantiate<ScoreBubble>();
        bubble.Text = actualScore.ToString("+0;-#");

        AddChild(bubble);
    }
}
