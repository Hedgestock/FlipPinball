using Godot;
using System.Collections.Generic;
using System.Linq;

public partial class BoardElementsGroup : Node2D
{
    [Signal]
    public delegate void AllOnEventHandler();
    [Signal]
    public delegate void AllOffEventHandler();

    [Signal]
    public delegate void AnyToggledEventHandler();

    public List<(Node node, OnOffLight light)> Nodes;

    // Looks for all OnOffLights in the same group as itself and adds them to its control flow
    public override void _Ready()
    {
        base._Ready();
        Nodes = GetChildren().Where(c => c.GetGroups().Intersect(GetGroups()).Any()).Select(c => (c, (OnOffLight)c.FindChild("OnOffLight"))).ToList();
        Nodes.ForEach(n => n.light.Toggled +=  CheckGroupStatus);
    }

    public void RotateStatus(int direction)
    {
        // Blinking means in busy
        if (Nodes.Any(n => n.light.IsBlinking)) 
            return;

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
        EmitSignalAnyToggled();
        foreach (var node in Nodes) // Maybe do an Any for readbiblity
            if (node.light.IsOn != status) return;

        // We call deferred here otherwise the on or off signal arrives before the triggering signal
        // in the cases where the group resets itself
        // MIGHT BE BULLSHIT I NEED TO TRY AGAIN
        if (status)
            CallDeferred(MethodName.EmitSignal, SignalName.AllOn);
        else
            CallDeferred(MethodName.EmitSignal, SignalName.AllOff);
    }

    public void SetAllOn()
    {
        foreach (var node in Nodes)
            node.light.TurnOn();
    }

    public void SetAllBlinking()
    {
        foreach (var node in Nodes)
            node.light.TurnBlinking();
    }

    public void SetAllOff()
    {
        foreach (var node in Nodes)
            node.light.TurnOff();
    }
}
