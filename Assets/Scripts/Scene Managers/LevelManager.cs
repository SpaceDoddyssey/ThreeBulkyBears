using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private GameManager gameManager;

    public GameObject player;
    public Camera maincamera;
    public Scene levelSubscene;
    private bool win = false;
    private bool lose = false;
    private bool paused = false;
    [SerializeField] private Canvas gameover;

    void Awake()
    {
        gameManager = GameObject.Find("GameManager")?.GetComponent<GameManager>();
        if (gameManager == null)
        {
            Debug.LogError("GameManager not found in scene");
        }
        Debug.Log("Loading level: " + gameManager.curLevelInfo.sceneName);
        SceneManager.LoadScene(gameManager.curLevelInfo.sceneName, LoadSceneMode.Additive);
    }

    void Start()
    {
        player = GameObject.Find("Bear");

        //replace this logic with game over screen prefab
        gameover = GameObject.Find("GameOverCanvas").GetComponent<Canvas>();
        gameover.enabled = false;

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

        gameover.enabled = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0;

        if (Input.GetKeyDown(KeyCode.K))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    //win logic
    public void Victory()
    {

    }
}
