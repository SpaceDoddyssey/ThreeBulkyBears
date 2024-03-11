using UnityEngine;

[CreateAssetMenu(fileName = "LevelInfo", menuName = "Level")]
public class LevelInfo : ScriptableObject
{

    [Header("Level Data")]
    public int levelID;
    public string levelName;
    public string sceneName;
    public string description;
    [Tooltip("In seconds")]
    public float goalTime;
    public bool isUnlocked;

    [Space]
    [Header("Level Construction Settings")]
    public float fallingDeathZoneY;
    public float cloudBoundLeft, cloudBoundRight;
    //There should maybe be a lower and upper cloudbound here too but I don't feel like adding those right now
}