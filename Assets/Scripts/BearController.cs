using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearController : MonoBehaviour
{
    //Components
    private Rigidbody2D rb;
    private CircleCollider2D cc;
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;
    private LevelManager levelMan;

    public AudioClip cantChangeSound;
    public LayerMask platforms;
    private Vector3 initialPos;
    public float castDistance;
    public float groundDrag;
    private float horizontalInput;
    [SerializeField]
    public BearStats bearStats;
    private BearStats baby, mama, papa;
    private KeyCode lastKeyPress;
    private KeyCode nextLastKeyPress;

    [SerializeField]
    private bool onGround = false;
    //bigger jump when holding jump button for longer
    private bool jumping = false, jumpHeld = false;
    //Disabled when in Game Over state
    public bool controllable = true;
    private float timeSincePress;
    private float storeSpeedBaby;
    private float storeSpeedMama;
    private float storeSpeedPapa;

    [SerializeField] private bool visualizeCircleCast = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cc = GetComponent<CircleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        levelMan = FindObjectOfType<LevelManager>();
        GameObject bearSpawnLoc = GameObject.Find("BearSpawnLoc");
        bearSpawnLoc.GetComponent<SpriteRenderer>().enabled = false;
        initialPos = bearSpawnLoc.transform.position;

        baby = Resources.Load("BearStats/BabyBear") as BearStats;
        mama = Resources.Load("BearStats/MamaBear") as BearStats;
        papa = Resources.Load("BearStats/PapaBear") as BearStats;

        storeSpeedBaby = baby.speed;
        storeSpeedMama = mama.speed;
        storeSpeedPapa = papa.speed;
        ResetBear();
    }

    // Update is called once per frame
    void Update()
    {
        checkIfOnGround();

        if (controllable)
        {
            HandleControls();
        }

        rb.drag = onGround ? groundDrag : 0f;

        OldHandleMovement();
    }

    private void HandleControls()
    {
        if (levelMan.paused) { return; }
        if (jumping && onGround && rb.velocity.y <= 0)
        {
            jumping = false;
        }
        if (!jumping && onGround && Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        jumpHeld = !onGround && Input.GetKey(KeyCode.Space);

        horizontalInput = Input.GetAxisRaw("Horizontal");
        if (Input.GetKeyDown(KeyCode.W)) ChangeBearUp();
        if (Input.GetKeyDown(KeyCode.S)) ChangeBearDown();
    }

    public void ResetBear()
    {
        controllable = true;
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

    void ChangeBearUp()
    {
        if (bearStats == baby)
        {
            if (CheckRoomForBear(mama))
            {
                ChangeBear(mama);
            }
            else
            {
                audioSource.PlayOneShot(cantChangeSound);
            }
        }
        else if (bearStats == mama)
        {
            if (CheckRoomForBear(papa))
            {
                ChangeBear(papa);
            }
            else
            {
                audioSource.PlayOneShot(cantChangeSound);
            }
        }
    }

    void ChangeBearDown()
    {
        if (bearStats == papa)
        {
            ChangeBear(mama);
        }
        else if (bearStats == mama)
        {
            ChangeBear(baby);
        }
    }

    void Jump()
    {
        rb.velocity += Vector2.up * bearStats.jumpvel;
        jumping = true;
        audioSource.PlayOneShot(bearStats.jumpSound);
    }

    void FixedUpdate()
    {
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

    private void checkIfOnGround()
    {
        onGround = Physics2D.CircleCast(transform.position, bearStats.circleRadius - 0.05f, new Vector2(0, -1), castDistance, platforms);
    }

    void OnDrawGizmos()
    {
        if (visualizeCircleCast)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position - new Vector3(0, castDistance, 0), bearStats.circleRadius - 0.05f);
        }
    }

    private void OldHandleMovement()
    {
        Vector2 xVelocity = new Vector2(rb.velocity.x, 0f);
        if (xVelocity.magnitude > bearStats.speed)
        {
            Vector2 cappedVelocity = xVelocity.normalized * bearStats.speed;
            rb.velocity = new(cappedVelocity.x, rb.velocity.y);
        }
    }

    //handle momentum when moving constantly in a direction, check if speed is above bear's speed if it is normalize it
    private void HandleMovement()
    {
        timeSincePress = 0f;
        Vector2 xVelocity = new Vector2(rb.velocity.x, 0f);
        Vector2 cappedVelocity;

        if (bearStats == baby)
        {
            if (Input.GetKeyDown(KeyCode.A) && onGround)
            {
                if (rb.velocity.x > 0)
                {
                    storeSpeedBaby = bearStats.speed;
                    timeSincePress = 0f;

                    cappedVelocity = xVelocity.normalized * (bearStats.speed * 1.5f);
                    rb.velocity = new(cappedVelocity.x, rb.velocity.y);
                }

                if (xVelocity.magnitude > (bearStats.speed * 1.5f))
                {
                    cappedVelocity = xVelocity.normalized * (bearStats.speed * 1.5f);
                    rb.velocity = new(cappedVelocity.x, rb.velocity.y);
                }

                if (timeSincePress < 1)
                {
                    storeSpeedBaby += 0.000000001f;
                }
                else if (timeSincePress < 2)
                {
                    storeSpeedBaby += 0.00000001f;
                }

                timeSincePress += Time.deltaTime;
            }
            else if (Input.GetKeyDown(KeyCode.D) && onGround)
            {
                if (rb.velocity.x < 0)
                {
                    storeSpeedBaby = bearStats.speed;
                    timeSincePress = 0f;

                    cappedVelocity = xVelocity.normalized * (bearStats.speed * 1.5f);
                    rb.velocity = new(cappedVelocity.x, rb.velocity.y);
                }

                if (xVelocity.magnitude > (bearStats.speed * 1.5f))
                {
                    cappedVelocity = xVelocity.normalized * (bearStats.speed * 1.5f);
                    rb.velocity = new(cappedVelocity.x, rb.velocity.y);
                }

                if (timeSincePress < 1)
                {
                    storeSpeedBaby += 0.000000001f;
                }
                else if (timeSincePress < 2)
                {
                    storeSpeedBaby += 0.00000001f;
                }

                timeSincePress += Time.deltaTime;
            }
        }
        else if (bearStats == mama)
        {
            if (Input.GetKeyDown(KeyCode.A) && onGround)
            {
                if (rb.velocity.x > 0)
                {
                    storeSpeedMama = bearStats.speed;
                    timeSincePress = 0f;

                    cappedVelocity = xVelocity.normalized * (bearStats.speed * 1.5f);
                    rb.velocity = new(cappedVelocity.x, rb.velocity.y);
                }
                lastKeyPress = nextLastKeyPress;

                if (timeSincePress < 5)
                {
                    storeSpeedMama += 0.1f;
                }

                if (xVelocity.magnitude > (bearStats.speed * 1.5f))
                {
                    cappedVelocity = xVelocity.normalized * (bearStats.speed * 1.5f);
                    rb.velocity = new(cappedVelocity.x, rb.velocity.y);
                }
            }
            else if (Input.GetKeyDown(KeyCode.D) && onGround)
            {
                if (rb.velocity.x < 0)
                {
                    storeSpeedMama = bearStats.speed;
                    timeSincePress = 0f;

                    cappedVelocity = xVelocity.normalized * (bearStats.speed * 1.5f);
                    rb.velocity = new(cappedVelocity.x, rb.velocity.y);
                }

                if (timeSincePress < 5)
                {
                    storeSpeedMama += 0.1f;
                }

                if (xVelocity.magnitude > (bearStats.speed * 1.5f))
                {
                    cappedVelocity = xVelocity.normalized * (bearStats.speed * 1.5f);
                    rb.velocity = new(cappedVelocity.x, rb.velocity.y);
                }

                timeSincePress += Time.deltaTime;
            }
            // else {
            //     if (xVelocity.magnitude > (storeSpeedMama * 1.5)) {
            //         cappedVelocity = xVelocity.normalized * storeSpeedMama;
            //         rb.velocity = new(cappedVelocity.x, rb.velocity.y);                
            //     }
            // }
        }
        else if (bearStats == papa)
        {
            if (Input.GetKeyDown(KeyCode.A) && onGround)
            {
                if (rb.velocity.x > 0)
                {
                    storeSpeedPapa = bearStats.speed;
                    timeSincePress = 0f;

                    cappedVelocity = xVelocity.normalized * (bearStats.speed * 1.5f);
                    rb.velocity = new(cappedVelocity.x, rb.velocity.y);
                }

                if (timeSincePress < 5)
                {
                    storeSpeedPapa += 0.1f;
                }

                if (xVelocity.magnitude > (bearStats.speed * 1.5f))
                {
                    cappedVelocity = xVelocity.normalized * (bearStats.speed * 1.5f);
                    rb.velocity = new(cappedVelocity.x, rb.velocity.y);
                }
            }
            else if (Input.GetKeyDown(KeyCode.D) && onGround)
            {
                if (rb.velocity.x > 0)
                {
                    storeSpeedPapa = bearStats.speed;
                    timeSincePress = 0f;

                    cappedVelocity = xVelocity.normalized * (bearStats.speed * 1.5f);
                    rb.velocity = new(cappedVelocity.x, rb.velocity.y);
                }

                if (timeSincePress < 5)
                {
                    storeSpeedPapa += 0.1f;
                }

                if (xVelocity.magnitude > (bearStats.speed * 1.5f))
                {
                    cappedVelocity = xVelocity.normalized * (bearStats.speed * 1.5f);
                    rb.velocity = new(cappedVelocity.x, rb.velocity.y);
                }

                timeSincePress += Time.deltaTime;
            }
            // else {
            //     if (xVelocity.magnitude > (storeSpeedPapa * 1.5f)) {
            //         cappedVelocity = xVelocity.normalized * storeSpeedPapa;
            //         rb.velocity = new(cappedVelocity.x, rb.velocity.y);                
            //     }                
            // }
        }
        // if (xVelocity.magnitude > bearStats.speed)
        // {
        //     Vector2 cappedVelocity = xVelocity.normalized * bearStats.speed;
        //     rb.velocity = new(cappedVelocity.x, rb.velocity.y);
        // }
    }

    bool CheckRoomForBear(BearStats newBear)
    {
        float newDiameter = newBear.circleRadius * 2;
        float verticalRoom, horizontalRoom;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, newDiameter, platforms);
        if (hit.collider == null)
            return true;
        else
            verticalRoom = hit.distance;

        hit = Physics2D.Raycast(transform.position, Vector2.down, newDiameter, platforms);
        if (hit.collider == null)
            return true;
        else
            verticalRoom += hit.distance;

        if (verticalRoom < newDiameter)
            return false;

        hit = Physics2D.Raycast(transform.position, Vector2.right, newDiameter, platforms);
        if (hit.collider == null)
            return true;
        else
            horizontalRoom = hit.distance;

        hit = Physics2D.Raycast(transform.position, Vector2.left, newDiameter, platforms);
        if (hit.collider == null)
            return true;
        else
            horizontalRoom += hit.distance;

        if (horizontalRoom < newDiameter)
            return false;
        return true;
    }
}
