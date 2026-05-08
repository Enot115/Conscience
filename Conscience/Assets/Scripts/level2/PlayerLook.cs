using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [Header("Настройки луча")]
    [Tooltip("Дальность взгляда")]
    public float maxDistance = 10f;

    [Tooltip("Слой, на котором висят картины")]
    public LayerMask pictureLayer; // Сюда в инспекторе выберешь слой "Picture"

    [Header("Отладка")]
    public bool showDebugRay = true; // Рисовать ли луч в редакторе

    private Camera _mainCamera; // Ссылка на главную камеру
    private PictureTrigger _currentPicture; // Картина, на которую смотрели в прошлом кадре

    void Start()
    {
        // Находим камеру игрока (она должна быть с тегом MainCamera)
        _mainCamera = Camera.main;
    }

    void Update()
    {
        ShootRayFromCamera();
    }

    void ShootRayFromCamera()
    {
        // Создаем луч: начало в позиции камеры, направление — вперед
        Ray ray = new Ray(_mainCamera.transform.position, _mainCamera.transform.forward);
        RaycastHit hit; // Сюда запишется информация о попадании

        // Визуализация луча в редакторе
        if (showDebugRay)
        {
            // Если ничего не бьет — рисуем красный луч на всю длину
            Debug.DrawRay(ray.origin, ray.direction * maxDistance, Color.red);
        }

        // Пускаем луч, проверяя ТОЛЬКО объекты на слое "pictureLayer"
        if (Physics.Raycast(ray, out hit, maxDistance, pictureLayer))
        {
            // Луч во что-то попал на нужном слое
            // Пытаемся найти на этом объекте компонент PictureTrigger
            PictureTrigger picture = hit.collider.GetComponent<PictureTrigger>();

            if (picture != null)
            {
                // Если это та же самая картина, что и в прошлом кадре — ничего не делаем
                if (picture != _currentPicture)
                {
                    // Картина новая! Вызываем её метод проигрывания звука.
                    picture.OnPlayerLookAt();

                    // Запоминаем, что теперь это "текущая" картина
                    _currentPicture = picture;
                }

                // Если включили отладку, перерисовываем луч зеленым до точки удара
                if (showDebugRay)
                {
                    Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.green);
                }
            }
        }
        else
        {
            // Луч ни во что не попал или попал не в слой картин.
            // Значит, мы больше ни на что не смотрим — обнуляем текущую картину.
            _currentPicture = null;
        }
    }
}