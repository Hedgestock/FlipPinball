using Godot;
using System;

public partial class Scorer : Node2D
{
    //[Flags]
    //public enum Attributes
    //{
    //    None = 0,
    //    Bumper = 1,
    //    Slingshot = 1 << 1,
    //    Spinner = 1 << 2,
    //    Target = 1 << 3,
    //    Rollover = 1 << 4,
    //    Plunger = 1 << 5,
    //    Magnet = 1 << 6,
    //    Teleport = 1 << 7,
    //    BallLock = 1 << 8,
    //    Round = 1 << 10,
    //    Square = 1 << 11,
    //}

    //public abstract Attributes Kind { get; }

    [Export]
    private int Value = 200;

    [Export]
    PackedScene ScoreBubbleScene;

    private void Score()
    {
        Score(1);
    }

    private void Score(int multiplier)
    {
        ScoreBubble bubble = ScoreBubbleScene.Instantiate<ScoreBubble>();
        bubble.Text = ScoreManager.FieldScore(Value * multiplier).ToString("+0;-#");
        AddChild(bubble);
    }
}
