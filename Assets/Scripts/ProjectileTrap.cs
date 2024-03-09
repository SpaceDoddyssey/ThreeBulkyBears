using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileTrap : MonoBehaviour  
{
    public GameObject spike;
    public Vector2 spawnLocation;
    public LayerMask platforms;

    [SerializeField] 
    private float timeSinceSpawn = 0;
    private float spawnTime = 2f;
    
    public float direction = 0; //0 if facing left, 1 if facing right

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
            Instantiate(spike, spawnLocation, Quaternion.Euler(0, 0, 0), this.transform);
            timeSinceSpawn = 0;
        }
        else if (timeSinceSpawn > spawnTime)
        {
            Instantiate(spike, spawnLocation, Quaternion.Euler(180, 0, 0), this.transform);
            timeSinceSpawn = 0;            
        }
    }
}