using UnityEngine;

public class TrainFlow : MonoBehaviour
{
    public float fastSpeed = 4f;     // скорость мелкой тряски (тук-тук)
    public float slowSpeed = 0.8f;   // скорость основного качания

    public float fastAmount = 0.008f; // амплитуда мелкой тряски
    public float slowAmount = 0.03f;  // амплитуда плавного качания

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.localPosition;
    }

    void Update()
    {
        float fast = Mathf.Sin(Time.time * fastSpeed) * fastAmount;
        float slow = Mathf.Sin(Time.time * slowSpeed) * slowAmount;

        transform.localPosition = startPos + new Vector3(fast + slow, fast * 0.5f, 0);
    }
}
