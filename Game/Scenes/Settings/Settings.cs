using Godot;
using System;

public partial class Settings : CanvasLayer
{
    [Export]
    private Slider MusicVolume;
    [Export]
    private Slider SFXVolume;

    private void MusicVolumeChanged(float volume)
    {
        AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("Music"), Mathf.LinearToDb(volume));
    }

    private void SFXVolumeChanged(float volume)
    {
        AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("BoardElements"), Mathf.LinearToDb(volume));
    }
}
