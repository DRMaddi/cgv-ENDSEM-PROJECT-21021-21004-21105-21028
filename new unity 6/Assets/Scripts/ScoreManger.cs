using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public Text scoreText;      // Reference to the UI Text to display the score
    public Transform player;    // Reference to the player object to track position

    private float score = 0;    // Variable to store the score

    void Update()
    {
        if (player != null)
        {
            // Set the score based on the player's X position
            score = player.position.x;

            // Display the score in the UI, rounded to an integer
            scoreText.text = "Score: " + Mathf.RoundToInt(score).ToString();
        }
    }
}