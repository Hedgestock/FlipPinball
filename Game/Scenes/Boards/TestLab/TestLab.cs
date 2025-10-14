using Godot;
using Godot.Collections;

public partial class TestLab : Board
{
    [Export]
    BoardElementsGroup Entry;

    [Export]
    BoardElementsGroup Lab;

    protected override void PaddleAdditionnalBehaviour(bool left)
    {
        base.PaddleAdditionnalBehaviour(left);
        Entry.RotateStatus(left ? 1 : -1);
        Lab.RotateStatus(left ? 1 : -1);
    }

    protected override int Score(int score)
    {
        return ScoreManager.Score(score * BoardMult);
    }

    private int BoardMult = 1;

    void SetBoardMultLevel(int level)
    {
        switch (level)
        {
            default:
                BoardMult = 1;
                break;
            case 1:
                BoardMult = 2;
                break;
            case 2:
                BoardMult = 3;
                break;
            case 3:
                BoardMult = 5;
                break;
            case 4:
                BoardMult = 10;
                break;
        }
        StatusManager.Instance.EmitSignal(StatusManager.SignalName.StatusChanged, $"Board multiplier x{BoardMult}");
    }

    void GetPrizes(int level)
    {
        switch (level)
        {
            default:
                break;
            case 1:
                Score(10000);
                StatusManager.Instance.EmitSignal(StatusManager.SignalName.StatusChanged, "Received research reward prize");
                break;
            case 2:
                Score(50000);
                StatusManager.Instance.EmitSignal(StatusManager.SignalName.StatusChanged, "Received breakthrough prize");
                break;
            case 3:
                GiveExtraBall();
                StatusManager.Instance.EmitSignal(StatusManager.SignalName.StatusChanged, "Extra ball granted for receiving a Nobel prize");
                break;
        }
    }

    [Export]
    OnOffLight MagicPostLight;
    [Export]
    Array<OnOffLight> OutLanesLights;
    void TunnelEntered(int level)
    {
        switch (level)
        {
            default:
                break;
            case 1:
                Score(10000);
                StatusManager.Instance.EmitSignal(StatusManager.SignalName.StatusChanged, "A step towards the antidote");
                break;
            case 2:
                Score(20000); // Add jackpot ???
                StatusManager.Instance.EmitSignal(StatusManager.SignalName.StatusChanged, "The research continues");
                break;
            case 3:
                Score(20000);
                MagicPostLight.TurnOn();
                StatusManager.Instance.EmitSignal(StatusManager.SignalName.StatusChanged, "Magic pill deployed");
                break;
            case 4:
                Score(50000);
                StatusManager.Instance.EmitSignal(StatusManager.SignalName.StatusChanged, "Extra ball chance in the out lanes");
                foreach (var light in OutLanesLights) light.TurnOn();
                break;
        }
    }

    [Export]
    Array<OnOffLight> SpinnersLights;
    void ModesHandler(int level, bool up)
    {
        if (up) Score(5000);
        switch (level)
        {
            case 0:
                if (!up)
                    foreach (var light in SpinnersLights) light.TurnOff();
                break;
            case 1:
                if (up)
                {
                    foreach (var light in SpinnersLights) light.TurnOn();
                    StatusManager.Instance.EmitSignal(StatusManager.SignalName.StatusChanged, "Centrifuges upgraded");
                }
                else
                { }
                break;
        }
    }
}
