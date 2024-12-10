using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MixRecall : MonoBehaviour
{
    [SerializeField]
    float saveInterval = 60f;
    [SerializeField]
    int trackIndex = -1;

    AudioSource audioSource;
    float prefsVolume;
    float prefsPan;
    public AudioSource ConnectedSource { 
        get
        {
            if (audioSource == null)
            {
                audioSource = GetComponent<AudioSource>();
            }
            return audioSource; 
        }
    }
    public int TrackIndex { get => trackIndex; }
    void Awake()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        prefsVolume = PlayerPrefs.GetFloat($"{name}_volume", audioSource.volume);
        audioSource.volume = prefsVolume;
        prefsPan = PlayerPrefs.GetFloat($"{name}_pan", audioSource.panStereo);
        audioSource.panStereo = prefsPan;
    }

    IEnumerator Start()
    {
        while (true)
        {
            yield return new WaitForSeconds(saveInterval);
            Save();
        }
    }

    public void Save()
    {
        if (audioSource.volume != prefsVolume || audioSource.panStereo != prefsPan)
        {
            PlayerPrefs.SetFloat($"{name}_volume", audioSource.volume);
            prefsVolume = audioSource.volume;
            PlayerPrefs.SetFloat($"{name}_pan", audioSource.panStereo);
            prefsPan = audioSource.panStereo;
        }
    }
}
