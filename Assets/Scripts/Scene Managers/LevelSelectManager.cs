using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelSelectManager : MonoBehaviour
{
    private GameManager gameManager;
    public List<LevelInfo> levels;
    public List<GameObject> levelIcons;
    private int curLevelIndex = 0;
    public TextMeshProUGUI levelNameText, bestTimeText, goalTimeText;

    //Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager")?.GetComponent<GameManager>();
        SelectLevel(0);
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
    }

    void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.A)) ChangeSelectedLevel(-1);
        if (Input.GetKeyDown(KeyCode.D)) ChangeSelectedLevel(1);
        if (Input.GetKeyDown(KeyCode.Space)) GoToLevel(levels[curLevelIndex]);
        if (Input.GetKeyDown(KeyCode.Escape)) SceneManager.LoadScene("MainMenu");
    }

    void ChangeSelectedLevel(int direction)
    {
        int newLevel = curLevelIndex + direction;
        if (newLevel < 0 || newLevel >= levels.Count || !levels[newLevel].isUnlocked)
        {
            return;
        }
        SelectLevel(newLevel);
    }

    public void SelectLevel(int index)
    {

        levelIcons[curLevelIndex].transform.localScale = new Vector2(1, 1);
        curLevelIndex = index;
        levelIcons[curLevelIndex].transform.localScale = new Vector2(1.5f, 1.5f);

        LevelInfo level = levels[curLevelIndex];

        levelNameText.text = level.levelName;

        if (level.bestTime == double.PositiveInfinity)
        {
            bestTimeText.text = "Best Time: None so far!";
        }
        else
        {
            TimeSpan bt = TimeSpan.FromSeconds(level.bestTime);
            bestTimeText.text = "Best Time: " + bt.ToString("m':'ss'.'fff");
        }
        TimeSpan gt = TimeSpan.FromSeconds(level.goalTime);
        goalTimeText.text = "Goal Time: " + gt.ToString("m':'ss'.'fff");
    }

    public void GoToLevel(LevelInfo level)
    {
        gameManager.curLevelInfo = level;
        SceneManager.LoadScene("MainLevelScene");
    }

    public void ReturnToMain()
    {
        SceneManager.LoadScene("MainMenu");
    }
}