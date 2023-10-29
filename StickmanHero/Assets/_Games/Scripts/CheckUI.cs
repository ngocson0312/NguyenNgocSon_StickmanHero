using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckUI : MonoBehaviour
{
    private void Start()
    {
        if (mygame.sdk.SdkUtil.isiPad())
        {
            Debug.Log("is ipad");
            GetComponent<CanvasScaler>().referenceResolution = new Vector2(720, 1788);
        }
        else
        {
            Debug.Log("is not ipad");
            GetComponent<CanvasScaler>().referenceResolution = new Vector2(720, 1280);
        }
    }
}
