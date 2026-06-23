using UnityEngine;

public class EnemiesHealth : MonoBehaviour
{
    [Header("Health settings")]
    public int maxHealth = 3;
    public int currentHealth;

    [Header("Effects")]
    public float flashDuration = 0.1f; 
    public float invincibilityTime = 1f;
    private bool isInvincible = false;

    [Header("Score settings")]
    public int pointsOnDeath = 10;

    private SpriteRenderer sprite;
    private bool killedByPlayer = false;

    private void Start()
    {
        currentHealth = maxHealth;
        sprite = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(int damage, bool byPlayer = false)
    {
        if (isInvincible) return;
        currentHealth -= damage;
        if (byPlayer)
        {
            killedByPlayer = true;
        }
        if (sprite != null)
        {
            StartCoroutine(Flash());
        }

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
        Debug.Log($"The {gameObject.name} has taken damage. Current health: {currentHealth}");
    }

    System.Collections.IEnumerator Flash()
    {
        isInvincible = true;
        Color original = sprite.color;

        if (sprite != null)
        {
            for (float i = 0; i < invincibilityTime; i += 0.2f)
            {
                sprite.color = Color.red;
                yield return new WaitForSeconds(flashDuration);
                sprite.color = original;

            }
            sprite.enabled = true;
        }
        else
        {
            yield return new WaitForSeconds(invincibilityTime);
        }

        isInvincible = false;
    }

    public void Die()
    {
        if (killedByPlayer && ScoreManager.Instance != null)
        {
            ScoreManager.Instance.AddPoints(pointsOnDeath);
            Debug.Log("+" + pointsOnDeath + " points");
        }
        else
        {
            Debug.Log($"{gameObject.name} is died!");
        }
        Destroy(gameObject);
    }
}