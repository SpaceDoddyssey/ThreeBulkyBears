using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    [SerializeField] LevelManager lm;

    [SerializeField] List<GameObject> buttons;
    private int curButtonIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        SelectButton(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (!lm.paused)
        {
            return;
        }
        CheckInput();
    }

    void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.W)) ChangeSelectedButton(-1);
        if (Input.GetKeyDown(KeyCode.S)) ChangeSelectedButton(1);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            buttons[curButtonIndex].GetComponent<Button>().onClick.Invoke();
        }
    }

    void ChangeSelectedButton(int direction)
    {
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

    public void ReturnToMenu()
    {
        Time.timeScale = 1;
        FindObjectOfType<GameManager>().LoadScene("MainMenu");
    }

    public void ReturnToLevelSelect()
    {
        Time.timeScale = 1;
        FindObjectOfType<GameManager>().LoadScene("LevelSelection");
    }

    public void ContinueGame()
    {
        lm.TogglePause();
    }
}
