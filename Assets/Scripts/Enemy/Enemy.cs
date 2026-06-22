using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int damage = 10; // Урон при столкновении

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // При столкновении с игроком
        if (collision.gameObject.CompareTag("Player"))
        {
            // Получаем компонент здоровья игрока
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }
    }

    // Альтернатива: если используешь Trigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }
    }
}
