using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public bool PauseGame;
    public GameObject pauseGameMenu;

    // Добавляем ссылку на скример (если нужно)
    private Image screamerImage;

    void Start()
    {
        // Убеждаемся, что при запуске игры курсор скрыт
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // Находим изображение скримера, чтобы управлять им
        screamerImage = GameObject.FindObjectOfType<ScreamerTrigger>()?.GetComponent<Image>();

        // Убеждаемся, что скример не блокирует нажатия
        if (screamerImage != null)
        {
            screamerImage.raycastTarget = false;
        }

        // Проверяем, что меню паузы выключено при старте
        if (pauseGameMenu != null)
        {
            pauseGameMenu.SetActive(false);
        }
    }

    void Update()
    {
        // Меню паузы можно открыть по Escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!PauseGame)
            {
                Pause();
            }
            // Убираем автоматическое закрытие по Escape
            // Теперь закрываем только через кнопку Resume
        }
    }

    public void Resume()
    {
        if (pauseGameMenu != null)
        {
            pauseGameMenu.SetActive(false);
        }

        Time.timeScale = 1f;
        PauseGame = false;

        // Скрываем курсор при возобновлении игры
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // Убеждаемся, что скример не блокирует нажатия
        if (screamerImage != null)
        {
            screamerImage.raycastTarget = false;
        }
    }

    public void Pause()
    {
        if (pauseGameMenu != null)
        {
            pauseGameMenu.SetActive(true);
        }

        Time.timeScale = 0f;
        PauseGame = true;

        // Показываем курсор при открытии меню паузы
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // Убеждаемся, что скример не блокирует кнопки в меню
        if (screamerImage != null)
        {
            screamerImage.raycastTarget = false;
        }

        // Дополнительная проверка: отключаем raycastTarget у всех изображений в ScreamerCanvas
        GameObject screamerCanvas = GameObject.Find("ScreamerCanvas");
        if (screamerCanvas != null)
        {
            Image[] images = screamerCanvas.GetComponentsInChildren<Image>();
            foreach (Image img in images)
            {
                img.raycastTarget = false;
            }
        }
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;

        // Показываем курсор перед загрузкой главного меню
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        SceneManager.LoadScene("Main Menu");
    }

    // Метод для принудительного сброса состояния (можно вызвать из другого скрипта)
    public void ResetPauseState()
    {
        if (PauseGame)
        {
            Resume();
        }
    }
}