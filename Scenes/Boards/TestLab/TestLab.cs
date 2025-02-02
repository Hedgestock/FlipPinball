using Godot;
using Godot.Collections;
using System.Linq;

public partial class TestLab : Board
{
    [Export]
    ScorerGroup EntryBumpers;

    [Export]
    ScorerGroup EntryRollovers;

    protected override void PaddleAdditionnalBehaviour(bool left)
    {
        base.PaddleAdditionnalBehaviour(left);
        EntryRollovers.RotateStatus(left ? 1 : -1);
    }


}
