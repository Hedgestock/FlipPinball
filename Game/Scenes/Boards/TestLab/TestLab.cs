using Godot;
using Godot.Collections;
using System.Linq;

public partial class TestLab : Board
{
    [Export]
    BumperGroup EntryBumpers;
    [Export]
    RolloverSwitchGroup EntryRollovers;

    [Export]
    BumperGroup LabBumpers;
    [Export]
    RolloverSwitchGroup LabRollovers;

    public override void _Ready()
    {
        base._Ready();
        EntryRollovers.AllOn += () => EntryBumpers.Level++;
        LabRollovers.AllOn += () => LabBumpers.Level++;
    }

    protected override void PaddleAdditionnalBehaviour(bool left)
    {
        base.PaddleAdditionnalBehaviour(left);
        EntryRollovers.RotateStatus(left ? 1 : -1);
        LabRollovers.RotateStatus(left ? 1 : -1);
    }
}
