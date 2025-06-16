using UnityEngine;

public enum BehaviorType { Idle, Wander, FollowPlayer, FleePlayer }

public class CrowdAgent : MonoBehaviour
{
    public BehaviorType behavior;
    public Transform player;
    public float speed = 2f;
    public float changeTargetDelay = 2f;
    public float avoidanceRadius = 1.5f;
    public float repulsionStrength = 3f;
    public float followMinDistance = 1f;
    public float fleeDistance = 4f;

    public GameObject zoneDeFoule;
    public GameObject interactionArrow;

    private Vector3 targetPosition;
    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        // Initialise la flèche si elle existe
        if (interactionArrow != null)
            interactionArrow.SetActive(false);

        if (behavior == BehaviorType.Wander)
            InvokeRepeating(nameof(SetNewTarget), 0f, changeTargetDelay);
    }

    void SetNewTarget()
    {
        var boxes = zoneDeFoule.GetComponentsInChildren<BoxCollider>();
        if (boxes == null || boxes.Length == 0) return;

        Vector3 pos = transform.position;
        int tries = 0;

        do
        {
            var bounds = boxes[Random.Range(0, boxes.Length)].bounds;
            float x = Random.Range(bounds.min.x, bounds.max.x);
            float z = Random.Range(bounds.min.z, bounds.max.z);
            pos = new Vector3(x, bounds.center.y, z);
            tries++;
        } while (!IsInsideAnyCollider(pos) && tries < 30);

        targetPosition = pos;
    }

    bool IsInsideAnyCollider(Vector3 point)
    {
        foreach (var col in zoneDeFoule.GetComponentsInChildren<BoxCollider>())
        {
            if (col.bounds.Contains(point))
                return true;
        }
        return false;
    }

    void Update()
    {
        Vector3 direction = Vector3.zero;

        switch (behavior)
        {
            case BehaviorType.Idle:
                return;

            case BehaviorType.Wander:
                direction = (targetPosition - transform.position).normalized;
                if (Vector3.Distance(transform.position, targetPosition) < 1f)
                    SetNewTarget();
                break;

            case BehaviorType.FollowPlayer:
                if (player != null)
                {
                    float dist = Vector3.Distance(transform.position, player.position);
                    if (dist > followMinDistance)
                        direction = (player.position - transform.position).normalized;
                }
                break;

            case BehaviorType.FleePlayer:
                if (player != null)
                {
                    float dist = Vector3.Distance(transform.position, player.position);
                    if (dist < fleeDistance)
                    {
                        // Direction opposée + légère variation aléatoire
                        Vector3 fleeDir = (transform.position - player.position).normalized;
                        fleeDir = Quaternion.Euler(0, Random.Range(-45f, 45f), 0) * fleeDir;
                        direction = fleeDir;
                    }
                    else
                    {
                        // Si trop loin, revient à du Wander
                        direction = Vector3.zero;
                    }
                }
                break;
        }

        // Évitement de foule
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
                    repulsion += away.normalized / dist;
                    count++;
                }
            }
        }

        if (count > 0)
            repulsion = repulsion.normalized * repulsionStrength;

        Vector3 move = (direction + repulsion).normalized;

        // Vérifie si le déplacement reste dans la zone
        Vector3 proposed = transform.position + move * speed * Time.deltaTime;
        if (IsInsideAnyCollider(proposed))
        {
            controller.Move(move * speed * Time.deltaTime);

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

    public void SetInteractable(bool state)
    {
        if (interactionArrow != null)
            interactionArrow.SetActive(state);
    }
}
