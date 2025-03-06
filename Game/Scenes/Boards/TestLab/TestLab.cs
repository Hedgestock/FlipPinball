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
    }

    protected override void PaddleAdditionnalBehaviour(bool left)
    {
        base.PaddleAdditionnalBehaviour(left);
        Entry.RotateStatus(left ? 1 : -1);
        Lab.RotateStatus(left ? 1 : -1);
    }


    #region Modes

    [Export]
    Leveler SpinnersLevel;
    void ModesHandler(int level, bool up)
    {
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


    #endregion
}
