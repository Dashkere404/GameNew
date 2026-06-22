using UnityEngine;
public class KillZoneScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        EnemiesHealth enemyHealth = other.GetComponent<EnemiesHealth>();
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

        if (enemyHealth != null)
        {
            enemyHealth.Die();
            Debug.Log($"The enemy {other.name} fell into the abyss!");
        }
        else if (playerHealth != null)
        {
            playerHealth.Die();
            Debug.Log("Player fell into the abyss!");
        }
        Destroy(other.gameObject);
    }
}
