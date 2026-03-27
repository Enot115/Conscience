using UnityEngine;

public class AnomalyWhiteNoise : MonoBehaviour
{
    public AudioSource noise;
    public float maxVolume = 0.5f;
    public float speed = 2f;
    bool inside;

    void Awake()
    {
        if (noise == null) noise = GetComponent<AudioSource>();
        if (noise != null) noise.volume = 0f;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) inside = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) inside = false;
    }

    void Update()
    {
        if (noise == null) return;
        float target = inside ? maxVolume : 0f;
        noise.volume = Mathf.Lerp(noise.volume, target, Time.deltaTime * speed);
    }
}
