using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 4f;
    public float mouseSensitivity = 150f;

    private CharacterController controller;
    private Transform cam;
    private float rotationX = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        // Ищем камеру внутри игрока
        cam = GetComponentInChildren<Camera>().transform;

        // Прячем курсор в центре
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // --- ДВИЖЕНИЕ ---

        float h = Input.GetAxis("Horizontal");  // A / D
        float v = Input.GetAxis("Vertical");    // W / S

        Vector3 move = transform.right * h + transform.forward * v;
        controller.SimpleMove(move * moveSpeed);

        // --- ПОВОРОТ МЫШЬЮ ---

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // вращаем тело по горизонтали
        transform.Rotate(Vector3.up * mouseX);

        // вращаем камеру по вертикали
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -80f, 80f);
        cam.localRotation = Quaternion.Euler(rotationX, 0f, 0f);

        // По Esc можно разблокировать курсор (по желанию)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}