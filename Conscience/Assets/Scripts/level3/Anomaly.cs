using UnityEngine;

[RequireComponent(typeof(AudioSource))] // Автоматически добавит AudioSource при назначении скрипта
public class Anomaly : MonoBehaviour
{
    public string anomalyName = "Странный предмет";
    private AudioSource myAudio;
    private bool isFound = false;
    private LevelManager manager;

    void Start()
    {
        myAudio = GetComponent<AudioSource>();

        // Настраиваем звук на бесконечный повтор
        myAudio.loop = true;
        myAudio.Play(); // Запускаем звук сразу при появлении вагона
        manager = Object.FindFirstObjectByType<LevelManager>();
    }

    public void OnInteract()
    {
        if (!isFound)
        {
            isFound = true;

            // Останавливаем звук перед тем, как объект исчезнет
            if (myAudio != null) myAudio.Stop();

            // Передаем данные в менеджер
            Object.FindFirstObjectByType<LevelManager>().MarkAnomalyFound(anomalyName, this.gameObject, null);
        }
    }
}