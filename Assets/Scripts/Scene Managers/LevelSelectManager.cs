using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class LevelSelectManager : MonoBehaviour
{
    private GameManager gameManager;
    public List<LevelInfo> levels;
    public List<GameObject> levelIcons;
    private int curLevelIndex = 0;
    public TextMeshProUGUI levelNameText, bestTimeText, goalTimeText;
    [SerializeField] Image starImage;
    [SerializeField] Sprite lockSprite, unlockedSprite;

    //Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager")?.GetComponent<GameManager>();
        SelectLevel(0);

        StartUnlocked(0);
        for (int i = 1; i < levels.Count; i++)
        {
            if (PlayerPrefs.HasKey(levels[i].levelName + "Unlocked"))
            {
                StartUnlocked(i);
            }
            else if (PlayerPrefs.HasKey(levels[i - 1].levelName + "Beaten"))
            {
                Unlock(i);
            }
        }
    }

    void StartUnlocked(int levelID)
    {
        levelIcons[levelID].transform.Find("LockImage").GetComponent<Image>().enabled = false;
    }

    IEnumerator FadeLockIcon(GameObject icon)
    {
        Image lockImage = icon.transform.Find("LockImage").GetComponent<Image>();
        lockImage.sprite = unlockedSprite;
        Color originalColor = lockImage.color;
        Color targetColor = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
        float elapsedTime = 0f;
        float fadeDuration = 1f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / fadeDuration;
            lockImage.color = Color.Lerp(originalColor, targetColor, t);
            yield return null;
        }

        lockImage.gameObject.SetActive(false);
        lockImage.color = originalColor;
    }

    void Unlock(int levelID)
    {
        StartCoroutine(FadeLockIcon(levelIcons[levelID]));
        PlayerPrefs.SetInt(levels[levelID].levelName + "Unlocked", 1);
    }

    void Lock(int levelID)
    {
        Image lockImage = levelIcons[levelID].transform.Find("LockImage").GetComponent<Image>();
        lockImage.sprite = lockSprite;
        lockImage.gameObject.SetActive(true);
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

        //Following are for testing, DELETE for final build ///////////////////////////////////////////////////////////
        if (Input.GetKeyDown(KeyCode.LeftBracket))
        {
            PlayerPrefs.DeleteAll();
            SelectLevel(0);
            for (int i = 0; i < levels.Count; i++)
            {
                Lock(i);
            }
            PlayerPrefs.SetInt("TutorialUnlocked", 1);
            StartUnlocked(0);
        }
        if (Input.GetKeyDown(KeyCode.RightBracket))
        {
            for (int i = 0; i < levels.Count; i++)
            {
                Unlock(i);
            }
        }
    }

    void ChangeSelectedLevel(int direction)
    {
        int newLevel = curLevelIndex + direction;
        if (newLevel < 0 || newLevel >= levels.Count || !PlayerPrefs.HasKey(levels[newLevel].levelName + "Unlocked"))
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

        float bestTime = PlayerPrefs.GetFloat(level.sceneName + "BestTime", float.PositiveInfinity);
        if (bestTime == float.PositiveInfinity)
        {
            bestTimeText.text = "Best Time: None so far!";
        }
        else
        {
            TimeSpan bt = TimeSpan.FromSeconds(bestTime);
            bestTimeText.text = "Best Time: " + bt.ToString("m':'ss'.'fff");
        }

        TimeSpan gt = TimeSpan.FromSeconds(level.goalTime);
        if (bestTime < level.goalTime)
        {
            goalTimeText.text = "Gold Time: " + gt.ToString("m':'ss'.'fff") + " (Achieved!)";
            starImage.enabled = true;
        }
        else
        {
            goalTimeText.text = "Gold Time: " + gt.ToString("m':'ss'.'fff");
            starImage.enabled = false;
        }
    }

    public void GoToLevel(LevelInfo level)
    {
        gameManager.curLevelInfo = level;
        if (PlayerPrefs.GetInt(level.levelName + "Unlocked") == 1)
        {
            SceneManager.LoadScene("MainLevelScene");
        }
    }

    public void ReturnToMain()
    {
        SceneManager.LoadScene("MainMenu");
    }
}