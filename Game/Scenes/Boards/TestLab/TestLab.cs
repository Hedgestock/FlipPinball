using Godot;
using Godot.Collections;
using System.Linq;

public partial class TestLab : Board
{
    [Export]
    RolloverSwitchGroup EntryRollovers;

    [Export]
    RolloverSwitchGroup LabRollovers;

    protected override void PaddleAdditionnalBehaviour(bool left)
    {
        base.PaddleAdditionnalBehaviour(left);
        EntryRollovers.RotateStatus(left ? 1 : -1);
        LabRollovers.RotateStatus(left ? 1 : -1);
    }
}
