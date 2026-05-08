using UnityEngine;

[RequireComponent(typeof(Animator))]
public class HeadLookAt : MonoBehaviour
{
    [Header("За кем следить")]
    public Transform target;

    [Header("Настройки взгляда")]
    [Range(0f, 1f)]
    public float lookWeight = 1.0f; // 1 - смотрит прямо на цель, 0 - не смотрит
    public float bodyWeight = 0.2f; // Насколько сильно поворачивается туловище
    public float headWeight = 0.9f; // Насколько сильно поворачивается голова
    public float eyesWeight = 1.0f; // Насколько сильно поворачиваются глаза

    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Специальный метод Unity для работы с IK (работает только если включен IK Pass!)
    void OnAnimatorIK(int layerIndex)
    {
        if (anim != null && target != null)
        {
            // Говорим аниматору, с какой силой смотреть
            anim.SetLookAtWeight(lookWeight, bodyWeight, headWeight, eyesWeight, 0.5f);

            // Задаем точку, КУДА смотреть
            anim.SetLookAtPosition(target.position);
        }
    }
}