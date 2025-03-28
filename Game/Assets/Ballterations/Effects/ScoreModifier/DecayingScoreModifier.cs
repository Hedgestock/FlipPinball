using Godot;
using System;

public partial class DecayingScoreModifier : ScoreModifier
{
    Timer Timer;

    // This is also exported to keep state when duplicating
    [Export]
    double timeleft = 15;

    float _StartValue = 1;

    public override string Description
    {
        get
        {
            return ($"{base.Description} Decaying over {timeleft:N1} seconds");
        }
    }

    public override void _Ready()
    {
        base._Ready();

        if (Timer == null)
        {
            Timer = new Timer();
            Timer.Timeout += () =>
            {
                QueueFree();
            };
            AddChild(Timer);
        }

        Timer.Start(timeleft);
        _StartValue = Value;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        //timeleft = Timer.TimeLeft;
        Value = (float)(_StartValue * (Timer.TimeLeft / timeleft));
        GD.Print($"duration {timeleft} timeleft {Timer.TimeLeft}");
    }

    public override DecayingScoreModifier Refine(Effect minimum, Effect maximum)
    {
        DecayingScoreModifier minimumSM = minimum as DecayingScoreModifier;
        DecayingScoreModifier maximumSM = maximum as DecayingScoreModifier;
        DecayingScoreModifier refined = (DecayingScoreModifier)base.Refine(minimum, maximum);

        // TODO: Refine

        return refined;
    }
}
