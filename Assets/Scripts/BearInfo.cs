using UnityEngine;

[CreateAssetMenu(fileName = "BearStats", menuName = "BearStats")]
public class BearStats : ScriptableObject
{
    //Add more stats as needed
    [Range(0, 15f)] public float fallLongMult = 1f;
    [Range(0, 15f)] public float fallShortMult = 2f;
    [Range(0, 15f)] public float jumpvel = 7f;
    [Range(0, 15f)] public float speed = 4f;
    [Range(0, 3f)] public float mass = 2f;
    public float gravityMult = 1f;
    public float circleRadius = 0.64f;
    public Sprite art;

    [Space]
    [Header("Abilities")]
    public bool breaksBranches = false;
}
