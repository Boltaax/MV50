using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    public float transparence = 1.0f;
    public bool fadeOut = false;
    public float step = 0.1f;
    private Image image;

    void Start()
    {
        image = GetComponent<Image>();
    }

    void Update()
    {
        
        if (fadeOut)
        {
            transparence += step * Time.deltaTime;
        }
        else
        {
            transparence -= step * Time.deltaTime;
        }

        
        transparence = Mathf.Clamp01(transparence);

        Color color = image.color;
        color.a = transparence;
        image.color = color;
    }
}
