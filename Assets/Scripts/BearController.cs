using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearController : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private CircleCollider2D cc;
    public LayerMask platforms;
    private Vector3 initialPos;
    public float castDistance;
    public float groundDrag;
    private float horizontalInput;
    [SerializeField]
    private BearStats bearStats;
    private BearStats baby, mama, papa;

    [SerializeField]
    private bool onGround = false;
    //bigger jump when holding jump button for longer
    private bool jump = false, jumpHeld = false;

    [SerializeField] private bool visualizeCircleCast = false;

    void Start()
    {
        initialPos = GameObject.Find("BearSpawnLoc").transform.position;
        rb = GetComponent<Rigidbody2D>();
        cc = GetComponent<CircleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        baby = Resources.Load("BearStats/BabyBear") as BearStats;
        mama = Resources.Load("BearStats/MamaBear") as BearStats;
        papa = Resources.Load("BearStats/PapaBear") as BearStats;

        ResetBear();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C)) ChangeBear(baby);
        if (Input.GetKeyDown(KeyCode.V)) ChangeBear(mama);
        if (Input.GetKeyDown(KeyCode.B)) ChangeBear(papa);

        checkIfOnGround();
        if (!jump)
        {
            jump = onGround && Input.GetKeyDown(KeyCode.Space);
        }
        jumpHeld = !onGround && Input.GetKey(KeyCode.Space);

        rb.drag = onGround ? groundDrag : 0f;

        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetBear();
        }

        horizontalInput = Input.GetAxisRaw("Horizontal");
        LimitSpeed();
    }

    public void ResetBear()
    {
        rb.velocity = Vector2.zero;
        transform.position = initialPos;
        ChangeBear(mama);
    }

    void ChangeBear(BearStats newBear)
    {
        bearStats = newBear;
        float spriteScale = bearStats.circleRadius * 2 * bearStats.spriteSizeMultiplier;
        spriteRenderer.size = new Vector2(spriteScale, spriteScale);
        spriteRenderer.sprite = bearStats.art;
        cc.radius = bearStats.circleRadius;
        rb.mass = bearStats.mass;
        rb.gravityScale = bearStats.gravityMult;
    }

    void FixedUpdate()
    {
        //universal jump logic
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
        rb.AddForce(new Vector2(horizontalInput, 0) * bearStats.speed * 10f, ForceMode2D.Force);

        if (onGround)
        {
            // Thanks to copilot for this code
            // Calculate rotation angle based on velocity
            float rotationAngle = -rb.velocity.x / (2 * Mathf.PI * bearStats.circleRadius) * 360f;

            // Apply rotation to the circle collider
            cc.transform.Rotate(Vector3.forward, rotationAngle * Time.fixedDeltaTime);
        }
    }

    float radiusDivisor = 5f; //I don't know why this is necessary to be honest, but it's related to the spriteRenderer
    //check if the bear is touching the ground
    private void checkIfOnGround()
    {
        onGround = Physics2D.CircleCast(transform.position, bearStats.circleRadius / radiusDivisor, new Vector2(0, -1), castDistance, platforms);
    }

    void OnDrawGizmos()
    {
        if (visualizeCircleCast)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position - new Vector3(0, castDistance, 0), bearStats.circleRadius / radiusDivisor);
        }
    }

    //check if speed is above bear's speed if it is normalize it
    private void LimitSpeed()
    {
        Vector2 xVelocity = new Vector2(rb.velocity.x, 0f);
        if (xVelocity.magnitude > bearStats.speed)
        {
            Vector2 cappedVelocity = xVelocity.normalized * bearStats.speed;
            rb.velocity = new(cappedVelocity.x, rb.velocity.y);
        }
    }
}
