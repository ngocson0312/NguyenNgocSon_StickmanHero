// using System;
// using UnityEngine;
// using xgame.sdk;
//
// namespace Myapi
// {
//     public class GetEnemyAttackApi : SingleTonApi<GetEnemyAttackApi, GetOtherResultOb>
//     {
//         public void Get(string userId, Action<GetOtherResultOb> _callBack)
//         {
//             string url = $"{ApiManager.baseAppUrl}{ApiManager.GetTower4Attack}&userid={userId}";
//             callBack = _callBack;
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
//         protected override void Process(GetOtherResultOb data)
//         {
//             if (callBack != null) callBack(data);
//         }
//     }
// }
