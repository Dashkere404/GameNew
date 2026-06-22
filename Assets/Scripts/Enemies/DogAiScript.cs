using UnityEngine;
using UnityEngine.InputSystem.XR;

public class DogAiScript : MonoBehaviour
{
    [Header("Start and end points")]
    public Transform pointA;
    public Transform pointB;
    public float patrolSpeed = 2f;

    [Header("Detection settings")]
    public float detectionRange = 5f;      
    public float chaseSpeed = 5f;        

    [Header("Damage settings")]
    public int damage = 30;

    [Header("Bounce force settings")]
    public float bounceForce = 3f;

    private Transform player;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private bool isChasing = false;
    private Transform currentPatrolTarget;
    private float lastAttackTime = 0f;
    public float attackCooldown = 0.5f;

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

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Check distance to player
        if (distanceToPlayer <= detectionRange)
        {
            isChasing = true;
        }

        // If find player
        if (isChasing)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }
    }

    void Patrol()
    {
        if (pointA == null || pointB == null || currentPatrolTarget == null) return;

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

    void ChasePlayer()
    {
        float direction = Mathf.Sign(player.position.x - transform.position.x);
        rb.linearVelocity = new Vector2(direction * chaseSpeed, rb.linearVelocity.y);
        spriteRenderer.flipX = direction < 0;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;
        if (Time.time < lastAttackTime + attackCooldown) return;
        lastAttackTime = Time.time;
        Attack(collision);
    }
    void Attack(Collision2D collision)
    {
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        Rigidbody2D playerRB = player.GetComponent<Rigidbody2D>();
        PlayerMovement controller = player.GetComponent<PlayerMovement>();

        if (playerHealth == null || playerRB == null) return;

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
            Debug.Log($"DOG is attacking! - {damage} HP");
        }

    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        if (pointA != null && pointB != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(pointA.position, pointB.position);
        }
    }
}