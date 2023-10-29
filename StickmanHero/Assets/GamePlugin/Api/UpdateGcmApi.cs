// using System;
// using UnityEngine;
// using UnityEngine.Networking;
// using xgame.sdk;
//
// namespace Myapi
// {
//     public class UpdateGcmApi : SingleTonApi<UpdateGcmApi, ApiResultOb>
//     {
// #if UNITY_ANDROID
//         string platform = "ANDROID";
// #elif UNITY_IOS
//         string platform = "IOS";
// #else
//         string platform = "";
// #endif
//
//         public void update()
//         {
//             string url = $"{ApiManager.baseAppUrl}{ApiManager.UpdateGcmId}platform={platform}&userid={GameHelper.Instance.deviceid}&gcm={FIRhelper.Instance.gcm_id}";
//             PostRequest(url);
//         }
//
//         protected override void OnError(string error)
//         {
//             base.OnError(error);
//             if (callBack != null)
//             {
//                 callBack(null);
//             }
//             Debug.Log("mysdk: api log err=" + error);
//         }
//
//         protected override void Process(ApiResultOb data)
//         {
//             if (data != null && data.status == 1)
//             {
//                 PlayerPrefs.SetInt("mem_send_gcm_ok", 1);
//             }
//         }
//     }
// }
