using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public List<GameObject> buttons;
    public int curButtonIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        selectButton(0);
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

    public void NewGame()
    {

    }
}
