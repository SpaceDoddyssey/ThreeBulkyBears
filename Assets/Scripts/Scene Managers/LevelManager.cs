using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelManager : MonoBehaviour
{
    private GameManager gameManager;

    public GameObject playerObj;
    public BearController bearController;
    public Camera maincamera;
    public Scene levelSubscene;
    public bool won = false;
    public bool lost = false;
    private bool paused = false;
    private GameObject gameOverText, victoryText;

    //timer components 
    public TextMeshProUGUI timerText;

    //timer settings 
    public float currentTime = 0;

    void Awake()
    {
        GameObject gameManagerObj = GameObject.Find("GameManager");
        if (gameManagerObj == null) { return; }
        gameManager = gameManagerObj.GetComponent<GameManager>();
        if (gameManager == null) { return; }
        if (gameManager.curLevelInfo != null)
        {
            Debug.Log("Loading level: " + gameManager.curLevelInfo.sceneName);
            SceneManager.LoadScene(gameManager.curLevelInfo.sceneName, LoadSceneMode.Additive);
        }
    }

    void Start()
    {
        playerObj = GameObject.Find("Bear");
        bearController = playerObj.GetComponent<BearController>();

        gameOverText = GameObject.Find("GameOverText");
        gameOverText.SetActive(false);
        victoryText = GameObject.Find("VictoryText");
        victoryText.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        //timer update 
        if (!paused && (!won || !lost))
        {
            currentTime = currentTime += Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            TogglePause();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.UnloadSceneAsync(gameManager.curLevelInfo.sceneName);
            SceneManager.LoadScene("LevelSelection");
        }
    }

    public void TogglePause()
    {
        if (!paused)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0;
            paused = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1;
            paused = false;
        }
    }

    //game over logic
    public void GameOver()
    {
        if (won) { return; }
        gameOverText.SetActive(true);

        lost = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        bearController.controllable = false;

        if (Input.GetKeyDown(KeyCode.K))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    //win logic
    public void Victory()
    {
        if (lost) { return; }
        victoryText.SetActive(true);

        won = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        bearController.controllable = false;

        if (gameManager.curLevelInfo != null)
        {
            Debug.Log("Best Time: " + gameManager.curLevelInfo.bestTime);
            Debug.Log("Current Time: " + currentTime);

            if (currentTime <= gameManager.curLevelInfo.bestTime)
            {
                gameManager.curLevelInfo.bestTime = currentTime;
            }
        }
    }
}
