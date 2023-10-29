// using System;
// using UnityEngine;
// using UnityEngine.Networking;
// using xgame.sdk;
//
// namespace Myapi
// {
//     public class LoginApi : SingleTonApi<LoginApi, LoginResultOb>
//     {
// #if UNITY_ANDROID
//         string platform = "ANDROID";
// #elif UNITY_IOS
//         string platform = "IOS";
// #else
//         string platform = "";
// #endif
//
//         public void login(int type, string userId, string userName, Action<LoginResultOb> _callback)
//         {
//             string encodename = UnityWebRequest.EscapeURL(userName);
//             callBack = _callback;
//             string keyuserid = "";
//             if (type == 1)
//             {//gg
//                 keyuserid = $"&googleid={userId}";
//             }
//             else if (type == 2)
//             {//game center
//                 keyuserid = $"&gamecenterid={userId}";
//             }
//             else if (type == 3)
//             {//fb
//                 keyuserid = $"&fbid={userId}";
//             }
//             string url = $"{ApiManager.baseAppUrl}{ApiManager.Login}platform={platform}&deviceid={GameHelper.Instance.deviceid}{keyuserid}&username={encodename}&gcm={FIRhelper.Instance.gcm_id}";
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
//         protected override void Process(LoginResultOb data)
//         {
//             if (callBack != null)
//             {
//                 callBack(data);
//             }
//         }
//     }
// }
