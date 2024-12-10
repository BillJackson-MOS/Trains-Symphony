using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PersistentToggle : MonoBehaviour
{
    [SerializeField]
    private Toggle _toggle;
    [SerializeField]
    private Snore _snore;

    public Toggle buttonModeToggle
    {
        get
        {
            if (_toggle == null)
            {
                _toggle = GetComponent<Toggle>();
            }
            return _toggle;
        }
    }

    public Snore snore
    {
        get
        {
            if (_snore == null)
            {
                _snore = FindObjectOfType<Snore>();
            }
            return _snore;
        }
    }

    private void Start()
    {
        StartCoroutine(LoadSettings());
    }

    void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus)
        {
            StartCoroutine(LoadSettings());
        }
    }
    private IEnumerator LoadSettings()
    {
        FAST.Application.ReadSettings();
        yield return new WaitForEndOfFrame();
        var settings = FAST.Application.settings as ApplicationSettings;
        buttonModeToggle.isOn = settings.toggleMode;
        var snore = FindObjectOfType<Snore>();
        snore.SetDelayTime(settings.snoreDelaySeconds);
        snore.SetSnoreInterval(settings.snoreIntervalSeconds);
    }
    public void Toggle()
    {
        buttonModeToggle.isOn = !buttonModeToggle.isOn;
        var settings = FAST.Application.settings as ApplicationSettings;
        settings.toggleMode = buttonModeToggle.isOn;
        FAST.Application.WriteSettings();
    }
}
