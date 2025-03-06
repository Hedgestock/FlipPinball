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
        Nodes.ForEach(s => (s.FindChild("OnOffLight") as OnOffLight).Toggled += (on) =>
        {
            if (on) CheckGroupStatus();
        });
    }

    public void RotateStatus(int direction)
    {
        bool[] statuses = Nodes.Select(s => s.GetNode<OnOffLight>("OnOffLight").IsOn).ToArray();
        //We do that to avoid triggering the "AllOn" Event by accident
        Nodes.ForEach(s => s.GetNode<OnOffLight>("OnOffLight").IsOn = false);

        foreach (var (node, i) in Nodes.Select((n, i) => (n, i)))
        {
            (node.FindChild("OnOffLight") as OnOffLight).IsOn = statuses[Mathf.PosMod(i + direction, statuses.Length)];
        }
    }

    private void CheckGroupStatus()
    {
        bool firstStatus = (Nodes[0].FindChild("OnOffLight") as OnOffLight).IsOn;
        foreach (var node in Nodes)
            if ((node.FindChild("OnOffLight") as OnOffLight).IsOn != firstStatus) return;

        if (firstStatus)
            EmitSignal(SignalName.AllOn);
        else
            EmitSignal(SignalName.AllOff);
    }
}
