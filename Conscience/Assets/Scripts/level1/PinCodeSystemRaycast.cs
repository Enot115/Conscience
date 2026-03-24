using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class PinCodeSystemRaycast : MonoBehaviour
{
    [Header("Pin Code Settings")]
    [SerializeField] private string correctPinCode = "0812";
    [SerializeField] private TextMeshPro displayText; // 3D Text
    [SerializeField] private TextMeshPro feedbackText; // 3D Text

    [Header("Next Level Settings")]
    [SerializeField] private string nextLevelName = "level 2";

    [Header("Raycast Settings")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float raycastDistance = 5f;
    [SerializeField] private LayerMask buttonLayer;

    [Header("Visual Settings")]
    [SerializeField] private GameObject pinCodePanel;
    [SerializeField] private Material activePanelMaterial;
    [SerializeField] private Material inactivePanelMaterial;

    private string currentInput = "";
    private bool isActive = false;
    private PinCodeButton3D currentButton = null;
    private Renderer panelRenderer;

    void Start()
    {
        // Находим камеру
        if (playerCamera == null)
            playerCamera = Camera.main;

        // Получаем рендерер панели
        if (pinCodePanel != null)
            panelRenderer = pinCodePanel.GetComponent<Renderer>();

        // Деактивируем систему при старте
        SetActiveState(false);
        UpdateDisplay();
    }

    void Update()
    {
        // Обработка нажатия E
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!isActive)
            {
                // Активируем систему
                SetActiveState(true);
                StartCoroutine(ShowFeedback("Введите код. Нажмите ESC для отмены.", Color.yellow));
            }
            else
            {
                // Если система активна, нажимаем на подсвеченную кнопку
                if (currentButton != null)
                {
                    currentButton.Press();
                }
            }
        }

        // Деактивация по ESC
        if (Input.GetKeyDown(KeyCode.Escape) && isActive)
        {
            SetActiveState(false);
            StartCoroutine(ShowFeedback("Ввод кода отменен.", Color.gray));
        }

        // Raycast для подсветки кнопок
        if (isActive)
        {
            RaycastForButtons();
        }
        else if (currentButton != null)
        {
            // Убираем подсветку
            currentButton.Unhighlight();
            currentButton = null;
        }
    }

    // Raycast для поиска кнопок
    private void RaycastForButtons()
    {
        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, raycastDistance, buttonLayer))
        {
            PinCodeButton3D button = hit.collider.GetComponent<PinCodeButton3D>();

            if (button != null)
            {
                if (currentButton != button)
                {
                    if (currentButton != null)
                        currentButton.Unhighlight();

                    currentButton = button;
                    currentButton.Highlight();
                }
            }
            else if (currentButton != null)
            {
                currentButton.Unhighlight();
                currentButton = null;
            }
        }
        else if (currentButton != null)
        {
            currentButton.Unhighlight();
            currentButton = null;
        }
    }

    // Активация/деактивация системы
    public void SetActiveState(bool active)
    {
        isActive = active;

        // Меняем внешний вид панели
        if (panelRenderer != null)
        {
            if (active && activePanelMaterial != null)
                panelRenderer.material = activePanelMaterial;
            else if (!active && inactivePanelMaterial != null)
                panelRenderer.material = inactivePanelMaterial;
        }

        // Очищаем ввод при деактивации
        if (!active)
        {
            ClearInput();

            if (currentButton != null)
            {
                currentButton.Unhighlight();
                currentButton = null;
            }

            // Скрываем курсор
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            // Показываем курсор
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    // Добавление цифры
    public void AddDigit(string digit)
    {
        if (!isActive) return;

        if (currentInput.Length < 4)
        {
            currentInput += digit;
            UpdateDisplay();

            StartCoroutine(ShowFeedback($"Введено: {currentInput.Length}/4", Color.cyan, 0.5f));

            // Автоматическая проверка при вводе 4 цифр
            if (currentInput.Length == 4)
            {
                CheckPinCode();
            }
        }
        else
        {
            StartCoroutine(ShowFeedback("Введено 4 цифры", Color.yellow));
        }
    }

    // Очистка ввода
    public void ClearInput()
    {
        currentInput = "";
        UpdateDisplay();

        if (feedbackText != null)
            feedbackText.text = "";
    }

    // Обновление отображения
    private void UpdateDisplay()
    {
        if (displayText != null)
        {
            string display = "";
            for (int i = 0; i < currentInput.Length; i++)
            {
                display += "•";
            }
            for (int i = currentInput.Length; i < 4; i++)
            {
                display += "_";
            }
            displayText.text = display;
        }
    }

    // Проверка пин-кода
    public void CheckPinCode()
    {
        if (!isActive) return;

        if (currentInput == correctPinCode)
        {
            StartCoroutine(ShowFeedback("Правильно! Загрузка...", Color.green));
            UnlockNextLevel();
        }
        else
        {
            StartCoroutine(ShowFeedback("Неверный код! Попробуйте снова.", Color.red));
            ClearInput();
        }
    }

    // Показать сообщение
    private IEnumerator ShowFeedback(string message, Color color, float duration = -1)
    {
        if (feedbackText != null)
        {
            float time = duration > 0 ? duration : 2f;
            feedbackText.text = message;
            feedbackText.color = color;
            yield return new WaitForSeconds(time);
            if (feedbackText.text == message)
                feedbackText.text = "";
        }
    }

    // Разблокировка следующего уровня
    private void UnlockNextLevel()
    {
        PlayerPrefs.SetInt("LevelCompleted", 1);
        PlayerPrefs.Save();

        if (!string.IsNullOrEmpty(nextLevelName))
        {
            SceneManager.LoadScene(nextLevelName);
        }
        else
        {
            Debug.LogError("Следующий уровень не указан!");
        }
    }

    // Проверка активна ли система
    public bool IsActive()
    {
        return isActive;
    }
}