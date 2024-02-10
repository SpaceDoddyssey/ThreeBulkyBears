using UnityEngine;

[CreateAssetMenu(fileName = "LevelInfo", menuName = "Level")]
public class LevelInfo : ScriptableObject
{
    public int levelID;
    public string levelName;
    public string sceneName;
    public string description;
    public float bestTime;
    public bool isUnlocked;
}