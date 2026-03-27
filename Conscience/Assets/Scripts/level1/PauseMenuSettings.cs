using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PauseMenuSettings : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject mainButtonsPanel;

    [Header("Sensitivity")]
    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private TextMeshProUGUI sensitivityValueText;

    [Header("Volume")]
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private TextMeshProUGUI volumeValueText;

    private PauseMenu pauseMenu;

    void Start()
    {
        pauseMenu = GetComponent<PauseMenu>();

        // Загружаем сохраненные настройки
        LoadSettings();

        // Добавляем слушатели
        if (sensitivitySlider != null)
            sensitivitySlider.onValueChanged.AddListener(OnSensitivityChanged);

        if (volumeSlider != null)
            volumeSlider.onValueChanged.AddListener(OnVolumeChanged);

        // Скрываем панель настроек
        if (settingsPanel != null)
            settingsPanel.SetActive(false);
    }

    public void OpenSettings()
    {
        Debug.Log("=== ОТКРЫТИЕ НАСТРОЕК ===");

        if (settingsPanel != null)
        {
            settingsPanel.SetActive(true);
            Debug.Log("SettingsPanel активирован");
        }

        if (mainButtonsPanel != null)
        {
            mainButtonsPanel.SetActive(false);
            Debug.Log("MainButtonsPanel скрыт");
        }

        LoadSettings();
    }

    public void CloseSettings()
    {
        Debug.Log("=== ЗАКРЫТИЕ НАСТРОЕК ===");

        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
            Debug.Log("SettingsPanel скрыт");
        }

        if (mainButtonsPanel != null)
        {
            mainButtonsPanel.SetActive(true);
            Debug.Log("MainButtonsPanel показан");
        }

        PlayerPrefs.Save();
    }

    private void OnSensitivityChanged(float value)
    {
        float roundedValue = Mathf.Round(value * 100f) / 100f;

        if (sensitivityValueText != null)
            sensitivityValueText.text = roundedValue.ToString("F2");

        PlayerPrefs.SetFloat("Sensitivity", roundedValue);
        PlayerPrefs.Save();

        Debug.Log($"Чувствительность изменена: {roundedValue}");

        // Применяем к игроку
        ApplySensitivity(roundedValue);
    }

    private void OnVolumeChanged(float value)
    {
        float roundedValue = Mathf.Round(value * 100f) / 100f;

        if (volumeValueText != null)
            volumeValueText.text = Mathf.RoundToInt(roundedValue * 100) + "%";

        AudioListener.volume = roundedValue;

        PlayerPrefs.SetFloat("Volume", roundedValue);
        PlayerPrefs.Save();

        Debug.Log($"Громкость изменена: {roundedValue}");
    }

    private void LoadSettings()
    {
        float sensitivity = PlayerPrefs.GetFloat("Sensitivity", 1f);
        float volume = PlayerPrefs.GetFloat("Volume", 1f);

        if (sensitivitySlider != null)
            sensitivitySlider.value = sensitivity;

        if (volumeSlider != null)
            volumeSlider.value = volume;

        if (sensitivityValueText != null)
            sensitivityValueText.text = sensitivity.ToString("F2");

        if (volumeValueText != null)
            volumeValueText.text = Mathf.RoundToInt(volume * 100) + "%";

        AudioListener.volume = volume;

        Debug.Log($"Настройки загружены: S={sensitivity}, V={volume}");
    }

    private void ApplySensitivity(float value)
    {
        PlayerMovement playerMovement = FindObjectOfType<PlayerMovement>();
        if (playerMovement != null)
        {
            playerMovement.UpdateSensitivity(value);
            Debug.Log($"Чувствительность применена к PlayerMovement: {value}");
        }
        else
        {
            Debug.LogWarning("PlayerMovement не найден!");
        }
    }
}