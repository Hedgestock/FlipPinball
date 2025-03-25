using Godot;
using System;

public partial class BallTimer : Effect
{
    public override string Description
    {
        get
        {
            return $"Ball destroys itself after {timeleft:0.00} seconds";
        }
    }

    public override float AnalogRarity => throw new NotImplementedException();

    [Export]
    Timer Timer;
    [Export]
    Label Label;

    // This is only Exported to keep state when duplicating
    [Export]
    double timeleft = 15;

    public override void _Ready()
    {
        base._Ready();

        Timer.Start(timeleft);
        Timer.Timeout += () =>
            {
                GetParent().GetParent().EmitSignal(Ball.SignalName.SelfDestruct);
            };
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        Label.Text = Timer.TimeLeft.ToString("0.00");
        timeleft = Timer.TimeLeft;
    }

    const int minTime = 40;
    const int maxTime = 40;
    static string ScenePath = "res://Game/Assets/Ballterations/Effects/BallTimer/BallTimer.tscn";

    public static BallTimer CreateRandom(int minTime = minTime, int maxTime = maxTime)
    {
        BallTimer ballTimer = GD.Load<PackedScene>(ScenePath).Instantiate<BallTimer>();
        ballTimer.timeleft = GD.RandRange(minTime, maxTime);
        return ballTimer;
    }

    public override Effect Refine(Effect minimum = null, Effect maximum = null)
    {
        BallTimer minimumBT = minimum as BallTimer;
        BallTimer maximumBT = maximum as BallTimer;
        return CreateRandom((int?)minimumBT?.timeleft ?? minTime, (int?)maximumBT?.timeleft ?? maxTime);
    }
}
