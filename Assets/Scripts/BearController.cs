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
    private BearStats curr_bear;
    private BearStats next_bear;
    private KeyCode lastKeyPress;
    private KeyCode nextLastKeyPress;

    [SerializeField]
    private bool onGround = false;
    //bigger jump when holding jump button for longer
    private bool jumping = false, jumpHeld = false;
    //Disabled when in Game Over state
    public bool controllable = true;
    private bool activeMomentum;
    private float cap;

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

        if (activeMomentum && controllable) {
            HandleMomentum(curr_bear, bearStats);
        }
        else {
            cap = bearStats.speed;
            LimitSpeed(bearStats.speed);
        }
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
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) ChangeBearUp();
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) ChangeBearDown();
    }

    public void ResetBear()
    {
        controllable = true;
        rb.velocity = Vector2.zero;
        transform.position = initialPos;
        ChangeBear(mama, mama);
    }

    void ChangeBear(BearStats oldBear, BearStats newBear)
    {
        bearStats = newBear;
        float spriteScale = bearStats.circleRadius * 2 * bearStats.spriteSizeMultiplier;
        spriteRenderer.size = new Vector2(spriteScale, spriteScale);
        spriteRenderer.sprite = bearStats.art;
        cc.radius = bearStats.circleRadius;
        rb.mass = bearStats.mass;
        rb.gravityScale = bearStats.gravityMult;
        curr_bear = oldBear;
        cap = oldBear.speed;
        activeMomentum = true;
    }

    void ChangeBearUp()
    {
        if (bearStats == baby)
        {
            if (CheckRoomForBear(mama))
            {
                ChangeBear(baby, mama);
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
                ChangeBear(mama, papa);
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
            ChangeBear(papa, mama);
        }
        else if (bearStats == mama)
        {
            ChangeBear(mama, baby);
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
    void HandleMomentum(BearStats curr_bear, BearStats next_bear)
    {       
        if (curr_bear.speed > next_bear.speed) {
            cap -= 0.01f;
            LimitSpeed(cap); //edit speed cap over time until it matches new bear

            if (cap <= next_bear.speed) {
                cap = next_bear.speed;
                activeMomentum = false;
                return;
            }              
        }
        else if (next_bear.speed > curr_bear.speed) {
            cap += 0.01f;
            LimitSpeed(cap); //edit speed cap over time until it matches new bear

            if (cap >= next_bear.speed) {
                cap = next_bear.speed;
                activeMomentum = false;
                return;
            }
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
