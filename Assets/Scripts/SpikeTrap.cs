using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    public GameObject spike;
    public Vector2 spawnLocation;
    public LayerMask platforms;

    [SerializeField] private float timeSinceSpawn = 0;
    public float direction = 0; //0 if facing left, 1 if facing right
    public float spawnTime = 0.5f;

    void Start()
    {
        if (direction == 0) {
            spawnLocation = new Vector2(GetComponent<Transform>().position.x - 1, GetComponent<Transform>().position.y);
        }
        else{
            spawnLocation = new Vector2(GetComponent<Transform>().position.x + 1, GetComponent<Transform>().position.y);            
        }
    }

    void Update()
    {
        timeSinceSpawn += Time.deltaTime;

        if (timeSinceSpawn > spawnTime && direction == 0) {
            Instantiate(spike, spawnLocation, Quaternion.Euler(0, 0, 0));
            timeSinceSpawn = 0;
        }
        else {
            Instantiate(spike, spawnLocation, Quaternion.Euler(180, 0, 0));
            timeSinceSpawn = 0;            
        }
    }
}