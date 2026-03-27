using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public Door door;
    public Camera playerCamera;     
    public float interactDistance = 3f;
    public KeyCode interactKey = KeyCode.E;

    void Update()
    {
        // Проверка на паузу
        PauseMenu pauseMenu = FindObjectOfType<PauseMenu>();
        if (pauseMenu != null && pauseMenu.IsPaused)
        {
            return;
        }

        if (Input.GetKeyDown(interactKey))
        {
            TryInteract();
        }
    }

    void TryInteract()
    {
        if (playerCamera == null) return;

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        // Добавляем QueryTriggerInteraction.Ignore, чтобы невидимые триггеры не мешали
        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
        {
            // 1. Проверяем на анимированную дверь
            DoorAnimated animDoor = hit.collider.GetComponentInParent<DoorAnimated>();
            if (animDoor != null)
            {
                animDoor.OnInteract();
                return;
            }

            // 2. Проверяем на аномалию
            Anomaly anomaly = hit.collider.GetComponentInParent<Anomaly>();
            if (anomaly != null)
            {
                anomaly.OnInteract();
                return;
            }

            // 3. Проверяем на обычную дверь
            Door door = hit.collider.GetComponentInParent<Door>();
            if (door != null)
            {
                door.OnInteract();
                return;
            }
        }
    }
}
