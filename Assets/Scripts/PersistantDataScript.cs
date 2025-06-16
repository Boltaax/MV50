using UnityEngine;

/*
 Class singleton pour transmettre des données entre les scènes
 */
public class PersistantDataScript : MonoBehaviour
{
    public static PersistantDataScript instance;
    public bool dayStarted = false;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
