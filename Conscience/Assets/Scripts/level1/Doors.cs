using UnityEngine;

public class Door : MonoBehaviour
{
    public bool isOpen = false;      // открыта или закрыта
    public float openAngle = 90f;    // угол, на который откроется дверь
    public float openSpeed = 3f;     // скорость поворота двери

    private Quaternion closedRotation;  // стартовое положение двери
    private Quaternion openRotation;    // положение, когда дверь открыта

    void Start()
    {
        // запоминаем позицию закрытой двери
        closedRotation = transform.rotation;

        // куда дверь должна повернуться
        openRotation = Quaternion.Euler(
            transform.eulerAngles + new Vector3(0f, openAngle, 0f)
        );
    }

    void Update()
    {
        // плавное вращение
        if (isOpen)
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                openRotation,
                Time.deltaTime * openSpeed
            );
        }
        else
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                closedRotation,
                Time.deltaTime * openSpeed
            );
        }
    }

    // вызывается, когда игрок нажимает E по двери
    public void OnInteract()
    {
        isOpen = !isOpen; // переключаем состояние
        Debug.Log($"Door [{name}] → {(isOpen ? "OPEN" : "CLOSED")}");
    }
}
