using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 4f;

    [Header("Sensitivity Settings")]
    public float mouseSensitivity = 1f; // Изменено с 150f на 1f для соответствия слайдеру

    private CharacterController controller;
    private Transform cam;
    private float rotationX = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        cam = GetComponentInChildren<Camera>().transform;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Загружаем настройки чувствительности при старте
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
        // --- ДВИЖЕНИЕ ---
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 move = transform.right * h + transform.forward * v;
        controller.SimpleMove(move * moveSpeed);

        // --- ПОВОРОТ МЫШЬЮ ---
        // Используем mouseSensitivity напрямую (теперь это множитель от 0 до 2)
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * 100f * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * 100f * Time.deltaTime;

        transform.Rotate(Vector3.up * mouseX);

        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -80f, 80f);
        cam.localRotation = Quaternion.Euler(rotationX, 0f, 0f);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    // Публичный метод для обновления чувствительности извне
    public void UpdateSensitivity(float newSensitivity)
    {
        mouseSensitivity = newSensitivity;
        Debug.Log($"🔄 PlayerMovement: Чувствительность обновлена до {newSensitivity}");
    }
}