using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AudioClipName
{
    Card,
    Success,
    GameOver
}

public class SoundManager : MonoSington<SoundManager>
{
    [SerializeField] private AudioSource audioSource;
    public List<AudioClip> sounds = new List<AudioClip>();

    public void Play(AudioClipName name, float pitch = 1.0f)
    {
        if (sounds[(int)name] == null)
            return;

        audioSource.pitch = pitch;
        audioSource.PlayOneShot(sounds[(int)name]);
    }
}
