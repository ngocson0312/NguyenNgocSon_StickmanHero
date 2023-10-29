using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using mygame.sdk;

public class PopupNoConnectionCtr : MonoBehaviour
{
    public Text title;
    public Text msgtxt;

    public void show(string ti, string msg)
    {
        RectTransform rc = GetComponent<RectTransform>();
        rc.anchorMin = new Vector2(0, 0);
        rc.anchorMax = new Vector2(1, 1);
        rc.sizeDelta = new Vector2(0, 0);
        rc.anchoredPosition = Vector2.zero;
        rc.anchoredPosition3D = Vector3.zero;
        if (ti != null && ti.Length > 5)
        {
            title.text = ti;
        }
        else
        {
            title.text = "Warning!";
        }
        if (msg != null && msg.Length > 5)
        {
            msgtxt.text = msg;
        }
        else
        {
            msgtxt.text = "Can't load new levels from server. Please check the connection and try again";
        }
    }
}
