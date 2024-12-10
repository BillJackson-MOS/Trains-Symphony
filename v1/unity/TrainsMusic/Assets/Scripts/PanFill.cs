using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class PanFill : MonoBehaviour
{
    public Image fillImageLeft;
    public Image fillImageRight;
    public Slider slider;

    void Update()
    {
        fillImageLeft.fillAmount = slider.normalizedValue.Remap(.5f, 0f, 0f, .9f);
        fillImageRight.fillAmount = slider.normalizedValue.Remap(.5f, 1f, 0f, .9f);
    }
}
