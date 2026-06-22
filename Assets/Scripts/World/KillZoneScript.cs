using UnityEngine;
public class KillZoneScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

        if (playerHealth != null)
        {
            Debug.Log("Player fell into the abyss!");
            playerHealth.Die();
            return;
        }

        EnemiesHealth enemyHealth = other.GetComponent<EnemiesHealth>();

        if (enemyHealth != null)
        {
            enemyHealth.Die();
            Debug.Log($"Enemy {other.name} fell into the abyss!");
        }
    }
}
