using Godot;
using System.Collections.Generic;
using System.Linq;

public partial class BoardElementsGroup : Node2D
{
    [Signal]
    public delegate void AllOnEventHandler();

    [Signal]
    public delegate void AllOffEventHandler();

    [Export]
    protected string Group;

    protected List<(Node node, OnOffLight light)> Nodes;

    public override void _Ready()
    {
        base._Ready();
        Nodes = GetChildren().Where(c => c.IsInGroup(Group)).Select(c => (c, (OnOffLight)c.FindChild("OnOffLight"))).ToList();
        Nodes.ForEach(n => n.light.Toggled +=  CheckGroupStatus);
    }

    public void RotateStatus(int direction)
    {
        bool[] statuses = Nodes.Select(n => n.light.IsOn).ToArray();
        //We do that to avoid triggering the "AllOn" Event by accident
        Nodes.ForEach(n => n.light.IsOn = false);

        foreach (var (n, i) in Nodes.Select((n, i) => (n, i)))
        {
            n.light.IsOn = statuses[Mathf.PosMod(i + direction, statuses.Length)];
        }
    }

    void CheckGroupStatus(bool status)
    {
        foreach (var node in Nodes)
            if (node.light.IsOn != status) return;

        // We call deferred here otherwise the on or off signal arrives before the triggerring signal
        // in the cases where the group resets itself
        // MIGHT BE BULLSHIT I NEED TO TRY AGAIN
        if (status)
            CallDeferred(MethodName.EmitSignal, SignalName.AllOn);
        else
            CallDeferred(MethodName.EmitSignal, SignalName.AllOff);
    }

    protected void SetAllOn()
    {
        foreach (var node in Nodes)
            node.light.TurnOn();
    }

    protected void SetAllBlinking()
    {
        foreach (var node in Nodes)
            node.light.TurnBlinking();
    }

    protected void SetAllOff()
    {
        foreach (var node in Nodes)
            node.light.TurnOff();
    }
}
