using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Mouvement")]
    public float speed = 5f;
    public float sprintSpeed = 10f;
    public float jumpForce = 10f;
    public float dashForce = 15f;
    [SerializeField] private DialogueManager dialogueManager;

    [Header("Saut")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float jumpDelay = 0.08f;

    [Header("Plateforme traversable")]
    [SerializeField] private float fallThroughTime = 0.75f;

    [Header("Knockback")]
    [SerializeField] private float knockbackForce = 8f;
    [SerializeField] private float knockbackDuration = 0.2f;

    private bool isKnockedBack = false;

    private Rigidbody2D rb;
    private Animator animator;
    private Collider2D playerCollider;

    private bool isGrounded;
    private bool isPreparingJump = false;
    private bool isDroppingThroughPlatform = false;

    private int jumpCount = 0;
    private int maxJumps = 2;

    private Vector3 originalScale;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerCollider = GetComponent<Collider2D>();
        originalScale = transform.localScale;
    }

    void Update()
    {
        if (isKnockedBack) return;

        isGrounded = CheckGrounded();

        Move();
        Jump();
        Dash();
        Crouch();

        UpdateAnimations();
    }

    void Move()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");

        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : speed;

        rb.linearVelocity = new Vector2(moveInput * currentSpeed, rb.linearVelocity.y);
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < maxJumps && !isPreparingJump)
        {
            if (!isGrounded && jumpCount == 0)
            {
                jumpCount = 1;
            }

            StartCoroutine(PrepareJump());
        }
    }

    bool CheckGrounded()
    {
        return Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );
    }

    void Dash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            float direction = Input.GetAxisRaw("Horizontal");

            if (direction != 0)
            {
                rb.linearVelocity = new Vector2(direction * dashForce, rb.linearVelocity.y);
            }
        }
    }

    void Crouch()
    {
        if (Input.GetKeyDown(KeyCode.S) && isGrounded)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1f, groundLayer);

            if (hit.collider != null && hit.collider.CompareTag("OneWayPlatform"))
            {
                StartCoroutine(FallThroughPlatform(hit.collider));
            }
        }
    }

    void UpdateAnimations()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");

        if (isDroppingThroughPlatform)
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);
            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", true);
            return;
        }

        animator.SetBool("isWalking", moveInput != 0 && isGrounded);
        animator.SetBool("isRunning", moveInput != 0 && Input.GetKey(KeyCode.LeftShift) && isGrounded);

        if (!isPreparingJump)
        {
            animator.SetBool("isJumping", !isGrounded);

            if (isGrounded)
            {
                animator.SetBool("isFalling", false);
            }
            else
            {
                animator.SetBool("isFalling", rb.linearVelocity.y < -0.1f);
            }
        }

        if (moveInput > 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        }
        else if (moveInput < 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        }
    }

    IEnumerator FallThroughPlatform(Collider2D platformCollider)
    {
        isDroppingThroughPlatform = true;

        animator.SetBool("isWalking", false);
        animator.SetBool("isRunning", false);
        animator.SetBool("isJumping", false);
        animator.SetBool("isFalling", true);

        animator.Play("JumpFall", 0, 0f);

        Physics2D.IgnoreCollision(playerCollider, platformCollider, true);

        yield return new WaitForSeconds(fallThroughTime);

        Physics2D.IgnoreCollision(playerCollider, platformCollider, false);

        isDroppingThroughPlatform = false;
    }

    private IEnumerator PrepareJump()
    {
        isPreparingJump = true;

        animator.SetBool("isJumping", false);
        animator.SetBool("isFalling", false);

        animator.Play("JumpStart", 0, 0f);

        yield return new WaitForSeconds(jumpDelay);

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);

        jumpCount++;

        isPreparingJump = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            jumpCount = 0;
        }

        if (collision.gameObject.CompareTag("OneWayPlatform"))
        {
            if (rb.linearVelocity.y <= 0.1f)
            {
                jumpCount = 0;
            }
        }
    }

    public void ApplyKnockback(Vector2 sourcePosition)
    {
        StartCoroutine(KnockbackCoroutine(sourcePosition));
    }

    IEnumerator KnockbackCoroutine(Vector2 sourcePosition)
    {
        isKnockedBack = true;

        Vector2 direction = ((Vector2)transform.position - sourcePosition).normalized;

        rb.linearVelocity = Vector2.zero;
        rb.AddForce(direction * knockbackForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(knockbackDuration);

        isKnockedBack = false;
    }
}