using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 12f;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.15f;
    [SerializeField] private LayerMask groundLayer = ~0;

    private Rigidbody2D rb;
    private Collider2D ownCollider;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private float horizontalInput;
    private bool jumpRequested;
    private bool isGrounded;

    public bool IsGrounded => isGrounded;
    public float HorizontalInput => horizontalInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        ownCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        animator.SetBool("isRunning", horizontalInput != 0);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            jumpRequested = true;
        }

        FlipSprite();
        UpdateAnimator();
    }

    private void FixedUpdate()
    {
        CheckGrounded();

        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);

        if (jumpRequested)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpRequested = false;
        }
    }

    private void CheckGrounded()
    {
        Vector2 checkPosition = GetGroundCheckPosition();
        Collider2D[] hits = Physics2D.OverlapCircleAll(checkPosition, groundCheckRadius, groundLayer);

        isGrounded = false;

        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i] != null && hits[i] != ownCollider)
            {
                isGrounded = true;
                return;
            }
        }
    }

    private Vector2 GetGroundCheckPosition()
    {
        if (groundCheck != null)
        {
            return groundCheck.position;
        }

        if (ownCollider != null)
        {
            Bounds bounds = ownCollider.bounds;
            return new Vector2(bounds.center.x, bounds.min.y);
        }

        return transform.position;
    }

    private void FlipSprite()
    {
        if (spriteRenderer == null || Mathf.Approximately(horizontalInput, 0f))
        {
            return;
        }

        spriteRenderer.flipX = horizontalInput < 0f;
    }

    private void UpdateAnimator()
    {
        if (animator == null)
        {
            return;
        }

        SetAnimatorFloat("Speed", Mathf.Abs(horizontalInput));
        SetAnimatorFloat("VerticalSpeed", rb.linearVelocity.y);
        SetAnimatorBool("IsGrounded", isGrounded);
    }

    private void SetAnimatorFloat(string parameterName, float value)
    {
        if (HasAnimatorParameter(parameterName, AnimatorControllerParameterType.Float))
        {
            animator.SetFloat(parameterName, value);
        }
    }

    private void SetAnimatorBool(string parameterName, bool value)
    {
        if (HasAnimatorParameter(parameterName, AnimatorControllerParameterType.Bool))
        {
            animator.SetBool(parameterName, value);
        }
    }

    private bool HasAnimatorParameter(string parameterName, AnimatorControllerParameterType parameterType)
    {
        for (int i = 0; i < animator.parameters.Length; i++)
        {
            AnimatorControllerParameter parameter = animator.parameters[i];

            if (parameter.name == parameterName && parameter.type == parameterType)
            {
                return true;
            }
        }

        return false;
    }

    private void OnDrawGizmosSelected()
    {
        Vector2 checkPosition = groundCheck != null ? groundCheck.position : transform.position;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(checkPosition, groundCheckRadius);
    }
}