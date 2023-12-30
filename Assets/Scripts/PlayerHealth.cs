// PlayerHealth.cs

using TMPro;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxLives = 3; // Maximum number of lives
    public int currentLives; // Current number of lives
    public TMP_Text livesText; // Reference to a TMP text element to display lives (optional)

    private void Start()
    {
        currentLives = maxLives;
        UpdateLivesUI();
    }

    private void UpdateLivesUI()
    {
        // Update the TMP text to display the current number of lives
        if (livesText != null)
        {
            livesText.text = "Lives: " + Mathf.Max(0, currentLives); // Ensure it doesn't display negative lives
        }
    }

    public void TakeDamage()
    {
        // Decrement the number of lives
        currentLives--;

        // Update the UI
        UpdateLivesUI();

        // Check if the player is out of lives
        if (currentLives <= 0)
        {
            currentLives = 0; // Ensure lives don't go negative
            GameOver();
        }
        else
        {
            // Handle other actions when the player takes damage (e.g., respawn)
            Respawn();
        }
    }

    private void Respawn()
    {
        // Implement logic for respawning the player
        // This could involve resetting the player's position, restoring health, etc.
        Debug.Log("Respawn the player!");
    }

    private void GameOver()
    {
        // Implement actions for when the player runs out of lives
        // This could involve showing a game over screen, resetting the level, etc.
        Debug.Log("Game Over!");
    }
}
