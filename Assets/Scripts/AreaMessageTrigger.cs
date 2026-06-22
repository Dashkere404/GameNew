using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AreaMessageTrigger : MonoBehaviour
{
    [Header("Сообщение")]
    public GameObject messageText; // UI текст (перетащить из Hierarchy)
    public float displayTime = 3f; // сколько секунд показывать
    public string message = "УБЕЙ ВСЕХ ВРАГОВ";

    void Start()
    {
        // Проверяем, что текст назначен
        if (messageText != null)
        {
            messageText.SetActive(false); // убеждаемся, что скрыт в начале
            Text textComponent = messageText.GetComponent<Text>();
            if (textComponent != null)
            {
                textComponent.text = message;
            }
        }
        else
        {
            Debug.LogWarning("Message Text не назначен в инспекторе!");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ShowMessage();
        }
    }

    void ShowMessage()
    {
        if (messageText != null)
        {
            StopAllCoroutines(); // останавливаем старые корутины
            StartCoroutine(DisplayMessageRoutine());
        }
    }

    IEnumerator DisplayMessageRoutine()
    {
        messageText.SetActive(true);  // показываем текст
        yield return new WaitForSeconds(displayTime); // ждём
        messageText.SetActive(false); // скрываем текст
    }

    // Для отладки: рисуем зону триггера в редакторе
    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.3f);
        Gizmos.DrawCube(transform.position, GetComponent<BoxCollider2D>().size);
    }
}
