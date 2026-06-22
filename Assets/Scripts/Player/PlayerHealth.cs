using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth;

    [Header("Damage Feedback")]
    [SerializeField] private float invulnerabilityDuration = 1f;
    [SerializeField] private float flashInterval = 0.1f;
    [SerializeField] private float deathDisableDelay = 0.5f;

    private SpriteRenderer spriteRenderer;
    private Collider2D playerCollider;
    private Rigidbody2D rb;
    private PlayerMovement movement;
    private Animator animator;
    private bool isInvulnerable;
    private bool isDead;

    public event Action<int, int> OnHealthChanged;
    public event Action OnDied;

    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;
    public bool IsDead => isDead;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerCollider = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        movement = GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        currentHealth = Mathf.Clamp(currentHealth <= 0 ? maxHealth : currentHealth, 0, maxHealth);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void TakeDamage(int damage)
    {
        if (damage <= 0 || isInvulnerable || isDead)
        {
            return;
        }

        currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
            return;
        }

        TriggerAnimator("Hurt");
        StartCoroutine(InvulnerabilityRoutine());
    }

    public void Heal(int amount)
    {
        if (amount <= 0 || isDead)
        {
            return;
        }

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void ResetHealth()
    {
        isDead = false;
        isInvulnerable = false;
        currentHealth = maxHealth;

        if (movement != null)
        {
            movement.enabled = true;
        }

        if (playerCollider != null)
        {
            playerCollider.enabled = true;
        }

        if (rb != null)
        {
            rb.simulated = true;
        }

        spriteRenderer.enabled = true;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    private IEnumerator InvulnerabilityRoutine()
    {
        isInvulnerable = true;
        float elapsed = 0f;

        while (elapsed < invulnerabilityDuration)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(flashInterval);
            elapsed += flashInterval;
        }

        spriteRenderer.enabled = true;
        isInvulnerable = false;
    }

    public void Die()
    {
        if (isDead)
        {
            return;
        }

        isDead = true;
        TriggerAnimator("Death");
        OnDied?.Invoke();
        GameManager.Instance.LoseLife();

        if (movement != null)
        {
            movement.enabled = false;
        }

        if (playerCollider != null)
        {
            playerCollider.enabled = false;
        }

        StartCoroutine(DisableAfterDeathRoutine());
    }

    private IEnumerator DisableAfterDeathRoutine()
    {
        yield return new WaitForSeconds(deathDisableDelay);

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.simulated = false;
        }

        spriteRenderer.enabled = true;
    }

    private void TriggerAnimator(string triggerName)
    {
        if (animator == null || !HasAnimatorParameter(triggerName, AnimatorControllerParameterType.Trigger))
        {
            return;
        }

        animator.SetTrigger(triggerName);
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
}
