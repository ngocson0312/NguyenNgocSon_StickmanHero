// using System;
// using UnityEngine;
// using xgame.sdk;
//
// namespace Myapi
// {
//     public class CheckTowerInfoApi : SingleTonApi<CheckTowerInfoApi, LoginResultOb>
//     {
//         public void Check(string userId, Action<LoginResultOb> _callBack)
//         {
//             callBack = _callBack;
//             string url = $"{ApiManager.baseAppUrl}{ApiManager.GetTowerInfo}&userid={userId}";
//             PostRequest(url);
//         }
//
//         protected override void OnError(string error)
//         {
//             base.OnError(error);
//             if (callBack != null) callBack(null);
//             Debug.Log("mysdk: api log err=" + error);
//         }
//
//         protected override void Process(LoginResultOb data)
//         {
//             if (callBack != null) callBack(data);
//         }
//     }
// }
