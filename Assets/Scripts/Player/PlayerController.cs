using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Parameters")]
    [SerializeField] private float generalWalkSpeed = 10.0f;
    [SerializeField] private float walkAccelerationSpeed = 0.1f;
    [SerializeField] private float walkDecelerationSpeed = 0.5f;
    [SerializeField] private float slowWalkSpeed = 5.0f;
    [SerializeField] private float slowWalkAccelerationSped = 0.05f;
    private bool isSlowWalking;

    [Header("Ladder Parameters")]
    [SerializeField] private float ladderSpeed = 4.0f;
    private bool isOnLadder;

    [Header("Jump Parameters")]
    [SerializeField] private float jumpForce = 5.0f;
    [SerializeField] private float jumpMoveSpeed = 0.1f;
    private bool isGrounded;
    private bool shouldJump;

    // Player Components
    private Rigidbody2D rb;
    private CapsuleCollider2D capsuleCollider;
    private PlayerInputHandler playerInputHandler;
    private SpriteRenderer spriteRenderer;

    // Specific Movement Components
    private float horizontalInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        playerInputHandler = PlayerInputHandler.Instance;
        isSlowWalking = false;
    }

    // Update is called once per frame
    void Update()
    {
     
        InputCallHandler();
    }

    private void FixedUpdate()
    {
        ApplyMovement();

        if (shouldJump)
        { 
            ApplyJump();
        }
    }

    void ApplyMovement()
    {
        float walkHorizontalSpeedIncrement = horizontalInput * walkAccelerationSpeed;
        float newWalkHorizontalSpeed = Mathf.Clamp(rb.velocity.x + walkHorizontalSpeedIncrement, -generalWalkSpeed, generalWalkSpeed);

        float slowWalkHorizontalSpeedIncrement = horizontalInput * slowWalkAccelerationSped;
        float newSlowWalkHorizontalSpeed = Mathf.Clamp(rb.velocity.x + slowWalkHorizontalSpeedIncrement, -slowWalkSpeed, slowWalkSpeed);

        float speed;

        if (isSlowWalking)
        {
            Debug.Log("Slow Walk Active");
            speed = newSlowWalkHorizontalSpeed;
        }

        else
        {
            speed = newWalkHorizontalSpeed;
        }

        rb.velocity = new Vector2(speed, rb.velocity.y);

        if (horizontalInput == 0 && isGrounded)
        {
            rb.velocity = Vector2.MoveTowards(new Vector2(rb.velocity.x, rb.velocity.y), new Vector2(0.0f, rb.velocity.y), walkDecelerationSpeed);
        }

        else if (horizontalInput == 0 && isGrounded == false)
        {
            rb.velocity = Vector2.MoveTowards(new Vector2(rb.velocity.x, rb.velocity.y), new Vector2(0.0f, rb.velocity.y), jumpMoveSpeed);
        }
    }

    void ApplyJump()
    {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        isGrounded = false;
        shouldJump = false;
    }

    private void InputCallHandler()
    {
        horizontalInput = playerInputHandler.MoveInput.x;
        shouldJump = playerInputHandler.UseJumpTriggered && isGrounded;
        isSlowWalking = playerInputHandler.UseSlowWalkTriggered;

        if (horizontalInput != 0)
        {
            FlipSprite(horizontalInput);
        }
    }

    private void FlipSprite(float horizontalMovement)
    {
        if (horizontalMovement < 0)
        {
            spriteRenderer.flipX = true;
        }

        else if (horizontalMovement > 0)
        {
            spriteRenderer.flipX = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            rb.velocity = new Vector2(rb.velocity.x, 0.0f);
        }
    }
}
