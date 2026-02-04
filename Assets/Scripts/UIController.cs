using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{

    public TextMeshProUGUI scoreText;


    public TetrisManager tetrisManager;

    public GameObject endGamePanel;
    public void UpdateScore()
    {
        scoreText.text = $"SCORE: {tetrisManager.score:n0}";
    }

    public void UpdateGameOver()
    {
        endGamePanel.SetActive(tetrisManager.gameOver);
    }

    public void PlayAgain()
    {
        //Since we have a preset board, we have a set start, so we can reload the scene upon a reset.
        SceneManager.LoadScene("SampleScene");
    }
}
