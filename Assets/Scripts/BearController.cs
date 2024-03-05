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
    private CameraController cameraController;
    private CurBearDisplay curBearDisplay;

    [Header("Set in Inspector")]
    public AudioClip cantChangeSound;
    public LayerMask platforms;
    public float castDistance;
    public float groundDrag, airStopMult;
    public float acceleration, deceleration;

    [Header("Changed dynamically")]
    public BearStats curBearStats;
    public BearStats prevBearStats;
    private BearStats baby, mama, papa;

    [SerializeField]
    private bool onGround = false;
    //bigger jump when holding jump button for longer
    private bool jumping = false, jumpHeld = false;
    //Disabled when in Game Over state
    public bool controllable = true;
    public float maxSpeed;
    private float horizontalInput;
    private GameObject bearSpawnLoc;

    [SerializeField] private bool visualizeCircleCast = false;

    void GetReferences()
    {
        rb = GetComponent<Rigidbody2D>();
        cc = GetComponent<CircleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        bearSpawnLoc = GameObject.Find("BearSpawnLoc");
        cameraController = GameObject.Find("Main Camera").GetComponent<CameraController>();
        curBearDisplay = GameObject.Find("CurBearDisplay").GetComponent<CurBearDisplay>();
    }

    void Start()
    {
        GetReferences();
        bearSpawnLoc.GetComponent<SpriteRenderer>().enabled = false;

        baby = Resources.Load("BearStats/BabyBear") as BearStats;
        mama = Resources.Load("BearStats/MamaBear") as BearStats;
        papa = Resources.Load("BearStats/PapaBear") as BearStats;

        ResetBear();
    }

    // Update is called once per frame
    void Update()
    {
        CheckIfOnGround();

        if (controllable)
            HandleControls();

        ApplyDrag();

        HandleMomentum();
    }

    private void ApplyDrag()
    {
        if (onGround)
            rb.drag = groundDrag;
        else
        {
            rb.drag = 0;
            if (horizontalInput == 0)
                rb.velocity = new Vector2(rb.velocity.x * airStopMult, rb.velocity.y);
        }
    }

    private void HandleControls()
    {
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
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) ChangeBearUp();
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) ChangeBearDown();
    }

    public void ResetBear()
    {
        controllable = true;
        rb.velocity = Vector2.zero;
        transform.position = bearSpawnLoc.transform.position;
        ChangeBear(mama);
        maxSpeed = mama.speed;
    }

    void ChangeBear(BearStats newBear)
    {
        prevBearStats = curBearStats;
        curBearStats = newBear;
        float spriteScale = curBearStats.circleRadius * 2 * curBearStats.spriteSizeMultiplier;
        spriteRenderer.size = new Vector2(spriteScale, spriteScale);
        spriteRenderer.sprite = curBearStats.art;
        cc.radius = curBearStats.circleRadius;
        rb.mass = curBearStats.mass;
        rb.gravityScale = curBearStats.gravityMult;
        curBearDisplay.UpdateDisplay(curBearStats);
    }

    void ChangeBearUp()
    {
        if (curBearStats == baby)
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
        else if (curBearStats == mama)
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
        if (curBearStats == papa)
        {
            ChangeBear(mama);
        }
        else if (curBearStats == mama)
        {
            ChangeBear(baby);
        }
    }

    void Jump()
    {
        rb.velocity += Vector2.up * curBearStats.jumpvel;
        jumping = true;
        audioSource.PlayOneShot(curBearStats.jumpSound);
    }

    void FixedUpdate()
    {
        if (rb.velocity.y > 0)
        {
            //Long jump or short jump
            float multiplier = jumpHeld ? curBearStats.fallLongMult : curBearStats.fallShortMult;
            rb.velocity += Vector2.up * Physics2D.gravity.y * (multiplier - 1) * Time.fixedDeltaTime;
        }

        //add force in direction we are moving
        rb.AddForce(new Vector2(horizontalInput, 0) * curBearStats.speed * 10f, ForceMode2D.Force);

        if (onGround)
        {
            // Thanks to copilot for this code
            // Calculate rotation angle based on velocity
            float rotationAngle = -rb.velocity.x / (2 * Mathf.PI * curBearStats.circleRadius) * 360f;

            // Apply rotation to the circle collider
            cc.transform.Rotate(Vector3.forward, rotationAngle * Time.fixedDeltaTime);
        }
    }

    private bool CheckIfOnGround()
    {
        var cast = Physics2D.CircleCast(transform.position, curBearStats.circleRadius - 0.05f, new Vector2(0, -1), castDistance, platforms);

        onGround = cast.collider != null;
        if (onGround)
        {
            MovingPlatform movingPlatform = cast.collider.GetComponent<MovingPlatform>();
            if (movingPlatform != null)
            {
                transform.parent = movingPlatform.transform;
                return true;
            }
        }

        transform.parent = null;
        return false;
    }

    void OnDrawGizmos()
    {
        if (visualizeCircleCast)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position - new Vector3(0, castDistance, 0), curBearStats.circleRadius - 0.05f);
        }
    }

    private void LimitSpeed(float xcap)
    {
        Vector2 xVelocity = new Vector2(rb.velocity.x, 0f);
        if (xVelocity.magnitude > xcap)
        {
            Vector2 cappedVelocity = xVelocity.normalized * xcap;
            rb.velocity = new(cappedVelocity.x, rb.velocity.y);
        }
    }

    //handle momentum when moving constantly in a direction by editing speed cap
    void HandleMomentum()
    {
        float speedDifference = maxSpeed - curBearStats.speed;
        if (Mathf.Abs(speedDifference) < 0.1f)
        {
            maxSpeed = curBearStats.speed;
            LimitSpeed(maxSpeed);
            return;
        }

        if (speedDifference > 0)
            maxSpeed -= deceleration * Time.deltaTime;
        else if (speedDifference < 0)
            maxSpeed += acceleration * Time.deltaTime;

        LimitSpeed(maxSpeed); //edit speed cap over time until it matches new bear
    }

    void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        if (curBearStats == papa)
        {
            float collisionForce = collisionInfo.relativeVelocity.magnitude / Time.fixedDeltaTime;
            cameraController.Shake(collisionForce / 100, collisionForce / 500);
        }
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
