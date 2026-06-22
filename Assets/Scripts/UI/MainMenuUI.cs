using UnityEngine;
using UnityEngine.SceneManagement;

// Кнопки главного меню: "Играть" и "Выход".
public class MainMenuUI : MonoBehaviour
{
    // Вызывается кнопкой "ИГРАТЬ"
    public void PlayGame()
    {
        if (SceneTransition.Instance != null)
        {
            SceneTransition.Instance.LoadScene(UIScenes.Level1);
        }
        else
        {
            // Фолбэк, если SceneTransition ещё не добавлен в сцену
            SceneManager.LoadScene(UIScenes.Level1);
        }
    }

    // Вызывается кнопкой "ВЫХОД"
    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Выход из игры"); // в редакторе Quit не работает, это для теста
    }
}
