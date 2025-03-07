using Godot;
using System;

public partial class OnOffLight : AnimatedSprite2D
{
    [Signal]
    public delegate void ToggledEventHandler(bool isOn);

    [Signal]
    public delegate void TurnedOnEventHandler();

    [Signal]
    public delegate void TurnedOffEventHandler();

    public bool IsOn
    {
        get { return Animation == "on"; }
        set
        {
            Stop();
            if (value) Animation = "on";
            else Animation = "off";
            EmitSignal(SignalName.Toggled, value);
        }
    }

    public bool IsBlinking
    {
        get { return Animation == "blinking"; }
    }

    public bool IsOnOrBlinking
    {
        get { return IsOn || IsBlinking; }
    }

    public void TurnOn()
    {
        IsOn = true;
        EmitSignal(SignalName.TurnedOn);
    }

    public void TurnOff()
    {
        IsOn = false;
        EmitSignal(SignalName.TurnedOff);
    }

    public void TurnBlinking()
    {
        Animation = "blinking";
        Play();
    }
}
