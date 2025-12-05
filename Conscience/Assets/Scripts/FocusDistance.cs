using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class AutoFocusDOF : MonoBehaviour
{
    [SerializeField] private Volume volume;
    [SerializeField] private Camera cam;
    [SerializeField] private LayerMask focusMask = ~0;
    [SerializeField] private float defaultDistance = 10f;
    [SerializeField] private float smooth = 10f;

    private DepthOfField dof;
    private float current;

    void Awake()
    {
        if (!cam) cam = Camera.main;
        volume.profile.TryGet(out dof);
        current = defaultDistance;
    }

    void Update()
    {
        if (dof == null || cam == null) return;

        float target = defaultDistance;

        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, focusMask))
            target = hit.distance;

        current = Mathf.Lerp(current, target, 1f - Mathf.Exp(-smooth * Time.deltaTime));
        dof.focusDistance.value = current;
    }
}
