using UnityEngine;

public class CrowdManager : MonoBehaviour
{
    public GameObject agentPrefab;
    public int numberOfAgents = 30;
    public Vector2 areaSize = new Vector2(10, 10);
    public float minSpacing = 1.2f;
    public Transform player;

    void Start()
    {
        SpawnAgents();
    }

    void SpawnAgents()
    {
        for (int i = 0; i < numberOfAgents; i++)
        {
            Vector3 pos = GetValidPosition();
            GameObject agent = Instantiate(agentPrefab, pos, Quaternion.identity);
            var script = agent.GetComponent<CrowdAgent>();

            script.player = player;
            script.areaSize = areaSize;
            script.behavior = (BehaviorType)Random.Range(0, 3);
        }
    }

    Vector3 GetValidPosition()
    {
        Vector3 pos;
        int tries = 0;
        do
        {
            float x = Random.Range(-areaSize.x / 2, areaSize.x / 2);
            float z = Random.Range(-areaSize.y / 2, areaSize.y / 2);
            pos = new Vector3(x, 1, z);
            tries++;
        } while (Physics.CheckSphere(pos, minSpacing) && tries < 10);
        return pos;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(areaSize.x, 0.1f, areaSize.y));
    }
}

