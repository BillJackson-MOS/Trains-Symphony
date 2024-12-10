using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Snore : MonoBehaviour
{
    [SerializeField]
    AnimationCurve yCurveOverSnore;
    [SerializeField]
    AnimationCurve xCurveOverSnore;
    [SerializeField]
    List<Transform> tempChildren = new List<Transform>();
    List<Transform> originalParents = new List<Transform>();
    AudioSource snore;
    RectTransform rectTransform;
    Vector2 initialSizeDelta;
    Coroutine snoreCoroutine = null;
    private float initialDelayTime = 0.5f;
    private float timeBetweenSnores = 3f;
    public void SetSnoreInterval(float time)
    {
        timeBetweenSnores = time;
    }
    public void SetDelayTime(float time)
    {
        initialDelayTime = time;
    }
    private void Awake() {
        snore = GetComponent<AudioSource>();
        rectTransform = GetComponent<RectTransform>();
        initialSizeDelta = rectTransform.sizeDelta;
    }
    private void Start() {
        foreach (Transform child in tempChildren)
        {
            originalParents.Add(child.parent);
        }

        TrackToggleManager.Instance.onTrackCountChange.AddListener((int count) =>
        {
            if (count == 0)
            {
                Enable();
            }
            else
            {
                Disable();
            }
        });
    }
    public void Enable() {
        if (snoreCoroutine != null)
        {
            StopCoroutine(snoreCoroutine);
        }
        snoreCoroutine = StartCoroutine(SnoreAnimation());
    }
    public void Disable() {
        if (snoreCoroutine != null)
        {
            StopCoroutine(snoreCoroutine);
        }
        snoreCoroutine = StartCoroutine(ResetCoroutine());
    }

    private IEnumerator SnoreAnimation(bool initialDelay = true)
    {
        if (initialDelay)
        {
            yield return new WaitForSeconds(initialDelayTime);
        }
        snore.PlayOneShot(snore.clip);
        float duration = snore.clip.length;
        float time = 0;
        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            rectTransform.sizeDelta = new Vector2(initialSizeDelta.x + xCurveOverSnore.Evaluate(t), initialSizeDelta.y + yCurveOverSnore.Evaluate(t));
            yield return null;
        }
        yield return new WaitForSeconds(timeBetweenSnores);
        snoreCoroutine = StartCoroutine(SnoreAnimation(false));
    }
    private IEnumerator ResetCoroutine()
    {
        Vector2 resetFrom = rectTransform.sizeDelta;
        yield return Wrj.Utils.MapToCurve.Ease.ManipulateFloat((t) =>
        {
            rectTransform.sizeDelta = Vector2.Lerp(resetFrom, initialSizeDelta, t);
        }, 0f, 1f, .5f);
        foreach (Transform child in tempChildren)
        {
            child.SetParent(originalParents[tempChildren.IndexOf(child)]);
        }
    }
}
