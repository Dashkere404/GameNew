using UnityEngine;

public class BirdScript : MonoBehaviour
{
    [Header("Start and end points")]
    public Transform pointA;
    public Transform pointB;
    public float patrolSpeed = 2f;

    [Header("Damage settings")]
    public int damage = 30;

    [Header("Bounce force settings")]
    public float bounceForce = 7f;

    private Transform currentPatrolTarget;
    private SpriteRenderer spriteRenderer;
    private Transform player;


    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
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
        if (!collision.gameObject.CompareTag("Player")) return;

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
            Debug.Log($"BIRD is attacking! - {damage} HP");
        }
    }

    void Patrol()
    {
        if (pointA == null || pointB == null || currentPatrolTarget == null) return;
        transform.position = Vector2.MoveTowards(transform.position, currentPatrolTarget.position, patrolSpeed * Time.deltaTime);

        // Turning towards traffic
        float direction = currentPatrolTarget.position.x - transform.position.x;
        if (Mathf.Abs(direction) > 0.1f)
        {
            spriteRenderer.flipX = direction < 0;
        }

        if (Vector2.Distance(transform.position, currentPatrolTarget.position) < 0.2f)
        {
            currentPatrolTarget = (currentPatrolTarget == pointA) ? pointB : pointA;
        }
    }
}
