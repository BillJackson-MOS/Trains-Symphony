using System.Collections;
using System.Collections.Generic;
using SonicBloom.Koreo;
using UnityEngine;
using UnityEngine.Events;

public class OnKoreoEvent : KoreographyActor
{
    public string koreoTextPayloadMatch = "";
    public int koreoIntPayloadMatch = -1;
    public UnityEvent onKoreoEvent;
    public override void EventTrigger(KoreographyEvent koreoEvent, int sampleTime, int sampleDelta, DeltaSlice deltaSlice)
    {
        // Debug.Log(name + " EventTrigger");
        if (!TrackToggleManager.Instance.IsTrackEnabled(koreoTrackID))
        {
            // Debug.Log("Track is disabled");
            return;
        }
        if (!string.IsNullOrEmpty(koreoTextPayloadMatch) && koreoEvent.HasTextPayload())
        {
            string text = koreoEvent.GetTextValue();
            Debug.Log(text + " - " + koreoTextPayloadMatch);
            if (text == koreoTextPayloadMatch)
            {
                onKoreoEvent.Invoke();
            }
        }
        else if (koreoIntPayloadMatch != -1 && koreoEvent.HasIntPayload())
        {
            int intPayload = koreoEvent.GetIntValue();
            Debug.Log(intPayload + " - " + koreoIntPayloadMatch);
            if (intPayload == koreoIntPayloadMatch)
            {
                onKoreoEvent.Invoke();
            }
        }
    }
}
