using Godot;
using System.Collections.Generic;
using System.Linq;

public partial class BumperGroup : Node2D
{
    [Export]
    AudioStream LevelUpSFX;

    protected AudioStreamPlayer LevelUpSoundPlayer = new AudioStreamPlayer();

    int _level = 1;

    public int Level
    {
        get { return _level; }
        set {
            if (value > 4 || value < 1) return;
            if (value > _level) LevelUpSoundPlayer.Play();
            Bumpers.ForEach(b => b.Level = value);
            _level = value;
        }
    }

    List<Bumper> Bumpers;

    public override void _Ready()
    {
        base._Ready();
        Bumpers = GetChildren().Where(c => c is Bumper).Cast<Bumper>().ToList();
        LevelUpSoundPlayer.Bus = "BoardElements";
        LevelUpSoundPlayer.Stream = LevelUpSFX;
        AddChild(LevelUpSoundPlayer);
    }
}
