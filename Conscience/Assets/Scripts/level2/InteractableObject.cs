using UnityEngine;
using UnityEngine.UI;

public class InteractableObject : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private string objectName = "Object";
    [SerializeField] private GameObject interactionPrompt; // UI подсказка [E]

    [Header("References")]
    [SerializeField] private CounterManager counterManager;

    private bool isPlayerInRange = false;
    private bool isCollected = false;

    private void Start()
    {
        // Скрываем подсказку в начале
        if (interactionPrompt != null)
            interactionPrompt.SetActive(false);

        // Если менеджер не назначен, ищем его на сцене
        if (counterManager == null)
            counterManager = FindObjectOfType<CounterManager>();
    }

    private void Update()
    {
        // Если игрок в зоне и объект еще не собран
        if (isPlayerInRange && !isCollected && Input.GetKeyDown(KeyCode.E))
        {
            Collect();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isCollected)
        {
            isPlayerInRange = true;
            if (interactionPrompt != null)
                interactionPrompt.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            if (interactionPrompt != null)
                interactionPrompt.SetActive(false);
        }
    }

    private void Collect()
    {
        isCollected = true;

        // Скрываем подсказку
        if (interactionPrompt != null)
            interactionPrompt.SetActive(false);

        // Увеличиваем счетчик
        if (counterManager != null)
            counterManager.AddScore(1);

        // Эффекты (опционально)
        PlayCollectionEffects();

        // Уничтожаем объект
        Destroy(gameObject);
    }

    private void PlayCollectionEffects()
    {
        // Здесь можно добавить:
        // - Звук сбора
        // - Партиклы
        // - Анимацию
        // AudioSource.PlayClipAtPoint(collectionSound, transform.position);
        // Instantiate(collectionParticle, transform.position, Quaternion.identity);

        Debug.Log($"Collected: {objectName}");
    }

    // Для отладки в редакторе
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, GetComponent<SphereCollider>()?.radius ?? 2f);
    }
}