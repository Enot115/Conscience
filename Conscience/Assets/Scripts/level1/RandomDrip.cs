using UnityEngine;

public class RandomDrip : MonoBehaviour
{
    public AudioSource source;
    public float minDelay = 1.2f;
    public float maxDelay = 3.5f;

    public float minVolume = 0.15f;
    public float maxVolume = 0.30f;

    public float minPitch = 0.95f;
    public float maxPitch = 1.05f;

    float timer;

    void Awake()
    {
        if (source == null) source = GetComponent<AudioSource>();
        timer = Random.Range(minDelay, maxDelay);
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            source.pitch = Random.Range(minPitch, maxPitch);
            source.volume = Random.Range(minVolume, maxVolume);
            source.PlayOneShot(source.clip);
            timer = Random.Range(minDelay, maxDelay);
        }
    }
}