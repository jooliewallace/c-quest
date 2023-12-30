// ScoreManager.cs

using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText; // Use TextMeshProUGUI for TMP text
    private int score = 0;

    private void Start()
    {
        UpdateScoreUI();
    }

    public void CollectPresent(int points)
    {
        score += points;
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }
}
