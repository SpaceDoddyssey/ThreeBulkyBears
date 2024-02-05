using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private GameManager gameManager;

    public GameObject playerObj;
    public BearController bearController;
    public Camera maincamera;
    public Scene levelSubscene;
    private bool win = false;
    private bool lose = false;
    private bool paused = false;
    private GameObject gameOverText, victoryText;

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

        Pause();

        if (lose)
        {
            GameOver();
        }

        if (win)
        {
            Victory();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            bearController.ResetBear();
            lose = false;
            win = false;
            gameOverText.SetActive(false);
            victoryText.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            lose = true;
        }

        if (Input.GetKeyDown(KeyCode.Escape) && !paused)
        {
            paused = true;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && paused)
        {
            paused = false;
        }

    }

    //check if game is paused and pause logic
    public void Pause()
    {
        if (paused)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1;
        }
    }

    //game over logic
    public void GameOver()
    {
        if (win) { return; }
        gameOverText.SetActive(true);

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
        victoryText.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        bearController.controllable = false;

        if (Input.GetKeyDown(KeyCode.K))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
