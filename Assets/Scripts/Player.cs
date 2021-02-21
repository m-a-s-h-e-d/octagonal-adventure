using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private const float MaxSpeed = 15;

    [SerializeField]
    private float moveSpeed = 0;

    [SerializeField]
    private float jumpSpeed = 0;

    private bool grounded;

    [SerializeField, HideInInspector]
    private new Rigidbody2D rigidbody;

    [SerializeField, HideInInspector]
    private new Collider2D collider;

    private Grapple grapple;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
    }

    private void Start()
    {
        Game.InstantiatePrefab("Crosshair");
    }
    
    private void Update()
    {
        ProcessCollision();
        ProcessInput();
    }

    // Gets called on first frame when grounded
    private void OnGrounded()
    {
        // Prevents character from sliding on ground
        collider.sharedMaterial.friction = 1;
        collider.enabled = false;
        collider.enabled = true;
    }

    // Gets called on first frame when not grounded
    private void OnAirborne()
    {
        // Prevents character from sticking to walls
        collider.sharedMaterial.friction = 0;
        collider.enabled = false;
        collider.enabled = true;
    }

    // Gets called every frame when move input is not zero
    private void Move(float input)
    {
        // Prevent additional force if going too much fast
        if (Mathf.Abs(rigidbody.velocity.x) > MaxSpeed) { return; }

        Vector2 force = new Vector2(input * moveSpeed, 0);
        rigidbody.AddForce(force, ForceMode2D.Force);
    }

    // Gets called on jump input
    private void Jump()
    {
        // Prevent jump if not grounded
        if (!grounded) { return; }

        Vector2 force = new Vector2(0, jumpSpeed);
        rigidbody.AddForce(force, ForceMode2D.Impulse);
    }

    private void Fire()
    {
        if (grapple == null)
        {
            grapple = Game.InstantiatePrefab("Grapple", transform.position, transform).GetComponent<Grapple>();
        }
        else
        {
            Destroy(grapple.gameObject);
        }
    }

    private void ProcessCollision()
    {
        CheckGrounded();
    }

    private void ProcessInput()
    {
        float inputHorizontal = Input.GetAxisRaw("Horizontal");
        bool inputJump = Input.GetButtonDown("Jump");
        bool inputFire = Input.GetMouseButtonDown(0);

        if (!Mathf.Approximately(inputHorizontal, 0))
        {
            Move(inputHorizontal);
        }

        if (inputJump)
        {
            Jump();
        }

        if (inputFire)
        {
            Fire();
        }
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

            Debug.DrawLine(origin, new Vector3(origin.x, origin.y - rayDistance), Color.red);

            if (!grounded)
            {
                grounded = true;
                OnGrounded();
            }

            return;
        }

        // No collisions have been found

        Debug.DrawLine(origin, new Vector3(origin.x, origin.y - rayDistance), Color.green);

        if (grounded)
        {
            grounded = false;
            OnAirborne();
        }
    }
}
