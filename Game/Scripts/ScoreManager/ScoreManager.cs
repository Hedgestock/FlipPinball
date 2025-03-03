using Godot;
using System;

public partial class ScoreManager : Singleton
{
    [Signal]
    public delegate void ScoringEventHandler(long totalScoreValue, int currentlyScoring);


    public static long ScoreValue {  get; private set; }

    public override void _Ready()
    {
        base._Ready();
        ScoreValue = 0;
    }

    public static int Score(int baseScore)
    {
        int adjustedScore = baseScore;
        ScoreValue += adjustedScore;
        _instance.EmitSignal(SignalName.Scoring, ScoreValue, adjustedScore);
        return adjustedScore;

    }
}
