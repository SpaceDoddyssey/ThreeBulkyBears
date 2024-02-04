using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public LevelInfo curLevelInfo = null;
    [SerializeField]
    private string sceneToLoad;

    public void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    public void Start()
    {
        if (sceneToLoad != "")
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}