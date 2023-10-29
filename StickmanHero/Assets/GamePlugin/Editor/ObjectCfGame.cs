using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
using System.IO;
#endif

[Serializable]
public class ObjectCfGame
{
    public string name;
    public string pkg;
    public string GameId;

    public string keystore_atlas;
    public string keystore_pass;

    public string ads_max_devKey;
    public string ads_max_banner;
    public string ads_max_full;
    public string ads_max_gift;

    public string ads_iron_app_id;

    public string ads_admob_appid;
    public string ads_admob_openads;
    public string ads_admob_banner;
    public string ads_admob_full;
    public string ads_admob_rewarded;

    public string adjust_app_id;
    List<string> listEvent = new List<string>();

    public string define;

}
