using UnityEngine;
using System.Collections;

public class FullScreamer : MonoBehaviour
{
    public GameObject jumpscareImage; // Наша картинка из Canvas
    public AudioSource jumpscareAudio; // AudioSource со звуком крика
    public float duration = 0.5f;     // Сколько висит картинка

    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        // Проверяем тег игрока
        if (other.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true;
            StartCoroutine(ShowScreamer());
        }
    }

    private IEnumerator ShowScreamer()
    {
        // Включаем звук (лучше 2D, чтобы орало прямо в уши)
        if (jumpscareAudio != null) jumpscareAudio.Play();

        // Включаем картинку
        if (jumpscareImage != null) jumpscareImage.SetActive(true);

        // Ждем доли секунды
        yield return new WaitForSeconds(duration);

        // Выключаем картинку
        if (jumpscareImage != null) jumpscareImage.SetActive(false);

        // Опционально: удаляем триггер, чтобы не пугать дважды
        Destroy(gameObject, 1f);
    }
}