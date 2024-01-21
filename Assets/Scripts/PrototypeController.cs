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
    public float groundDrag;
    private float horizontalInput;
    Vector2 moveDirection;
    [Range(0, 10f)] [SerializeField] private float speed = 2f;

    //float horizontal = 0f;
    //float lastJumpY = 0f;
    //private bool isFacingRight = true;

    //bigger jump when holding jump button for longer
    bool jump = false, jumpHeld = false;

    [Range(0, 15f)][SerializeField] private float fallLongMult = 1f;
    [Range(0, 15f)][SerializeField] private float fallShortMult = 2f;
    [Range(0, 15f)][SerializeField] private float jumpvel = 7f;
    //edit above values based on size of the bear

    [SerializeField] private bool visualizeCircleCast = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cc = GetComponent<CircleCollider2D>();
        circleRadius = cc.radius;
    }

    // Update is called once per frame
    void Update()
    {
        if (!jump) {
            jump = isOnGround() && Input.GetKeyDown(KeyCode.Space);
        }

        jumpHeld = !isOnGround() && Input.GetKey(KeyCode.Space);

        if (isOnGround()) {
            rb.drag = groundDrag;
        }
        else {
            rb.drag = 0;
        }
        
        horizontalInput = Input.GetAxisRaw("Horizontal");
        SpeedControl();
    }

    void FixedUpdate()
    {
        //universal jump logic
        moveDirection = new Vector2(1, 0) * horizontalInput;

        //rb.AddTorque(moveDirection.normalized * speed * 10f, ForceMode2D.Force);
        
        if (jump)
        {
            rb.velocity += Vector2.up * jumpvel;
            jump = false;
        }

        if (rb.velocity.y > 0)
        {
            //Long jump or short jump
            float multiplier = jumpHeld ? fallLongMult : fallShortMult;
            rb.velocity += Vector2.up * Physics2D.gravity.y * (multiplier - 1) * Time.fixedDeltaTime;
        }

        if (isOnGround()) {
            rb.AddForce(moveDirection.normalized * speed * 10f, ForceMode2D.Force);
        } //add force in direction we are moving
    }

    //check if the bear is touching the ground
    private bool isOnGround()
    {
        return Physics2D.CircleCast(transform.position, circleRadius, new Vector2(0, -1), castDistance, platforms);
    }

    void OnDrawGizmos()
    {
        if (visualizeCircleCast)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position - new Vector3(0, castDistance, 0), circleRadius);
        }
    }

    private void SpeedControl()
    {
        Vector2 flatvel = new Vector2(rb.velocity.x, 0f);

        if (flatvel.magnitude > speed) {
            Vector2 limitedvel = flatvel.normalized*speed;
            rb.velocity = new (limitedvel.x, rb.velocity.y);
        }
        //check if speed is above bear's speed if it is normalize it
    }
}
