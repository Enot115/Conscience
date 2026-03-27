using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelManager : MonoBehaviour
{
    [Header("Настройки телепорта")]
    public Transform startPoint;      // Точка в начале вагона
    public GameObject player;         // Объект игрока

    [Header("Настройки интерфейса")]
    public CanvasGroup fadeGroup;     // Наш FadeImage с компонентом Canvas Group
    public float fadeDuration = 1.0f; // Длительность затемнения

    [Header("Логика уровней")]
    public int currentLevel = 0;      // Текущий номер уровня (0-8)

    // Основная функция перехода
    public void GoToNextRoom()
    {
        StartCoroutine(TransitionRoutine());
    }

    private IEnumerator TransitionRoutine()
    {
        // 1. Плавное затемнение
        yield return StartCoroutine(Fade(1));

        // 2. Логика уровня (пока просто прибавляем)
        currentLevel++;
        Debug.Log("Переход на вагон №" + currentLevel);

        // 3. Телепортация
        CharacterController cc = player.GetComponent<CharacterController>();
        cc.enabled = false; // Отключаем, чтобы не мешал менять позицию
        player.transform.position = startPoint.position;
        player.transform.rotation = startPoint.rotation;
        cc.enabled = true;

        // Небольшая пауза в темноте для атмосферы
        yield return new WaitForSeconds(0.5f);

        // 4. Плавное осветление
        yield return StartCoroutine(Fade(0));
    }

    private IEnumerator Fade(float targetAlpha)
    {
        float startAlpha = fadeGroup.alpha;
        float time = 0;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            fadeGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / fadeDuration);
            yield return null;
        }
        fadeGroup.alpha = targetAlpha;
    }
}