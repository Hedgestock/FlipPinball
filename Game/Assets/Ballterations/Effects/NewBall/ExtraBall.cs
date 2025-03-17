using Godot;
using System;

public partial class ExtraBall : MetaEffect
{
    public override string Description
    {
        get
        {
            return $"Adds a copy of the currently loaded ball to your ball queue.";
        }
    }

    public override void Activate()
    {
        throw new NotImplementedException();
    }
}
