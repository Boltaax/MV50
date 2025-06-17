using UnityEngine;

public enum BehaviorType { Idle, Wander, FollowPlayer, FleePlayer }

[RequireComponent(typeof(CharacterController))]
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
    private Animator animator;
    private float fixedY; // Y constant

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        fixedY = transform.position.y;

        if (interactionArrow != null)
        {
            interactionArrow.SetActive(true);
            interactionArrow.transform.SetParent(transform); // pour qu’elle suive l’agent
            interactionArrow.transform.localPosition = new Vector3(0, 2, 0);
            interactionArrow.transform.localRotation = Quaternion.Euler(90, 0, 0);
        }

        if (behavior == BehaviorType.Wander)
            InvokeRepeating(nameof(SetNewTarget), 0f, changeTargetDelay);
    }

    void SetNewTarget()
    {
        var boxes = zoneDeFoule.GetComponentsInChildren<BoxCollider>();
        if (boxes == null || boxes.Length == 0) return;

        var bounds = boxes[Random.Range(0, boxes.Length)].bounds;
        float x = Random.Range(bounds.min.x, bounds.max.x);
        float z = Random.Range(bounds.min.z, bounds.max.z);
        targetPosition = new Vector3(x, fixedY, z);
    }

    bool IsInsideAnyCollider(Vector3 point)
    {
        /*foreach (var col in zoneDeFoule.GetComponentsInChildren<BoxCollider>())
        {
            if (col.bounds.Contains(point))
                return true;
        }
        return false;*/
        return true;
    }

    void Update()
    {
        Vector3 direction = Vector3.zero;

        switch (behavior)
        {
            case BehaviorType.Idle:
                SetWalkAnimation(false);
                return;

            case BehaviorType.Wander:
                direction = (targetPosition - transform.position).normalized;
                if (Vector3.Distance(transform.position, targetPosition) < 0.2f)
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
                        Vector3 fleeDir = (transform.position - player.position).normalized;
                        fleeDir = Quaternion.Euler(0, Random.Range(-45f, 45f), 0) * fleeDir;
                        direction = fleeDir;
                    }
                }
                break;
        }

        // Évitement
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

        // Appliquer déplacement uniquement sur XZ, Y constant
        Vector3 proposed = transform.position + move * speed * Time.deltaTime;
        proposed.y = fixedY;

        if (IsInsideAnyCollider(proposed))
        {
            Vector3 moveDirection = (proposed - transform.position);
            moveDirection.y = 0;

            if (moveDirection.magnitude > 0.05f)
            {
                controller.Move(moveDirection.normalized * speed * Time.deltaTime);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDirection), Time.deltaTime * 5f);
                SetWalkAnimation(true);
            }
            else
            {
                SetWalkAnimation(false);
            }
        }
        else
        {
            // Hors zone : choisir nouvelle cible
            SetNewTarget();
            SetWalkAnimation(false);
        }
    }

    void SetWalkAnimation(bool walking)
    {
        if (animator != null)
            animator.SetBool("Walk", walking);
    }
}
