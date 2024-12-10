using UnityEngine;
using SonicBloom.Koreo;

public class MusicScale : KoreographyActor
{
    public Transform scaleTransform;
    public Vector3 scaleStart = Vector3.one;
    [NaughtyAttributes.Button("Set Start")]
    public void SetStartToCurrent() { scaleStart = ScaleTransform.localScale; }
    public Vector3 scaleTarget = Vector3.one;
    [NaughtyAttributes.Button("Set Target")]
    public void SetSTargetToCurrent() { scaleTarget = ScaleTransform.localScale; }

    private Transform ScaleTransform
    {
        get
        {
            if (scaleTransform == null)
            {
                scaleTransform = transform;
            }
            return scaleTransform;
        }
    }

    private void OnValidate()
    {
        if (scaleTransform == null)
        {
            scaleTransform = transform;
        }
    }

    public override void EventTrigger(KoreographyEvent koreoEvent, int sampleTime, int sampleDelta, DeltaSlice deltaSlice)
    {
        if (koreoEvent.HasCurvePayload())
        {
            ScaleTransform.localScale = Vector3.Lerp(scaleStart, scaleTarget, koreoEvent.GetValueOfCurveAtTime(sampleTime));
        }    
    }
}
