using Godot;
using System;

public partial class OnOffLight : AnimatedSprite2D
{
    [Signal]
    public delegate void ToggledEventHandler(bool isOn);

    public bool IsOn
    {
        get { return Animation == "on"; }
        set
        {
            if (value) Animation = "on";
            else Animation = "off";
            EmitSignal(SignalName.Toggled, value);
        }
    }

    public void TurnOn()
    {
        IsOn = true;
    }

    public void TurnOff()
    {
        IsOn = false;
    }

    public void TurnBlinking()
    {
        Animation = "blinking";
    }
}
