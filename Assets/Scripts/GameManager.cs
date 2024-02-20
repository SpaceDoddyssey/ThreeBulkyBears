using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public LevelInfo curLevelInfo = null;

    // Language variables
    public enum SupportedLanguage { English, Chinese }
    private SupportedLanguage currentLanguage;

    void Awake()
    {
        GameManager[] gameManagers = FindObjectsOfType<GameManager>();
        if (gameManagers.Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    void Start()
    {
        // Set the default language to English
        currentLanguage = SupportedLanguage.English;
        DetectSystemLanguage();
        UpdateLanguage();
    }

    void DetectSystemLanguage()
    {
        Debug.Log("--- The system language is: " + Application.systemLanguage);
        switch (Application.systemLanguage)
        {
            case SystemLanguage.English:
                currentLanguage = SupportedLanguage.English;
                Debug.Log("--- The system is in English.");
                break;
            case SystemLanguage.Chinese:
                currentLanguage = SupportedLanguage.Chinese;
                Debug.Log("--- The system is in Chinese.");
                break;
            default:
                Debug.LogWarning("Unsupported language.");
                currentLanguage = SupportedLanguage.English;
                break;
        }
    }

    // Method to toggle between English and Chinese languages
    public void ToggleLanguage()
    {
        // Toggle the language
        currentLanguage = (currentLanguage == SupportedLanguage.English) ? SupportedLanguage.Chinese : SupportedLanguage.English;
        UpdateLanguage();
    }

    void UpdateLanguage()
    {
        // Update the language of the game
        switch (currentLanguage)
        {
            case SupportedLanguage.English:
                Debug.Log("--- The language is set to English.");
                break;
            case SupportedLanguage.Chinese:
                Debug.Log("--- The language is set to Chinese.");
                break;
            default:
                Debug.LogWarning("Unsupported language.");
                break;
        }
    }
}