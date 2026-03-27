using UnityEngine;

public class LevelTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Ищем наш менеджер и запускаем переход
            Object.FindFirstObjectByType<LevelManager>().GoToNextRoom();
        }
    }
}