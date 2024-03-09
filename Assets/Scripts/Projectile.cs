using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    public LayerMask platforms;
    private PolygonCollider2D pc;
    private Rigidbody2D rb;
    public ProjectileTrap trap;

    public float moveSpeed = 6f;
    public float sdirection;

    void Start() 
    {
        pc = GetComponent<PolygonCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        trap = GetComponentInParent<ProjectileTrap>();
        sdirection = trap.direction;

        rb.gravityScale = 0f;
    }

    void Update() 
    {
        if (sdirection == 0) {
            transform.position += moveSpeed * -transform.right * Time.deltaTime;
        }
        else {
            transform.position += moveSpeed * transform.right * Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.gameObject.tag == "Player")
        {
            Destroy(gameObject);
            GameObject.Find("LevelManager").GetComponent<LevelManager>().GameOver();
        } 
        else if (collision.gameObject.tag == "Ground") 
        {
            Destroy(gameObject);       
        }   
    }
}