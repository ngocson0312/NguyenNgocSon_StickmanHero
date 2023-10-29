using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using mygame.sdk;

public class PopupNoAllow : MonoBehaviour
{
    public void onclickGotoStoee()
    {
        GameHelper.Instance.gotoStore(AppConfig.appid);
    }
}
