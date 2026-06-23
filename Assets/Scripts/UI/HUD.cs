using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

// HUD во время игры: полоска HP, счёт очков и жизни.
// HP обновляется через событие PlayerHealth.OnHealthChanged.
// Счёт и жизни обновляются через SetScore/AddScore/SetLives, когда
// появится GameManager.
public class HUD : MonoBehaviour
{
    [Header("HP")]
    public Image healthBarFill;

    [Header("Счёт")]
    public TextMeshProUGUI scoreText;

    [Header("Жизни (опционально)")]
    public TextMeshProUGUI livesText;

    private PlayerHealth playerHealth;

    public void SetLives(int newLives)
    {
        if (livesText != null)
            livesText.text = "Жизни: " + newLives;
    }

    void OnEnable()
    {
        StartCoroutine(Init());
    }

    IEnumerator Init()
    {
        yield return new WaitForSeconds(0.05f);

        playerHealth = FindFirstObjectByType<PlayerHealth>();

        if (playerHealth != null)
        {
            playerHealth.OnHealthChanged += UpdateHealthBar;
            UpdateHealthBar(playerHealth.CurrentHealth, playerHealth.MaxHealth);
        }
    }

    void OnDisable()
    {
        if (playerHealth != null)
        {
            playerHealth.OnHealthChanged -= UpdateHealthBar;
        }
    }

    void OnDestroy()
    {
        if (playerHealth != null)
        {
            playerHealth.OnHealthChanged -= UpdateHealthBar;
        }
    }

    void UpdateHealthBar(int current, int max)
    {
        Debug.Log($"HUD HP update: {current}/{max}");
        Debug.Log("FILL = " + healthBarFill.fillAmount);

        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = (float)current / max;
        }
    }

    public void SetScore(int newScore)
    {
        if (scoreText != null)
            scoreText.text = "Очки: " + newScore;
    }
}
