using UnityEngine;

public class PinCodeUIButton : MonoBehaviour
{
    [SerializeField] private string digit;
    [SerializeField] private PinCodeSystemUI pinCodeSystem;

    public void OnButtonClick()
    {
        if (pinCodeSystem != null && pinCodeSystem.IsActive())
        {
            pinCodeSystem.AddDigit(digit);
        }
    }
}