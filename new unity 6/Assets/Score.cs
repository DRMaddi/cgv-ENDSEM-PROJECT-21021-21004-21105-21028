using UnityEngine;
using TMPro;
using System.Collections;

public class Score : MonoBehaviour {
    
    public TextMeshProUGUI scoreText;
    public static int score = 0;
    private Coroutine scoreCoroutine;

    void Start() {
        // Start the coroutine to update the score every second
        scoreCoroutine = StartCoroutine(UpdateScoreText());
        score = PlayerPrefs.GetInt("score", +score);
    }

    IEnumerator UpdateScoreText() {
        while (true) {
            score++;
            scoreText.text = "Score: " + score.ToString();
            PlayerPrefs.SetInt("score", score);
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void StopScore() {
        if (scoreCoroutine != null) {
            StopCoroutine(scoreCoroutine);
        }
    }
}
