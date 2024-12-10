using System.Collections;
using System.Collections.Generic;
using SonicBloom.Koreo;
using UnityEngine;

public class PositionToNote : KoreographyActor
{
    public int lowNote = 0;
    public int highNote = 127;
    public Vector2 xRange = new Vector2(0, 1);
    public Vector2 yRange = new Vector2(0, 1);
    [NaughtyAttributes.Button]
    public void AutoSetNoteRange()
    {
        highNote = 0;
        lowNote = 127;
        var events = track.GetAllEvents();
        foreach (var e in events)
        {
            if (e.HasIntPayload())
            {
                int note = e.GetIntValue();
                if (note < lowNote)
                {
                    lowNote = note;
                }
                if (note > highNote)
                {
                    highNote = note;
                }
            }
        }
    }

    private KoreographyTrackBase track
    {
        get
        {
            return ActivityManager.Koreography().GetTrackByID(koreoTrackID);
        }
    }

    public override void EventTrigger(KoreographyEvent koreoEvent, int sampleTime, int sampleDelta, DeltaSlice deltaSlice)
    {
        if (koreoEvent.HasIntPayload())
        {
            int note = koreoEvent.GetIntValue();
            float x = note.Remap(lowNote, highNote, xRange.x, xRange.y);
            float y = note.Remap(lowNote, highNote, yRange.x, yRange.y);
            transform.localPosition = new Vector3(x, y, transform.localPosition.z);
            Log(name + ": " + note);
        }
    }
}