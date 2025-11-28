using UnityEngine;

public class DoorAnimated : MonoBehaviour
{
    public Animator animator;
    private bool opened = false;

    void Awake()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }

    public void OnInteract()
    {
        if (opened) return;   // если нужно открыть только один раз
        opened = true;
        if (animator != null)
        {
            animator.Play("DoorOpen",0,0f);
            //animator.SetTrigger("Open");
            Debug.Log("Door: Open triggered" + name);
        }
        else
        {
            Debug.LogWarning("DoorAnimated: нет Animator на " + name);
        }
    }
}