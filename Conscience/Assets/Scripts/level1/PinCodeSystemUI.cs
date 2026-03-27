using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class PinCodeSystemUI : MonoBehaviour
{
    [Header("Pin Code Settings")]
    [SerializeField] private string correctPinCode = "5252";
    [SerializeField] private TMP_Text displayText;
    [SerializeField] private TMP_Text feedbackText;
    [SerializeField] private float feedbackDuration = 2f;

    [Header("Next Level Settings")]
    [SerializeField] private string nextLevelName = "level 2";

    [Header("UI Settings")]
    [SerializeField] private CanvasGroup canvasGroup; // Для управления активностью

    private string currentInput = "";
    private bool isActive = false;

    void Start()
    {
        if (feedbackText != null)
            feedbackText.text = "";

        // Делаем панель неактивной при старте
        SetActiveState(false);
        UpdateDisplay();
    }

    void Update()
    {
        // Нажатие E для активации/ввода
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!isActive)
            {
                SetActiveState(true);
                StartCoroutine(ShowFeedback("Введите код. Нажмите F для отмены.", Color.red));
            }
            else
            {
                // Если система активна - ничего не делаем
                // Ввод через кнопки UI
            }
        }

        // Деактивация по ESC
        if (Input.GetKeyDown(KeyCode.F) && isActive)
        {
            SetActiveState(false);
            StartCoroutine(ShowFeedback("Ввод кода отменен.", Color.gray));
        }

        // Поддержка ввода с клавиатуры (для тестирования)
        if (isActive)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0)) AddDigit("0");
            else if (Input.GetKeyDown(KeyCode.Alpha1)) AddDigit("1");
            else if (Input.GetKeyDown(KeyCode.Alpha2)) AddDigit("2");
            else if (Input.GetKeyDown(KeyCode.Alpha3)) AddDigit("3");
            else if (Input.GetKeyDown(KeyCode.Alpha4)) AddDigit("4");
            else if (Input.GetKeyDown(KeyCode.Alpha5)) AddDigit("5");
            else if (Input.GetKeyDown(KeyCode.Alpha6)) AddDigit("6");
            else if (Input.GetKeyDown(KeyCode.Alpha7)) AddDigit("7");
            else if (Input.GetKeyDown(KeyCode.Alpha8)) AddDigit("8");
            else if (Input.GetKeyDown(KeyCode.Alpha9)) AddDigit("9");
            else if (Input.GetKeyDown(KeyCode.Backspace)) RemoveLastDigit();
            else if (Input.GetKeyDown(KeyCode.Return)) CheckPinCode();
        }
    }

    public void SetActiveState(bool active)
    {
        isActive = active;

        // Визуально показываем активность
        if (canvasGroup != null)
        {
            canvasGroup.interactable = active;
            canvasGroup.blocksRaycasts = active;
            canvasGroup.alpha = active ? 1f : 0.5f;
        }

        if (!active)
        {
            ClearInput();
        }
    }

    public void AddDigit(string digit)
    {
        if (!isActive) return;

        if (currentInput.Length < 4)
        {
            currentInput += digit;
            UpdateDisplay();

            StartCoroutine(ShowFeedback($"Введено: {currentInput.Length}/4", Color.cyan, 0.5f));

            if (currentInput.Length == 4)
            {
                CheckPinCode();
            }
        }
    }

    public void RemoveLastDigit()
    {
        if (!isActive) return;

        if (currentInput.Length > 0)
        {
            currentInput = currentInput.Substring(0, currentInput.Length - 1);
            UpdateDisplay();
        }
    }

    public void ClearInput()
    {
        currentInput = "";
        UpdateDisplay();

        if (feedbackText != null)
            feedbackText.text = "";
    }

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

    private IEnumerator ShowFeedback(string message, Color color, float duration = -1)
    {
        if (feedbackText != null)
        {
            float time = duration > 0 ? duration : feedbackDuration;
            feedbackText.text = message;
            feedbackText.color = color;
            yield return new WaitForSeconds(time);
            if (feedbackText.text == message)
                feedbackText.text = "";
        }
    }

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

    public bool IsActive()
    {
        return isActive;
    }
}