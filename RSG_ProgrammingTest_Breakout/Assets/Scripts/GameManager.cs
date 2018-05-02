using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public bool gamePaused;
    private int playerScore;
    public GameObject ballPrefab;
    public Vector3 ballSpawnPos;
    private GameObject ballObject;
    public GameObject pauseCanvas;
    public GameObject gameOverCanvas;
    public Text scoreText;

	// Use this for initialization
	void Start ()
    {
        Time.timeScale = 0.0f;
        playerScore = 0;
        ballObject = (GameObject)Instantiate(ballPrefab, ballSpawnPos, Quaternion.identity);
        pauseCanvas.gameObject.SetActive(false);
        gameOverCanvas.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseGame();
        }
	}

    private void StartGame()
    {
        Time.timeScale = 1.0f;
        //Set ball velocity
    }

    private void UpdateUI()
    {
        scoreText.text = playerScore.ToString();
    }

    private void UpdateScore(int scoreToAdd)
    {
        playerScore += scoreToAdd;
        UpdateUI();
    }

    public void BallHitTopWall()
    {
        GameOver("Won");
    }

    public void BallLost()
    {
        GameOver("Lost");
    }

    public void GameOver(string gOCondition)
    {
        switch(gOCondition)
        {
            case "Won":

                break;
            case "Lost":

                break;
            default:
                Debug.Log("Error, incorrect input");
                break;
        }
    }

    public void TogglePauseGame()
    {
        if(gamePaused)
        {
            gamePaused = false;
            Time.timeScale = 1.0f;
        }
        else
        {
            gamePaused = true;
            Time.timeScale = 0.0f;
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Menu()
    {
        SceneManager.LoadScene(0);
    }
}
