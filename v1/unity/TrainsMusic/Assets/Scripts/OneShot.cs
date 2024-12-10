using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneShot : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip clip;

    private AudioSource AudioSource
    {
        get
        {
            if (audioSource == null)
            {
                audioSource = GetComponent<AudioSource>();
            }
            return audioSource;
        }
    }
    private AudioClip Clip
    {
        get
        {
            if (clip == null)
            {
                clip = audioSource.clip;
            }
            return clip;
        }
    }
    
    private void OnValidate()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
        if (clip == null && audioSource != null)
        {
            clip = audioSource.clip;
        }
    }
    public void Play(bool play=true)
    {
        if (!play)
        {
            return;
        }
        audioSource.PlayOneShot(Clip);
    }
}
