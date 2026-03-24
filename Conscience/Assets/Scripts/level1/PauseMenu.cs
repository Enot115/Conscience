using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseGameMenu;

    private bool isPaused = false;
    private Image screamerImage;
    private PlayerMovement playerMovement; // Ссылка на скрипт движения игрока

    void Start()
    {
        // Находим скрипт движения игрока
        playerMovement = FindObjectOfType<PlayerMovement>();

        // Находим изображение скримера
        screamerImage = FindObjectOfType<ScreamerTrigger>()?.GetComponent<Image>();

        // Убеждаемся, что меню паузы выключено при старте
        if (pauseGameMenu != null)
        {
            pauseGameMenu.SetActive(false);
        }

        // Начальное состояние курсора
        SetCursorState(false);
    }

    void Update()
    {
        // Обработка нажатия Escape для переключения паузы
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    // Переключение состояния паузы
    void TogglePause()
    {
        if (isPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    public void Resume()
    {
        if (pauseGameMenu != null)
        {
            pauseGameMenu.SetActive(false);
        }

        Time.timeScale = 1f;
        isPaused = false;

        // Скрываем курсор и блокируем его
        SetCursorState(false);

        // Восстанавливаем управление игроком (если нужно)
        if (playerMovement != null)
        {
            playerMovement.enabled = true;
        }

        // Убеждаемся, что скример не блокирует нажатия
        DisableScreamerRaycast(true);
    }

    public void Pause()
    {
        if (pauseGameMenu != null)
        {
            pauseGameMenu.SetActive(true);
        }

        Time.timeScale = 0f;
        isPaused = true;

        // Показываем курсор при открытии меню паузы
        SetCursorState(true);

        // Отключаем управление игроком
        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }

        // Убеждаемся, что скример не блокирует кнопки в меню
        DisableScreamerRaycast(false);
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        isPaused = false;

        // Показываем курсор перед загрузкой главного меню
        SetCursorState(true);

        SceneManager.LoadScene("Main Menu");
    }

    // Метод для управления курсором
    void SetCursorState(bool showCursor)
    {
        if (showCursor)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    // Метод для отключения raycast у скримера
    void DisableScreamerRaycast(bool enablePlayerControl)
    {
        // Отключаем raycast у всех изображений в ScreamerCanvas
        GameObject screamerCanvas = GameObject.Find("ScreamerCanvas");
        if (screamerCanvas != null)
        {
            Image[] images = screamerCanvas.GetComponentsInChildren<Image>();
            foreach (Image img in images)
            {
                img.raycastTarget = false;
            }
        }

        // Отдельно обрабатываем скример
        if (screamerImage != null)
        {
            screamerImage.raycastTarget = false;
        }
    }

    // Метод для принудительного сброса состояния
    public void ResetPauseState()
    {
        if (isPaused)
        {
            Resume();
        }
    }

    // Свойство для проверки состояния паузы
    public bool IsPaused
    {
        get { return isPaused; }
    }
}