using UnityEngine;
using TMPro;

public class CountdownTimer : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    public float totalTime = 60f;
    private float currentTime;

    public PlayerHealth playerHealth;

    private bool timerActive = true; // Added flag to control timer activation

    void Start()
    {
        Debug.Log("Start method in player health is called!");

        currentTime = totalTime;
    }

    void Update()
    {

    Debug.Log("Start method is called!");

        if (timerActive)
        {
            currentTime -= Time.deltaTime;
            currentTime = Mathf.Max(currentTime, 0f);
            UpdateCountdownText();

            if (currentTime <= 0f)
            {
                Debug.Log("Countdown reached zero!");

                // Decrement player lives
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage();
                    Debug.Log("Player lives after TakeDamage: " + playerHealth.currentLives);
                }

                // Prevent immediate restart
                timerActive = false;
                Invoke("RestartTimer", 2f); // Adjust the delay as needed
            }
        }
    }

    void UpdateCountdownText()
    {
        if (timeText != null)
        {
            int minutes = Mathf.FloorToInt(currentTime / 60f);
            int seconds = Mathf.FloorToInt(currentTime % 60f);
            string formattedTime = string.Format("{0:00}:{1:00}", minutes, seconds);
            timeText.text = formattedTime;
        }
    }

    void RestartTimer()
    {
        // Reset the timer and allow it to start again
        currentTime = totalTime;
        timerActive = true;
    }
}
