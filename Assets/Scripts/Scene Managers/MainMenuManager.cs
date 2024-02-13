using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] GameObject mainMenuScreen, settingsScreen, creditsScreen;
    [SerializeField] List<GameObject> buttons;
    private int curButtonIndex = 0;
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        selectButton(0);
        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
    }

    void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            int newButton = curButtonIndex - 1;
            if (newButton < 0)
            {
                return;
            }
            selectButton(newButton);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            int newButton = curButtonIndex + 1;
            if (newButton >= buttons.Count)
            {
                return;
            }
            selectButton(newButton);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            buttons[curButtonIndex].GetComponent<Button>().onClick.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ReturnToMain();
        }
    }

    public void selectButton(int index)
    {
        buttons[curButtonIndex].transform.localScale = new Vector2(1, 1);
        curButtonIndex = index;
        buttons[curButtonIndex].transform.localScale = new Vector2(1.3f, 1.3f);
    }

    public void NewGame()
    {
        gameManager.LoadScene("LevelSelection");
    }

    public void Credits()
    {
        mainMenuScreen.SetActive(false);
        creditsScreen.SetActive(true);
    }

    public void Settings()
    {
        mainMenuScreen.SetActive(false);
        settingsScreen.SetActive(true);
    }

    public void ReturnToMain()
    {
        mainMenuScreen.SetActive(true);
        creditsScreen.SetActive(false);
        settingsScreen.SetActive(false);
    }
}
