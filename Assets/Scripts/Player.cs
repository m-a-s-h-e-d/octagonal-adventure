using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private const float MaxSpeed = 15;

    [SerializeField]
    private float moveSpeed;

    [SerializeField]
    private float jumpSpeed;

    private bool grounded;
    private int jumpsRemaining;

    [SerializeField, HideInInspector]
    private Rigidbody2D rigidbody;

    [SerializeField, HideInInspector]
    private Collider2D collider;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        ProcessCollision();
        ProcessInput();
    }

    private void ProcessCollision()
    {
        CheckGrounded();
    }

    private void ProcessInput()
    {
        float inputHorizontal = Input.GetAxisRaw("Horizontal");
        bool inputJump = Input.GetButtonDown("Jump");

        if (!Mathf.Approximately(inputHorizontal, 0))
        {
            Move(inputHorizontal);
        }

        if (inputJump)
        {
            Jump();
        }
    }

    // Movement Behaviour
    private void Move(float input)
    {
        // Prevent additional force if going too much fast
        if (Mathf.Abs(rigidbody.velocity.x) > MaxSpeed) { return; }
        Vector2 force = new Vector2(input * moveSpeed, 0);
        rigidbody.AddForce(force, ForceMode2D.Force);
    }

    // Jump Behaviour
    private void Jump()
    {
        if (!grounded) { return; }
        Vector2 force = new Vector2(0, jumpSpeed);
        rigidbody.AddForce(force, ForceMode2D.Impulse);
    }

    // Checks if grounded or not
    private void CheckGrounded()
    {
        float rayDistance = 0.1f;
        Vector2 origin = new Vector2(transform.position.x, collider.bounds.min.y);

        RaycastHit2D[] collisions = Physics2D.RaycastAll(origin, Vector2.down, rayDistance);

        // Checks for solid ground collisions
        foreach (RaycastHit2D collision in collisions)
        {
            if (collision.collider == null) { continue; }
            if (collision.collider.isTrigger) { continue; }

            // Debug draw a red line indicating ground collision
            Debug.DrawLine(origin, new Vector3(origin.x, origin.y - rayDistance), Color.red);
            grounded = true;
            return;
        }

        // Debug draw a green line indicating no ground collision
        grounded = false;
        Debug.DrawLine(origin, new Vector3(origin.x, origin.y - rayDistance), Color.green);
    }
}
