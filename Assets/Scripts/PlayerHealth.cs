// PlayerHealth.cs

using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxLives = 3;
    public int currentLives;
    public TMP_Text livesText;

    private Vector3 respawnPosition; // Store the respawn position

    private void Start()
    {
        Debug.Log("Start method in countdown is called!");

        currentLives = maxLives;
        respawnPosition = transform.position; // Record the initial position as the respawn position
        UpdateLivesUI();
    }

    private void UpdateLivesUI()
    {
        if (livesText != null)
        {
            livesText.text = "Lives: " + Mathf.Max(0, currentLives);
        }
    }

    public void TakeDamage()
    {
        Debug.Log("Player took damage!");

        currentLives--;
        UpdateLivesUI();

        if (currentLives <= 0)
        {
            currentLives = 0;
            GameOver();
        }
        else
        {
            Respawn();
        }
    }

    private void Respawn()
    {
        // Reset the player's position to the respawn position
        transform.position = respawnPosition;
        Debug.Log("Respawn the player to the position: " + respawnPosition);
    }


    private void GameOver()
    {
        Debug.Log("Game Over!");

        // Load the "Game Over" scene
        SceneManager.LoadScene("GameOver"); // Replace "GameOverScene" with the actual name of your scene
    }
}
