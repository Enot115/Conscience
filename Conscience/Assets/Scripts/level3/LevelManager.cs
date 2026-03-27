using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic; // Для работы со списками ламп и имен
using TMPro; // Для работы с текстом списка аномалий

public class LevelManager : MonoBehaviour
{
    [Header("Настройки телепорта")]
    public Transform startPoint;      // Точка появления в начале вагона
    public GameObject player;         // Объект игрока

    [Header("Настройки интерфейса")]
    public CanvasGroup fadeGroup;     // Черный экран для затухания
    public TextMeshProUGUI anomalyListText; // Текст списка найденного
    public GameObject hintObject;     // Текст-подсказка "Нужно что-то найти"
    public float fadeDuration = 1.0f; // Длительность затемнения
    public float darkPause = 0.5f;    // Пауза в полной темноте

    [Header("Состояния окружения")]
    public GameObject state0Normal;   // Обычный вагон
    public GameObject state1Anomaly;  // Вагон с первой аномалией
    public GameObject state2Anomaly;  // Вагон со второй аномалией
    public GameObject state3Anomaly;  // Вагон со второй аномалией
    public GameObject chelik;         // NPC (пассажир)

    [Header("Освещение и Звук")]
    public List<Light> allLevelLights; // Все лампы вагона для затемнения
    public AudioSource audioSource;    // Компонент для проигрывания звуков

    [Header("Логика")]
    public int currentLevel = 0;      // 0 = норма, 1 = аномалия 1, 2 = аномалия 2
    public bool anomalyFound = false; // Флаг: найдена ли аномалия в текущем вагоне

    private bool isTransitioning = false;
    private List<string> foundAnomaliesNames = new List<string>(); // История находок
    private Coroutine hintCoroutine;

    private void Start()
    {
        ShowCurrentState();
        UpdateAnomalyUI();
        if (hintObject != null) hintObject.SetActive(false); // Прячем подсказку
    }

    // Вызывается при нажатии E на аномалии
    public void MarkAnomalyFound(string anomalyName, GameObject anomalyObject, AudioClip sound)
    {
        if (!anomalyFound)
        {
            anomalyFound = true;
            foundAnomaliesNames.Add(anomalyName); // Записываем в список
            UpdateAnomalyUI();

            // Убираем объект аномалии из мира
            if (anomalyObject != null) anomalyObject.SetActive(false);

            // Играем уникальный звук аномалии
            if (audioSource != null && sound != null)
            {
                audioSource.PlayOneShot(sound);
            }

            Debug.Log($"Найдено: {anomalyName}");
        }
    }

    private void UpdateAnomalyUI()
    {
        if (anomalyListText == null) return;
        anomalyListText.text = "<b>Найденные аномалии:</b>\n";
        foreach (string name in foundAnomaliesNames)
        {
            anomalyListText.text += "- " + name + "\n";
        }
    }

    public void GoToNextRoom()
    {
        if (isTransitioning) return;

        // Если в вагоне есть аномалия, но игрок её не нашел — не пускаем
        if ((currentLevel == 1 || currentLevel == 2) && !anomalyFound)
        {
            ShowHint(); // Показываем надпись "Нужно что-то найти"
            return;
        }

        StartCoroutine(TransitionRoutine());
    }

    private IEnumerator TransitionRoutine()
    {
        isTransitioning = true;
        yield return StartCoroutine(Fade(1f)); // Затемняем экран

        // Уменьшаем яркость всех ламп на 25%
        foreach (Light l in allLevelLights)
        {
            if (l != null) l.intensity *= 0.55f;
        }

        // Логика переключения между состояниями
        if (currentLevel == 0)
        {
            currentLevel = 1;
        }
        else if (currentLevel == 1 && anomalyFound)
        {
            anomalyFound = false;
            currentLevel = 2;
        }
        else if (currentLevel == 2 && anomalyFound)
        {
            anomalyFound = false;
            currentLevel = 3; // Возвращаемся в чистый вагон (или делай победу)
        }
        else if (currentLevel == 3 && anomalyFound)
        {
            anomalyFound = false;
            currentLevel = 0; // Возвращаемся в чистый вагон (или делай победу)
        }

        ShowCurrentState();

        // Телепортация игрока
        CharacterController cc = player.GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;
        player.transform.position = startPoint.position;
        player.transform.rotation = startPoint.rotation;
        if (cc != null) cc.enabled = true;

        yield return new WaitForSeconds(darkPause);
        yield return StartCoroutine(Fade(0f)); // Проявляем экран
        isTransitioning = false;
    }

    private void ShowCurrentState()
    {
        // Выключаем всё
        if (state0Normal != null) state0Normal.SetActive(false);
        if (state1Anomaly != null) state1Anomaly.SetActive(false);
        if (state2Anomaly != null) state2Anomaly.SetActive(false);
        if (state3Anomaly != null) state3Anomaly.SetActive(false);
        if (chelik != null) chelik.SetActive(false);

        // Включаем нужное состояние
        switch (currentLevel)
        {
            case 0:
                if (state0Normal != null) state0Normal.SetActive(true);
                break;
            case 1:
                if (state1Anomaly != null)
                {
                    chelik.SetActive(true);
                    state1Anomaly.SetActive(true);
                }
                break;
            case 2:
                if (state2Anomaly != null) state2Anomaly.SetActive(true);
                break;
            case 3:
                if (state3Anomaly != null) state3Anomaly.SetActive(true);
                break;
        }
    }

    private void ShowHint()
    {
        if (hintCoroutine != null) StopCoroutine(hintCoroutine);
        hintCoroutine = StartCoroutine(HintRoutine());
    }

    private IEnumerator HintRoutine()
    {
        hintObject.SetActive(true);
        yield return new WaitForSeconds(2.5f);
        hintObject.SetActive(false);
    }

    private IEnumerator Fade(float targetAlpha)
    {
        float startAlpha = fadeGroup.alpha;
        float time = 0f;
        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            fadeGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / fadeDuration);
            yield return null;
        }
        fadeGroup.alpha = targetAlpha;
    }
}