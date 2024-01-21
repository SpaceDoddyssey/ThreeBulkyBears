using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2 : MonoBehaviour
{
    private Rigidbody2D rb;
    private CircleCollider2D cc;
    public LayerMask platforms;
    private Vector3 initialPos;
    public float castDistance;
    public float groundDrag;
    private float horizontalInput;
    Vector2 moveDirection;
    [SerializeField]
    private BearStats bearStats;
    [Range(0, 10f)][SerializeField] private float speed = 4f;

    [SerializeField]
    private bool onGround = false;
    //bigger jump when holding jump button for longer
    private bool jump = false, jumpHeld = false;

    [SerializeField] private bool visualizeCircleCast = false;

    void Start()
    {
        initialPos = transform.position;
        rb = GetComponent<Rigidbody2D>();
        cc = GetComponent<CircleCollider2D>();
        bearStats.circleRadius = cc.radius;
    }

    // Update is called once per frame
    void Update()
    {
        checkOnGround();
        if (!jump)
        {
            jump = onGround && Input.GetKeyDown(KeyCode.Space);
        }

        jumpHeld = !onGround && Input.GetKey(KeyCode.Space);

        rb.drag = onGround ? groundDrag : 0f;

        if (Input.GetKeyDown(KeyCode.R))
        {
            transform.position = initialPos;
        }

        horizontalInput = Input.GetAxisRaw("Horizontal");
        LimitSpeed();
    }

    void FixedUpdate()
    {
        //universal jump logic
        moveDirection = new Vector2(1, 0) * horizontalInput;

        if (jump)
        {
            rb.velocity += Vector2.up * bearStats.jumpvel;
            jump = false;
        }

        if (rb.velocity.y > 0)
        {
            //Long jump or short jump
            float multiplier = jumpHeld ? bearStats.fallLongMult : bearStats.fallShortMult;
            rb.velocity += Vector2.up * Physics2D.gravity.y * (multiplier - 1) * Time.fixedDeltaTime;
        }

        //add force in direction we are moving
        rb.AddForce(new Vector2(horizontalInput, 0) * speed * 10f, ForceMode2D.Force);

        if (onGround)
        {
            // Thanks to copilot for this code
            // Calculate rotation angle based on velocity
            float rotationAngle = -rb.velocity.x / (2 * Mathf.PI * bearStats.circleRadius) * 360f;

            // Apply rotation to the circle collider
            cc.transform.Rotate(Vector3.forward, rotationAngle * Time.fixedDeltaTime);
        }
    }

    //check if the bear is touching the ground
    private void checkOnGround()
    {
        onGround = Physics2D.CircleCast(transform.position, bearStats.circleRadius, new Vector2(0, -1), castDistance, platforms);
    }

    void OnDrawGizmos()
    {
        if (visualizeCircleCast)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position - new Vector3(0, castDistance, 0), bearStats.circleRadius);
        }
    }

    //check if speed is above bear's speed if it is normalize it
    private void LimitSpeed()
    {
        Vector2 xVelocity = new Vector2(rb.velocity.x, 0f);

        if (xVelocity.magnitude > speed)
        {
            Vector2 cappedVelocity = xVelocity.normalized * speed;
            rb.velocity = new(cappedVelocity.x, rb.velocity.y);
        }
    }
}
