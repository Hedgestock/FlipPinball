using Godot;
using System.Collections.Generic;
using System.Linq;

public partial class BoardElementsGroup : Node
{
    [Signal]
    public delegate void AllOnEventHandler();

    [Signal]
    public delegate void AllOffEventHandler();

    [Export]
    string Group;

    List<Node> Nodes;

    public override void _Ready()
    {
        base._Ready();
        Nodes = GetChildren().Where(c => c.IsInGroup(Group)).ToList();
        Nodes.ForEach(s => ((OnOffLight)s.FindChild("OnOffLight")).Toggled += (on) => { if (on) CheckGroupStatus(); });
    }

    public void RotateStatus(int direction)
    {
        bool[] statuses = Nodes.Select(s => s.GetNode<OnOffLight>("OnOffLight").IsOn).ToArray();
        //We do that to avoid triggering the "AllOn" Event by accident
        Nodes.ForEach(s => s.GetNode<OnOffLight>("OnOffLight").IsOn = false);

        foreach (var (node, i) in Nodes.Select((n, i) => (n, i)))
        {
            ((OnOffLight)node.FindChild("OnOffLight")).IsOn = statuses[Mathf.PosMod(i + direction, statuses.Length)];
        }
    }

    void CheckGroupStatus()
    {
        bool firstStatus = ((OnOffLight)Nodes[0].FindChild("OnOffLight")).IsOn;
        foreach (var node in Nodes)
            if (((OnOffLight)node.FindChild("OnOffLight")).IsOn != firstStatus) return;

        // We call deferred here otherwise the on or off signal arrives before the triggerring signal
        // in the cases where the group resets itself
        if (firstStatus)
            CallDeferred(MethodName.EmitSignal, SignalName.AllOn);
        else
            CallDeferred(MethodName.EmitSignal, SignalName.AllOff);
    }

    void SetAllOn()
    {
        foreach (var node in Nodes)
            ((OnOffLight)node.FindChild("OnOffLight")).TurnOn();
    }

    void SetAllOff()
    {
        foreach (var node in Nodes)
            ((OnOffLight)node.FindChild("OnOffLight")).TurnOff();
    }
}
