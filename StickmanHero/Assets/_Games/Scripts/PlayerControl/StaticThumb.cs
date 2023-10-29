using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticThumb : MonoBehaviour
{
    public float horizontal;
    public void LeftEnter()
    {
        horizontal = -1;
    }
    public void RightEnter()
    {
        horizontal = 1;
    }
    public void InputExit()
    {
        horizontal = 0;
    }
    private void LateUpdate()
    {
        if (Input.touchCount <= 0)
        {
            horizontal = 0;
        }
    }
}
