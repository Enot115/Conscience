using UnityEngine;
using System.Collections;

public class FlatJumpscare : MonoBehaviour
{
    [Header("UI Скримера")]
    [Tooltip("Перетащи сюда выключенный объект Jumpscare_Image из Canvas")]
    public GameObject jumpscareUI;

    [Header("Звук")]
    public AudioSource scareSound; // Звук (не забудь убрать Play On Awake)

    [Header("Настройки")]
    public float scareDuration = 0.3f; // Время на экране. Плоские скримеры лучше делать короткими!

    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        // Срабатывает только от игрока
        if (other.CompareTag("Player") && !hasTriggered)
        {
            StartCoroutine(FlashScare());
        }
    }

    private IEnumerator FlashScare()
    {
        hasTriggered = true;

        // Включаем картинку и звук
        jumpscareUI.SetActive(true);
        if (scareSound != null)
        {
            scareSound.Play();
        }

        // Ждем указанное время
        yield return new WaitForSeconds(scareDuration);

        // Выключаем картинку
        jumpscareUI.SetActive(false);

        // Удаляем сам невидимый триггер, чтобы он больше не сработал
        Destroy(gameObject);
    }
}