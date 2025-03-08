using Godot;
using Godot.Collections;
using System.Linq;

public partial class TestLab : Board
{
    [Export]
    BoardElementsGroup Entry;

    [Export]
    BoardElementsGroup Lab;

    public override void _Ready()
    {
        base._Ready();
        ScoreManager.BoardScore = Score;
    }

    protected override void PaddleAdditionnalBehaviour(bool left)
    {
        base.PaddleAdditionnalBehaviour(left);
        Entry.RotateStatus(left ? 1 : -1);
        Lab.RotateStatus(left ? 1 : -1);
    }

    private int BoardMult = 1;
    private int Score(int score)
    {
        return ScoreManager.Score(score * BoardMult);
    }

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
    }

    void GetPrizes(int level)
    {
        switch (level)
        {
            default:
                break;
            case 1:
                Score(10000);
                break;
            case 2:
                Score(50000);
                break;
            case 3:
                AddExtraBall();
                break;
        }
    }

    [Export]
    OnOffLight MagicPostLight;
    void TunnelEntered(int level)
    {
        switch (level)
        {
            default:
                break;
            case 1:
                Score(10000);
                break;
            case 2:
                Score(20000); // Add jackpot ???
                break;
            case 3:
                Score(20000);
                MagicPostLight.TurnOn();
                break;
            case 4:
                Score(50000);
                AddExtraBall();
                break;
        }
    }

    [Export]
    Leveler SpinnersLevel;
    void ModesHandler(int level, bool up)
    {
        if (up) Score(5000);
        switch (level)
        {
            case 0:
                if (!up)
                    SpinnersLevel.MinimizeLevel();
                break;
            case 1:
                if (up)
                    SpinnersLevel.MaximizeLevel();
                else
                { }
                break;
        }
    }
}
