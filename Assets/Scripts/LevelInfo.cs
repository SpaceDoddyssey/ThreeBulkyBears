using UnityEngine;

[CreateAssetMenu(fileName = "LevelInfo", menuName = "Level")]
public class LevelInfo : ScriptableObject
{
    public int levelID;
    public string levelName;
    public string sceneName;
    public string description;
    private double _bestTime = double.PositiveInfinity;
    public float goalTime;
    public bool isUnlocked;

    public double bestTime
    {
        get
        {
            return _bestTime;
        }
        set
        {
            _bestTime = value;
        }
    }
}