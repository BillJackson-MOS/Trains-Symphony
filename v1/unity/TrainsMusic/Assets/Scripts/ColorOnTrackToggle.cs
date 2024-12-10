using UnityEngine;
using UnityEngine.UI;

public class ColorOnTrackToggle : MonoBehaviour
{
    public Color colorOnTrack = Color.white;
    public Color colorOffTrack = Color.black;
    public float fadeDuration = .5f;
    [NaughtyAttributes.HideIf("isUIMode")]
    public  SpriteRenderer spriteRenderer;
    [NaughtyAttributes.ShowIf("isUIMode")]
    public MaskableGraphic graphic;

    private bool isUIMode => GetComponent<RectTransform>() != null;
    
    public bool Enable
    {
        set
        {
            if (spriteRenderer != null)
            {
                spriteRenderer.transform.Color(value ? colorOnTrack : colorOffTrack, fadeDuration);
            }
            else if (graphic != null)
            {
                graphic.transform.Color(value ? colorOnTrack : colorOffTrack, fadeDuration);
            }
        }
    }
}
