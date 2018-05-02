using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    //Variables
    public GameState gameState;
    private int playerScore;
    private int currentLives;
    private int maxLives = 3;
    public GameObject ballPrefab;
    public Vector3 ballSpawnPos;
    private GameObject ballObject;
    public GameObject pauseCanvas;
    public GameObject gameOverCanvas;
    public Text gameOverMessage;
    public Text scoreText;
    public Text livesText;

	// Use this for initialization
	void Start ()
    {
        //Setup game
        Time.timeScale = 0.0f;
        gameState = GameState.Paused;
        currentLives = maxLives;
        playerScore = 0;
        ballObject = (GameObject)Instantiate(ballPrefab, ballSpawnPos, Quaternion.identity);
        pauseCanvas.gameObject.SetActive(false);
        gameOverCanvas.gameObject.SetActive(false);
        UpdateUI();
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

    //Unpause the game and start
    private void StartGame()
    {
        gameState = GameState.Playing;
        Time.timeScale = 1.0f;
        ballObject.GetComponent<BallController>().SetProperties(new Vector3(-0.5f, 0.5f, 0), 0.25f);
    }

    //Update score and player lives on hud
    private void UpdateUI()
    {
        scoreText.text = "Score: " + playerScore.ToString();
        livesText.text = "Lives: " + currentLives.ToString();
    }

    //Increase playerScore
    public void UpdateScore(int scoreToAdd)
    {
        playerScore += scoreToAdd;
        UpdateUI();
    }
    
    public void BallHitTopWall()
    {
        GameOver("Won");
    }

    //Ball hit bottom wall
    public void BallLost()
    {
        currentLives--;
        UpdateUI();
        if(currentLives <= 0)
        {
            GameOver("Lost");
        }
        else
        {
            ballObject = (GameObject)Instantiate(ballPrefab, ballSpawnPos, Quaternion.identity);
            ballObject.GetComponent<BallController>().SetProperties(new Vector3(-0.5f, 0.5f, 0), 0.25f);
        }
    }

    //Game has ended, and player has either won or lost
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
        else if(gameState == GameState.Playing)
        {
            gameState = GameState.Paused;
            Time.timeScale = 0.0f;
            pauseCanvas.gameObject.SetActive(true);
        }
        else
        {
            Debug.Log("Game over");
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Quit()
    {
        Application.Quit();
    }
}

public enum GameState { Playing, Paused, Lost, Won}
