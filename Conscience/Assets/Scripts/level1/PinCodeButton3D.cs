using UnityEngine;
using System.Collections;

public class PinCodeButton3D : MonoBehaviour
{
    [Header("Button Settings")]
    [SerializeField] private string digit;
    [SerializeField] private PinCodeSystemRaycast pinCodeSystem;

    [Header("Visual Settings")]
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material highlightMaterial;
    [SerializeField] private Color pressedColor = Color.green;

    [Header("Audio")]
    [SerializeField] private AudioClip clickSound;

    private MeshRenderer meshRenderer;
    private Material originalMaterial;
    private bool isHighlighted = false;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            originalMaterial = meshRenderer.material;
            if (defaultMaterial == null)
                defaultMaterial = originalMaterial;
        }
    }

    public void Highlight()
    {
        if (!isHighlighted && meshRenderer != null && highlightMaterial != null)
        {
            meshRenderer.material = highlightMaterial;
            isHighlighted = true;
        }
    }

    public void Unhighlight()
    {
        if (isHighlighted && meshRenderer != null)
        {
            meshRenderer.material = defaultMaterial;
            isHighlighted = false;
        }
    }

    public void Press()
    {
        if (pinCodeSystem != null && pinCodeSystem.IsActive())
        {
            if (clickSound != null)
            {
                AudioSource.PlayClipAtPoint(clickSound, transform.position);
            }

            pinCodeSystem.AddDigit(digit);
            StartCoroutine(AnimatePress());
        }
    }

    private IEnumerator AnimatePress()
    {
        Vector3 originalScale = transform.localScale;
        Vector3 originalPosition = transform.localPosition;

        transform.localScale = originalScale * 0.9f;
        transform.localPosition = originalPosition + new Vector3(0, -0.02f, 0);

        if (meshRenderer != null)
        {
            meshRenderer.material.color = pressedColor;
        }

        yield return new WaitForSeconds(0.1f);

        transform.localScale = originalScale;
        transform.localPosition = originalPosition;

        if (meshRenderer != null && isHighlighted)
        {
            meshRenderer.material = highlightMaterial;
        }
        else if (meshRenderer != null)
        {
            meshRenderer.material = defaultMaterial;
        }
    }
}