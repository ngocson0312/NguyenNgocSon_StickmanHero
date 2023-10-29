// using System;
// using UnityEngine;
// using xgame.sdk;
//
// namespace Myapi
// {
//     public class UpdateBattleApi : SingleTonApi<UpdateBattleApi, ApiResultOb>
//     {
//
//         public void submit(string userId, int coin, int attack, string otherUserid, int othercoin, int othercoinlost, int otherAttack, string userwin, string otherTowerInfo, int otherDefendLost, Action<ApiResultOb> _callBack)
//         {
//             callBack = _callBack;
//             string url = $"{ApiManager.baseAppUrl}{ApiManager.UpdateBattleInfo}&userid={userId}&coin={coin}&attack={attack}&otheruserid={otherUserid}&othercoin={othercoin}&otherattack={otherAttack}&otherdefendwarriorlost={otherDefendLost}&useridwin={userwin}";
//             PostRequest(url, otherTowerInfo);
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
