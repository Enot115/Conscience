using UnityEngine;

public class CameraBob : MonoBehaviour
{
    [Header("Links")]
    public Transform target; // usually the camera transform

    [Header("Walk Bob")]
    public float frequency = 1.7f;   // speed of bobbing
    public float amplitude = 0.04f;  // up/down amount
    public float sway = 0.02f;       // slight left/right

    [Header("Rotation Bob")]
    public float rotAmplitude = 1.2f; // degrees
    public float rotFrequency = 1.5f;

    [Header("Smoothing")]
    public float smooth = 10f;

    [Header("Movement detection")]
    public CharacterController controller; // optional
    public float minSpeed = 0.1f;

    Vector3 startLocalPos;
    Quaternion startLocalRot;
    float t;

    void Awake()
    {
        if (target == null) target = transform;
        startLocalPos = target.localPosition;
        startLocalRot = target.localRotation;
    }

    void Update()
    {
        bool moving = IsMoving();

        if (moving) t += Time.deltaTime;
        else t = Mathf.Lerp(t, 0f, Time.deltaTime * 4f); // slowly relax to idle

        float sin = Mathf.Sin(t * Mathf.PI * 2f * frequency);
        float cos = Mathf.Cos(t * Mathf.PI * 2f * frequency);

        Vector3 posOffset = new Vector3(cos * sway, Mathf.Abs(sin) * amplitude, 0f);

        float rSin = Mathf.Sin(t * Mathf.PI * 2f * rotFrequency);
        Quaternion rotOffset = Quaternion.Euler(0f, 0f, rSin * rotAmplitude);

        Vector3 desiredPos = startLocalPos + (moving ? posOffset : Vector3.zero);
        Quaternion desiredRot = startLocalRot * (moving ? rotOffset : Quaternion.identity);

        target.localPosition = Vector3.Lerp(target.localPosition, desiredPos, Time.deltaTime * smooth);
        target.localRotation = Quaternion.Slerp(target.localRotation, desiredRot, Time.deltaTime * smooth);
    }

    bool IsMoving()
    {
        if (controller != null)
        {
            Vector3 v = controller.velocity;
            v.y = 0f;
            return v.magnitude > minSpeed && controller.isGrounded;
        }

        // fallback: WASD check
        return (Mathf.Abs(Input.GetAxisRaw("Horizontal")) + Mathf.Abs(Input.GetAxisRaw("Vertical"))) > 0.01f;
    }
}