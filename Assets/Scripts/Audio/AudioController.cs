using System;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

[Serializable]
public class SoundEffect
{
    public Sounds sound;
    public SoundInfo[] soundInfo;
}

[Serializable]
public class SoundInfo
{
    [Range(0.0f, 1.0f)]
    public float volume;
    public AudioClip clip;
}


public class AudioController : MonoBehaviour
{
    public static AudioController Current;
    private Bus musicBus;
    private Bus effectsBus;
    private Bus masterBus;


    static Dictionary<Sounds, string> soundEvents = new Dictionary<Sounds, string>();

    void MapSoundsToEvents() {
        soundEvents.Clear();
        soundEvents.Add(Sounds.Countdown, "event:/Sounds/Countdown");
        soundEvents.Add(Sounds.CountdownFinish, "event:/Sounds/Launch");
    }

    void Awake()
    {
        MapSoundsToEvents();
        masterBus = RuntimeManager.GetBus("bus:/");
        musicBus = RuntimeManager.GetBus("bus:/Music");
        effectsBus = RuntimeManager.GetBus("bus:/Effects");

        Current = this;
    }

    public static float MusicVolume
    {
        get
        {
            float v;
            Current.musicBus.getVolume(out v);
            return v;
        }
        set
        {
            Current.musicBus.setVolume(value);
        }
    }

    public static float EffectVolume
    {
        get
        {
            float v;
            Current.effectsBus.getVolume(out v);
            return v;
        }
        set
        {
            Current.effectsBus.setVolume(value);
        }
    }

    public static float OverallVolume{
        get{
            float v;
            Current.masterBus.getVolume(out v);
            return v;
        }
        set
        {
            Current.masterBus.setVolume(value);
        }
    }

    public static bool MuteAllVolume
    {
        get
        {
            return RuntimeManager.IsMuted;
        }
        set
        {
            RuntimeManager.MuteAllEvents(value);
        }
    }

    public static void PlaySound(Sounds sound)
    {
        FMODUnity.RuntimeManager.PlayOneShot(soundEvents[sound]);
    }
}
