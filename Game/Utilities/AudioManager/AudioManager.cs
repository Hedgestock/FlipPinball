using Godot;
using System;

public partial class AudioManager : Node
{
    [Export]
    private AudioStreamPlayer Music;

    static private AudioManager _instance = null;
    public static AudioManager Instance { get { return _instance; } }

    private AudioManager()
    {
        if (_instance != null)
            return;
        _instance = this;
    }

    public override void _Notification(int what)
    {
        if (what == NotificationPaused)
            SetPlayerVolume(_instance.Music, .5f);
        else if (what == NotificationUnpaused)
            SetPlayerVolume(_instance.Music, 1f);
    }

    public static void PlayMusic(AudioStream music)
    {
        if (music == _instance.Music.Stream) return;
        Tween tween = _instance.CreateTween().SetPauseMode(Tween.TweenPauseMode.Process);
        if (_instance.Music.Stream != null)
            tween.TweenProperty(_instance.Music, "volume_linear", 0, .5f);
        tween.TweenCallback(
            Callable.From(() =>
            {
                _instance.Music.Stream = music;
                _instance.Music.Play();
            }));
        tween.TweenProperty(_instance.Music, "volume_linear", 1, .5f);
    }

    public static void StopMusic()
    {
        Tween tween = _instance.CreateTween().SetPauseMode(Tween.TweenPauseMode.Process);
        tween.TweenProperty(_instance.Music, "volume_linear", 0, .5f);
        tween.TweenCallback(
            Callable.From(() =>
            {
                _instance.Music.Stop();
                _instance.Music.Stream = null;
            }));
    }

    public static void SetPlayerVolume(AudioStreamPlayer player, float volume)
    {
        Tween tween = _instance.CreateTween().SetPauseMode(Tween.TweenPauseMode.Process);
        tween.TweenProperty(player, "volume_linear", volume, .5f);
    }
}