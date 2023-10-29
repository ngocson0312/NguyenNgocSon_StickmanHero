using System;
using System.Collections;
using System.Collections.Generic;
using MyJson;
using UnityEngine;
using UnityEngine.Analytics;

#if FIRBASE_ENABLE
using Firebase.RemoteConfig;
#elif ENABLE_GETCONFIG
using Firebase.RemoteConfig;
#endif

namespace mygame.sdk
{
    public class FIRParserOtherConfig
    {
        public static void parserInGameConfig()
        {
#if FIRBASE_ENABLE || ENABLE_GETCONFIG
            string keycfframerate = "cf_game_mode";
            ConfigValue v = FirebaseRemoteConfig.DefaultInstance.GetValue(keycfframerate);
            if (v.StringValue != null && v.StringValue.Length > 0)
            {
                int per = (int)v.LongValue;
                PlayerPrefs.SetInt("cf_game_mode", per);
            }
            v = FirebaseRemoteConfig.DefaultInstance.GetValue("cf_type_game_lose");
            if (v.StringValue != null && v.StringValue.Length > 0)
            {
                int per = (int)v.LongValue;
                PlayerPrefs.SetInt("cf_type_game_lose", per);
            }
            v = FirebaseRemoteConfig.DefaultInstance.GetValue("apply_zoom_setting");
            if (v.StringValue != null && v.StringValue.Length > 0)
            {
                int per = (int)v.LongValue;
                PlayerPrefs.SetInt("apply_zoom_setting", per);
            }
            v = FirebaseRemoteConfig.DefaultInstance.GetValue("cf_boss_health");
            if (v.StringValue != null && v.StringValue.Length > 0)
            {
                int per = (int)v.LongValue;
                PlayerPrefs.SetInt("cf_boss_health", per);
            }
            v = FirebaseRemoteConfig.DefaultInstance.GetValue("cf_boss_damage");
            if (v.StringValue != null && v.StringValue.Length > 0)
            {
                int per = (int)v.LongValue;
                PlayerPrefs.SetInt("cf_boss_damage", per);
            }
#endif
        }
    }
}