using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] GameObject mainMenuScreen, settingsScreen, creditsScreen;
    [SerializeField] List<GameObject> buttons;
    private int curButtonIndex = 0;
    private GameManager gameManager;
    private bool onMainScreen = true;

    // Start is called before the first frame update
    void Start()
    {
        SelectButton(0);
        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
    }

    void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.W)) ChangeSelectedButton(-1);
        if (Input.GetKeyDown(KeyCode.S)) ChangeSelectedButton(1);
        if (Input.GetKeyDown(KeyCode.Escape)) ReturnToMain();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            buttons[curButtonIndex].GetComponent<Button>().onClick.Invoke();
        }
    }

    void ChangeSelectedButton(int direction)
    {
        if (!onMainScreen) { return; } //Don't change buttons while on settings or credits screens (settings will have buttons but not yet implemented)
        int newButton = curButtonIndex + direction;
        if (newButton < 0 || newButton >= buttons.Count)
        {
            return;
        }
        SelectButton(newButton);
    }

    public void SelectButton(int index)
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
        onMainScreen = false;
    }

    public void Settings()
    {
        mainMenuScreen.SetActive(false);
        settingsScreen.SetActive(true);
        onMainScreen = false;
    }

    public void ReturnToMain()
    {
        mainMenuScreen.SetActive(true);
        creditsScreen.SetActive(false);
        settingsScreen.SetActive(false);
        onMainScreen = true;
    }
}