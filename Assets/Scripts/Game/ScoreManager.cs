using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    private int score = 0;
    public TextMeshProUGUI scoreText;
    private HUD hud;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        hud = FindFirstObjectByType<HUD>();
    }

    public void AddPoints(int points)
    {
        Debug.Log("ScoreManager получил: " + points);

        score += points;

        if (scoreText != null)
            scoreText.text = "Очки: " + score;
    }

    public void ResetScore()
    {
        score = 0;

        if (hud != null)
        {
            hud.SetScore(score);
        }
    }
}
