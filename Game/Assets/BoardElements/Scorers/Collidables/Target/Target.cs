using Godot;
using System;

public partial class Target : Node2D
{
    [Export]
    OnOffLight OnOffLight;

    void OnHit()
    {
        if (OnOffLight.IsOnOrBlinking) return;
        OnOffLight.TurnOn();
    }
}
