using UnityEngine;

public class CreepyPointer : MonoBehaviour
{
    [Header("Цель")]
    public Transform target;

    [Header("Кости Головы")]
    public Transform headBone;
    public Vector3 headOffset;

    [Header("Цепочка Руки")]
    public Transform shoulder;
    public Transform forearm;
    public Transform hand;
    public Vector3 armOffset;

    [Header("Пальцы: Указательный (Целится)")]
    public Transform[] indexFingers; // Фаланги указательного
    public Vector3 indexOffset;      // Насколько довернуть (оставь 0,0,0 если он и так прямой)

    [Header("Пальцы: Остальные (В кулак)")]
    public Transform[] otherFingers; // Средний, безымянный, мизинец
    public Vector3 othersBendOffset = new Vector3(80, 0, 0); // Угол сгиба (пробуй 80 по X, Y или Z)

    [Header("Настройки дистанции")]
    public float triggerDistance = 5f;
    public float lookSpeed = 2.5f;

    private Quaternion initHeadRot, initShoulderRot, initForearmRot, initHandRot;
    private Quaternion[] initIndexRots;
    private Quaternion[] initOtherRots;
    private float currentWeight = 0f;

    void Start()
    {
        if (headBone) initHeadRot = headBone.localRotation;
        if (shoulder) initShoulderRot = shoulder.localRotation;
        if (forearm) initForearmRot = forearm.localRotation;
        if (hand) initHandRot = hand.localRotation;

        // Запоминаем исходные позы пальцев
        initIndexRots = new Quaternion[indexFingers.Length];
        for (int i = 0; i < indexFingers.Length; i++)
            initIndexRots[i] = indexFingers[i].localRotation;

        initOtherRots = new Quaternion[otherFingers.Length];
        for (int i = 0; i < otherFingers.Length; i++)
            initOtherRots[i] = otherFingers[i].localRotation;
    }

    void LateUpdate()
    {
        if (!target) return;

        float distance = Vector3.Distance(transform.position, target.position);
        float targetWeight = (distance <= triggerDistance) ? 1f : 0f;
        currentWeight = Mathf.Lerp(currentWeight, targetWeight, Time.deltaTime * lookSpeed);

        if (currentWeight < 0.001f)
        {
            ResetToInitial();
            return;
        }

        // Голова
        if (headBone)
        {
            Vector3 headDir = target.position - headBone.position;
            Quaternion targetHeadWorld = Quaternion.LookRotation(headDir) * Quaternion.Euler(headOffset);
            headBone.rotation = Quaternion.Slerp(headBone.rotation, targetHeadWorld, currentWeight);
        }

        // Рука (плечо, локоть, кисть)
        if (shoulder)
        {
            Vector3 armDir = target.position - shoulder.position;
            Quaternion targetArmWorld = Quaternion.LookRotation(armDir) * Quaternion.Euler(armOffset);
            shoulder.rotation = Quaternion.Slerp(shoulder.rotation, targetArmWorld, currentWeight);

            if (forearm) forearm.localRotation = Quaternion.Slerp(forearm.localRotation, Quaternion.identity, currentWeight);
            if (hand) hand.localRotation = Quaternion.Slerp(hand.localRotation, Quaternion.identity, currentWeight);
        }

        // --- ПАЛЬЦЫ ---
        // Указательный палец (держим прямо или чуть довертываем)
        for (int i = 0; i < indexFingers.Length; i++)
        {
            Quaternion targetRot = initIndexRots[i] * Quaternion.Euler(indexOffset);
            indexFingers[i].localRotation = Quaternion.Slerp(indexFingers[i].localRotation, targetRot, currentWeight);
        }

        // Остальные пальцы (сгибаем в кулак)
        for (int i = 0; i < otherFingers.Length; i++)
        {
            Quaternion targetRot = initOtherRots[i] * Quaternion.Euler(othersBendOffset);
            otherFingers[i].localRotation = Quaternion.Slerp(otherFingers[i].localRotation, targetRot, currentWeight);
        }
    }

    void ResetToInitial()
    {
        if (headBone) headBone.localRotation = Quaternion.Slerp(headBone.localRotation, initHeadRot, Time.deltaTime * lookSpeed);
        if (shoulder) shoulder.localRotation = Quaternion.Slerp(shoulder.localRotation, initShoulderRot, Time.deltaTime * lookSpeed);
        if (forearm) forearm.localRotation = Quaternion.Slerp(forearm.localRotation, initForearmRot, Time.deltaTime * lookSpeed);
        if (hand) hand.localRotation = Quaternion.Slerp(hand.localRotation, initHandRot, Time.deltaTime * lookSpeed);

        for (int i = 0; i < indexFingers.Length; i++)
            indexFingers[i].localRotation = Quaternion.Slerp(indexFingers[i].localRotation, initIndexRots[i], Time.deltaTime * lookSpeed);

        for (int i = 0; i < otherFingers.Length; i++)
            otherFingers[i].localRotation = Quaternion.Slerp(otherFingers[i].localRotation, initOtherRots[i], Time.deltaTime * lookSpeed);
    }
}