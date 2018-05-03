using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class GameManager : MonoBehaviour {

    //Variables
    public GameState gameState;
    public string gameMode;
    private int playerScore;
    private int currentLives;
    private int maxLives = 3;
    public GameObject[] brickRowPrefabs;
    private int brickWidth = 10;
    private int brickDepth = 8;
    public GameObject ballPrefab;
    public Vector3 ballSpawnPos;
    private GameObject ballObject;
    public Slider specialBrickSpawnChanceSlider;
    public Text specialBrickSpawnChanceText;
    public GameObject gameModeCanvas;
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
        pauseCanvas.gameObject.SetActive(false);
        gameOverCanvas.gameObject.SetActive(false);
        gameModeCanvas.gameObject.SetActive(true);
        UpdateUI();
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseGame();
        }
	}

    //Generate bricks, with the colour of the bricks based on brickRowPrefabs
    private void GenerateBricks()
    {
        int[] columns = GetBrickLayout();
        //Get brick layout
        for(int x = 0; x < columns.Length; x++)
        {
            //Max of 10 columns
            if(x < 10)
            {
                for (int y = 0; y < columns[x]; y++)
                {
                    //Max of 8 rows
                    if(y < 8)
                    {
                        //Chance to spawn special brick instead
                        float specialChance = Random.Range(1, 100);
                        if (specialChance <= specialBrickSpawnChanceSlider.value)
                        {
                            GameObject _brick = (GameObject)Instantiate(brickRowPrefabs[y], new Vector3((-13.5f + (x * 3)), (8.5f - y), 0), Quaternion.identity);
                            _brick.GetComponent<BrickClass>().MakeSpecial();
                        }
                        else
                        {
                            Instantiate(brickRowPrefabs[y], new Vector3((-13.5f + (x * 3)), (8.5f - y), 0), Quaternion.identity);
                        }
                    }
    
                    
                }
            }
            
        }
    }

    //Generate the brick layout for the level
    public int[] GetBrickLayout()
    {
        //Set default layout
        int[] layout = new int[10] { 8, 8, 8, 8, 8, 8, 8, 8, 8, 8 };
        string path = "BrickLayouts/bricklayout.txt";
        //Check if file exists at path
        if(File.Exists(path))
        {
            StreamReader reader = new StreamReader(path);
            string layoutText = reader.ReadLine();
            Debug.Log("Imported text: " + layoutText);
            if (layoutText != "")
            {
                char[] layoutChars = layoutText.ToCharArray();
                for (int i = 0; i < layoutChars.Length; i++)
                {
                    //Only import up to 10 characters
                    if(i < 10)
                    {
                        int num;
                        //Check that the character is a number
                        if (int.TryParse(layoutChars[i].ToString(), out num))
                        {
                            //Set the column depth to the new number
                            layout[i] = num;
                        }
                    }
                    
                }
            }
        }
        else
        {
            Debug.Log("File doesn't exist");
        }
        
        //Return brick layout
        return layout;
    }

    //Spawn a new ball, and if loseLife is true, remove a life as well
    public void SpawnBall(bool loseLife = true)
    {
        if(loseLife)
        {
            currentLives--;
            UpdateUI();
        }
        ballObject = (GameObject)Instantiate(ballPrefab, ballSpawnPos, Quaternion.identity);
        ballObject.GetComponent<BallController>().SetProperties(new Vector3(-0.5f, 0.5f, 0), 0.25f);
    }

    //Set the game mode, either Reach the top, or Clear the screen
    public void SetGameMode(string _gameMode)
    {
        if(_gameMode == "Clear")
        {
            gameMode = "Clear";
        }
        else
        {
            gameMode = "Reach";
        }
        gameModeCanvas.gameObject.SetActive(false);
        GenerateBricks();
        ballObject = (GameObject)Instantiate(ballPrefab, ballSpawnPos, Quaternion.identity);
        StartGame();
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

    public void UpdateSpawnChanceText()
    {
        specialBrickSpawnChanceText.text = "Chance to spawn special bricks: " + specialBrickSpawnChanceSlider.value.ToString("F0");
    }

    //Increase playerScore
    public void UpdateScore(int scoreToAdd)
    {
        playerScore += scoreToAdd;
        UpdateUI();
    }
    
    public void BallHitTopWall()
    {
        if (gameMode == "Reach")
        {
            GameOver("Won");
        }
    }

    public void CheckAllBallsLost(GameObject _recentBall)
    {
        GameObject[] balls = GameObject.FindGameObjectsWithTag("Ball");
        if (balls.Length == 0)
        {
            AllBallsLost();
        }
        else if (balls.Length == 1 && balls[0] == _recentBall)
        {
            AllBallsLost();
        }
    }

    public void CheckAllBricksDestroyed(GameObject _recentBrick)
    {
        GameObject[] bricks = GameObject.FindGameObjectsWithTag("Brick");
        if(bricks.Length == 0)
        {
            GameOver("Won");
        }
        else if(bricks.Length == 1 && bricks[0] == _recentBrick)
        {
            GameOver("Won");
        }
    }

    //Ball hit bottom wall
    public void AllBallsLost()
    {
        if(currentLives > 0)
        {
            SpawnBall();
        }
        else
        {
            GameOver("Lost");
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
