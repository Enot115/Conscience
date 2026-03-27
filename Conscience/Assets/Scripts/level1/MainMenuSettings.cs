using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenuSettings : MonoBehaviour
{
    [Header("Sensitivity Settings")]
    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private TextMeshProUGUI sensitivityValueText;

    [Header("Volume Settings")]
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private TextMeshProUGUI volumeValueText;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource menuMusicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("References")]
    [SerializeField] private PlayerSettingsLoader settingsLoader;

    private void Start()
    {
        Debug.Log("=== MainMenuSettings: Start ===");

        // Загружаем сохраненные настройки
        LoadSettings();

        // Добавляем слушатели событий
        sensitivitySlider.onValueChanged.AddListener(OnSensitivityChanged);
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);

        // Находим загрузчик настроек, если не назначен
        if (settingsLoader == null)
        {
            settingsLoader = FindObjectOfType<PlayerSettingsLoader>();
        }
    }

    private void OnSensitivityChanged(float value)
    {
        float roundedValue = Mathf.Round(value * 100f) / 100f;

        if (sensitivityValueText != null)
            sensitivityValueText.text = roundedValue.ToString("F2");

        PlayerPrefs.SetFloat("Sensitivity", roundedValue);
        PlayerPrefs.Save();

        Debug.Log($"✅ Сохранено в главном меню: Sensitivity = {roundedValue}");

        // Применяем настройки к игре, если загрузчик существует
        if (settingsLoader != null)
        {
            settingsLoader.LoadAndApplySettings();
        }
    }

    private void OnVolumeChanged(float value)
    {
        float roundedValue = Mathf.Round(value * 100f) / 100f;

        if (volumeValueText != null)
            volumeValueText.text = Mathf.RoundToInt(roundedValue * 100) + "%";

        if (menuMusicSource != null)
            menuMusicSource.volume = roundedValue;

        if (sfxSource != null)
            sfxSource.volume = roundedValue;

        AudioListener.volume = roundedValue;

        PlayerPrefs.SetFloat("Volume", roundedValue);
        PlayerPrefs.Save();

        Debug.Log($"✅ Сохранено в главном меню: Volume = {roundedValue}");

        // Применяем настройки к игре, если загрузчик существует
        if (settingsLoader != null)
        {
            settingsLoader.LoadAndApplySettings();
        }
    }

    private void LoadSettings()
    {
        float sensitivity = PlayerPrefs.GetFloat("Sensitivity", 1f);
        float volume = PlayerPrefs.GetFloat("Volume", 1f);

        Debug.Log($"=== ЗАГРУЗКА НАСТРОЕК В МЕНЮ ===");
        Debug.Log($"Загружено из PlayerPrefs - Sensitivity: {sensitivity}, Volume: {volume}");

        sensitivitySlider.value = sensitivity;
        volumeSlider.value = volume;

        if (sensitivityValueText != null)
            sensitivityValueText.text = sensitivity.ToString("F2");

        if (volumeValueText != null)
            volumeValueText.text = Mathf.RoundToInt(volume * 100) + "%";

        AudioListener.volume = volume;
    }

    public void PlayTestSound()
    {
        if (sfxSource != null && sfxSource.clip != null)
            sfxSource.PlayOneShot(sfxSource.clip);

        Debug.Log("▶️ Воспроизведен тестовый звук");
    }

    public void ApplySettingsToGame()
    {
        Debug.Log("🔄 Принудительное применение настроек к игре");
        PlayerPrefs.Save();

        if (settingsLoader != null)
        {
            settingsLoader.LoadAndApplySettings();
        }
        else
        {
            Debug.LogWarning("PlayerSettingsLoader не найден, ищем...");
            settingsLoader = FindObjectOfType<PlayerSettingsLoader>();
            if (settingsLoader != null)
            {
                settingsLoader.LoadAndApplySettings();
            }
        }
    }

    private void OnDestroy()
    {
        sensitivitySlider?.onValueChanged.RemoveListener(OnSensitivityChanged);
        volumeSlider?.onValueChanged.RemoveListener(OnVolumeChanged);
    }
}