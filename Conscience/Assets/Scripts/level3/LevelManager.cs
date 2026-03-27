using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic; // Нужно для работы со списками
using TMPro; // Нужно для текста

public class LevelManager : MonoBehaviour
{
    [Header("Настройки телепорта")]
    public Transform startPoint;
    public GameObject player;

    [Header("Настройки интерфейса")]
    public CanvasGroup fadeGroup;
    public TextMeshProUGUI anomalyListText; // Ссылка на твой текст сбоку
    public float fadeDuration = 1.0f;
    public float darkPause = 0.5f;

    [Header("Состояния окружения")]
    public GameObject state0Normal;
    public GameObject state1Anomaly;
    public GameObject state2Anomaly;

    [Header("Логика")]
    public int currentLevel = 0;
    public bool anomalyFound = false;
    private bool isTransitioning = false;

    [Header("Челик")]
    public GameObject chelik;
    // Список для хранения имен найденных аномалий
    private List<string> foundAnomaliesNames = new List<string>();

    private void Start()
    {
        ShowCurrentState();
        chelik.SetActive(true);
        UpdateAnomalyUI();
    }

    // Теперь метод принимает имя аномалии
    public void MarkAnomalyFound(string anomalyName)
    {
        if (!anomalyFound)
        {
            anomalyFound = true;
            foundAnomaliesNames.Add(anomalyName); // Добавляем в список
            UpdateAnomalyUI(); // Обновляем текст на экране
            Debug.Log($"Аномалия '{anomalyName}' найдена!");
        }
    }

    // Обновление текста в UI
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

        // ПРОВЕРКА: Если мы в вагоне с аномалией (1), но не нашли её
        if (currentLevel == 1 && !anomalyFound)
        {
            Debug.Log("Дверь заперта! Вы не нашли аномалию в этом вагоне.");
            // Здесь можно проиграть звук закрытой двери
            return;
        }

        StartCoroutine(TransitionRoutine());
    }

    private IEnumerator TransitionRoutine()
    {
        isTransitioning = true;
        yield return StartCoroutine(Fade(1f));

        if (currentLevel == 0)
        {
            // После первого обычного вагона идём к 1 аномалии
            currentLevel = 1;
        }
        else if (currentLevel == 1)
        {
            if (anomalyFound)
            {
                Debug.Log("Первая аномалия найдена, переходим ко второй.");
                anomalyFound = false;
                currentLevel = 2;
            }
        }
        else if (currentLevel == 2)
        {
            if (anomalyFound)
            {
                Debug.Log("Вторая аномалия найдена. Тут можно делать победу или следующий уровень.");
                anomalyFound = false;

                // Пока оставим 2, если дальше уровня нет
                currentLevel = 2;
            }
        }

        ShowCurrentState();

        CharacterController cc = player.GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;
        player.transform.position = startPoint.position;
        player.transform.rotation = startPoint.rotation;
        if (cc != null) cc.enabled = true;

        yield return new WaitForSeconds(darkPause);
        yield return StartCoroutine(Fade(0f));
        isTransitioning = false;
    }

    private void ShowCurrentState()
    {
        if (state0Normal != null) state0Normal.SetActive(false);
        if (state1Anomaly != null) state1Anomaly.SetActive(false);
        if (state2Anomaly != null)
        {
            state2Anomaly.SetActive(false);
            chelik.SetActive(false);
        }

        switch (currentLevel)
        {
            case 0:
                if (state0Normal != null) state0Normal.SetActive(true);
                break;

            case 1:
                if (state1Anomaly != null)
                {
                    state1Anomaly.SetActive(true);
                    chelik.SetActive(true);
                }
                break;

            case 2:
                if (state2Anomaly != null) state2Anomaly.SetActive(true);
                break;
        }
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