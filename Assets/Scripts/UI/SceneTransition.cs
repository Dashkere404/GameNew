using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

// Синглтон для плавных переходов между сценами (fade in/out).
// Живёт между сценами через DontDestroyOnLoad.
public class SceneTransition : MonoBehaviour
{
    public static SceneTransition Instance;

    [Header("Чёрная панель на весь экран (Image)")]
    public Image fadePanel;

    [Header("Длительность затемнения, сек")]
    public float fadeDuration = 0.5f;

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

    // Загрузить сцену с плавным переходом
    public void LoadScene(string sceneName)
    {
        // Если панель не назначена - грузим сцену сразу, без анимации
        if (fadePanel == null)
        {
            Debug.LogWarning("SceneTransition: fadePanel не назначен, переход без затемнения.");
            SceneManager.LoadScene(sceneName);
            return;
        }

        StartCoroutine(FadeAndLoad(sceneName));
    }

    IEnumerator FadeAndLoad(string sceneName)
    {
        yield return StartCoroutine(Fade(0f, 1f));
        SceneManager.LoadScene(sceneName);
        yield return StartCoroutine(Fade(1f, 0f));
    }

    IEnumerator Fade(float from, float to)
    {
        float elapsed = 0f;
        Color c = fadePanel.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            c.a = Mathf.Lerp(from, to, elapsed / fadeDuration);
            fadePanel.color = c;
            yield return null;
        }

        c.a = to;
        fadePanel.color = c;
    }
}
