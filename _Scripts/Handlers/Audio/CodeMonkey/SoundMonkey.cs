using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMonkey : MonoBehaviour
{
    public SoundAudioClip[] soundAudioClipArray;

    [System.Serializable]
    public class SoundAudioClip
    {
        public string name;

        public AudioClip clip;

        [Range(0f, 1f)]
        public float volume = 1f;

        [Range(.1f, 3f)]
        public float pitch = 1f;

        public bool isLoop;
        public bool hasCooldown;
        public AudioSource source;
    }
}
