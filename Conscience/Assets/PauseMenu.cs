using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public bool PauseGame;
    public GameObject pauseGameMenu;

    void Start()
    {
        // ”беждаемс€, что при запуске игры курсор скрыт
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // ћеню паузы можно открыть по Escape, но закрыть только через кнопку в меню
        if (Input.GetKeyDown(KeyCode.Escape) && !PauseGame)
        {
            Pause();
        }
    }

    public void Resume()
    {
        pauseGameMenu.SetActive(false);
        Time.timeScale = 1f;
        PauseGame = false;

        // —крываем курсор при возобновлении игры
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Pause()
    {
        pauseGameMenu.SetActive(true);
        Time.timeScale = 0f;
        PauseGame = true;

        // ѕоказываем курсор при открытии меню паузы
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;

        // ѕоказываем курсор перед загрузкой главного меню
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        SceneManager.LoadScene("Main Menu");
    }

}
