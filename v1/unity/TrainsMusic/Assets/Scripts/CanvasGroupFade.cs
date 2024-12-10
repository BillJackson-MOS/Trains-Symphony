using System;
using System.Collections;
using System.Collections.Generic;
using SonicBloom.Koreo;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class CanvasGroupFade : MonoBehaviour
{
    public bool startState = false;
    public float inDuration = .5f;
    public float outDuration = .5f;
    public int trackIndex = 0;
    private Coroutine fadeCoroutine;
    private CanvasGroup _group;
    private CanvasGroup group
    {
        get
        {
            if (_group == null)
            {
                _group = GetComponent<CanvasGroup>();
            }
            return _group;
        }
    }

    private void Start() 
    {
        TrackToggleManager.Instance.onTrackCountChange.AddListener(OnTrackCountChange);
        group.alpha = startState ? 1f : 0f;
    }

    private void OnTrackCountChange(int trackIndex)
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        if (TrackToggleManager.Instance.IsTrackEnabled(this.trackIndex))
        {
            fadeCoroutine = StartCoroutine(FadeIn());
        }
        else
        {
            fadeCoroutine = StartCoroutine(FadeOut());
        }
    }

    IEnumerator FadeIn()
    {
        float t = 0f;
        float startAlpha = group.alpha;
        while (t < inDuration)
        {
            t += Time.deltaTime;
            group.alpha = t.Remap(0f, inDuration, startAlpha, 1f);
            yield return null;
        }
        group.alpha = 1f;
    }
    IEnumerator FadeOut()
    {
        float t = 0;
        float startAlpha = group.alpha;
        while (t < outDuration)
        {
            t += Time.deltaTime;
            group.alpha = t.Remap(0f, outDuration, startAlpha, 0f);
            yield return null;
        }
        group.alpha = 0f;
    }
}
