using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 4f;
    public float runSpeed = 7f;
    public float jumpHeight = 1.2f;
    public float gravity = -9.81f;

    [Header("Sensitivity Settings")]
    public float mouseSensitivity = 1f;

    private CharacterController controller;
    private Transform cam;
    private float rotationX = 0f;
    private Vector3 velocity;
    private bool isGrounded;

    // Для определения бега
    private bool isRunning = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        cam = GetComponentInChildren<Camera>().transform;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        LoadSensitivity();
    }

    void LoadSensitivity()
    {
        if (PlayerPrefs.HasKey("Sensitivity"))
        {
            mouseSensitivity = PlayerPrefs.GetFloat("Sensitivity", 1f);
            Debug.Log($"✅ PlayerMovement: Загружена чувствительность {mouseSensitivity}");
        }
    }

    void Update()
    {
        // --- ПРОВЕРКА НА ЗЕМЛЕ ---
        isGrounded = controller.isGrounded;

        // Сбрасываем вертикальную скорость если на земле
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Небольшое отрицательное значение для лучшего прилипания к земле
        }

        // --- ДВИЖЕНИЕ И БЕГ ---
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // Проверка бега (зажатый Shift)
        isRunning = Input.GetKey(KeyCode.LeftShift) && v > 0;
        float currentSpeed = isRunning ? runSpeed : walkSpeed;

        Vector3 move = transform.right * h + transform.forward * v;
        move.Normalize(); // Нормализуем для диагонального движения с той же скоростью

        // Перемещение с учетом скорости бега
        controller.Move(move * currentSpeed * Time.deltaTime);

        // --- ПРЫЖОК ---
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // --- ГРАВИТАЦИЯ ---
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // --- ПОВОРОТ МЫШЬЮ ---
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * 100f * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * 100f * Time.deltaTime;

        transform.Rotate(Vector3.up * mouseX);

        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -80f, 80f);
        cam.localRotation = Quaternion.Euler(rotationX, 0f, 0f);

        // --- КУРСОР ---
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        // Нажмите левый Alt для возврата курсора в игру (опционально)
        if (Input.GetMouseButtonDown(0) && Cursor.lockState == CursorLockMode.None)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    // Публичный метод для обновления чувствительности извне
    public void UpdateSensitivity(float newSensitivity)
    {
        mouseSensitivity = newSensitivity;
        Debug.Log($"🔄 PlayerMovement: Чувствительность обновлена до {newSensitivity}");
    }

    // Геттер для проверки бега (можно использовать в CameraBob и Footsteps)
    public bool IsRunning()
    {
        return isRunning;
    }
}