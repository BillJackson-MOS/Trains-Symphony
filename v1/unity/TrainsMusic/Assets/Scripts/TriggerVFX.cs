using UnityEngine;
using SonicBloom.Koreo;
using UnityEngine.VFX;
using NaughtyAttributes;
using UnityEngine.Events;

public class TriggerVFX : KoreographyActor
{
    public VisualEffect vfx; // Updated the type to VisualEffectGraph
    public string floatParameterName;
    [MinMaxSlider(0f, 1f)]
    public Vector2 rangeFrom;
    public Vector2 rangeTo;
    public UnityEvent<float> onValueChange;
    public UnityEvent<Vector3> onVector3ValueChange;

    public override void EventTrigger(KoreographyEvent koreoEvent, int sampleTime, int sampleDelta, DeltaSlice deltaSlice)
    {
        if (koreoEvent.HasCurvePayload())
        {
            float value = koreoEvent.GetValueOfCurveAtTime(sampleTime);
            float mappedValue = value.Remap(rangeFrom.x, rangeFrom.y, rangeTo.x, rangeTo.y);
            if (!string.IsNullOrEmpty(floatParameterName))
            {
                vfx.SetFloat(floatParameterName, mappedValue);
            }
            if (onValueChange != null)
            {
                onValueChange.Invoke(mappedValue);
            }
            if (onVector3ValueChange != null)
            {
                onVector3ValueChange.Invoke(Vector3.one * mappedValue);
            }
            Debug.Log($"{value}=>{mappedValue}");
        }
    }
}
