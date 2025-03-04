using Godot;
using System;

public partial class ScoreManager : Node
{
    [Signal]
    public delegate void ScoringEventHandler(long totalScoreValue, int currentlyScoring);

    protected static ScoreManager _instance;
    public static ScoreManager Instance { get { return _instance; } }

    public ScoreManager()
    {
        if (_instance != null)
            return;
        _instance = this;
    }

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
