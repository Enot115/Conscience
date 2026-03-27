using UnityEngine;

public class PlayerSettingsLoader : MonoBehaviour
{
    [Header("Player Components")]
    [SerializeField] private PlayerMovement playerMovement;

    [Header("Audio")]
    [SerializeField] private AudioSource[] gameAudioSources;

    void Start()
    {
        Debug.Log("=== PlayerSettingsLoader: СТАРТ ===");

        // Автоматически ищем PlayerMovement, если не назначен
        if (playerMovement == null)
        {
            playerMovement = FindObjectOfType<PlayerMovement>();
            if (playerMovement != null)
            {
                Debug.Log($"✅ PlayerMovement автоматически найден: {playerMovement.gameObject.name}");
            }
        }

        LoadAndApplySettings();
    }

    void OnEnable()
    {
        // Загружаем настройки при каждом включении объекта
        LoadAndApplySettings();
    }

    public void LoadAndApplySettings()
    {
        float sensitivity = PlayerPrefs.GetFloat("Sensitivity", 1f);
        float volume = PlayerPrefs.GetFloat("Volume", 1f);

        // Добавьте проверку на NaN или некорректные значения
        if (float.IsNaN(volume) || volume < 0 || volume > 1)
        {
            volume = 1f;
            Debug.LogWarning("Некорректное значение громкости, установлено 1");
        }

        Debug.Log($"Загружена громкость: {volume}");
        ApplyVolume(volume);
    }

    private void ApplySensitivity(float value)
    {
        Debug.Log($"=== ПРИМЕНЕНИЕ ЧУВСТВИТЕЛЬНОСТИ: {value} ===");

        if (playerMovement == null)
        {
            Debug.LogError("❌ PlayerMovement = NULL! Ищем...");
            playerMovement = FindObjectOfType<PlayerMovement>();

            if (playerMovement == null)
            {
                Debug.LogError("❌ PlayerMovement НЕ НАЙДЕН в сцене!");
                return;
            }
        }

        Debug.Log($"✅ PlayerMovement найден на объекте: {playerMovement.gameObject.name}");

        // Прямое присвоение значения через публичный метод
        playerMovement.UpdateSensitivity(value);

        // Также можно присвоить напрямую поле, если нужно
        // playerMovement.mouseSensitivity = value;

        Debug.Log($"✅ Чувствительность применена: {value}");
    }

    private void ApplyVolume(float value)
    {
        Debug.Log($"Применение громкости: {value}");

        // Применяем к AudioListener
        AudioListener.volume = value;

        // Применяем к отдельным источникам, если они есть
        if (gameAudioSources != null && gameAudioSources.Length > 0)
        {
            foreach (AudioSource source in gameAudioSources)
            {
                if (source != null)
                {
                    source.volume = value;
                    Debug.Log($"✅ Громкость применена к {source.gameObject.name}");
                }
            }
        }
        else
        {
            Debug.Log("✅ Громкость применена через AudioListener");
        }
    }

    // Метод для принудительной перезагрузки
    public void ReloadSettings()
    {
        Debug.Log("🔄 Принудительная перезагрузка настроек");
        LoadAndApplySettings();
    }
}