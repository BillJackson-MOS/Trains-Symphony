using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SonicBloom.Koreo;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Linq;
using FAST;
using System;

public class TrackToggleManager : KoreographyActor
{
    [SerializeField]
    AudioMixer audioMixer;
    // [SerializeField]
    // int groupCount = 16;
    // [SerializeField]
    // List<bool> mixerGroupStates = new List<bool>();
    // [SerializeField]
    // Button[] buttons;
    [SerializeField]
    Color enabledColor;
    [SerializeField]
    Color disabledColor;
    [SerializeField]
    Toggle latchToggle;
    //MidiDevice midiDevice;
    [SerializeField]
    int[] midiKeys;
    public Track[] tracks;
    [SerializeField]
    int[] beats;
    public UnityEvent<int> onTrackCountChange;
    private bool[] cachedStates;
    private float lastInteractionTime = 0f;
    private float idlePeriod = 120f;
    public int PlayingTrackCount => tracks.Where(x => x.isEnabled == true).Count();

    public static TrackToggleManager Instance;
    void Awake ()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("Multiple TrackToggleManager's instantiated. Component removed from " + gameObject.name + ". Instance already found on " + Instance.gameObject.name + "!");
            Destroy(this);
        }
        cachedStates = new bool[tracks.Length];
        for (int i = 0; i < tracks.Length; i++)
        {
            bool state = audioMixer.GetFloat($"Volume{i}", out float volume) && volume == 0f;
            tracks[i].midiNote = midiKeys[i];
            tracks[i].isEnabled = state;
            cachedStates[i] = state;
        }
        latchToggle.onValueChanged.AddListener((bool value) =>
        {
            Reset();
        });
        for (int i = 0; i < tracks.Length; i++)
        {
            int index = i;
            if (tracks[i].button != null)
            {
                tracks[i].button.AddEventTrigger(UnityEngine.EventSystems.EventTriggerType.PointerDown, () => Click(index));
                tracks[i].button.AddEventTrigger(UnityEngine.EventSystems.EventTriggerType.PointerUp, () => Unclick(index));
            }
        }
        StartCoroutine(SerialListenRoutine());
    }
    // private void Start() {
    // }
    private IEnumerator SerialListenRoutine()
    {
        string cachedStates = string.Empty;
        while (true)
        {
            try 
            {
                var serial = FAST.Application.serialConnections[0];
                string currentData = string.Empty;
                foreach (string data in serial.Data)
                {
                    if (data.Length == 16) 
                    {
                        currentData = data;
                        if (string.IsNullOrEmpty(cachedStates))
                        {
                            cachedStates = currentData; 
                        }
                    }
                } 
                // Debug.LogWarning($"Data: {currentData} vs {cachedStates}");
                if (currentData != cachedStates)
                {
                    for (int i = 0; i < currentData.Length; i++)
                    {
                        if (currentData[i] != cachedStates[i])
                        {
                            if (currentData[i] == '1')
                            {
                                Debug.LogWarning($"{i} ON!");
                                Click(i);
                            }
                            else
                            {
                                Debug.LogWarning($"{i} OFF!");
                                Unclick(i);
                            }
                        }
                    }
                }
                cachedStates = currentData;
            }
            catch   
            {
                
            }
        yield return new WaitForSeconds(0.2f);
        }
        
    }

    void Click(int index)
    {
        Print($"Clicked {index}");
        if (latchToggle.isOn)
        {
            tracks[index].isEnabled = !tracks[index].isEnabled;
        }
        else    
        {
            tracks[index].isEnabled = true;
        }
        if (tracks[index].button != null)
            tracks[index].button.GetComponent<Image>().color = tracks[index].isEnabled ? enabledColor : disabledColor;
        lastInteractionTime = Time.time;
    }
    void Unclick(int index)
    {
        Print($"Unclicked {index}");
        if (!latchToggle.isOn)
        {
            tracks[index].isEnabled = false;
            if (tracks[index].button != null)
                tracks[index].button.GetComponent<Image>().color = tracks[index].isEnabled ? enabledColor : disabledColor;
        }
        lastInteractionTime = Time.time;
    }

    public void Reset()
    {
        for (int i = 0; i < tracks.Length; i++)
        {
            tracks[i].isEnabled = false;
            // tracks[i].button.GetComponent<Image>().color = disabledColor;
        }
    }

    void Update()
    {
        KeyCheck();
        if (Time.time - lastInteractionTime > idlePeriod)
        {
            Reset();
        }
    }

    public bool IsTrackEnabled(int index)
    {
        return tracks[index].isEnabled;
    }
    public bool IsTrackEnabled(string trackId)
    {
        return tracks.Where(s => s.koreoTrackID == trackId && s.isEnabled).ToArray().Length > 0;
    }

    public override void EventTrigger(KoreographyEvent koreoEvent, int sampleTime, int sampleDelta, DeltaSlice deltaSlice)
    {
        Print("Event Triggered");
        if (koreoEvent.HasIntPayload())
        {
            int beatPayload = koreoEvent.GetIntValue();
            Print("Has Int Payload: " + beatPayload);
            if (beats.Length == 0 || beats.Contains(beatPayload))
            {
                ApplyQueuedToggle();
            }
        }
    }
    private void ApplyQueuedToggle()
    {
        bool isChangeDetected = false;
        for (int i = 0; i < tracks.Length; i++)
        {
            if (cachedStates[i] == tracks[i].isEnabled)
            {
                continue;
            }
            cachedStates[i] = tracks[i].isEnabled;
            isChangeDetected = true;
            float level = tracks[i].isEnabled ? 0f : -80f;
            float currentLevel = level == 0f ? -80f : 0f;
            StartCoroutine(UpdateMixerGroupVolume($"Volume{i}", currentLevel, level));
        }
        if (isChangeDetected)
        {
            onTrackCountChange.Invoke(PlayingTrackCount);
            string data = "";
            for (int i = 0; i < tracks.Length; i++)
            {
                data += tracks[i].isEnabled ? "1" : "0";
            }
            FAST.Application.serialConnections[0].SendAsLine(data);
        }
    }

    private IEnumerator UpdateMixerGroupVolume(string parameter, float startLevel, float endLevel)
    {
        Print($"Updating {parameter} from {startLevel} to {endLevel}");
        float currentTime = 0;
        float duration = 0.25f;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            float newLevel = Mathf.Lerp(startLevel, endLevel, currentTime / duration);
            audioMixer.SetFloat(parameter, newLevel);
            yield return null;
        }
        audioMixer.SetFloat(parameter, endLevel);
    }
    private void Print(string message)
    {
        if (enableLogging)
        {
            Debug.Log(message);
        }
    }
    private void KeyCheck(int note = 128)
    {
        if (Input.GetKeyDown(KeyCode.Space) || (note != 128 && !midiKeys.Contains(Mathf.Abs(note))))
        {
            Reset();
        }
        if (Input.GetKeyDown(KeyCode.Alpha1) || note == midiKeys[0])
        {
            Click(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) || note == midiKeys[1])
        {
            Click(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) || note == midiKeys[2])
        {
            Click(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4) || note == midiKeys[3])
        {
            Click(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5) || note == midiKeys[4])
        {
            Click(4);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6) || note == midiKeys[5])
        {
            Click(5);
        }
        if (Input.GetKeyDown(KeyCode.Alpha7) || note == midiKeys[6])
        {
            Click(6);
        }
        if (Input.GetKeyDown(KeyCode.Alpha8) || note == midiKeys[7])
        {
            Click(7);
        }
        if (Input.GetKeyDown(KeyCode.Q) || note == midiKeys[8])
        {
            Click(8);
        }
        if (Input.GetKeyDown(KeyCode.W) || note == midiKeys[9])
        {
            Click(9);
        }
        if (Input.GetKeyDown(KeyCode.E) || note == midiKeys[10])
        {
            Click(10);
        }
        if (Input.GetKeyDown(KeyCode.R) || note == midiKeys[11])
        {
            Click(11);
        }
        if (Input.GetKeyDown(KeyCode.T) || note == midiKeys[12])
        {
            Click(12);
        }
        if (Input.GetKeyDown(KeyCode.Y) || note == midiKeys[13])
        {
            Click(13);
        }
        if (Input.GetKeyDown(KeyCode.U) || note == midiKeys[14])
        {
            Click(14);
        }
        if (Input.GetKeyDown(KeyCode.I) || note == midiKeys[15])
        {
            Click(15);
        }

        if(Input.GetKeyUp(KeyCode.Alpha1) || note == midiKeys[0] * -1)
        {
            Unclick(0);
        }
        if (Input.GetKeyUp(KeyCode.Alpha2) || note == midiKeys[1] * -1)
        {
            Unclick(1);
        }
        if (Input.GetKeyUp(KeyCode.Alpha3) || note == midiKeys[2] * -1)
        {
            Unclick(2);
        }
        if (Input.GetKeyUp(KeyCode.Alpha4) || note == midiKeys[3] * -1)
        {
            Unclick(3);
        }
        if (Input.GetKeyUp(KeyCode.Alpha5) || note == midiKeys[4] * -1)
        {
            Unclick(4);
        }
        if (Input.GetKeyUp(KeyCode.Alpha6) || note == midiKeys[5] * -1)
        {
            Unclick(5);
        }
        if (Input.GetKeyUp(KeyCode.Alpha7) || note == midiKeys[6] * -1)
        {
            Unclick(6);
        }
        if (Input.GetKeyUp(KeyCode.Alpha8) || note == midiKeys[7] * -1)
        {
            Unclick(7);
        }
        if (Input.GetKeyUp(KeyCode.Q) || note == midiKeys[8] * -1)
        {
            Unclick(8);
        }
        if (Input.GetKeyUp(KeyCode.W) || note == midiKeys[9] * -1)
        {
            Unclick(9);
        }
        if (Input.GetKeyUp(KeyCode.E) || note == midiKeys[10] * -1)
        {
            Unclick(10);
        }
        if (Input.GetKeyUp(KeyCode.R) || note == midiKeys[11] * -1)
        {
            Unclick(11);
        }
        if (Input.GetKeyUp(KeyCode.T) || note == midiKeys[12] * -1)
        {
            Unclick(12);
        }
        if (Input.GetKeyUp(KeyCode.Y) || note == midiKeys[13] * -1)
        {
            Unclick(13);
        }
        if (Input.GetKeyUp(KeyCode.U) || note == midiKeys[14] * -1)
        {
            Unclick(14);
        }
        if (Input.GetKeyUp(KeyCode.I) || note == midiKeys[15] * -1)
        {
            Unclick(15);
        }
    }
    [System.Serializable]
    public class Track
    {
        [NaughtyAttributes.Dropdown("koreographyTracks")]
        public string koreoTrackID;

        private List<string> koreographyTracks
        {
            get
            {
                return ActivityManager.KoreographyTracks;
            }
        }        
        public bool isEnabled;
        public KeyCode key;
        public int midiNote = 128;
        public Button button;
        // public void OnValidate()
        // {
        //     int index = System.Array.IndexOf(TrackToggleManager.Instance.tracks, this);
        //     if (string.IsNullOrEmpty(trackId))
        //     {
        //         trackId = koreographyTracks[index];
        //     }
        //     if (midiNote == 128)
        //     {
        //         midiNote = index;
        //     }
        // }
    }
}

