using System.Collections;
using UnityEngine;

public class ScareTrigger : MonoBehaviour
{
    [Header("Настройки скримера")]
    [SerializeField] private GameObject scareObject;
    [SerializeField] private AudioClip scareSound;
    [SerializeField] private float scareDuration = 1f;
    [SerializeField] private float cameraShakeDuration = 1f;
    [SerializeField] private float cameraShakeMagnitude = 1f;

    [Header("Настройки срабатывания")]
    [SerializeField] private bool triggerOnlyOnce = true;
    [SerializeField] private float destroyDelay = 3f;

    private bool hasTriggered = false;
    private Transform cameraTransform;
    private Vector3 originalCameraPosition;
    private Quaternion originalCameraRotation;
    private AudioSource audioSource;

    private void Start()
    {
        // Ищем камеру по имени "PlayerCamera" внутри объекта Player
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            // Ищем дочерний объект с именем PlayerCamera
            Transform playerCamera = player.transform.Find("PlayerCamera");

            if (playerCamera != null)
            {
                cameraTransform = playerCamera;
                Debug.Log("Камера найдена: " + cameraTransform.name);
            }
            else
            {
                // Если не нашли по имени, ищем любой компонент Camera в дочерних объектах
                Camera cam = player.GetComponentInChildren<Camera>();
                if (cam != null)
                {
                    cameraTransform = cam.transform;
                    Debug.Log("Камера найдена через компонент Camera: " + cameraTransform.name);
                }
            }
        }

        // Если всё ещё не нашли, пробуем Camera.main
        if (cameraTransform == null && Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
            Debug.Log("Камера найдена через Camera.main");
        }

        // Сохраняем исходные позицию и вращение камеры
        if (cameraTransform != null)
        {
            originalCameraPosition = cameraTransform.localPosition;
            originalCameraRotation = cameraTransform.localRotation;
            Debug.Log("Исходная позиция камеры: " + originalCameraPosition);
        }
        else
        {
            Debug.LogError("КАТАСТРОФА: Камера не найдена! Проверь структуру объекта Player");
        }

        // Настраиваем AudioSource
        audioSource = gameObject.AddComponent<AudioSource>();

        if (scareObject != null)
            scareObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasTriggered)
        {
            Debug.Log("Скример активирован!");
            StartCoroutine(ScareSequence());

            if (triggerOnlyOnce)
            {
                hasTriggered = true;
                Destroy(gameObject, destroyDelay);
            }
        }
    }

    private IEnumerator ScareSequence()
    {
        // Показываем скример
        if (scareObject != null)
        {
            scareObject.SetActive(true);
            Debug.Log("Скример показан");
        }

        // Воспроизводим звук
        if (scareSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(scareSound);
            Debug.Log("Звук воспроизведен");
        }

        // Запускаем тряску камеры
        StartCoroutine(ShakeCamera());

        // Ждем длительность скримера
        yield return new WaitForSeconds(scareDuration);

        // Прячем скример
        if (scareObject != null)
        {
            scareObject.SetActive(false);
            Debug.Log("Скример скрыт");
        }
    }

    private IEnumerator ShakeCamera()
    {
        if (cameraTransform == null)
        {
            Debug.LogError("Камера не найдена для тряски!");
            yield break;
        }

        float elapsed = 0f;

        while (elapsed < cameraShakeDuration)
        {
            // Тряска позиции
            float posX = Random.Range(-1f, 1f) * cameraShakeMagnitude;
            float posY = Random.Range(-1f, 1f) * cameraShakeMagnitude;
            cameraTransform.localPosition = originalCameraPosition + new Vector3(posX, posY, 0);

            // Тряска вращения (более реалистично для FPS)
            float rotX = Random.Range(-1f, 1f) * cameraShakeMagnitude * 2;
            float rotY = Random.Range(-1f, 1f) * cameraShakeMagnitude * 2;
            cameraTransform.localRotation = originalCameraRotation * Quaternion.Euler(rotX, rotY, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Возвращаем камеру в исходное состояние
        cameraTransform.localPosition = originalCameraPosition;
        cameraTransform.localRotation = originalCameraRotation;

        Debug.Log("Тряска камеры завершена");
    }

    // Визуализация триггера в редакторе
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        BoxCollider boxCollider = GetComponent<BoxCollider>();
        if (boxCollider != null)
        {
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawWireCube(boxCollider.center, boxCollider.size);
        }
    }
}