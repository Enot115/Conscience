using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class LevelManager : MonoBehaviour
{
    [Header("Настройки телепорта")]
    public Transform startPoint;          // Точка спавна для короткого вагона
    public Transform longTrainStartPoint; // Точка спавна для длинного поезда
    public GameObject player;

    [Header("Настройки интерфейса")]
    public CanvasGroup fadeGroup;
    public TextMeshProUGUI anomalyListText;
    public GameObject hintObject;
    public float fadeDuration = 1.0f;
    public float darkPause = 0.5f;

    [Header("Окружение (Сами поезда)")]
    public GameObject standardTrain;
    public GameObject longTrain;

    [Header("Состояния уровней")]
    public GameObject state0Normal;
    public GameObject state1Anomaly;
    public GameObject state2Anomaly;
    public GameObject state3Normal;
    public GameObject state4Anomaly;

    [Header("NPC (Персонажи)")]
    public GameObject chelik;
    public GameObject newChelik;

    [Header("Освещение и Звук")]
    public List<Light> allLevelLights;
    public AudioSource audioSource;

    [Header("Логика")]
    public int currentLevel = 0;
    public bool anomalyFound = false;
    [Header("Концовка игры")]
    public GameObject endgameVideoPlayer;

    private bool isTransitioning = false;
    private List<string> foundAnomaliesNames = new List<string>();
    private Coroutine hintCoroutine;

    private void Start()
    {
        ShowCurrentState();
        UpdateAnomalyUI();
        if (hintObject != null) hintObject.SetActive(false);
    }

    public void MarkAnomalyFound(string anomalyName, GameObject anomalyObject, AudioClip sound)
    {
        if (!anomalyFound)
        {
            anomalyFound = true;
            foundAnomaliesNames.Add(anomalyName);
            UpdateAnomalyUI();

            if (anomalyObject != null) anomalyObject.SetActive(false);

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

        // Искать аномалии нужно ТОЛЬКО на 1 и 2 уровне. На 4 уровне нужно просто выжить и убежать.
        bool levelNeedsAnomaly = (currentLevel == 1 || currentLevel == 2);

        if (levelNeedsAnomaly && !anomalyFound)
        {
            ShowHint();
            return;
        }

        StartCoroutine(TransitionRoutine());
    }

    private IEnumerator TransitionRoutine()
    {
        isTransitioning = true;
        yield return StartCoroutine(Fade(1f)); // Экран темнеет

        // Запоминаем исходную яркость ламп и приглушаем их
        float[] initialIntensities = new float[allLevelLights.Count];
        for (int i = 0; i < allLevelLights.Count; i++)
        {
            if (allLevelLights[i] != null)
            {
                initialIntensities[i] = allLevelLights[i].intensity;
                allLevelLights[i].intensity *= 0.55f;
            }
        }

        // --- ЛОГИКА ПЕРЕКЛЮЧЕНИЯ УРОВНЕЙ ---
        if (currentLevel == 0)
        {
            currentLevel = 1;
        }
        else if (currentLevel == 1 && anomalyFound)
        {
            currentLevel = 2;
            anomalyFound = false;
        }
        else if (currentLevel == 2 && anomalyFound)
        {
            currentLevel = 3; // Передышка
            anomalyFound = false;
        }
        else if (currentLevel == 3)
        {
            currentLevel = 4; // Прыгаем в длинный поезд
        }
        else if (currentLevel == 4)
        {
            currentLevel = 0; // Сбежали из длинного поезда в нормальный
            anomalyFound = false;
        }

        ShowCurrentState();

        // --- ТЕЛЕПОРТАЦИЯ ИГРОКА ---
        CharacterController cc = player.GetComponent<CharacterController>();

        // ОБЯЗАТЕЛЬНО выключаем контроллер перед перемещением
        if (cc != null) cc.enabled = false;

        // Выбираем точку спавна: если 4 уровень — кидаем в длинный, иначе — в обычный
        Transform spawnPointToUse = (currentLevel == 4) ? longTrainStartPoint : startPoint;

        // Защита от дурака: если забыл вставить точку в Инспекторе
        if (spawnPointToUse != null)
        {
            player.transform.position = spawnPointToUse.position;
            player.transform.rotation = spawnPointToUse.rotation;
        }
        else
        {
            Debug.LogError("ОШИБКА: Не назначена точка спавна в Инспекторе!");
        }

        // Включаем контроллер обратно
        if (cc != null) cc.enabled = true;

        yield return new WaitForSeconds(darkPause);

        // Возвращаем свет лампам в норму, чтобы игра не ушла во мрак
        /*for (int i = 0; i < allLevelLights.Count; i++)
        {
            if (allLevelLights[i] != null)
            {
                allLevelLights[i].intensity = initialIntensities[i];
            }
        }
        */
        yield return StartCoroutine(Fade(0f)); // Экран светлеет
        isTransitioning = false;
    }

    private void ShowCurrentState()
    {
        // Выключаем всё
        if (standardTrain != null) standardTrain.SetActive(false);
        if (longTrain != null) longTrain.SetActive(false);
        if (state0Normal != null) state0Normal.SetActive(false);
        if (state1Anomaly != null) state1Anomaly.SetActive(false);
        if (state2Anomaly != null) state2Anomaly.SetActive(false);
        if (state3Normal != null) state3Normal.SetActive(false);
        if (state4Anomaly != null) state4Anomaly.SetActive(false);
        if (chelik != null) chelik.SetActive(false);
        if (newChelik != null) newChelik.SetActive(false);

        // Включаем нужное
        switch (currentLevel)
        {
            case 0:
                if (standardTrain != null) standardTrain.SetActive(true);
                if (state0Normal != null) state0Normal.SetActive(true);
                break;
            case 1:
                if (standardTrain != null) standardTrain.SetActive(true);
                if (state1Anomaly != null) state1Anomaly.SetActive(true);
                if (chelik != null) chelik.SetActive(true);
                break;
            case 2:
                if (standardTrain != null) standardTrain.SetActive(true);
                if (state2Anomaly != null) state2Anomaly.SetActive(true);
                break;
            case 3:
                if (standardTrain != null) standardTrain.SetActive(true);
                if (state3Normal != null) state3Normal.SetActive(true);
                break;
            case 4:
                if (longTrain != null) longTrain.SetActive(true);
                if (state4Anomaly != null) state4Anomaly.SetActive(true);
                if (newChelik != null) newChelik.SetActive(true);
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
        if (hintObject != null)
        {
            hintObject.SetActive(true);
            yield return new WaitForSeconds(2.5f);
            hintObject.SetActive(false);
        }
    }

    private IEnumerator Fade(float targetAlpha)
    {
        if (fadeGroup != null)
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
    // Вызывается триггером в конце 4 уровня
    public void PlayEndGameVideo()
    {
        PlayerMovement movementScript = player.GetComponent<PlayerMovement>();
        if (movementScript != null) movementScript.enabled = false;

        // 1. Отключаем управление игроку, чтобы он не ходил во время видео
        CharacterController cc = player.GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;

        // Если у тебя есть скрипт вращения камерой (MouseLook/CameraLook), 
        // его тоже можно отключить здесь, чтобы игрок не крутил головой во время заставки.

        // 2. Включаем объект с видеоплеером
        if (endgameVideoPlayer != null)
        {
            endgameVideoPlayer.SetActive(true);
        }
        else
        {
            Debug.LogError("Не назначен объект финального видео в LevelManager!");
        }

        // Выключаем монстра, если он бежал за нами, чтобы он нас не "убил" во время катсцены
        if (newChelik != null) newChelik.SetActive(false);
    }
}