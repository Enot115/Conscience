using UnityEngine;

public class PlayerInteraction2 : MonoBehaviour
{
    [Header("Interaction Settings")]
    [SerializeField] private float interactionRange = 2f;
    [SerializeField] private LayerMask interactableLayer;

    private Camera playerCamera;

    private void Start()
    {
        playerCamera = GetComponentInChildren<Camera>();
    }

    private void Update()
    {
        // Альтернативный вариант: Raycast вместо триггеров
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryInteract();
        }
    }

    private void TryInteract()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionRange, interactableLayer))
        {
            InteractableObject interactable = hit.collider.GetComponent<InteractableObject>();
            if (interactable != null)
            {
                // Здесь можно вызвать метод взаимодействия
                // Но в нашем случае объект сам обрабатывает нажатие E
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (playerCamera != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * interactionRange);
        }
    }
}