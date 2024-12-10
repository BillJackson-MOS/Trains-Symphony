using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SonicBloom.Koreo;

public abstract class KoreographyActor : MonoBehaviour
{
    public bool enableLogging = false;
    [NaughtyAttributes.Dropdown("koreographyTracks")]
    public string koreoTrackID;

    private List<string> koreographyTracks
    {
        get
        {
            return ActivityManager.KoreographyTracks;
        }
    }

    void Start()
    {
        ActivityManager.Koreographer().RegisterForEventsWithTime(koreoTrackID, EventTrigger);  
    }
    protected void Log(string message)
    {
        if (enableLogging)
        {
            Debug.Log(message);
        }
    }
    public abstract void EventTrigger(KoreographyEvent koreoEvent, int sampleTime, int sampleDelta, DeltaSlice deltaSlice);

}
