using UnityEngine;

public class PlayerFootsteps : MonoBehaviour
{
    [Header("Компоненты")]
    public CharacterController controller;
    public AudioSource source;
    public AudioClip[] stepClips;

    [Header("Настройки")]
    public float stepInterval = 0.5f;
    public float runningMultiplier = 0.7f;

    private float timer;
    private bool wasMoving = false;

    void Awake()
    {
        if (controller == null) controller = GetComponent<CharacterController>();
        if (source == null) source = GetComponent<AudioSource>();

        if (source == null)
        {
            source = gameObject.AddComponent<AudioSource>();
            Debug.Log(" Создан новый AudioSource");
        }

        source.spatialBlend = 1f;
        source.volume = 0.8f;

        if (stepClips == null || stepClips.Length == 0)
        {
            Debug.LogError(" Добавьте аудиоклипы в Step Clips!");
        }
    }

    void Update()
    {
        // Проверка на паузу
        PauseMenu pauseMenu = FindObjectOfType<PauseMenu>();
        if (pauseMenu != null && pauseMenu.IsPaused) return;

        // ТОЛЬКО ПРОВЕРКА НАЖАТИЯ КЛАВИШ
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        bool isMoving = (Mathf.Abs(horizontal) + Mathf.Abs(vertical)) > 0.01f;

        // Если игрок не нажимает клавиши движения - НЕТ ЗВУКА
        if (!isMoving)
        {
            if (wasMoving)
            {
                Debug.Log(" Клавиши отпущены, звук остановлен");
                wasMoving = false;
            }
            timer = 0f;
            return;
        }

        // Игрок нажимает клавиши
        if (!wasMoving)
        {
            Debug.Log(" Нажаты клавиши движения, начинаем шаги");
            wasMoving = true;
        }

        // Проверка бега
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float interval = stepInterval;
        if (isRunning) interval *= runningMultiplier;

        timer += Time.deltaTime;

        if (timer >= interval)
        {
            timer = 0f;
            PlayStep();
        }
    }

    void PlayStep()
    {
        if (source == null || stepClips == null || stepClips.Length == 0) return;

        var validClips = System.Array.FindAll(stepClips, clip => clip != null);
        if (validClips.Length == 0) return;

        AudioClip clip = validClips[Random.Range(0, validClips.Length)];
        source.pitch = Random.Range(0.9f, 1.1f);
        source.volume = 0.6f;
        source.PlayOneShot(clip);

 
    }
}