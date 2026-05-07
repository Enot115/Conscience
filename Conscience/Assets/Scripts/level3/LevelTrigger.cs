using UnityEngine;

public class LevelTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            LevelManager manager = Object.FindFirstObjectByType<LevelManager>();

            if (manager != null)
            {
                // Если мы в длинном поезде (уровень 4) — запускаем финальное видео!
                if (manager.currentLevel == 4)
                {
                    manager.PlayEndGameVideo();
                    return;
                }

                // Обычный переход для уровней 0, 1, 2, 3
                manager.GoToNextRoom();
            }
        }
    }
}