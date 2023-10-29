// using UnityEngine;
// using UnityEngine.Networking;
// using xgame.sdk;
//
// namespace Myapi
// {
//     public class LogInstallApi : SingleTonApi<LogInstallApi, LogInstallOb>
//     {
//         private bool isRequesting = false;
//         public void check()
//         {
//             int ischeck = PlayerPrefs.GetInt("key_is_check_install", 0);
//             if (ischeck < 3 && !isRequesting && Application.internetReachability != NetworkReachability.NotReachable)
//             {
//                 if (GameHelper.Instance.AdsIdentify != null && GameHelper.Instance.AdsIdentify.Length > 3)
//                 {
//                     isRequesting = true;
//                     string url = ApiManager.API_LOG_INSTALL;
//                     string endpkg = UnityWebRequest.EscapeURL(Application.identifier);
//                     #if UNITY_IOS
//                     endpkg += "_iOS";
//                     #endif
//                     string endid = UnityWebRequest.EscapeURL(GameHelper.Instance.AdsIdentify);
//                     string endcountry = UnityWebRequest.EscapeURL(GameHelper.Instance.countryCode);
//                     url += ("package_name=" + endpkg);
//                     url += ("&idfa=" + endid);
//                     url += ("&country=" + endcountry);
//                     GetRequest(url);
//                 }
//             }
//         }
//
//         protected override void OnError(string error)
//         {
//             base.OnError(error);
//             Debug.Log("mysdk: api check install err=" + error);
//         }
//
//         protected override void Process(LogInstallOb data)
//         {
//             if (data != null && data.status == 1)
//             {
//                 Debug.Log("mysdk: api check install ok");
//                 PlayerPrefs.SetInt("key_is_check_install", 10);
//             }
//             else
//             {
//                 Debug.Log("mysdk: api check install status err");
//                 int ischeck = PlayerPrefs.GetInt("key_is_check_install", 0);
//                 ischeck++;
//                 PlayerPrefs.SetInt("key_is_check_install", ischeck);
//                 if (ischeck < 3)
//                 {
//                     Debug.Log("mysdk: api check install re check ischeck=" + ischeck);
//                     isRequesting = false;
//                     check();
//                 }
//             }
//         }
//     }
// }
