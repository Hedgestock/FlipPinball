using Godot;
using System.Collections.Generic;
using System.Linq;

public partial class BumperGroup : Node2D
{
    [Export]
    AudioStream LevelUpSFX;

    protected AudioStreamPlayer LevelUpSoundPlayer = new AudioStreamPlayer();

    List<Bumper> Bumpers;

    public override void _Ready()
    {
        base._Ready();
        Bumpers = GetChildren().Where(c => c is Bumper).Cast<Bumper>().ToList();
        LevelUpSoundPlayer.Bus = "BoardElements";
        LevelUpSoundPlayer.Stream = LevelUpSFX;
        AddChild(LevelUpSoundPlayer);
    }

    private void LevelUp()
    {
        foreach (var bumper in Bumpers)
        {
            if (!bumper.LevelUp()) return;
        }
        LevelUpSoundPlayer.Play();
    }
}
