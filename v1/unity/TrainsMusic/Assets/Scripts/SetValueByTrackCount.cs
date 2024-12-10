using UnityEngine;
using UnityEngine.UI;

public class SetValueByTrackCount : MonoBehaviour
{
    public Image image;
    public Animator animator;
    public int minTrackCount = 0;
    public int maxTrackCount = 16;
    public float minValue = 0f;
    public float maxValue = 1f;
    public string propertyName = "_Offset";
    public bool isModAdditive = false;

    private float targetValue = 0f;
    private float velocity = 0f;
    private float currentSpeed = 0f;
    private float currentValue = 0f;
    private Material material;

    private void Start()
    {
        TrackToggleManager.Instance.onTrackCountChange.AddListener(OnTrackCountChange);
        if (image != null)
        {
            material = image.material;
            material.SetFloat(propertyName, minValue);
            // Debug.Log($"Set initial value on {image.gameObject.name} material to {minValue}");
        }
        else if (animator != null)
        {
            animator.SetFloat(propertyName, minValue);
            // Debug.Log($"Set initial value on {animator.gameObject.name} to {minValue}");
        }
        currentSpeed = minValue;
    }
    private void OnTrackCountChange(int trackCount)
    {
        targetValue = trackCount.Remap(minTrackCount, maxTrackCount, minValue, maxValue);
    }
    private void Update()
    {
        if (targetValue == currentValue)
        {
            return;
        }
        targetValue = Mathf.Clamp(targetValue, minValue, maxValue);
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetValue, ref velocity, 0.1f);
        if (isModAdditive)
        {
            currentValue += currentSpeed * Time.deltaTime;
            currentValue %= 1f;
        }
        else
        {
            currentValue = currentSpeed;
        }

        if (image != null)
        {
            material.SetFloat(propertyName, currentValue);
            // Debug.Log($"Set {propertyName} on {image.gameObject} material to {currentValue}");
        }
        else if (animator != null)
        {
            targetValue = Mathf.Clamp(targetValue, minValue, maxValue);
            animator.SetFloat(propertyName, currentValue);
            // Debug.Log($"Set {propertyName} on {animator.gameObject.name} to {currentValue}");
        }
    }
}
