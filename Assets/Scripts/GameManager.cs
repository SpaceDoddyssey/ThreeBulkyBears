using UnityEngine;
using UnityEngine.SceneManagement;
using System.Globalization;

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
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}