// using System;
// using UnityEngine;
// using xgame.sdk;
//
// namespace Myapi
// {
//     public class UpdatePlayerApi : SingleTonApi<UpdatePlayerApi, ApiResultOb>
//     {
//         public void submit(string userId, string info,int currentCoin,int attackPower,int defendWarrior,int attackWarrior,int world,string username, Action<ApiResultOb> _callBack)
//         {
//             callBack = _callBack;
//             string url = $"{ApiManager.baseAppUrl}{ApiManager.UpdatePlayerInfo}&userid={userId}&currentcoin={currentCoin}&attack={attackPower}&defendwarrior={defendWarrior}&attackwarrior={attackWarrior}&world={world}&username={username}";
//             PostRequest(url, info);
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
//             if (callBack != null)
//             {
//                 callBack(data);
//             }
//         }
//     }
// }
