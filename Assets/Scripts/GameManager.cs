using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    public GameObject player;
    public MenuManager menuManager;
    public Camera maincamera;
    public GameObject levelPrefab;   
    // public cameracontrol cam;
    private bool win = false;
    private bool lose = false;
    private bool paused = false;
    [SerializeField] private Canvas gameover;

    void Start()
    {
        menuManager = GameObject.Find("MenuManager").GetComponent<MenuManager>();
        Debug.Log("Loading " + menuManager.curLevelInfo.levelName);
        levelPrefab = GameObject.Instantiate(menuManager.curLevelInfo.levelPrefab) as GameObject;
        
        player = GameObject.Find("Bear");

        //replace this logic with game over screen prefab
        gameover = GameObject.Find("GameOver").GetComponent<Canvas>();
        gameover.enabled = false;
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
    }

    void Update() {
        
        Pause();
        
        if (lose) {
            GameOver();
        }

        if (win) {
            Victory();
        }

        if (Input.GetKeyDown(KeyCode.P)) {
            lose = true;
        }

        if (Input.GetKeyDown(KeyCode.Escape) && !paused) {
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
        else {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1;
        }
    }

    //game over logic
    public void GameOver() {
        
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
    public void Victory() {

    }    
}
