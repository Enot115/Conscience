using UnityEngine;
using TMPro; // Для TextMeshPro

public class AnomalyFinder : MonoBehaviour
{
    [Header("UI элементы")]
    [SerializeField] private TextMeshProUGUI promptText;      // Текст "Нажмите E"
    [SerializeField] private TextMeshProUGUI counterText;     // Текст счетчика 0/1

    [Header("Настройки")]
    [SerializeField] private float rayDistance = 50f;          // Дальность взаимодействия
    [SerializeField] private LayerMask anomalyLayer;          // Слой для аномалий (опционально)
    [SerializeField] private KeyCode interactKey = KeyCode.E; // Кнопка действия

    // Переменные для внутренней работы
    private GameObject currentAnomaly;    // На какую аномалию сейчас смотрим
    private int anomaliesFound = 0;       // Сколько нашли
    private int totalAnomalies = 1;       // Сколько всего нужно найти (можно изменить)
    private Camera playerCamera;           // Ссылка на камеру игрока

    void Start()
    {
        playerCamera = GetComponent<Camera>();

        // НОВОЕ: Проверяем что все ссылки назначены
        if (promptText == null)
            Debug.LogError("promptText не назначен в инспекторе!");
        if (counterText == null)
            Debug.LogError("counterText не назначен в инспекторе!");

        if (promptText != null)
        {
            promptText.gameObject.SetActive(false);
            Debug.Log("Подсказка изначально скрыта");
        }

        UpdateCounterDisplay();
    }

    void Update()
    {
        // НОВОЕ: Проверяем нажатие E в принципе
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Кнопка E нажата! currentAnomaly = " + (currentAnomaly != null ? currentAnomaly.name : "null"));
        }

        CheckForAnomaly();

        if (currentAnomaly != null && Input.GetKeyDown(interactKey))
        {
            Debug.Log("Пытаемся взаимодействовать с аномалией");
            InteractWithAnomaly();
        }
    }

    void CheckForAnomaly()
    {
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        // Рисуем луч красным
        Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.red, 1f);

        // Пускаем луч
        bool hitSomething = Physics.Raycast(ray, out hit, rayDistance);

        if (hitSomething)
        {
            Debug.Log($"Луч попал в: {hit.collider.gameObject.name} на расстоянии {hit.distance}");
            Debug.Log($"Тег объекта: {hit.collider.tag}");
            Debug.Log($"Тип коллайдера: {hit.collider.GetType()}");

            if (hit.collider.CompareTag("Anomaly"))
            {
                Debug.Log(" ЭТО АНОМАЛИЯ!");

                if (currentAnomaly != hit.collider.gameObject)
                {
                    if (currentAnomaly != null)
                        ShowPrompt(false);

                    currentAnomaly = hit.collider.gameObject;
                    ShowPrompt(true);
                }
            }
            else
            {
                Debug.Log("Не аномалия");
                ClearCurrentAnomaly();
            }
        }
        else
        {
            Debug.Log(" Луч ни во что не попал");
            ClearCurrentAnomaly();
        }
    }

    void ClearCurrentAnomaly()
    {
        if (currentAnomaly != null)
        {
            currentAnomaly = null;
            ShowPrompt(false);
        }
    }

    void InteractWithAnomaly()
    {
        Debug.Log("Аномалия найдена!");

        // Увеличиваем счетчик
        if (anomaliesFound < totalAnomalies)
        {
            anomaliesFound++;
            UpdateCounterDisplay();
        }

        // Что делаем с аномалией?
        if (currentAnomaly != null)
        {
            // Вариант 1: Просто выключаем
            currentAnomaly.SetActive(false);

 
        }

        // Очищаем всё
        currentAnomaly = null;
        ShowPrompt(false);

        // Проверка на победу
        if (anomaliesFound >= totalAnomalies)
        {
            Debug.Log("ПОБЕДА! Все аномалии найдены!");
            // Можно вывести сообщение на экран, заморозить время и т.д.
        }
    }

    void ShowPrompt(bool show)
    {
        if (promptText != null)
        {
            promptText.gameObject.SetActive(show);
        }
    }

    void UpdateCounterDisplay()
    {
        if (counterText != null)
        {
            counterText.text = $"Аномалий {anomaliesFound}/{totalAnomalies}";
        }
    }
}