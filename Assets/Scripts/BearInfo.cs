using UnityEngine;

[CreateAssetMenu(fileName = "BearStats", menuName = "BearStats")]
public class BearStats : ScriptableObject
{
    [Range(0, 15f)][SerializeField] public float fallLongMult = 1f;
    [Range(0, 15f)][SerializeField] public float fallShortMult = 2f;
    [Range(0, 15f)][SerializeField] public float jumpvel = 7f;
    public float circleRadius = 0.64f;
    // Add more stats as needed
}
