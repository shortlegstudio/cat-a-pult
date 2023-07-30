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

    public float MusicVolume = 1f;
    public float EffectVolume = 1f;



    static Dictionary<Sounds, string> soundEvents = new Dictionary<Sounds, string>();

    void MapSoundsToEvents() {
        soundEvents.Clear();
        soundEvents.Add(Sounds.Countdown, "event:/Sounds/Countdown");
        soundEvents.Add(Sounds.CountdownFinish, "event:/Sounds/Launch");
        soundEvents.Add(Sounds.Landing, "event:/Sounds/Landing");
        soundEvents.Add(Sounds.ClickForward, "event:/UI/ClickForward");
        soundEvents.Add(Sounds.ClickBack, "event:/UI/ClickBack");
    }

    void Awake()
    {
        MapSoundsToEvents();
        masterBus = RuntimeManager.GetBus("bus:/");
        musicBus = RuntimeManager.GetBus("bus:/Music");
        effectsBus = RuntimeManager.GetBus("bus:/Effects");
        musicBus.setVolume(MusicVolume);
        effectsBus.setVolume(EffectVolume);

        Current = this;
    }

    public void PlayClick()
    {
        PlaySound(Sounds.ClickForward);
    }

    public void PlayClickBack()
    {
        PlaySound(Sounds.ClickBack);
    }

    public static void PlaySound(Sounds sound)
    {
        FMODUnity.RuntimeManager.PlayOneShot(soundEvents[sound]);
    }
}
