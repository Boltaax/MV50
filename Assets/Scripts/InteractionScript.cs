using UnityEngine;

public class InteractionScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Interactible"))
        {
            other.gameObject.GetComponent<InteractibleScript>().StartInteraction();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Interactible"))
        {
            other.gameObject.GetComponent<InteractibleScript>().StopInteraction();
        }
    }
}
