using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.Localization.Settings;

public class LevelSelectManager : MonoBehaviour
{
    private GameManager gameManager;
    public List<LevelInfo> levels;
    public List<GameObject> levelIcons;
    private int curLevelIndex = 0;
    public TextMeshProUGUI levelNameText, bestTimeText, goalTimeText;
    [SerializeField] Image starImage, porridgeImage;
    [SerializeField] Sprite lockSprite, unlockedSprite;
    [SerializeField] GameObject thanksForPlayingScreen;
    private bool thanksScreenShowing = false;

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

        for (int i = 0; i < levels.Count; i++)
        {
            Image checkmarkImage = levelIcons[i].transform.Find("CheckmarkImage").GetComponent<Image>();
            checkmarkImage.enabled = PlayerPrefs.HasKey(levels[i].levelName + "Beaten");
        }

        if (PlayerPrefs.HasKey(levels[levels.Count - 1].levelName + "Beaten") && !PlayerPrefs.HasKey("ThanksForPlayingShown"))
        {
            StartCoroutine(FadeInThanks());
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
        GetComponent<AudioSource>().Play();
        PlayerPrefs.SetInt(levels[levelID].levelName + "Unlocked", 1);
    }

    void Lock(int levelID)
    {
        Image lockImage = levelIcons[levelID].transform.Find("LockImage").GetComponent<Image>();
        lockImage.color = new Color(1, 1, 1, 1);
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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (thanksScreenShowing)
                StartCoroutine(FadeOutThanks());
            else
                SceneManager.LoadScene("MainMenu");
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            StartCoroutine(FadeInThanks());
        }

        //Following are for testing, DELETE for final build ///////////////////////////////////////////////////////////
        if (Input.GetKeyDown(KeyCode.LeftBracket))
        {
            PlayerPrefs.DeleteAll();
            SelectLevel(0);
            StopAllCoroutines();
            for (int i = 0; i < levels.Count; i++)
            {
                Lock(i);
            }
            PlayerPrefs.SetInt("TutorialUnlocked", 1);
            StartUnlocked(0);
        }
        // if (Input.GetKeyDown(KeyCode.RightBracket))
        // {
        //     for (int i = 0; i < levels.Count; i++)
        //     {
        //         Unlock(i);
        //     }
        // }
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

    public string localString(string tableref, string key)
    {
        return LocalizationSettings.StringDatabase.GetLocalizedString(tableref, key);
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
            bestTimeText.text = localString("Level Select", "besttime") + " " + localString("Level Select", "nobesttime");
        }
        else
        {
            TimeSpan bt = TimeSpan.FromSeconds(bestTime);
            bestTimeText.text = localString("Level Select", "besttime") + " " + bt.ToString("m':'ss'.'fff");
        }

        TimeSpan gt = TimeSpan.FromSeconds(level.goalTime);
        if (bestTime < level.goalTime)
        {
            goalTimeText.text = localString("Level Select", "goldtime") + " " + gt.ToString("m':'ss'.'fff") + " (Achieved!)";
            starImage.enabled = true;
        }
        else
        {
            goalTimeText.text = localString("Level Select", "goldtime") + " " + gt.ToString("m':'ss'.'fff");
            starImage.enabled = false;
        }

        if (level.hasPorridge)
        {
            porridgeImage.enabled = true;
            if (PlayerPrefs.GetInt(level.levelName + "PorridgeCollected") == 1)
            {
                porridgeImage.color = new Color(1, 1, 1, 1);
            }
            else
            {
                porridgeImage.color = new Color(0.8f, 0.8f, 0.8f, 0.8f);
            }
        }
        else
        {
            porridgeImage.enabled = false;
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

    IEnumerator FadeInThanks()
    {
        thanksScreenShowing = true;
        thanksForPlayingScreen.SetActive(true);
        Image img = thanksForPlayingScreen.GetComponent<Image>();
        Color originalColor = new Color(1, 1, 1, 0);
        Color targetColor = new Color(1, 1, 1, 1);
        float elapsedTime = 0f;
        float fadeDuration = 0.8f;
        foreach (TextMeshProUGUI textComponent in thanksForPlayingScreen.GetComponentsInChildren<TextMeshProUGUI>())
        {
            Color originalTextColor = textComponent.color;
            Color transparentColor = new Color(originalTextColor.r, originalTextColor.g, originalTextColor.b, 0f);
            textComponent.color = transparentColor;
        }

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / fadeDuration;
            img.color = Color.Lerp(originalColor, targetColor, t);

            // Fade in text components in child objects
            foreach (TextMeshProUGUI textComponent in thanksForPlayingScreen.GetComponentsInChildren<TextMeshProUGUI>())
            {
                textComponent.color = Color.Lerp(new Color(textComponent.color.r, textComponent.color.g, textComponent.color.b, 0f), new Color(textComponent.color.r, textComponent.color.g, textComponent.color.b, 1f), t);
            }

            yield return null;
        }

        img.color = targetColor;
        PlayerPrefs.SetInt("ThanksForPlayingShown", 1);
    }

    IEnumerator FadeOutThanks()
    {
        thanksScreenShowing = false;
        Image img = thanksForPlayingScreen.GetComponent<Image>();
        Color originalColor = img.color;
        Color targetColor = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
        float elapsedTime = 0f;
        float fadeDuration = 0.8f;

        foreach (TextMeshProUGUI textComponent in thanksForPlayingScreen.GetComponentsInChildren<TextMeshProUGUI>())
        {
            Color originalTextColor = textComponent.color;
            Color transparentColor = new Color(originalTextColor.r, originalTextColor.g, originalTextColor.b, 0f);
            textComponent.color = transparentColor;
        }

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / fadeDuration;
            img.color = Color.Lerp(originalColor, targetColor, t);

            // Fade out text components in child objects
            foreach (TextMeshProUGUI textComponent in thanksForPlayingScreen.GetComponentsInChildren<TextMeshProUGUI>())
            {
                textComponent.color = Color.Lerp(new Color(textComponent.color.r, textComponent.color.g, textComponent.color.b, 1f), new Color(textComponent.color.r, textComponent.color.g, textComponent.color.b, 0f), t);
            }

            yield return null;
        }

        img.color = targetColor;
        thanksForPlayingScreen.SetActive(false);
    }

    public void ReturnToMain()
    {
        SceneManager.LoadScene("MainMenu");
    }
}