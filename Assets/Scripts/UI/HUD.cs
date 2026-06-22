using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    private int score = 0;

    [Header("Жизни (опционально)")]
    public TextMeshProUGUI livesText;
    private int lives = 3;

    private PlayerHealth playerHealth;

    void Start()
    {
        playerHealth = FindFirstObjectByType<PlayerHealth>();

        if (playerHealth != null)
        {
            playerHealth.OnHealthChanged += UpdateHealthBar;
            UpdateHealthBar(playerHealth.CurrentHealth, playerHealth.MaxHealth);
        }

        UpdateScoreText();
        UpdateLivesText();
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
        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = (float)current / max;
        }
    }

    // Вызывается из GameManager, когда счёт меняется
    public void SetScore(int newScore)
    {
        score = newScore;
        UpdateScoreText();
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreText();
    }

    public void SetLives(int newLives)
    {
        lives = newLives;
        UpdateLivesText();
    }

    void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Очки: " + score;
        }
    }

    void UpdateLivesText()
    {
        if (livesText != null)
        {
            livesText.text = "Жизни: " + lives;
        }
    }
}
