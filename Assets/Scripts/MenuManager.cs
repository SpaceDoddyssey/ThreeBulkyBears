using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public List<LevelInfo> levels;
    public List<GameObject> levelIcons;
    private int curLevelSelected = 0;

    // Start is called before the first frame update
    void Start()
    {
        selectLevel(0);
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
    }

    void CheckInput(){
        if(Input.GetKeyDown(KeyCode.A)){
            int newLevel = curLevelSelected - 1;
            if(newLevel < 0){
                return;
            }
            selectLevel(newLevel);
        }
        if(Input.GetKeyDown(KeyCode.D)){
            //Debug.Log("hello");
            int newLevel = curLevelSelected + 1;
            if(newLevel >= levels.Count || !levels[newLevel].isUnlocked){
                return;
            }
            selectLevel(newLevel);
        }
        if(Input.GetKeyDown(KeyCode.Space)){
            goToLevel(levels[curLevelSelected].sceneName);
        }
    }

    public void selectLevel(int levelIndex){

        levelIcons[curLevelSelected].transform.localScale = new Vector2(1, 1);
        curLevelSelected = levelIndex;
        levelIcons[curLevelSelected].transform.localScale = new Vector2(1.5f, 1.5f);
    }

    public void goToLevel(string level) {
        SceneManager.LoadScene(level);
    }
}