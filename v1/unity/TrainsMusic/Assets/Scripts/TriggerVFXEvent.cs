using UnityEngine;
using SonicBloom.Koreo;
using NaughtyAttributes;
using UnityEngine.Events;

public class TriggerVFXEvent : MonoBehaviour
{
    public Koreographer koreographer;
    
    public string koreoTrackID;
    public bool useTextEvent = true;
    [ShowIf("useTextEvent")]
    public string textEvent;
    [HideIf("useTextEvent")]
    public int intEvent;

    public UnityEvent onTextEvent;
    private void OnValidate()
    {
        if (koreographer == null)
        {
            koreographer = FindObjectOfType<Koreographer>();
        }
    }
    void Start()
    {
        koreographer.RegisterForEventsWithTime(koreoTrackID, TriggerEvent);
    }

    private void TriggerEvent(KoreographyEvent koreoEvent, int sampleTime, int sampleDelta, DeltaSlice deltaSlice)
    {
        if (useTextEvent)
        {
            if (koreoEvent.HasTextPayload() && koreoEvent.GetTextValue() == textEvent)
            {
                onTextEvent.Invoke();
            }
        }
        else
        {
            if (koreoEvent.HasIntPayload() && koreoEvent.GetIntValue() == intEvent)
            {
                onTextEvent.Invoke();
            }
        }
    }
}

