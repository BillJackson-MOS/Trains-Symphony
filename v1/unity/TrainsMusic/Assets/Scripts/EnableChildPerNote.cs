using System.Collections;
using System.Collections.Generic;
using SonicBloom.Koreo;
using UnityEngine;

public class EnableChildPerNote : KoreographyActor
{
    public int trackIndex = 0;
    public int currentChildIndex = 0;
    public bool oneAtATime = true;
    public bool directional = false;
    private int lastNoteNumber = 0;
    public override void EventTrigger(KoreographyEvent koreoEvent, int sampleTime, int sampleDelta, DeltaSlice deltaSlice)
    {
        if (TrackToggleManager.Instance.IsTrackEnabled(trackIndex) == false)
        {
            currentChildIndex = -1;
            return;
        }

        if (koreoEvent.HasIntPayload())
        {
            int intPayload = koreoEvent.GetIntValue();
            // Debug.Log("intPayload: " + intPayload);
            if (directional && currentChildIndex != -1)
            {
                if (lastNoteNumber > intPayload)
                {
                    currentChildIndex += transform.childCount - 1;
                }
                else
                {
                    currentChildIndex++;
                }
                currentChildIndex %= transform.childCount;
                currentChildIndex = Mathf.Clamp(currentChildIndex, 0, transform.childCount - 1);
                lastNoteNumber = intPayload;
            }
            else
            {
                currentChildIndex++;
                currentChildIndex %= transform.childCount;
            }
            if (oneAtATime)
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).gameObject.SetActive(i == currentChildIndex);
                }
            }
            else
            {
                transform.GetChild(currentChildIndex).gameObject.SetActive(false);
                transform.GetChild(currentChildIndex).gameObject.SetActive(true);
            }

        }
    }
}
