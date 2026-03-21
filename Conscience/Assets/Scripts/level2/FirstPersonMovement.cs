using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FirstPersonMovement : MonoBehaviour
{
    [Header("��������")]
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float jumpForce = 5f;
    public float groundCheckDistance = 0.5f;

    [Header("��������� ����")]
    public float mouseSensitivity = 2f;
    public float maxLookAngle = 80f; // ������������ ���� �������� ������ �����/����

    private Rigidbody rb;
    private Transform cameraTransform;
    private float verticalRotation = 0f;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // ������� ������ ������ �������� ��������
        cameraTransform = GetComponentInChildren<Camera>().transform;

        // ��������� ������ � ������ ������ � ������ ��� ���������
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        HandleMouseLook();
        HandleJump();

        // Дополнительная проверка на земле через скорость
        if (rb.linearVelocity.y == 0 && !isGrounded)
        {
            CheckGround(); // Принудительная проверка
        }
    }

    void FixedUpdate()
    {
        // 3. �������� (������) � FixedUpdate
        HandleMovement();

        // 4. �������� �����
        CheckGround();
    }

    void HandleMouseLook()
    {
        // �������� �������� ����
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // ������� ��������� �� ����������� (Yaw)
        transform.Rotate(Vector3.up * mouseX);

        // ������� ������ �� ��������� (Pitch) � ������������
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -maxLookAngle, maxLookAngle);
        cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
    }

    void HandleMovement()
    {
        // �������� ���� � ���������� (WASD)
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        // ������� ������ ����������� ������������ �������� ���������
        Vector3 moveDirection = (transform.right * moveHorizontal + transform.forward * moveVertical).normalized;

        // ���������� �������� (��� �� Shift)
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

        // ������� ������ �������� � ��������� XZ (��� ������� �� Y)
        Vector3 velocity = moveDirection * currentSpeed;

        // ��������� ������� ������������ �������� (���������� � ������)
        velocity.y = rb.linearVelocity.y;

        // ��������� �������� � Rigidbody
        rb.linearVelocity = velocity;
    }

    void HandleJump()
    {
        bool spacePressed = Input.GetKeyDown(KeyCode.Space);

        if (spacePressed)
        {
            Debug.Log($"Прыжок нажат! isGrounded = {isGrounded}");

            if (isGrounded)
            {
                Debug.Log("Прыгаем!");
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
            else
            {
                Debug.Log("Не на земле!");
            }
        }
    }

    void CheckGround()
    {
        float sphereRadius = 0.4f; // Радиус сферы
        float sphereDistance = 0.6f; // Дистанция проверки

        RaycastHit hit;
        Vector3 origin = transform.position + Vector3.up * sphereRadius;

        if (Physics.SphereCast(origin, sphereRadius, Vector3.down, out hit, sphereDistance))
        {
            isGrounded = true;
            Debug.Log($"Земля: {hit.collider.gameObject.name}, расстояние: {hit.distance}");
        }
        else
        {
            isGrounded = false;
        }

        // Визуализация сферы
        Debug.DrawRay(origin, Vector3.down * sphereDistance, Color.yellow);
    }
}