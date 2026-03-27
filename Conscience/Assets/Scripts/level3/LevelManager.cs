using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelManager : MonoBehaviour
{
    [Header("Настройки телепорта")]
    public Transform startPoint;
    public GameObject player;

    [Header("Настройки интерфейса")]
    public CanvasGroup fadeGroup;
    public float fadeDuration = 1.0f;
    public float darkPause = 0.5f;

    [Header("Состояния окружения")]
    public GameObject state0Normal;   // без аномалии
    public GameObject state1Anomaly;  // с аномалией

    [Header("Логика")]
    public int currentLevel = 0;      // 0 = без аномалии, 1 = с аномалией
    public bool anomalyFound = false; // нашёл ли игрок аномалию
    private bool isTransitioning = false;

    private void Start()
    {
        ShowCurrentState();
    }

    public void MarkAnomalyFound()
    {
        anomalyFound = true;
        Debug.Log("Аномалия найдена");
    }

    public void GoToNextRoom()
    {
        if (isTransitioning) return;
        StartCoroutine(TransitionRoutine());
    }

    private IEnumerator TransitionRoutine()
    {
        isTransitioning = true;

        // 1. Затемнение
        yield return StartCoroutine(Fade(1f));

        // 2. Логика перехода
        if (currentLevel == 0)
        {
            // Первый проход всегда переводит нас в вагон с аномалией
            currentLevel = 1;
        }
        else
        {
            // Если мы уже на уровне с аномалией:
            // - если нашли, тут потом можно будет вести дальше
            // - если не нашли, остаёмся на 1
            if (anomalyFound)
            {
                Debug.Log("Аномалия была найдена. Тут потом можно вести на следующий уровень.");
                anomalyFound = false;

                // Пока для теста оставим снова 1,
                // чтобы не ломать логику, пока у нас только один уровень с аномалией
                currentLevel = 1;
            }
            else
            {
                Debug.Log("Аномалия не найдена. Повторяем тот же вагон.");
                currentLevel = 1;
            }
        }

        ShowCurrentState();

        // 3. Телепорт игрока
        CharacterController cc = player.GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;

        player.transform.position = startPoint.position;
        player.transform.rotation = startPoint.rotation;

        if (cc != null) cc.enabled = true;

        // 4. Пауза в темноте
        yield return new WaitForSeconds(darkPause);

        // 5. Осветление
        yield return StartCoroutine(Fade(0f));

        isTransitioning = false;
    }

    private void ShowCurrentState()
    {
        if (state0Normal != null) state0Normal.SetActive(false);
        if (state1Anomaly != null) state1Anomaly.SetActive(false);

        switch (currentLevel)
        {
            case 0:
                if (state0Normal != null) state0Normal.SetActive(true);
                Debug.Log("Включено состояние 0: без аномалии");
                break;

            case 1:
                if (state1Anomaly != null) state1Anomaly.SetActive(true);
                Debug.Log("Включено состояние 1: с аномалией");
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