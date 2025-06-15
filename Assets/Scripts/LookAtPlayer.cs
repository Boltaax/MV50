using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    private Transform player;
    public float lookRadius = 100f;
    public float rotationSpeed = 2f;

    private float followDuration;
    private Quaternion originalRotation;
    private bool isFollowing = false;
    private float followTimer = 0f;
    private bool followed = false;

    void Start()
    {
        followDuration = Random.Range(3f, 10f); // 10f est inclus
        originalRotation = transform.rotation;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance < lookRadius)
        {
            if (!isFollowing && !followed)
            {
                isFollowing = true;
                followTimer = followDuration;
                followed = true;
            }
        }

        if (isFollowing)
        {
            followTimer -= Time.deltaTime;

            if (followTimer > 0f)
            {
                // Suivre le joueur
                Vector3 direction = player.position - transform.position;
                direction.y = 0; // Pas d'inclinaison verticale
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            }
            else
            {
                // Retour à la rotation de départ
                transform.rotation = Quaternion.Slerp(transform.rotation, originalRotation, Time.deltaTime * rotationSpeed);

                // Si on est presque revenu à la rotation de départ, arrêter le suivi
                if (Quaternion.Angle(transform.rotation, originalRotation) < 1f)
                {
                    isFollowing = false;
                }
            }
        }
    }
}
