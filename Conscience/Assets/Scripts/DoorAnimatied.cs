using UnityEngine;

public class DoorAnimated : MonoBehaviour
{
    public Animator animator;
    private bool opened = false;

    void Awake()
    {
        if (animator == null)
            animator = GetComponentInParent<Animator>();
    }

    public void OnInteract()
    {
        if (opened) return;  
        opened = true;

        if (animator != null)
        {
            animator.SetTrigger("Open");
            Debug.Log("Door: Open trigger on " + name);
        }
    }
}