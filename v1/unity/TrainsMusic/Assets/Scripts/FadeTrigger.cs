using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeTrigger : MonoBehaviour
{
    [SerializeField]
    AnimationCurve curve;
    [SerializeField]
    float duration = 1f;
    [SerializeField]
    Image image;

    public void Trigger()
    {
        StartCoroutine(Fade());
    }
    IEnumerator Fade()
    {
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float p = t / duration;
            float v = curve.Evaluate(p);
            image.color = new Color(1, 1, 1, v);
            yield return null;
        }
    }
}
