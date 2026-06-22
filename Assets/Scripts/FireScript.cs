using UnityEngine;

public class FireScript : MonoBehaviour
{
    public int damage = 30;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
        PlayerKnockback knockback = other.GetComponent<PlayerKnockback>();

        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
        }

        if (knockback != null)
        {
            knockback.ApplyKnockback(transform.position);
        }
        Debug.Log($"PLAYER got burned! - {damage} HP");
    }
}