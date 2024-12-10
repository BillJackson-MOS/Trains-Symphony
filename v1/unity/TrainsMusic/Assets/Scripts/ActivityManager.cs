using UnityEngine;
using SonicBloom.Koreo;
using SonicBloom.Koreo.Players;
using System.Collections.Generic;

public class ActivityManager : MonoBehaviour
{
    [SerializeField]
    private MultiMusicPlayer player;
    public static MultiMusicPlayer Player { get; private set; }
    private static Koreographer koreographer;
    public static Koreographer Koreographer()
    {
        if (koreographer == null)
        {
            koreographer = FindObjectOfType<Koreographer>();
        }
        return koreographer;
    }
    public static Koreography Koreography() 
    {
        return Koreographer().GetKoreographyAtIndex(0);
    }
    public static List<string> KoreographyTracks
    {
        get
        {
            var tracks = Koreography().Tracks;
            List<string> trackNames = new List<string>();
            tracks.ForEach(x => trackNames.Add(x.name));
            return trackNames;
        }
    }

    private void Awake()
    {
        if (player == null)
        {
            player = FindObjectOfType<MultiMusicPlayer>();
        }
        Player = player;
    }

    private void Start()
    {
        Cursor.visible = false;
        player.Loop = true;
        player.Play();
    }
    public void Restart()
    {
        player.Stop();
        player.Play();
    }
    public static float GetNormalizedBeatTime(int subdivisions = 16)
    {
            string clipName = Player.GetCurrentClipName();
            double total =  Player.GetTotalSampleTimeForClip(clipName);
            double beat = total / subdivisions;
            double beatTime = Player.GetSampleTimeForClip(clipName) % beat;
            return (float)(beatTime / beat);
    }
    public static float GetNormalizedClipTime()
    {
            string clipName = Player.GetCurrentClipName();
            double total =  Player.GetTotalSampleTimeForClip(clipName);
            double time = Player.GetSampleTimeForClip(clipName);
            return (float)(time / total);
    }
}
