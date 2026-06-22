using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    private int score = 0;
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
        score += points;

        if (hud != null)
        {
            hud.SetScore(score);
        }
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
