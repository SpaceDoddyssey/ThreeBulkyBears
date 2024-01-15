using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float moveSpeed = 5f;
    private float jumpForce = 5f;
    private bool inTheAir = false;
    private float playerRadius = 0.5f;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        CheckIfInTheAir();
        HandleInput();

        //Todo: manually handle rotation
    }

    void CheckIfInTheAir()
    {
        // An array to store the raycast hits
        RaycastHit2D[] results = new RaycastHit2D[2];

        // Perform a downward raycast (help from copilot)
        int numHits = Physics2D.RaycastNonAlloc(
            new Vector2(transform.position.x, transform.position.y), Vector2.down, results, playerRadius + 0.1f
        );

        // Check if the raycast hit the ground and ignore the player GameObject
        inTheAir = numHits <= 1 || !results[1].collider.CompareTag("Ground");
    }

    void HandleInput()
    {
        float moveX = Input.GetAxis("Horizontal");

        if (inTheAir)
        {
            rb.velocity = new Vector2(moveX * moveSpeed, rb.velocity.y);
        }
        else
        {
            var newVel = new Vector2(moveX * moveSpeed, rb.velocity.y);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                newVel.y += jumpForce;
            }

            rb.velocity = newVel;
        }
    }
}
