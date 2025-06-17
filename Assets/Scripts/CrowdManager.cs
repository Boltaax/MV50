using UnityEngine;
using System.Collections.Generic;

public class CrowdManager : MonoBehaviour
{
    public GameObject agentPrefab;
    public Transform player;
    public GameObject zoneDeFoule;
    public GameObject interactionArrow;

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

        int randomAgent = Random.Range(0, settings.agentCount);
        for (int i = 0; i < settings.agentCount; i++)
        {
            Vector3 pos = GetValidPosition();
            var agent = Instantiate(agentPrefab, pos, Quaternion.Euler(0, Random.Range(0f, 360f), 0));
            var script = agent.GetComponent<CrowdAgent>();
            script.player = player;
            script.speed = settings.agentSpeed;
            script.repulsionStrength = settings.repulsionStrength;
            script.avoidanceRadius = settings.avoidanceRadius;
            script.changeTargetDelay = settings.changeTargetDelay;
            script.behavior = (BehaviorType)1;
            script.zoneDeFoule = zoneDeFoule;
            if (randomAgent == i)
                script.interactionArrow = interactionArrow;
            agents.Add(agent);
        }
    }

    Vector3 GetValidPosition()
    {
        Vector3 pos = new Vector3(0, 0, 0);
        var boxes = zoneDeFoule.GetComponentsInChildren<BoxCollider>();
        var box = boxes[Random.Range(0, boxes.Length)];

        int tries = 0;
        do
        {
            float x = Random.Range(-box.size.x / 2, box.size.x / 2);
            float z = Random.Range(-box.size.z / 2, box.size.z / 2);
            pos = new Vector3(box.center.x + x, 0.07f, box.center.z + z);
            tries++;
        } while (tries < 10);

        return pos;
    }

    void SetInteractiveAgent(CrowdAgent agent)
    {
        agent.interactionArrow = interactionArrow;
    }
}
