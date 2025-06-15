using TMPro;
using UnityEngine;

public class Test : MonoBehaviour
{
    private readonly string[] dialogs =
    {
        "Hello",
        "Hi",
        "KYS",
    };
    public TextMeshProUGUI tmp;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        ChangeText();   
    }

    public void ChangeText()
    {
        tmp.text = dialogs[Random.Range(0, dialogs.Length)];
    }
}
