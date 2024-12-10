using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRotate : MonoBehaviour
{
    public RectTransform rectTransform;
    public float speed = 1f;
    private float angle = 0f;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        angle = rectTransform.localEulerAngles.z;
    }
    void Update()
    {
        angle = (angle + speed) % 360;
        rectTransform.LocalEulerWith(z: angle);
    }
}
