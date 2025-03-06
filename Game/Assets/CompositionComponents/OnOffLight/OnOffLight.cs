using Godot;
using System;

public partial class OnOffLight : Sprite2D
{
    [Signal]
    public delegate void ToggledEventHandler(bool isOn);

    [Export]
    Texture2D On;
    [Export]
    Texture2D Off;

    [Export]
    private bool _isOn;

    public override void _Ready()
    {
        base._Ready();
        IsOn = _isOn;
    }

    public bool IsOn
    {
        get { return _isOn; }
        set
        {
            if (value) Texture = On;
            else Texture = Off;
            if (_isOn == value) return;
            _isOn = value;
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
}
