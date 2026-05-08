using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float jumpHeight = 1.2f;
    public float gravity = -19.62f; // Увеличил гравитацию для более "четкого" приземления

    [Header("Sensitivity Settings")]
    public float mouseSensitivity = 1f;

    private CharacterController controller;
    private Transform cam;
    private float rotationX = 0f;
    private Vector3 velocity;
    private bool isGrounded;
    private bool isRunning = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        // Ищем камеру в дочерних объектах
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
        }
    }

    void Update()
    {
        CharacterController cc = GetComponent<CharacterController>();

        // Если контроллер выключен - просто прерываем выполнение Update в этом кадре
        if (cc != null && !cc.enabled)
        {
            return;
        }

        // 1. Проверка паузы
        PauseMenu pauseMenu = FindObjectOfType<PauseMenu>();
        if (pauseMenu != null && pauseMenu.IsPaused) return;

        // 2. Проверка земли
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // 3. ВВОД ДАННЫХ (Используем GetAxisRaw для мгновенной остановки)
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        isRunning = Input.GetKey(KeyCode.LeftShift) && v > 0;
        float currentSpeed = isRunning ? runSpeed : walkSpeed;

        // Направление движения
        Vector3 move = transform.right * h + transform.forward * v;

        // Нормализуем только если есть ввод, чтобы избежать ошибок и лишних расчетов
        if (move.sqrMagnitude > 0.01f)
        {
            move.Normalize();
            controller.Move(move * currentSpeed * Time.deltaTime);
        }

        // 4. ПРЫЖОК
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // 5. ГРАВИТАЦИЯ
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // 6. ПОВОРОТ МЫШЬЮ
        RotatePlayer();
    }

    void RotatePlayer()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * 100f * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * 100f * Time.deltaTime;

        transform.Rotate(Vector3.up * mouseX);

        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -80f, 80f);
        cam.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
    }

    public void UpdateSensitivity(float newSensitivity)
    {
        mouseSensitivity = newSensitivity;
    }

    public bool IsRunning() => isRunning;
}