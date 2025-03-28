using Godot;
using System;

public partial class BallTimer : Effect
{
    public override string Description
    {
        get
        {
            return $"Ball destroys itself after {timeleft:N1} seconds";
        }
    }

    public override float AnalogRarity => throw new NotImplementedException();


    //Event with export, this doesn't seem to carry over duplication...
    Timer Timer;

    // This is also exported to keep state when duplicating
    [Export]
    double timeleft = 15;

    public override void _Ready()
    {
        base._Ready();

        if (Timer == null) {
            Timer = new Timer();
            Timer.Timeout += () =>
            {
                GetParent().GetParent().EmitSignal(Ball.SignalName.SelfDestruct);
            };
            AddChild(Timer);
            //Timer.WaitTime = 15;
        }
        Timer.Start(timeleft);

    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        timeleft = Timer.TimeLeft;
        //Timer.WaitTime = Timer.TimeLeft;
        //Label.Text = timeleft.ToString("0.00");
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
