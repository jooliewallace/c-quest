using UnityEngine;
using System.Collections;

public class SpikeTrap : MonoBehaviour
{
    public Animator spikeAnimator;
    public PlayerHealth playerHealth;

    private bool isAnimating = false;

    void Start()
    {
        // Ensure the animation doesn't start on entry
        spikeAnimator.SetBool("SpikeTrap", false);
    }

    IEnumerator AnimateSpikesRandomly()
    {
        while (true)
        {
            // Wait for a random time before triggering the animation
            yield return new WaitForSeconds(Random.Range(1f, 5f));

            // Check if spikes are not already animating
            if (!isAnimating)
            {
                // Set the trigger only if not already animating
                spikeAnimator.SetTrigger("SpikeTrap");
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Trigger player health logic when collision with spikes occurs
            playerHealth.TakeDamage();
        }
    }

    // Called by the Animation Event when the spike animation starts
    public void SpikeTrapAnimationStart()
    {
        isAnimating = true;
    }

    // Called by the Animation Event when the spike animation ends
    public void SpikeTrapAnimationEnd()
    {
        isAnimating = false;
    }
}
