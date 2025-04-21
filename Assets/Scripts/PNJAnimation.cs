using UnityEngine;

public class PNJAnimation : MonoBehaviour
{
    public Animator animator;
    public float walkCycleDuration = 1.367f; // Durée d’un cycle de "Walking"
    public float lookDuration = 13.33f;

    public int minCycle = 2;
    public int maxCycle = 5;

    private float timer;
    private bool isLooking = false;
    private int currentWalkCycles;

    void Start()
    {
        ResetWalkCycle();
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (isLooking)
        {
            if (timer <= 0f)
            {
                isLooking = false;
                animator.SetBool("IsLooking", false);
                RotateRandomY();
                ResetWalkCycle();
            }
        }
        else
        {
            if (timer <= 0f)
            {
                currentWalkCycles--;
                animator.SetInteger("WalkCycles", currentWalkCycles);

                if (currentWalkCycles <= 0)
                {
                    isLooking = true;
                    animator.SetBool("IsLooking", true);
                    timer = lookDuration;
                }
                else
                {
                    timer = walkCycleDuration;
                }
            }
        }
    }

    void ResetWalkCycle()
    {
        currentWalkCycles = Random.Range(minCycle, maxCycle+1);
        animator.SetInteger("WalkCycles", currentWalkCycles);
        timer = walkCycleDuration;
    }

    void RotateRandomY()
    {
        float angle = Random.Range(90f, 270f);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);
    }
}


