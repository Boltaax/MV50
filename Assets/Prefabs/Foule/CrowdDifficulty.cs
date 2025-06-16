using UnityEngine;

[System.Serializable]
public class CrowdDifficulty
{
    public string label = "Niveau 1";
    public int agentCount = 20;
    public float agentSpeed = 2f;
    public float spacing = 1.2f;
    public float changeTargetDelay = 3f;
    public float repulsionStrength = 2f;
    public float avoidanceRadius = 1.5f;

    // Tu pourras ajouter d'autres paramètres plus tard, comme du bruit ou de l'agitation
}
