using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Sleep : MonoBehaviour
{

    public UnityEvent OnSnooze;
    public UnityEvent OnWakeUp;
    bool isSnoozing = false;
    void Start()
    {
        TrackToggleManager.Instance.onTrackCountChange.AddListener(OnTrackCountChange);
        Snooze();   
    }

    private void OnTrackCountChange(int count)
    {
        if (!isSnoozing && count == 0)
        {
            Snooze();
            return;
        }
        if (isSnoozing && count > 0)
        {
            WakeUp();
            return;
        }
    }

    private void Snooze()
    {
        isSnoozing = true;
        OnSnooze.Invoke();
    }
    private void WakeUp()
    {
        isSnoozing = false;
        OnWakeUp.Invoke();
    }
}
