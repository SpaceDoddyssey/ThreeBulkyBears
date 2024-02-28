using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Cinemachine;
using System;

public class LevelManager : MonoBehaviour
{
    private GameManager gameManager;

    public GameObject playerObj;
    public BearController bearController;
    public Scene levelSubscene;
    public bool isGameOver;
    public bool paused = false;
    public GameObject pauseMenu;

    //Sign vars
    public GameObject gameOverObj, victoryObj;
    public Vector3 signStartPos;

    //timer 
    public TextMeshProUGUI timerText;
    public float currentTime = 0;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        if (gameManager == null) { Debug.Log("Game Manager not found"); return; }
        if (gameManager.curLevelInfo != null)
        {
            Debug.Log("Loading level: " + gameManager.curLevelInfo.sceneName);
            SceneManager.LoadScene(gameManager.curLevelInfo.sceneName, LoadSceneMode.Additive);
        }
    }

    void Start()
    {
        Cursor.visible = false;
    }

    void Update()
    {
        //timer update 
        if (!paused && !isGameOver)
        {
            UpdateTimer();
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
            if (isGameOver)
            {
                SceneManager.LoadScene("LevelSelection");
            }
            else
            {
                TogglePause();
            }
        }
    }

    public void UpdateTimer()
    {
        currentTime += Time.deltaTime;
        TimeSpan curTime = TimeSpan.FromSeconds(currentTime);
        timerText.text = curTime.ToString("m':'ss'.'fff");
    }

    public void TogglePause()
    {
        if (!paused)
        {
            Cursor.visible = true;
            Time.timeScale = 0;
            paused = true;
            pauseMenu.SetActive(true);
        }
        else
        {
            Cursor.visible = false;
            Time.timeScale = 1;
            paused = false;
            pauseMenu.SetActive(false);
        }
    }

    public void GameOver()
    {
        if (isGameOver) { return; }
        StartCoroutine(DropDownSign(gameOverObj));

        GameObject.Find("CameraFollowPoint").transform.parent = null;
        isGameOver = true;
        bearController.controllable = false;
    }

    public void Victory()
    {
        if (isGameOver) { return; }
        StartCoroutine(DropDownSign(victoryObj));

        GameObject.Find("CameraFollowPoint").transform.parent = null;
        isGameOver = true;
        bearController.controllable = false;

        if (gameManager.curLevelInfo != null)
        {
            float bestTime = PlayerPrefs.GetFloat(gameManager.curLevelInfo.sceneName + "BestTime", float.PositiveInfinity);
            Debug.Log("Best Time: " + bestTime);
            Debug.Log("Current Time: " + currentTime);

            if (currentTime <= bestTime)
            {
                PlayerPrefs.SetFloat(gameManager.curLevelInfo.sceneName + "BestTime", currentTime);
            }
        }
    }

    public IEnumerator DropDownSign(GameObject sign)
    {
        float delay = 0.3f;
        yield return new WaitForSeconds(delay);

        Vector3 targetPos = new Vector3(0, 0, 0);
        Vector3 startPos = signStartPos;
        float elapsedTime = 0;
        float initialFallDuration = 1.5f;
        while (elapsedTime < initialFallDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / initialFallDuration);
            t = Mathf.SmoothStep(0, 1, t); //smoothing function
            sign.transform.localPosition = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }
    }
}
