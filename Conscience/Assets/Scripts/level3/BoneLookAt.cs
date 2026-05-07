using UnityEngine;

public class BoneLookAt : MonoBehaviour
{
    [Header("Объекты")]
    public Transform target;   // Камера игрока
    public Transform headBone; // Кость головы мужика

    [Header("Настройки зрения")]
    public float maxDistance = 5f; // На каком расстоянии он начинает следить
    public float maxAngle = 70f;   // Угол обзора (чтобы не сворачивал шею за спину)
    public float lookSpeed = 3f;   // Скорость поворота головы (меньше = страшнее/медленнее)

    private Quaternion defaultLocalRotation; // Исходное положение головы

    void Start()
    {
        // Запоминаем, как голова стояла изначально, чтобы возвращать её на место
        if (headBone != null)
        {
            defaultLocalRotation = headBone.localRotation;
        }
    }

    void LateUpdate()
    {
        if (target == null || headBone == null) return;

        // Вычисляем направление на игрока и дистанцию
        Vector3 directionToPlayer = target.position - headBone.position;
        float distance = Vector3.Distance(transform.position, target.position);

        // Вычисляем угол между направлением туловища и направлением на игрока
        float angle = Vector3.Angle(transform.forward, directionToPlayer);

        // Если игрок достаточно близко И находится спереди (в поле зрения)
        if (distance <= maxDistance && angle <= maxAngle)
        {
            // Вычисляем нужный поворот к игроку
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);

            // Если голова смотрит боком/затылком, раскомментируй строку ниже и поменяй нули
            // targetRotation *= Quaternion.Euler(0, 0, 0); 

            // Плавно поворачиваем голову к игроку (Slerp делает переход мягким)
            headBone.rotation = Quaternion.Slerp(headBone.rotation, targetRotation, Time.deltaTime * lookSpeed);
        }
        else
        {
            // Если игрок ушел слишком далеко или зашел за спину — плавно возвращаем голову прямо
            headBone.localRotation = Quaternion.Slerp(headBone.localRotation, defaultLocalRotation, Time.deltaTime * lookSpeed);
        }
    }
}