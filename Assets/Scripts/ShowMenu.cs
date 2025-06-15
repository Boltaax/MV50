using UnityEngine;

public class ShowMenu : MonoBehaviour
{

    public InteractibleScript target;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider other){
        if (other.gameObject.CompareTag("Player")){
            Debug.Log("grosse collision");
            target.StartInteraction();
        }
        
    }

    void OnTriggerExit(Collider other){
        if (other.gameObject.CompareTag("Player")){
            target.StopInteraction();
        }
    }
}
