using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

// Экран победы: итоговый счёт, кнопка "В меню".
// Финальный счёт выставляется через SetFinalScore(int) перед загрузкой сцены.
public class WinUI : MonoBehaviour
{
    [Header("Текст с финальным счётом")]
    public TextMeshProUGUI finalScoreText;

    private static int pendingFinalScore = 0;

    public static void SetFinalScore(int score)
    {
        pendingFinalScore = score;
    }

    void Start()
    {
        if (finalScoreText != null)
        {
            finalScoreText.text = "Ваш счёт: " + pendingFinalScore;
        }
    }

    public void GoToMainMenu()
    {
        if (SceneTransition.Instance != null)
        {
            SceneTransition.Instance.LoadScene(UIScenes.MainMenu);
        }
        else
        {
            SceneManager.LoadScene(UIScenes.MainMenu);
        }
    }
}
