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
        selectButton(0);
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
            //Debug.Log("hello");
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
    }

    public void selectButton(int index)
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
