using Godot;
using System;

public abstract partial class Scorer : Node2D
{



    [Signal]
    public delegate void ChangedStateEventHandler();

    //[Flags]
    //public enum Attributes
    //{
    //    None = 0,
    //    Bumper = 1,
    //    Slingshot = 1 << 1,
    //    Spinner = 1 << 2,
    //    Target = 1 << 3,
    //    Rollover = 1 << 4,
    //    Plunger = 1 << 5,
    //    Magnet = 1 << 6,
    //    Teleport = 1 << 7,
    //    BallLock = 1 << 8,
    //    Round = 1 << 10,
    //    Square = 1 << 11,
    //}

    //public abstract Attributes Kind { get; }

    [Export]
    public int BaseValue = 200;
    public virtual int Value { get { return BaseValue; } }

    [Export]
    protected Sprite2D Light;
    [Export]
    protected Texture2D LightOn;
    [Export]
    protected Texture2D LightOff;

    [Export]
    AudioStream ScoreSFX;

    //protected AudioStreamPlayer ScoreSoundPlayer = new AudioStreamPlayer();

    protected bool _isOn = true;
    public bool IsOn
    {
        get { return _isOn; }
        set
        {
            SetOnValue(value);
            EmitSignal(SignalName.ChangedState);
        }
    }

    public override void _Ready()
    {
        base._Ready();
        //ScoreSoundPlayer.Bus = "BoardElements";
        //ScoreSoundPlayer.Stream = ScoreSFX;
        //ScoreSoundPlayer.MaxPolyphony = 2;
        //AddChild(ScoreSoundPlayer);
    }

    protected void Score()
    {
        if (!IsOn) return;

        ScoreBubble bubble = GD.Load<PackedScene>("res://Game/Assets/ScoreBubble/ScoreBubble.tscn").Instantiate<ScoreBubble>();
        bubble.Text = ScoreManager.Score(Value).ToString("+0;-#");
        AddChild(bubble);
        //ScoreSoundPlayer.Play();
    }

    protected virtual void SetOnValue(bool value)
    {
        _isOn = value;
        Light.Texture = value ? LightOn : LightOff;
    }
}
