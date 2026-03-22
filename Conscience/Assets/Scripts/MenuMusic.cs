using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuMusic : MonoBehaviour
{
    private AudioSource audioSource;

    void Start()
    {
        // Получаем компонент Audio Source
        audioSource = GetComponent<AudioSource>();

        // Запускаем музыку в меню
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    // Этот метод вызывается при загрузке уровня
    public void StopMusicAndLoadLevel()
    {
        // Останавливаем музыку
        if (audioSource != null)
        {
            audioSource.Stop();
        }

        // Загружаем уровень (индекс 1 или название сцены)
        SceneManager.LoadScene(1); // или "GameLevel"
    }
}