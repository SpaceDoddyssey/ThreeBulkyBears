using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2 : MonoBehaviour
{
    private Rigidbody2D rb;
    private CircleCollider2D cc;
    //[SerializeField] private 
    public LayerMask platforms;
    public float circleRadius;
    public float castDistance;
    //[Range(0, 10f)] [SerializeField] private float speed = 0f;
    
    //float horizontal = 0f;
    //float lastJumpY = 0f;
    //private bool isFacingRight = true;
    
    //bigger jump when holding jump button for longer
    bool jump = false, jumpHeld = false;
    
    [Range(0, 15f)] [SerializeField] private float fallLongMult = 1f;
    [Range(0, 15f)] [SerializeField] private float fallShortMult = 2f;
    //edit above values based on size of the bear
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cc = GetComponent<CircleCollider2D>();
        circleRadius = cc.radius;
    }

    // Update is called once per frame
    void Update()
    {
        if (isOnGround() && Input.GetKeyDown(KeyCode.Space)) {
            jump = true;
        }
        if (!isOnGround() && Input.GetKeyDown(KeyCode.Space)) {
            jumpHeld = true;                        
        }
        else {
            jumpHeld = false;
        }

        //checking if jump is short or high
    }

    void FixedUpdate()
    {
        //universal jump logic
        if (jump)
        {
            float jumpvel = 7f;
            rb.velocity = Vector2.up * jumpvel;
            jump = false;
        }

        //high jump
        if (jumpHeld && rb.velocity.y > 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallLongMult - 1) * Time.fixedDeltaTime;
        }
        else if(!jumpHeld && rb.velocity.y > 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallShortMult - 1) * Time.fixedDeltaTime;
        }
        //low jump
    }

    //check if the bear is touching the ground
    private bool isOnGround() {
        if (Physics2D.CircleCast(transform.position, circleRadius, -transform.up, castDistance, platforms))
        {
            return true;
        }
        else 
        {
            return false;
        }
    }
}
