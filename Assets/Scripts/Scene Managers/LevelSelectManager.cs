using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectManager : MonoBehaviour
{
    private GameManager gameManager;
    public List<LevelInfo> levels;
    public List<GameObject> levelIcons;
    private int curLevelIndex = 0;

    //Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        selectLevel(0);
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
    }

    void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            int newLevel = curLevelIndex - 1;
            if (newLevel < 0)
            {
                return;
            }
            selectLevel(newLevel);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            //Debug.Log("hello");
            int newLevel = curLevelIndex + 1;
            if (newLevel >= levels.Count || !levels[newLevel].isUnlocked)
            {
                return;
            }
            selectLevel(newLevel);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            goToLevel(levels[curLevelIndex]);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    public void selectLevel(int index)
    {

        levelIcons[curLevelIndex].transform.localScale = new Vector2(1, 1);
        curLevelIndex = index;
        levelIcons[curLevelIndex].transform.localScale = new Vector2(1.5f, 1.5f);
    }

    public void goToLevel(LevelInfo level)
    {
        gameManager.curLevelInfo = level;
        SceneManager.LoadScene("MainLevelScene");
    }
}