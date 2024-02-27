using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public LevelInfo curLevelInfo = null;

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
        LocalizationSettings.InitializationOperation.WaitForCompletion();
        Debug.Log(LocalizationSettings.AvailableLocales.Locales[0].LocaleName);
        Debug.Log(LocalizationSettings.AvailableLocales.Locales[1].LocaleName);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (LocalizationSettings.SelectedLocale == LocalizationSettings.AvailableLocales.Locales[1])
                LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[0];
            else
                LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[1];
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            if (LocalizationSettings.SelectedLocale == LocalizationSettings.AvailableLocales.Locales[2])
                LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[0];
            else
                LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[2];
        }
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}