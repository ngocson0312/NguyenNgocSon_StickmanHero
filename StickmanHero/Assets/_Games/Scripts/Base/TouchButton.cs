using UnityEngine;
using UnityEngine.EventSystems;
public class TouchButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    int pointerID = -999;
    bool buttonPressed;
    bool buttonHeld;
    public void OnPointerDown(PointerEventData eventData)
    {
        buttonPressed = true;
        buttonHeld = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        buttonPressed = false;
        buttonHeld = false;
    }
    public bool GetButtonDown()
    {
        if (buttonPressed)
        {
            buttonPressed = false;
            return true;
        }
        return false;
    }
    public bool GetButton()
    {
        return buttonHeld;
    }
    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }
}
