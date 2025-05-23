using UnityEngine;

public class InteractibleScript : MonoBehaviour
{
    public GameObject menu;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartInteraction()
    {
        menu.SetActive(true);
    }

    public void StopInteraction()
    {
        menu.SetActive(false);
    }
}
