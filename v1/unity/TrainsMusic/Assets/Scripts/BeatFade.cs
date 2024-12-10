using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using SonicBloom.Koreo;

[RequireComponent(typeof(Image))]
public class BeatFade : KoreographyActor
{
    public Image image;
    public int beatIndex;

    public override void EventTrigger(KoreographyEvent koreoEvent, int sampleTime, int sampleDelta, DeltaSlice deltaSlice)
    {
        if (koreoEvent.HasIntPayload() && koreoEvent.GetIntValue() == beatIndex)
        {
            Trigger();
        }
    }

    [NaughtyAttributes.Button]
    public void Trigger()
    {
        GetImage().color = GetImage().color.With(a: 1f);
        Wrj.Utils.MapToCurve.EaseOut.FadeAlpha(GetImage(), 0f, .25f);
    }

    private Image GetImage()
    {
        if (image == null)
        {
            image = GetComponent<Image>();
        }
        return image;
    }    
}
