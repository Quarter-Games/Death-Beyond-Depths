using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField] GameObject MusicContainer;
    [SerializeField] GameObject AmbienceContainer;
    [SerializeField] GameObject SoundContainer;
    [SerializeField, UnityEngine.Range(0, 1)] float MusicVolume = 1;
    [SerializeField, UnityEngine.Range(0, 1)] float AmbienceVolume = 1;
    [SerializeField, UnityEngine.Range(0, 1)] float SFXVolume = 1;

    private List<AudioSource> MusicSources;
    private List<AudioSource> AmbienceSources;
    private List<AudioSource> SoundSources;

    public static AudioManager Instance {get; private set;}

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        MusicSources = MusicContainer.GetComponentsInChildren<AudioSource>().ToList();
        AmbienceSources = AmbienceContainer.GetComponentsInChildren<AudioSource>().ToList();
        SoundSources = SoundContainer.GetComponentsInChildren<AudioSource>().ToList();
        SetVoulumes();
        PlayAmbience();
    }

    private void SetVoulumes()
    {
        foreach (AudioSource source in MusicSources)
        {
            source.volume = Mathf.Clamp(MusicVolume, 0, 1);
        }
        foreach (AudioSource source in AmbienceSources)
        {
            source.volume = Mathf.Clamp(AmbienceVolume, 0, 1);
        }
        foreach (AudioSource source in SoundSources)
        {
            source.volume = Mathf.Clamp(SFXVolume, 0, 1);
        }
    }

    public void PlayBackGroundMusic(AudioClip clip = null)
    {
        if(MusicSources == null || (clip == null && MusicSources[0].clip == null)) return;
        if (clip != null)
        {
            MusicSources[0].Play();
        }
    }

    public void PlayAmbience(AudioClip clip = null)
    {
        foreach (AudioSource source in AmbienceSources)
        {
            if (!source.isPlaying)
            {
                if(clip != null) source.clip = clip;
                source.Play();
                return;
            }
        }
    }

    public void PlaySoundEffect(AudioResource resource, Transform origin)
    {
        int sourceCount = SoundSources.Count;
        foreach (AudioSource source in SoundSources)
        {
            if (!source.isPlaying)
            {
                source.transform.position = origin.position;
                source.resource = resource;
                source.Play();
                return;
            }
        }
        // if code reached here, no source available, create new sources
        for (int i = 0; i < sourceCount; i++)
        {
            CreateNewAudioSource();
        }
        SoundSources[SoundSources.Count - 1].transform.position = origin.position;
        SoundSources[SoundSources.Count - 1].resource = resource;
        SoundSources[SoundSources.Count - 1].Play();
    }

    private void CreateNewAudioSource()
    {
        SoundSources.Add(Instantiate(SoundSources[0]));
    }
}
