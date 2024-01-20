using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2 : MonoBehaviour
{
    private Rigidbody2D rb;
    private CircleCollider2D cc;
    public LayerMask platforms;
    public float circleRadius;
    public float castDistance;
    //[Range(0, 10f)] [SerializeField] private float speed = 0f;

    //float horizontal = 0f;
    //float lastJumpY = 0f;
    //private bool isFacingRight = true;

    //bigger jump when holding jump button for longer
    bool jump = false, jumpHeld = false;

    [Range(0, 15f)][SerializeField] private float fallLongMult = 1f;
    [Range(0, 15f)][SerializeField] private float fallShortMult = 2f;
    [Range(0, 15f)][SerializeField] private float jumpvel = 7f;
    //edit above values based on size of the bear

    private bool visualizeCircleCast = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cc = GetComponent<CircleCollider2D>();
        circleRadius = cc.radius;
    }

    // Update is called once per frame
    void Update()
    {
        jump = isOnGround() && Input.GetKeyDown(KeyCode.Space);

        jumpHeld = !isOnGround() && Input.GetKey(KeyCode.Space);
    }

    void FixedUpdate()
    {
        //universal jump logic
        if (jump)
        {
            rb.velocity = Vector2.up * jumpvel;
            jump = false;
        }

        if (rb.velocity.y > 0)
        {
            //Long jump or short jump
            float multiplier = jumpHeld ? fallLongMult : fallShortMult;
            rb.velocity += Vector2.up * Physics2D.gravity.y * (multiplier - 1) * Time.fixedDeltaTime;
        }
    }

    //check if the bear is touching the ground
    private bool isOnGround()
    {
        return Physics2D.CircleCast(transform.position, circleRadius, -transform.up, castDistance, platforms);
    }

    void OnDrawGizmos()
    {
        if (visualizeCircleCast)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position - new Vector3(0, castDistance, 0), circleRadius);
        }
    }
}
