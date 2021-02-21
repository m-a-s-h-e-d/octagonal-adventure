using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    private const float MoveSpeedMultiplier = 10;
    private const float MaxSpeed = 15;

    [SerializeField]
    private float moveSpeed = 0;

    [SerializeField]
    private float jumpSpeed = 0;

    private bool grounded = false;

    private float inputHorizontal;
    private float inputVertical;
    private bool inputJump;
    private bool inputFire;

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
        GetInput();
        ProcessCollision();
        ProcessInputFrame();
    }
    
    private void FixedUpdate()
    {
        ProcessInputFixed();
    }

    private void OnDestroy()
    {
        if (grapple != null)
        {
            Destroy(grapple);
        }
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

    // Gets called every fixed frame when move input is not zero
    private void Move(float input)
    {
        // Prevent additional force if going too much fast
        if (Mathf.Abs(rigidbody.velocity.x) > MaxSpeed) { return; }

        Vector2 force = new Vector2(input * moveSpeed * MoveSpeedMultiplier, 0);
        rigidbody.AddForce(force , ForceMode2D.Force);
    }

    private void ChangeGrappleDistance(float input)
    {
        if (grapple == null) { return; }
        grapple.ChangeDistance(input);
    }

    // Gets called on jump input
    private void Jump()
    {
        // Prevent jump if not grounded
        if (!grounded) { return; }

        Vector2 force = new Vector2(0, jumpSpeed);
        rigidbody.AddForce(force, ForceMode2D.Impulse);
    }

    // Gets called on fire input
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

    private void GetInput()
    {
        inputHorizontal = Input.GetAxisRaw("Horizontal");
        inputVertical = Input.GetAxisRaw("Vertical");
        inputJump = Input.GetButtonDown("Jump");
        inputFire = Input.GetMouseButtonDown(0);
    }

    private void ProcessInputFrame()
    {
        if (inputJump)
        {
            Jump();
        }

        if (inputFire)
        {
            Fire();
        }
    }

    private void ProcessInputFixed()
    {
        if (!Mathf.Approximately(inputHorizontal, 0))
        {
            Move(inputHorizontal);
        }

        if (!Mathf.Approximately(inputVertical, 0))
        {
            ChangeGrappleDistance(inputVertical);
        }
    }

    // Checks if grounded or not
    private void CheckGrounded()
    {
        float rayDistance = 0.1f;

        Vector2 originLeft = new Vector2(transform.position.x - GetBoundsOffset(), collider.bounds.min.y);
        Vector2 originRight = new Vector2(transform.position.x + GetBoundsOffset(), collider.bounds.min.y);

        RaycastHit2D[] collisions = Physics2D.RaycastAll(originLeft, Vector2.down, rayDistance)
            .Concat(Physics2D.RaycastAll(originRight, Vector2.down, rayDistance))
            .ToArray();

        Debug.DrawLine(originLeft, new Vector3(originLeft.x, originLeft.y - rayDistance), Color.green);
        Debug.DrawLine(originRight, new Vector3(originRight.x, originRight.y - rayDistance), Color.green);

        // Checks for solid ground collisions
        foreach (RaycastHit2D collision in collisions)
        {
            if (collision.collider == null) { continue; }
            if (collision.collider.isTrigger) { continue; }

            if (!grounded)
            {
                grounded = true;
                OnGrounded();
            }

            return;
        }

        // No collisions have been found

        if (grounded)
        {
            grounded = false;
            OnAirborne();
        }

        // Used to offset the 2 raycasts by 15% of the collider size in the x dimension
        float GetBoundsOffset()
        {
            const float RelativeAmount = 0.15f;
            return collider.bounds.size.x * RelativeAmount;
        }
    }
}
