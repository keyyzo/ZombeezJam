using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
    private bool facingRight = true;

    // Gun Components
    private GameObject gunOne;
    private GameObject gunTwo;
    private bool haveGunOne = false;
    private bool haveGunTwo = false;
    private bool firingGun;
    private float tempDirection = 1f;


    BaseWeapon baseWeapon;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        baseWeapon = GetComponent<BaseWeapon>();
    }

    // Start is called before the first frame update
    void Start()
    {
        playerInputHandler = PlayerInputHandler.Instance;
        isSlowWalking = false;
        gunOne = baseWeapon.gameObject;
        gunTwo = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (gunOne)
        {
            haveGunOne = true;
        }

        else
        {
            haveGunOne = false;
        }

        if (gunTwo)
        {
            haveGunTwo = true;
        }

        else
        {
            haveGunTwo = false;
        }

        if (facingRight)
        {
            tempDirection = 1f;
        }

        else
        {
            tempDirection = -1f;
        }


        InputCallHandler();
    }

    private void FixedUpdate()
    {
        ApplyMovement();

        if (shouldJump)
        { 
            ApplyJump();
        }

        if (firingGun)
        {
            FireGun1();
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
        firingGun = playerInputHandler.UseFireTriggered;

        if (horizontalInput != 0)
        {
            //FlipSprite(horizontalInput);
            NewCharacterFlip();
        }
    }

    private void FlipSprite(float horizontalMovement)
    {
        Vector3 bulletTempPos = transform.GetChild(0).gameObject.transform.localPosition;

        if (horizontalMovement < 0)
        {
            spriteRenderer.flipX = true;
            transform.GetChild(0).gameObject.transform.localPosition = new Vector3(-bulletTempPos.x, bulletTempPos.y, bulletTempPos.z);
        }

        else if (horizontalMovement > 0)
        {
            spriteRenderer.flipX = false;
            transform.GetChild(0).gameObject.transform.localPosition = new Vector3(bulletTempPos.x, bulletTempPos.y, bulletTempPos.z);
        }


    }

    void NewCharacterFlip()
    {
        if ((horizontalInput < 0 && facingRight) || (horizontalInput > 0 && !facingRight))
        {
            facingRight = !facingRight;
            transform.Rotate(new Vector3(0, 180, 0));
        }
    }

    void FireGun1()
    {
        
        
        Debug.Log("Gun One Fired!");
        gunOne.GetComponent<BaseWeapon>().FireWeapon(tempDirection);
        
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
