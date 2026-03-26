using UnityEngine;
using UnityEngine.Video;

public class TVInteract : MonoBehaviour
{
    [Header("Настройки")]
    [SerializeField] private float interactionDistance = 3f;
    [SerializeField] private KeyCode interactionKey = KeyCode.E;
    [SerializeField] private Transform playerCamera;
    [SerializeField] private GameObject tvScreen;
    [SerializeField] private Material tvOnMaterial;

    private VideoPlayer videoPlayer;
    private MeshRenderer screenRenderer;
    private bool hasPlayed = false;

    void Start()
    {
        // Находим экран, если не назначен вручную
        if (tvScreen == null)
            tvScreen = transform.Find("TV_Screen")?.gameObject;

        if (tvScreen == null)
        {
            Debug.LogError("TV_Screen не найден!");
            return;
        }

        // Получаем компоненты с экрана
        videoPlayer = tvScreen.GetComponent<VideoPlayer>();
        screenRenderer = tvScreen.GetComponent<MeshRenderer>();

        if (videoPlayer == null)
            Debug.LogError("Video Player не найден на TV_Screen!");

        if (screenRenderer == null)
            Debug.LogError("Mesh Renderer не найден на TV_Screen!");
    }

    void Update()
    {
        if (hasPlayed) return;

        if (Input.GetKeyDown(interactionKey))
        {
            // Создаем луч от камеры
            Ray ray = new Ray(playerCamera.position, playerCamera.forward);
            RaycastHit hit;

            // Проверяем, попал ли луч в телевизор
            if (Physics.Raycast(ray, out hit, interactionDistance))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    PlayVideo();
                }
            }
        }
    }

    void PlayVideo()
    {
        if (videoPlayer == null) return;

        Debug.Log("▶️ Включаем телевизор!");

        // Меняем материал на включенный
        if (tvOnMaterial != null && screenRenderer != null)
            screenRenderer.material = tvOnMaterial;

        // Запускаем видео
        videoPlayer.Play();
        hasPlayed = true;
    }
}