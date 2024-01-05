using UnityEngine;

public class IdlePlayer : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();

        // Play the default animation (idle in this case) without using a trigger
        if (animator != null)
            animator.Play("StandingIdle"); // Replace "YourIdleAnimationName" with the actual name of your idle animation
    }
}
