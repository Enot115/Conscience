using UnityEngine;

public class breatheMan : MonoBehaviour
{
    public Transform chest;
    public float speed = 1.5f;
    public float amount = 2f;

    private Quaternion startRot;

    void Start()
    {
        startRot = chest.localRotation;
    }

    void Update()
    {
        float angle = Mathf.Sin(Time.time * speed) * amount;
        chest.localRotation = startRot * Quaternion.Euler(angle, 0, 0);
    }
}
