using UnityEngine;

public class PlayerMotionController : MonoBehaviour
{
    private Rigidbody2D playerBody;
    private CapsuleCollider2D capsule;
    private Camera gameCamera;

    public float moveSpeed = 8f;
    public float jumpHeight = 3f;              // base jump height in units
    public float jumpTime = 0.5f;              // time to reach apex in seconds
    public float doubleJumpMultiplier = 1.3f;  // second jump is 30% higher

    private Vector2 velocity;
    private float horizontalInput;
    private int jumpCount = 0;                 // counts consecutive jumps
    private bool isOnGround;

    // Gravity and jump force calculated from jumpHeight and jumpTime
    private float gravity => (-2f * jumpHeight) / Mathf.Pow(jumpTime, 2f);
    private float jumpingForce => Mathf.Sqrt(2f * Mathf.Abs(gravity) * jumpHeight);

    private void Awake()
    {
        playerBody = GetComponent<Rigidbody2D>();
        capsule = GetComponent<CapsuleCollider2D>();
        gameCamera = Camera.main;
    }

    private void Update()
    {
        // Horizontal movement
        horizontalInput = Input.GetAxis("Horizontal");
        velocity.x = Mathf.MoveTowards(velocity.x, horizontalInput * moveSpeed, moveSpeed * Time.deltaTime);

        // Ground check
        isOnGround = playerBody.Raycast(Vector2.down);

        if (isOnGround)
        {
            jumpCount = 0; // reset jump count when on ground
        }

        // Jump input
        if (Input.GetButtonDown("Jump") && (isOnGround || jumpCount < 1))
        {
            jumpCount++;
            float jumpForceToApply = jumpingForce;

            // Second jump is slightly higher
            if (jumpCount == 2) jumpForceToApply *= doubleJumpMultiplier;

            velocity.y = jumpForceToApply;
        }

        // Apply gravity
        if (!isOnGround)
        {
            velocity.y += gravity * Time.deltaTime;
        }
        else if (velocity.y < 0f)
        {
            velocity.y = 0f;
        }
    }

    private void FixedUpdate()
    {
        Vector2 position = playerBody.position;
        position += velocity * Time.fixedDeltaTime;

        // Keep within screen bounds
        Vector2 leftEdge = gameCamera.ScreenToWorldPoint(Vector2.zero);
        Vector2 rightEdge = gameCamera.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        position.x = Mathf.Clamp(position.x, leftEdge.x + 0.5f, rightEdge.x - 0.5f);

        playerBody.MovePosition(position);
    }

    private void OnCollisionEnter2D(Collision2D collision)
{
    // If player hits something from below (ceiling) → stop upward motion
    if (transform.DotTest(collision.transform, Vector2.up))
    {
        velocity.y = 0f;
    }
}

    
}
