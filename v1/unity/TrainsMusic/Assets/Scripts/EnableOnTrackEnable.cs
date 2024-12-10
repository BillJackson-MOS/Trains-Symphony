using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnableOnTrackEnable : MonoBehaviour
{
    public int trackIndex = 0;
    public UnityEvent<bool> onTrackStateChange;
    public Animator animator;
    public string animatorBoolName;
    public bool invert = false;
    private bool currentState = false;
    private void Start()
    {
        TrackToggleManager.Instance.onTrackCountChange.AddListener(OnTrackCountChange);
    }
    
    private void OnTrackCountChange(int trackCount)
    {
        bool trackEnabled = TrackToggleManager.Instance.IsTrackEnabled(trackIndex);
        if (currentState == trackEnabled)
        {
            return;
        }
        currentState = trackEnabled;
        if (invert)
        {
            onTrackStateChange.Invoke(!TrackToggleManager.Instance.IsTrackEnabled(trackIndex));
            if (animator != null && !string.IsNullOrEmpty(animatorBoolName))
            {
                animator.SetBool(animatorBoolName, !TrackToggleManager.Instance.IsTrackEnabled(trackIndex));
            }
        }
        else
        {
            onTrackStateChange.Invoke(TrackToggleManager.Instance.IsTrackEnabled(trackIndex));
            if (animator != null && !string.IsNullOrEmpty(animatorBoolName))
            {
                animator.SetBool(animatorBoolName, TrackToggleManager.Instance.IsTrackEnabled(trackIndex));
            }
        }
    }
}
