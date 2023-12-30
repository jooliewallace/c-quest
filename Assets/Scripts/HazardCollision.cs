// HazardCollision.cs (attached to the snowball GameObject)

using UnityEngine;

public class HazardCollision : MonoBehaviour
{
    public AudioClip impactSound; // Sound to play on impact

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Assuming the player has the PlayerHealth component
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                // Player takes damage when colliding with the hazard
                playerHealth.TakeDamage();
            }

            // Play the impact sound
            PlayImpactSound();

            // Destroy the snowball
            Destroy(gameObject);
        }
    }

    private void PlayImpactSound()
    {
        if (impactSound != null)
        {
            // Create an empty GameObject to handle audio playback
            GameObject audioObject = new GameObject("ImpactSoundObject");
            audioObject.transform.position = transform.position;

            // Add an AudioSource component to the audio object
            AudioSource audioSource = audioObject.AddComponent<AudioSource>();
            audioSource.clip = impactSound;

            // Adjust spatialBlend to control 3D audio effect
            audioSource.spatialBlend = 0f; // 0.0 makes it 2D, 1.0 makes it 3D

            audioSource.volume = 1.0f; // Adjust as needed

            // Play the sound
            audioSource.Play();

            // Destroy the audio object after the sound finishes playing
            Destroy(audioObject, impactSound.length + 0.01f);
        }
    }
}
