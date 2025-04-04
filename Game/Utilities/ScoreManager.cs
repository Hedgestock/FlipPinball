using Godot;
using System;

public partial class ScoreManager : Node
{
    [Signal]
    public delegate void ScoringEventHandler(int currentlyScoring);

    protected static ScoreManager _instance;
    public static ScoreManager Instance { get { return _instance; } }

    public ScoreManager()
    {
        if (_instance != null)
            return;
        _instance = this;
    }

    public static long ScoreValue { get; set; }
    public static long TotalScoreValue { get; set; }

    public static Func<int, int> BoardScore = Score;

    public static int Score(int score)
    {
        ScoreValue += score;
        TotalScoreValue += score;
        Instance.EmitSignal(SignalName.Scoring, score);
        return score;
    }
}
