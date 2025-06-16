using UnityEngine;

public class FloatingArrow : MonoBehaviour
{
    public float floatAmplitude = 0.2f;
    public float floatFrequency = 2f;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.localPosition;
    }

    void Update()
    {
        float offset = Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        transform.localPosition = startPos + new Vector3(0, offset, 0);
    }
}
