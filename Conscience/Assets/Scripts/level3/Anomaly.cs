using UnityEngine;

public class Anomaly : MonoBehaviour
{
    public string anomalyName = "Странный предмет";
    private bool isFound = false;

    public void OnInteract()
    {
        if (!isFound)
        {
            isFound = true;
            Debug.Log("Аномалия найдена: " + anomalyName);

            // Сообщаем менеджеру, что мы нашли аномалию
            Object.FindFirstObjectByType<LevelManager>().MarkAnomalyFound(anomalyName);

            // Можно добавить эффект (например, звук или вспышку)
            // gameObject.SetActive(false); // Или просто скрыть её
        }
    }
}