using UnityEngine;

public class PictureTrigger : MonoBehaviour
{
    [Header("Звук")]
    public AudioSource audioSource; // Сюда перетащишь Audio Source с этой же картины

    [Tooltip("Время в секундах перед повторным воспроизведением")]
    public float cooldown = 3f; // 3 секунды перерыва

    private float _lastPlayTime = -10f; // Время последнего проигрывания

    // Этот метод будем вызывать из скрипта игрока
    public void OnPlayerLookAt()
    {
        // Проверяем, прошло ли достаточно времени с прошлого раза
        if (Time.time - _lastPlayTime < cooldown)
        {
            // Кулдаун еще не прошел, просто выходим, ничего не играем
            return;
        }

        // Обновляем время и играем звук
        _lastPlayTime = Time.time;
        audioSource.Play();

        // Просто чтобы видеть в консоли, когда срабатывает
        Debug.Log($"Игрок посмотрел на: {gameObject.name}");
    }
}