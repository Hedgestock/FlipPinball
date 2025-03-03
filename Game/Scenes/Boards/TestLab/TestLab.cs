using Godot;
using Godot.Collections;
using System.Linq;

public partial class TestLab : Board
{
    [Export]
    Node2D EntryBumpers;

    [Export]
    Node2D EntryRollovers;

    [Export]
    Node2D LabBumpers;

    [Export]
    Node2D LabRollovers;

    public override void _Ready()
    {
        base._Ready();
        (EntryRollovers as RolloverSwitchGroup).AllOn += () => (EntryBumpers as BumperGroup).Level++;
        (LabRollovers as RolloverSwitchGroup).AllOn += () => (LabBumpers as BumperGroup).Level++;
    }

    protected override void PaddleAdditionnalBehaviour(bool left)
    {
        base.PaddleAdditionnalBehaviour(left);
        (EntryRollovers as RolloverSwitchGroup).RotateStatus(left ? 1 : -1);
        (LabRollovers as RolloverSwitchGroup).RotateStatus(left ? 1 : -1);
    }
}
