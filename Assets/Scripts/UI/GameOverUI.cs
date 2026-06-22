using UnityEngine;
using UnityEngine.SceneManagement;

// Экран "Game Over": "Заново" / "В меню".
// Перед переходом на эту сцену можно вызвать SetLevelToRestart(levelName),
// иначе по умолчанию используется Level1.
public class GameOverUI : MonoBehaviour
{
    // Имя уровня, на который нужно вернуться при "Заново".
    // Выставляется снаружи перед переходом на GameOver.
    private static string pendingLevelName = UIScenes.Level1;

    public static void SetLevelToRestart(string levelName)
    {
        pendingLevelName = levelName;
    }

    // Вызывается кнопкой "Заново"
    public void RestartLevel()
    {
        string levelName = !string.IsNullOrEmpty(pendingLevelName) ? pendingLevelName : UIScenes.Level1;

        if (SceneTransition.Instance != null)
        {
            SceneTransition.Instance.LoadScene(levelName);
        }
        else
        {
            SceneManager.LoadScene(levelName);
        }
    }

    // Вызывается кнопкой "В меню"
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
