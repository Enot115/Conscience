using UnityEngine;
using UnityEngine.UI;
using TMPro; // Если используешь TextMeshPro

public class CounterManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI counterText; // Для TMP
                                                          // [SerializeField] private Text counterText; // Для обычного UI Text

    [Header("Settings")]
    [SerializeField] private int currentScore = 0;
    [SerializeField] private int totalObjects = 3;

    private void Start()
    {
        UpdateCounterDisplay();

        // Автоматически считаем все интерактивные объекты на сцене
        totalObjects = FindObjectsOfType<InteractableObject>().Length;
    }

    public void AddScore(int amount)
    {
        currentScore += amount;
        UpdateCounterDisplay();

        // Проверяем, собраны ли все объекты
        if (currentScore >= totalObjects)
        {
            OnAllCollected();
        }
    }

    private void UpdateCounterDisplay()
    {
        if (counterText != null)
        {
            counterText.text = $"{currentScore} / 3";
            // Или просто: counterText.text = currentScore.ToString();
        }
    }

    private void OnAllCollected()
    {
        Debug.Log("All objects collected!");
        // Здесь можно добавить:
        // - Показ сообщения о завершении
        // - Загрузку следующего уровня
        // - Активацию портала
        // Time.timeScale = 0f; // Пауза игры
        // ShowCompletionPanel();
    }

    // Для отладки
    public int GetCurrentScore() => currentScore;
    public int GetTotalObjects() => totalObjects;
}