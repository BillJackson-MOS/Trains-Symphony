using SonicBloom.Koreo;
using UnityEngine;

public class TrackSliderPanel : MonoBehaviour
{
    public MixerUiElement prototype;

    void Start()
    {
        
        var mixRecalls = FindObjectsOfType<MixRecall>();
        foreach (var mixRecall in mixRecalls)
        {
            var mixerUiElement = Instantiate(prototype, transform);
            mixerUiElement.gameObject.SetActive(true);
            mixerUiElement.mixRecall = mixRecall;
        }
    }
}
