using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    public float moveSpeed = 2f;
    public LayerMask platforms;
    private PolygonCollider2D pc;

    void Start() 
    {
        pc = GetComponent<PolygonCollider2D>();
    }

    void Update() 
    {
        transform.position += moveSpeed * transform.right * Time.deltaTime;

        checkForCollision(pc);
    }

    void checkForCollision(PolygonCollider2D collision) 
    {
        if (collision.gameObject.tag == "Player")
        {
            GameObject.Find("LevelManager").GetComponent<LevelManager>().GameOver();
        }    
        else if (Physics2D.Raycast(transform.position, transform.right, 0.2f, platforms)) {
            Destroy(gameObject);        
        }
    }
}