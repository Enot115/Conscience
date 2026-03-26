using UnityEngine;
using TMPro;

public class PinCodeHint : MonoBehaviour
{
    [Header("Настройки")]
    [SerializeField] private GameObject hintObject; // Объект с текстом подсказки
    [SerializeField] private float activationDistance = 3f; // Дистанция активации
    [SerializeField] private KeyCode interactKey = KeyCode.E;

    [Header("Система пинкода")]
    [SerializeField] private PinCodeSystemUI pinCodeSystem; // Ваша UI система

    private Transform player;
    private bool isPlayerNear = false;

    void Start()
    {
        // Находим игрока
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;

        // Скрываем подсказку в начале
        if (hintObject != null)
            hintObject.SetActive(false);

        // Если не указали систему, ищем на этом же объекте
        if (pinCodeSystem == null)
            pinCodeSystem = GetComponent<PinCodeSystemUI>();
    }

    void Update()
    {
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
            return;
        }

        // Проверяем расстояние
        float distance = Vector3.Distance(transform.position, player.position);
        bool near = distance <= activationDistance;

        // Показываем/скрываем подсказку
        if (near && !isPlayerNear && !pinCodeSystem.IsActive())
        {
            isPlayerNear = true;
            if (hintObject != null)
                hintObject.SetActive(true);
        }
        else if ((!near || pinCodeSystem.IsActive()) && isPlayerNear)
        {
            isPlayerNear = false;
            if (hintObject != null)
                hintObject.SetActive(false);
        }

        // Активация пинкода по E
        if (near && Input.GetKeyDown(interactKey) && !pinCodeSystem.IsActive())
        {
            pinCodeSystem.SetActiveState(true);
        }
    }

    // Визуализация в редакторе
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, activationDistance);
    }
}