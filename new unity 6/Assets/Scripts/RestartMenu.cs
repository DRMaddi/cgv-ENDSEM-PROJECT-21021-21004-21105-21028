using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartMenu : MonoBehaviour
{
    public void RestartGame()
    {
        checkpointsystem.Instance.restart_var = 1;
        PlayerPrefs.SetInt("restart_var", checkpointsystem.Instance.restart_var);
        Score.score = 0;
        PlayerPrefs.SetInt("score", Score.score);
        SceneManager.LoadScene("Game");
    }
    public void continue_with_checkpoint()
    {
        SceneManager.LoadScene("Game");
    }
}
