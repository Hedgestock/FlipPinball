using Godot;
using System;

public partial class OnOffLight : AnimatedSprite2D
{
    [Signal]
    public delegate void ToggledEventHandler(bool isOn);

    [Signal]
    public delegate void TurnedOnEventHandler();

    [Signal]
    public delegate void TurnedBlinkingEventHandler();
    
    [Signal]
    public delegate void TurnedOffEventHandler();

    public bool IsOn
    {
        get { return Animation == "on"; }
        set
        {
            Stop();
            if (value)
            {
                if (Animation == "on") return;
                Animation = "on";
                EmitSignal(SignalName.TurnedOn);
            }
            else
            {
                if (Animation == "off") return;
                Animation = "off";
                EmitSignal(SignalName.TurnedOff);
            }
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
    }

    public void TurnOff()
    {
        IsOn = false;
    }

    public void TurnBlinking()
    {
        Animation = "blinking";
        EmitSignal(SignalName.TurnedBlinking);
        Play();
    }
}
