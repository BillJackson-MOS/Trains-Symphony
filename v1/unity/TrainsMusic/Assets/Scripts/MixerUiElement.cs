using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MixerUiElement : MonoBehaviour
{
    public Slider volumeSlider;
    public Slider panSlider;
    public Button button;
    public TMPro.TextMeshProUGUI sourceName;
    

    private MixRecall _mixRecall;

    public MixRecall mixRecall
    {
        get => _mixRecall;
        set
        {
            _mixRecall = value;
            sourceName.text = _mixRecall.name;
            volumeSlider.value = _mixRecall.ConnectedSource.volume;
            panSlider.value = _mixRecall.ConnectedSource.panStereo;
            button.onClick.AddListener(() => 
            {
                TrackToggleManager.Instance.tracks[_mixRecall.TrackIndex].isEnabled = 
                !TrackToggleManager.Instance.IsTrackEnabled(_mixRecall.TrackIndex);
            });
            volumeSlider.onValueChanged.AddListener((v) => _mixRecall.ConnectedSource.volume = v);
            panSlider.onValueChanged.AddListener((v) => _mixRecall.ConnectedSource.panStereo = v);
        }
    }
}
