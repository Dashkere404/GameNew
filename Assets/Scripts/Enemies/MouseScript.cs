using UnityEngine;

public class MouseScript : MonoBehaviour
{
    [Header("Start and end points")]
    public Transform pointA;
    public Transform pointB;
    public float patrolSpeed = 2f;

    [Header("Damage settings")]
    public int damage = 15;

    [Header("Bounce force settings")]
    public float bounceForce = 6f;

    private Transform currentPatrolTarget;
    private SpriteRenderer spriteRenderer;
    private Transform player;
    private Rigidbody2D rb;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (pointA != null && pointB != null)
        {
            currentPatrolTarget = pointB;
            transform.position = pointA.position;
        }
    }
    private void Update()
    {
        Patrol();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Access to object
        if (!collision.gameObject.CompareTag("Player")) return;

        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        Rigidbody2D playerRB = player.GetComponent<Rigidbody2D>();
        PlayerMovement controller = player.GetComponent<PlayerMovement>();

        if (playerHealth == null || rb == null) return;

        Collider2D playerCollider = player.GetComponent<Collider2D>();
        Collider2D enemyCollider = GetComponent<Collider2D>();

        if (playerCollider == null || enemyCollider == null) return;
        float playerBottom = playerCollider.bounds.min.y;
        float enemyTop = enemyCollider.bounds.max.y;
        bool isAttackingFromTop = playerBottom > enemyTop - 0.1f;

        if (isAttackingFromTop)
        {
            EnemiesHealth enemyHealth = GetComponent<EnemiesHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(1, true);
            }
            else
            {
                Destroy(gameObject);
            }
            playerRB.linearVelocity = new Vector2(playerRB.linearVelocity.x, bounceForce);
        }
        else
        {
            PlayerKnockback knockback = player.GetComponent<PlayerKnockback>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }

            if (knockback != null)
            {
                knockback.ApplyKnockback(transform.position);
            }
            Debug.Log($"MOUSE is attacking! - {damage} HP");
        }
    }
    
    void Patrol()
    {
        if (pointA == null || pointB == null || currentPatrolTarget == null) return;

        // Turning towards traffic
        float direction = Mathf.Sign(currentPatrolTarget.position.x - transform.position.x);
        rb.linearVelocity = new Vector2(direction * patrolSpeed, rb.linearVelocity.y);

        if (Mathf.Abs(direction) > 0.1f)
        {
            spriteRenderer.flipX = direction < 0;
        }

        if (Mathf.Abs(transform.position.x - currentPatrolTarget.position.x) < 0.2f)
        {
            currentPatrolTarget = (currentPatrolTarget == pointA) ? pointB : pointA;
        }
    }
}

