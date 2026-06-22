using UnityEngine;

public class CannedFishScript : MonoBehaviour
{
    [Header("Heal settings")]
    public int healAmount = 20;

    [Header("Effects")]
    public bool destroyOnPickup = true;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

        if (playerHealth != null)
        {
            playerHealth.Heal(healAmount);
            Debug.Log($"PLAYER eat canned fish! + {healAmount} HP");
            if (destroyOnPickup)
            {
                Destroy(gameObject);
            }
        }
    }

}

