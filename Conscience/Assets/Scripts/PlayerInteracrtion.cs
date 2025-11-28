using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public Door door;
    public Camera playerCamera;     
    public float interactDistance = 3f;
    public KeyCode interactKey = KeyCode.E;

    void Update()
    {
        if (Input.GetKeyDown(interactKey))
        {
            TryInteract();
        }
    }

    void TryInteract()
    {
        if (playerCamera == null)
        {
            Debug.LogWarning("PlayerInteraction: не назначена playerCamera");
            return;
        }

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance))
        {
            Debug.Log("RAY HIT: " + hit.collider.name);

            DoorAnimated animDoor = hit.collider.GetComponent<DoorAnimated>();
            if (animDoor != null)
            {
                Debug.Log("FOUND DoorAnimated на " + hit.collider.name);
                animDoor.OnInteract();
                return;
            }

            Debug.Log("На объекте " + hit.collider.name + " нет DoorAnimated");
        }
        else
        {
            Debug.Log("RAY: перед тобой ничего нет");
        }
    }
}
