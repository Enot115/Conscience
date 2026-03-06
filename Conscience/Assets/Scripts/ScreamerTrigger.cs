using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScreamerTrigger : MonoBehaviour
{
    [Header("Настройки скримера")]
    [SerializeField] private Image screamerImage;
    [SerializeField] private AudioSource screamerSound;
    [SerializeField] private float displayTime = 0.3f;

    [Header("Тестовые настройки")]
    [SerializeField] private bool oneTimeOnly = true;
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private bool useDebugLog = true;

    private bool triggered = false;

    private void Start()
    {
        // Проверка при старте
        if (screamerImage == null)
        {
            Debug.LogError("ОШИБКА: Изображение скримера не назначено!");
        }
        else
        {
            Debug.Log($"Image найден: {screamerImage.name}, начальная альфа: {screamerImage.color.a}");
            screamerImage.gameObject.SetActive(true); // Включаем
            screamerImage.color = new Color(1, 1, 1, 0); // Делаем прозрачным
        }

        if (screamerSound == null)
        {
            Debug.LogWarning("Звук не назначен");
        }

        // Проверка коллайдера
        Collider2D col2D = GetComponent<Collider2D>();
        Collider col3D = GetComponent<Collider>();

        if (col2D == null && col3D == null)
        {
            Debug.LogError("НЕТ КОЛЛАЙДЕРА! Добавь коллайдер и поставь Is Trigger");
        }
        else
        {
            if (col2D != null)
                Debug.Log($"2D Коллайдер найден, IsTrigger = {col2D.isTrigger}");
            if (col3D != null)
                Debug.Log($"3D Коллайдер найден, IsTrigger = {col3D.isTrigger}");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"OnTriggerEnter2D сработал! Объект: {other.gameObject.name}, Тег: {other.tag}");

        if (oneTimeOnly && triggered)
        {
            Debug.Log("Уже срабатывал, игнорирую");
            return;
        }

        if (other.CompareTag(playerTag))
        {
            Debug.Log("=== ИГРОК В ТРИГГЕРЕ! ЗАПУСКАЮ СКРИМЕР ===");
            TriggerScreamer();
        }
        else
        {
            Debug.Log($"Вошел объект с тегом '{other.tag}', а нужен '{playerTag}'");
        }
    }

    // Для 3D
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"OnTriggerEnter сработал! Объект: {other.gameObject.name}, Тег: {other.tag}");

        if (oneTimeOnly && triggered) return;

        if (other.CompareTag(playerTag))
        {
            Debug.Log("=== ИГРОК В ТРИГГЕРЕ! ЗАПУСКАЮ СКРИМЕР ===");
            TriggerScreamer();
        }
    }

    private void TriggerScreamer()
    {
        triggered = true;

        if (screamerImage != null)
        {
            Debug.Log("ПОКАЗЫВАЮ СКРИМЕР!");

            // Простой способ - меняем цвет
            screamerImage.color = Color.white;

            // Если не работает, пробуем включить/выключить объект
            screamerImage.gameObject.SetActive(false);
            screamerImage.gameObject.SetActive(true);

            // Запускаем таймер на скрытие
            StartCoroutine(HideAfterDelay());
        }
        else
        {
            Debug.LogError("НЕ МОГУ ПОКАЗАТЬ: изображение не назначено!");
        }

        if (screamerSound != null)
        {
            screamerSound.Play();
            Debug.Log("Звук проигрывается");
        }
    }

    private IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(displayTime);

        if (screamerImage != null)
        {
            Debug.Log("Скрываю скример");
            screamerImage.color = new Color(1, 1, 1, 0);
        }
    }

    // Для теста - нажми F2 в режиме игры
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F2))
        {
            Debug.Log("Ручной тест скримера по F2");
            TriggerScreamer();
        }
    }
}