using UnityEngine;
using UnityEngine.SceneManagement;

// Меню паузы: Escape открывает/закрывает, "Продолжить" / "В меню".
public class PauseMenu : MonoBehaviour
{
    [Header("Панель паузы (скрыта по умолчанию)")]
    public GameObject pausePanel;

    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void PauseGame()
    {
        if (pausePanel != null)
        {
            pausePanel.SetActive(true);
        }
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f; // обязательно сбросить время перед сменой сцены!

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
