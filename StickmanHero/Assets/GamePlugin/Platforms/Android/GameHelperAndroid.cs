#if UNITY_ANDROID

using System;
using UnityEngine;

namespace mygame.plugin.Android
{
    public class GameHelperAndroid
    {
        public static void Share(string body, string subject, string url, string[] filePaths, string mimeType, bool chooser, string chooserText)
        {
            Debug.Log("mysdk ShareAndroid 1");
            using (AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent"))
            using (AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent"))
            {
                Debug.Log("mysdk ShareAndroid 2");
                using (intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND")))
                { }
                Debug.Log("mysdk ShareAndroid 23");
                using (intentObject.Call<AndroidJavaObject>("setType", mimeType))
                { }
                Debug.Log("mysdk ShareAndroid 24");
                using (intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_SUBJECT"), subject))
                { }
                Debug.Log("mysdk ShareAndroid 25");
                using (intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), body))
                { }
                Debug.Log("mysdk ShareAndroid 3");
                if (!string.IsNullOrEmpty(url))
                {
                    Debug.Log("mysdk ShareAndroid 31");
                    // attach url
                    using (AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri"))
                    using (AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("parse", url))
                    using (intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_STREAM"), uriObject))
                    { }
                }
                Debug.Log("mysdk ShareAndroid 4");
                // finally start application
                using (AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                using (AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity"))
                {
                    if (chooser)
                    {
                        Debug.Log("mysdk ShareAndroid 31");
                        AndroidJavaObject jChooser = intentClass.CallStatic<AndroidJavaObject>("createChooser", intentObject, chooserText);
                        currentActivity.Call("startActivity", jChooser);
                    }
                    else
                    {
                        Debug.Log("mysdk ShareAndroid 32");
                        currentActivity.Call("startActivity", intentObject);
                    }
                }
            }
        }

        public static string getCountryCode(bool isRequestPermission)
        {
            string re = "";
            using (AndroidJavaClass playerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                using (AndroidJavaObject activity = playerClass.GetStatic<AndroidJavaObject>("currentActivity"))
                using (AndroidJavaObject gameUtil = new AndroidJavaObject("mygame.plugin.util.GameUtil"))
                {
                    re = gameUtil.CallStatic<string>("getCountryCode", activity, isRequestPermission);
                }
            }
            return re;
        }

        public static string getLanguageCode()
        {
            string re = "";
            using (AndroidJavaClass playerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                using (AndroidJavaObject activity = playerClass.GetStatic<AndroidJavaObject>("currentActivity"))
                using (AndroidJavaObject gameUtil = new AndroidJavaObject("mygame.plugin.util.GameUtil"))
                {
                    re = gameUtil.CallStatic<string>("getLanguageCode", activity);
                }
            }
            return re;
        }

        public static void getAdsIdentify()
        {
            using (AndroidJavaClass playerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                using (AndroidJavaObject activity = playerClass.GetStatic<AndroidJavaObject>("currentActivity"))
                using (AndroidJavaObject gameUtil = new AndroidJavaObject("mygame.plugin.util.GameUtil"))
                {
                    gameUtil.CallStatic("getAdsIdentify", activity);
                }
            }
        }

        public static bool checkPackageAppIsPresent(string pkg)
        {
            //            var up = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            //            var ca = up.GetStatic<AndroidJavaObject>("currentActivity");
            //            var packageManager = ca.Call<AndroidJavaObject>("getPackageManager");
            //
            //            //take the list of all packages on the device
            //            var appList = packageManager.Call<AndroidJavaObject>("getInstalledPackages", 0);
            //            var num = appList.Call<int>("size");
            //            for (var i = 0; i < num; i++)
            //            {
            //                var appInfo = appList.Call<AndroidJavaObject>("get", i);
            //                var packageNew = appInfo.Get<string>("packageName");
            //                if (packageNew.CompareTo(package) == 0) return true;
            //            }
            return false;
        }

        public static void Vibrate(int amply, int lenght)
        {
            using (AndroidJavaClass playerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                using (AndroidJavaObject activity = playerClass.GetStatic<AndroidJavaObject>("currentActivity"))
                using (AndroidJavaObject gameUtil = new AndroidJavaObject("mygame.plugin.util.GameUtil"))
                {
                    gameUtil.CallStatic("vibrate", activity, amply, lenght);
                }
            }
        }

        public static void setRemoveAds4OpenAds(int isRemove)
        {
            using (AndroidJavaClass playerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                using (AndroidJavaObject activity = playerClass.GetStatic<AndroidJavaObject>("currentActivity"))
                using (AndroidJavaObject gameUtil = new AndroidJavaObject("mygame.plugin.util.GameUtil"))
                {
                    gameUtil.CallStatic("setRemoveAds4OpenAds", activity, isRemove);
                }
            }
        }

        public static void configAppOpenAd(int typeShow, int showAtOpen, int isShowFirst, int deltaTimeShow, string aid)
        {
            using (AndroidJavaClass playerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                using (AndroidJavaObject activity = playerClass.GetStatic<AndroidJavaObject>("currentActivity"))
                using (AndroidJavaObject gameUtil = new AndroidJavaObject("mygame.plugin.util.GameUtil"))
                {
                    gameUtil.CallStatic("configAppOpenAd", activity, typeShow, showAtOpen, isShowFirst, deltaTimeShow, aid);
                }
            }
        }

        public static void showAppOpenAd(bool isShowFirst)
        {
            using (AndroidJavaObject gameUtil = new AndroidJavaObject("mygame.plugin.util.GameUtil"))
            {
                gameUtil.CallStatic("showAppOpenAd", isShowFirst);
            }
        }

        public static bool isOpenAdLoaded()
        {
            using (AndroidJavaObject gameUtil = new AndroidJavaObject("mygame.plugin.util.GameUtil"))
            {
                return gameUtil.CallStatic<bool>("isOpenAdLoaded");
            }
        }

        public static void setAllowShowOpen(bool isAllowShowOpenAd)
        {
            using (AndroidJavaObject gameUtil = new AndroidJavaObject("mygame.plugin.util.GameUtil"))
            {
                gameUtil.CallStatic("setAllowShowOpen", isAllowShowOpenAd);
            }
        }

        public static void appReview()
        {
            using (AndroidJavaClass playerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                using (AndroidJavaObject activity = playerClass.GetStatic<AndroidJavaObject>("currentActivity"))
                using (AndroidJavaObject gameUtil = new AndroidJavaObject("mygame.plugin.util.GameUtil"))
                {
                    gameUtil.CallStatic("appReview", activity);
                }
            }
        }

        public static void showCMP(bool istest = false)
        {
            using (AndroidJavaClass playerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                using (AndroidJavaObject activity = playerClass.GetStatic<AndroidJavaObject>("currentActivity"))
                using (AndroidJavaObject gameUtil = new AndroidJavaObject("mygame.plugin.util.GameUtil"))
                {
                    gameUtil.CallStatic("ShowconsentCMP", activity, istest);
                }
            }
        }

        public static bool deviceIsRooted()
        {
            using (AndroidJavaObject gameUtil = new AndroidJavaObject("mygame.plugin.util.GameUtil"))
            {
                bool isDvZin = gameUtil.CallStatic<bool>("xemMayZin");
                return !isDvZin;
            }
        }

        public static bool isInstallFromGooglePlay()
        {
            using (AndroidJavaClass playerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                using (AndroidJavaObject activity = playerClass.GetStatic<AndroidJavaObject>("currentActivity"))
                using (AndroidJavaObject gameUtil = new AndroidJavaObject("mygame.plugin.util.GameUtil"))
                {
                    return gameUtil.CallStatic<bool>("checkCaitugl", activity);
                }
            }
        }

        public static void checkPiraCheck()
        {
            using (AndroidJavaClass playerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                using (AndroidJavaObject activity = playerClass.GetStatic<AndroidJavaObject>("currentActivity"))
                using (AndroidJavaObject gameUtil = new AndroidJavaObject("mygame.plugin.util.GameUtil"))
                {
                    byte[] makey = { 106, 69, 88, 82, 66, 67, 100, 59, 86, 95, 105, 61, 116, 68, 102, 111, 117, 90, 104, 108, 78, 76, 119, 56, 53, 51, 77, 61 };
                    int[] paskey = { -1, -1, 1, 1, 5, 2, 10, 2, 6, 7, 13, 7, 13, 16, 16 };
                    byte[] pasva = { 5, 16, 13, 1, 18, 7, 12, 16, 12, 1, 8, 16, 3, 1, 0 };
                    string pkey = mygame.sdk.SdkUtil.myGiaima(makey, paskey, pasva);
                    gameUtil.CallStatic("piraCheck", activity, pkey);
                }
            }
        }

        public static void printSigning()
        {
            using (AndroidJavaClass playerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                using (AndroidJavaObject activity = playerClass.GetStatic<AndroidJavaObject>("currentActivity"))
                using (AndroidJavaObject gameUtil = new AndroidJavaObject("mygame.plugin.util.GameUtil"))
                {
                    gameUtil.CallStatic("printSigngame", activity);
                }
            }
        }
    }
}

#endif

