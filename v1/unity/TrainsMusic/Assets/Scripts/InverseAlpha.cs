using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InverseAlpha : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup targetGroup;
    [SerializeField]
    private MaskableGraphic targetGraphic;
    [SerializeField]
    private CanvasGroup invertFromGroup;
    [SerializeField]
    private MaskableGraphic invertFromGraphic;

    private void Update() {
        float sourceAlpha = (invertFromGroup != null) ? invertFromGroup.alpha : invertFromGraphic.color.a;
        sourceAlpha = sourceAlpha.Remap(0, 1, 1, 0);
        if (targetGroup != null)
        {
            targetGroup.alpha = 1 - sourceAlpha;
        } 
        else if (targetGraphic != null)
        {
            targetGraphic.color = targetGraphic.color.With(a: sourceAlpha);
        }
    }

}
