using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Игровые данные")]
    public int score = 0;
    public int lives = 3;
    public int maxLives = 3;
    public string currentLevel;
    public HUD hud;

    public void StartGame()
    {
        score = 0;
        lives = maxLives;

        Debug.Log("StartGame: lives = " + lives);

        if (hud != null)
        {
            hud.SetLives(lives);
        }
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Debug.Log("GameManager работает");
        FindHUD();
    }

    void FindHUD()
    {
        hud = FindFirstObjectByType<HUD>();

        if (hud != null)
        {
            hud.SetLives(lives);
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        currentLevel = scene.name;

        FindHUD();

    }

    void OnEnable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void LoseLife()
    {
        lives--;

        if (hud != null)
            hud.SetLives(lives);

        Debug.Log("Lives: " + lives);

        if (lives <= 0)
        {
            if (SceneTransition.Instance != null)
                SceneTransition.Instance.LoadScene(UIScenes.GameOver);
            else
                SceneManager.LoadScene(UIScenes.GameOver);
        }
        else
        {
            SceneManager.LoadScene(currentLevel);
        }
    }

    public void ResetLives()
    {
        lives = maxLives;
        Debug.Log("Lives reset: " + lives);

        if (hud != null)
            hud.SetLives(lives);
    }

    public void AddScore(int amount)
    {
        score += amount;
        Debug.Log("Score: " + score);
    }

    public void TakeDamage(int amount)
    {
        lives -= amount;

        if (lives <= 0)
        {
            lives = 0;
            Debug.Log("Game Over");
        }
    }

    public void Heal(int amount)
    {
        lives += amount;

        if (lives > maxLives)
            lives = maxLives;
    }
}