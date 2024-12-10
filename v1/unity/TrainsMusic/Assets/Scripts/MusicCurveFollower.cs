using UnityEngine;
using SonicBloom.Koreo;
using UnityEngine.Events;
public class MusicCurveFollower : MonoBehaviour
{
    [SerializeField]
    private string koreoTrackID;
    [SerializeField]
    private float scaler = 1f;
    public UnityEvent<float> onCurveValue;
    void Start()
    {
        Koreographer.Instance.RegisterForEvents(koreoTrackID, KoreoEvent);
    }

    private void KoreoEvent(KoreographyEvent koreoEvent)
    {
        if (koreoEvent.HasCurvePayload())
        {
            float val = koreoEvent.GetValueOfCurveAtTime(Koreographer.Instance.GetMusicSampleTime());
            Debug.Log("Curve Value: " + val);
            onCurveValue.Invoke(val * scaler);
        }

    }
}
