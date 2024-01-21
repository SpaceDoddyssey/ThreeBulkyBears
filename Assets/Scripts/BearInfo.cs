using UnityEngine;

[CreateAssetMenu(fileName = "BearStats", menuName = "BearStats")]
public class BearStats : ScriptableObject
{
    [Range(0, 15f)][SerializeField] public float fallLongMult = 1f;
    [Range(0, 15f)][SerializeField] public float fallShortMult = 2f;
    [Range(0, 15f)][SerializeField] public float jumpvel = 7f;
    [Range(0, 15f)][SerializeField] public float speed = 4f;
    [Range(0, 3f)][SerializeField] public float weight = 2f;
    public float circleRadius = 0.64f;
    public Sprite art;
    // Add more stats as needed
}
