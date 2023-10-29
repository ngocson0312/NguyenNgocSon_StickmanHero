//
// using UnityEngine;
// using Myapi;
// using System;
//
// public class GetEnemyRevengeApi : SingleTonApi<GetEnemyRevengeApi,GetListRevengeResult>
// {
//      public void Get(string userId, Action<GetListRevengeResult> _callBack)
//         {
//             string url = $"{ApiManager.baseAppUrl}{ApiManager.GetTowerRevenge}&userid={userId}";
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
//         protected override void Process(GetListRevengeResult data)
//         {
//             if (callBack != null) callBack(data);
//         }
// }
