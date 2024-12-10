using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SonicBloom.Koreo;
using System;
using SonicBloom.Koreo.Players;


public class BeatScale : MonoBehaviour
{
    public Transform target;
    public AnimationCurve scaleOverBeat;
    public int subdivisions = 16;
    public Vector3 upperScale = new Vector3(1f, 1.5f, 1f);
    public TrackToggleManager toggleManager;
    [NaughtyAttributes.Dropdown("koreographyTracks")]
    public string koreoTrackID = "All";
    private List<string> koreographyTracks
    {
        get
        {
            var tracks = ActivityManager.KoreographyTracks;
            tracks.Add("All");
            return tracks;
        }
    }

    private void Awake()
    {
        if (target == null)
        {
            target = transform;
        }
    }
    void Update()
    {
        if (ActivityManager.Player.IsPlaying && toggleManager.PlayingTrackCount > 0 && (koreoTrackID == "All" || toggleManager.IsTrackEnabled(koreoTrackID)))
        {
            float beatTime = ActivityManager.GetNormalizedBeatTime(subdivisions);
            float scale = scaleOverBeat.Evaluate(beatTime);
            target.localScale = Vector3.Lerp(Vector3.one, upperScale, scale);
        }       
        else
        {
            transform.localScale = Vector3.one;
        }
    }
}
                