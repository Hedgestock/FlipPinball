using Godot;
using System.Collections.Generic;
using System.Linq;

public partial class RolloverSwitchGroup : Node2D
{
    [Signal]
    public delegate void AllOnEventHandler();

    List<RolloverSwitch> Switches;

    public override void _Ready()
    {
        base._Ready();
        Switches = GetChildren().Where(c => c is RolloverSwitch).Cast<RolloverSwitch>().ToList();
        Switches.ForEach(s => s.ChangedState += CheckGroupStatus);
    }

    public void RotateStatus(int direction)
    {
        bool[] statuses = Switches.Select(s => s.IsOn).ToArray();

        foreach (var (rswitch, i) in Switches.Select((rswitch, i) => (rswitch, i)))
        {
            rswitch.IsOn = statuses[Mathf.PosMod(i + direction, statuses.Length)];
        }
    }

    private void CheckGroupStatus()
    {
        foreach (var rolloverSwitch in Switches)
            if (!rolloverSwitch.IsOn) return;

        EmitSignal(SignalName.AllOn);

        Switches.ForEach(s => s.IsOn = false);
    }
}
