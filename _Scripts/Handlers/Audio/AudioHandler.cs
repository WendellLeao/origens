using System;
using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class AudioHandler : MonoBehaviour
{
    public static AudioHandler instance;
    [SerializeField] private AudioMixerGroup mixerGroup;
    [SerializeField] private Sound[] sounds;
    private bool isPlayingMenuTheme;
    private bool isPlayingGameTheme = false;

    void Awake()
    {
        if(instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        
        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;

            s.source.loop = s.loop;

            //s.source.outputAudioMixerGroup = s.group;
            s.source.outputAudioMixerGroup = mixerGroup;
        }
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        
        if(s == null)
        {
            Debug.LogWarning("Sound" + name + " not found!");
            return;
        }

        s.source.Play();
    }
    public void StopPlaying (string sound)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
       
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        s.source.Stop();
    }

    public void ChangeSoundtrackMusic(string currentMusic, string nextMusic)
    {
        AudioHandler.instance.StopPlaying(currentMusic);
        AudioHandler.instance.Play(nextMusic);
    }

    public void ChangeSoundtrackMusicWithTimer(string currentMusic, string nextMusic, float timeToChange)
    {
        StartCoroutine(ChangeSoundtrackTimer(currentMusic, nextMusic, timeToChange));
        isPlayingGameTheme = true;
        isPlayingMenuTheme = false;
    }

    IEnumerator ChangeSoundtrackTimer(string currentMusic, string nextMusic, float timeToChange)
    {
        yield return new WaitForSeconds(timeToChange);
        AudioHandler.instance.StopPlaying(currentMusic);
        AudioHandler.instance.Play(nextMusic);
    }

    public bool IsPlayingMenuTheme
    {
        get{return isPlayingMenuTheme;}
        set{this.isPlayingMenuTheme = value;}
    }
    public bool IsPlayingGameTheme
    {
        get{return isPlayingGameTheme;}
        set{this.isPlayingGameTheme = value;}
    }
}
