using UnityEngine;

public class PlayerFootsteps : MonoBehaviour
{
    public CharacterController controller;
    public AudioSource source;
    public AudioClip[] stepClips;

    public float stepInterval = 0.5f;      // базовый интервал
    public float runningMultiplier = 0.7f; // если бежишь — шаги чаще (меньше интервал)
    public float minMoveSpeed = 1.0f;

    float timer;

    void Awake()
    {
        if (controller == null) controller = GetComponent<CharacterController>();
        if (source == null) source = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (controller == null || source == null || stepClips == null || stepClips.Length == 0)
            return;

        // скорость по плоскости (X,Z)
        Vector3 v = controller.velocity;
        float speed = new Vector3(v.x, 0f, v.z).magnitude;

        
        if (!controller.isGrounded || speed < minMoveSpeed)
        {
            timer = 0f;
            return;
        }

       
        float interval = stepInterval;
        if (speed > 3.5f) interval *= runningMultiplier;

        timer += Time.deltaTime;
        if (timer >= interval)
        {
            timer = 0f;
            PlayStep();
        }
    }

    void PlayStep()
    {
        int i = Random.Range(0, stepClips.Length);
        source.pitch = Random.Range(0.95f, 1.05f);
        source.volume = Random.Range(0.15f, 0.25f);
        source.PlayOneShot(stepClips[i]);
    }
}
