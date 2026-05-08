using UnityEngine;

public class RepeatingSoundOnTouch : MonoBehaviour
{
    [Header("Audio Settings")]
    [SerializeField] private AudioClip soundClip;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float volume = 1f;

    [Header("Player Detection")]
    [SerializeField] private string playerTag = "Player";

    private void Start()
    {
        // Если AudioSource не назначен в инспекторе, добавляем автоматически
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
                audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Настройка AudioSource
        audioSource.playOnAwake = false;
        audioSource.volume = volume;
        audioSource.clip = soundClip;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Проверяем, является ли вошедший объект игроком
        if (other.CompareTag(playerTag))
        {
            PlaySound();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Альтернативный вариант для Collision (не Trigger)
        if (collision.gameObject.CompareTag(playerTag))
        {
            PlaySound();
        }
    }

    private void PlaySound()
    {
        if (soundClip != null && audioSource != null)
        {
            audioSource.PlayOneShot(soundClip);
            // Или если хотите перезапускать каждый раз:
            // audioSource.Play();
        }
        else
        {
            Debug.LogWarning("Sound clip or AudioSource is missing on " + gameObject.name);
        }
    }

    // Опционально: визуальный отладчик в редакторе
    private void OnDrawGizmos()
    {
        BoxCollider boxCollider = GetComponent<BoxCollider>();
        if (boxCollider != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position + boxCollider.center, boxCollider.size);
        }
    }
}