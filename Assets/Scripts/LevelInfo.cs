using UnityEngine;

[CreateAssetMenu(fileName = "LevelInfo", menuName = "Level")]
public class LevelInfo : ScriptableObject
{
    public int levelID;
    public string levelName;
    public string sceneName;
    public string description;
    [Tooltip("In seconds")]
    public float goalTime;
    public bool isUnlocked;
}