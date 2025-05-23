using UnityEngine;

public enum BehaviorType { Idle, Wander, FollowPlayer, FleePlayer }

public class CrowdAgent : MonoBehaviour
{
    public BehaviorType behavior;
    public Transform player;
    public Vector2 areaSize = new Vector2(10, 10);
    public float speed = 2f;
    public float changeTargetDelay = 2f;
    public float avoidanceRadius = 1.5f;
    public float repulsionStrength = 3f;
    private Vector3 targetPosition;
    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (behavior == BehaviorType.Wander)
            InvokeRepeating(nameof(SetNewTarget), 0f, changeTargetDelay);
    }

    void SetNewTarget()
    {
        float x = Random.Range(-areaSize.x / 2, areaSize.x / 2);
        float z = Random.Range(-areaSize.y / 2, areaSize.y / 2);
        targetPosition = new Vector3(x, 1, z);
    }

    void Update()
    {
        Vector3 direction = Vector3.zero;

        switch (behavior)
        {
            case BehaviorType.Wander:
                direction = (targetPosition - transform.position).normalized;
                break;

            case BehaviorType.FollowPlayer:
                if (player != null)
                    direction = (player.position - transform.position).normalized;
                break;

            case BehaviorType.FleePlayer:
                if (player != null)
                    direction = (transform.position - player.position).normalized;
                break;
        }

        // Récupère les agents proches
        Collider[] nearby = Physics.OverlapSphere(transform.position, avoidanceRadius);
        Vector3 repulsion = Vector3.zero;
        int count = 0;

        foreach (var col in nearby)
        {
            if (col.gameObject != this.gameObject && col.TryGetComponent(out CrowdAgent other))
            {
                Vector3 away = transform.position - other.transform.position;
                float dist = away.magnitude;
                if (dist > 0.01f)
                {
                    repulsion += away.normalized / dist; // force inversement proportionnelle à la distance
                    count++;
                }
            }
        }

        if (count > 0)
            repulsion = repulsion.normalized * repulsionStrength;

        // Combine direction et répulsion
        Vector3 move = (direction + repulsion).normalized;

        controller.Move(speed * Time.deltaTime * move);

        if (move.magnitude > 0.1f)
        {
            Vector3 lookDirection = new Vector3(move.x, 0, move.z);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(lookDirection),
                Time.deltaTime * 5f
            );
        }
    }

}
