using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public GameState gameState;
    private int playerScore;
    public GameObject ballPrefab;
    public Vector3 ballSpawnPos;
    private GameObject ballObject;
    public GameObject pauseCanvas;
    public GameObject gameOverCanvas;
    public Text gameOverMessage;
    public Text scoreText;

	// Use this for initialization
	void Start ()
    {
        Time.timeScale = 0.0f;
        playerScore = 0;
        ballObject = (GameObject)Instantiate(ballPrefab, ballSpawnPos, Quaternion.identity);
        pauseCanvas.gameObject.SetActive(false);
        gameOverCanvas.gameObject.SetActive(false);
        StartGame();
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
        ballObject.GetComponent<BallController>().SetProperties(new Vector3(-0.5f, 0.5f, 0), 0.25f);
    }

    private void UpdateUI()
    {
        scoreText.text = "Score: " + playerScore.ToString();
    }

    public void UpdateScore(int scoreToAdd)
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
        Time.timeScale = 0.0f;
        switch(gOCondition)
        {
            case "Won":
                gameState = GameState.Won;
                gameOverCanvas.SetActive(true);
                gameOverMessage.text = "You Won!";
                break;
            case "Lost":
                gameState = GameState.Lost;
                gameOverCanvas.SetActive(true);
                gameOverMessage.text = "You Lost!";
                break;
            default:
                Debug.Log("Error, incorrect input");
                break;
        }
    }

    public void TogglePauseGame()
    {
        if(gameState == GameState.Paused)
        {
            gameState = GameState.Playing;
            Time.timeScale = 1.0f;
            pauseCanvas.gameObject.SetActive(false);
        }
        else
        {
            gameState = GameState.Paused;
            Time.timeScale = 0.0f;
            pauseCanvas.gameObject.SetActive(true);
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

public enum GameState { Playing, Paused, Lost, Won}
