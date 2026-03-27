using UnityEngine;

public class DoorAnimated : MonoBehaviour
{
    public Animator animator;

    public AudioClip openSound;
    public AudioClip closeSound;

    private AudioSource audioSource;
    private bool isOpen = false;

    void Awake()
    {
        animator = GetComponentInParent<Animator>();
        audioSource = GetComponentInParent<AudioSource>();
    }

    public void OnInteract()
    {
        // Сбрасываем старые триггеры
        animator.ResetTrigger("Open");
        animator.ResetTrigger("Close");

        // Переключаем состояние двери
        isOpen = !isOpen;

        if (isOpen)
        {
            animator.SetTrigger("Open");
            if (openSound != null && audioSource != null)
                audioSource.PlayOneShot(openSound);

            Debug.Log("DoorAnimated: OPEN");
        }
        else
        {
            animator.SetTrigger("Close");
            if (closeSound != null && audioSource != null)
                audioSource.PlayOneShot(closeSound);

            Debug.Log("DoorAnimated: CLOSE");
        }
    }
}