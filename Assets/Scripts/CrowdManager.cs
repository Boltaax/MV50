using UnityEngine;
using System.Collections.Generic;

public class CrowdManager : MonoBehaviour
{
    public GameObject agentPrefab;
    public Transform player;
    public GameObject zoneDeFoule;

    public List<CrowdDifficulty> difficultyLevels;
    public int currentDifficultyIndex = 0;

    private List<GameObject> agents = new List<GameObject>();

    void Start()
    {
        ApplyDifficulty(currentDifficultyIndex);
    }

    public void ApplyDifficulty(int index)
    {
        currentDifficultyIndex = Mathf.Clamp(index, 0, difficultyLevels.Count - 1);
        ClearAgents();
        SpawnAgents();
    }

    void ClearAgents()
    {
        foreach (var agent in agents)
            Destroy(agent);
        agents.Clear();
    }

    void SpawnAgents()
    {
        var settings = difficultyLevels[currentDifficultyIndex];

        int attempts = 0;
        for (int i = 0; i < settings.agentCount && attempts < settings.agentCount * 5; i++)
        {
            Vector3 pos = GetRandomPositionInZone();
            if (!Physics.CheckSphere(pos, settings.spacing))
            {
                var agent = Instantiate(agentPrefab, pos, Quaternion.identity);
                var script = agent.GetComponent<CrowdAgent>();
                script.player = player;
                script.speed = settings.agentSpeed;
                script.repulsionStrength = settings.repulsionStrength;
                script.avoidanceRadius = settings.avoidanceRadius;
                script.changeTargetDelay = settings.changeTargetDelay;
                script.behavior = (BehaviorType)Random.Range(0, 3);
                script.zoneDeFoule = zoneDeFoule;
                agents.Add(agent);
            }
            else i--; // réessaye une nouvelle position
            attempts++;
        }
    }

    Vector3 GetRandomPositionInZone()
    {
        // Choisit une position aléatoire dans les box triggers de la zone
        var boxes = zoneDeFoule.GetComponentsInChildren<BoxCollider>();
        var box = boxes[Random.Range(0, boxes.Length)];

        Vector3 local = new Vector3(
            Random.Range(-box.size.x / 2, box.size.x / 2),
            0f,
            Random.Range(-box.size.z / 2, box.size.z / 2)
        );

        return box.transform.TransformPoint(box.center + local);
    }
}
