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
        Switches.ForEach(s => s.GetNode<OnOffLight>("OnOffLight").Toggled += (on) => { if (on) CheckGroupStatus(); });
    }

    public void RotateStatus(int direction)
    {
        bool[] statuses = Switches.Select(s => s.GetNode<OnOffLight>("OnOffLight").IsOn).ToArray();
        //We do that to avoid triggering the "AllOn" Event by accident
        Switches.ForEach(s => s.GetNode<OnOffLight>("OnOffLight").IsOn = false);

        foreach (var (rswitch, i) in Switches.Select((rswitch, i) => (rswitch, i)))
        {
            rswitch.GetNode<OnOffLight>("OnOffLight").IsOn = statuses[Mathf.PosMod(i + direction, statuses.Length)];
        }
    }

    private void CheckGroupStatus()
    {
        foreach (var rolloverSwitch in Switches)
            if (!rolloverSwitch.GetNode<OnOffLight>("OnOffLight").IsOn) return;

        EmitSignal(SignalName.AllOn);
    }
}
