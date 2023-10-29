#if UNITY_IOS || UNITY_IPHONE

using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace mygame.sdk
{
    public class GameHelperIos
    {
        [DllImport("__Internal")] private static extern string getLanguageCodeNative();
        public static string getLanguageCode()
        {
            return getLanguageCodeNative();
        }

        [DllImport("__Internal")] private static extern string getCountryCodeNative();
        public static string GetCountryCode()
        {
            return getCountryCodeNative();
        }

        [DllImport("__Internal")] private static extern string getAdsIdentifyNative();
        public static string GetAdsIdentify()
        {
            return getAdsIdentifyNative();
        }

        [DllImport("__Internal")] private static extern string getGiftBoxNative();
        public static string GetGiftBox()
        {
            return getGiftBoxNative();
        }

        [DllImport("__Internal")] private static extern void vibrateNative(int type);
        public static void vibrate(int type)
        {
            vibrateNative(type);
        }

        [DllImport("__Internal")] private static extern long getMemoryLimit();
        public static long GetMemoryLimit()
        {
            return getMemoryLimit();
        }

        [DllImport("__Internal")] private static extern long getPhysicMemoryInfo();
        public static long GetPhysicMemoryInfo()
        {
            return getPhysicMemoryInfo();
        }

        [DllImport("__Internal")] private static extern float getScreenWidthNative();
        public static float getScreenWidth()
        {
            return getScreenWidthNative();
        }

        [DllImport("__Internal")] private static extern void setRemoveAds4OpenAdsNative(int isRemove);
        public static void setRemoveAds4OpenAds(int isRemove)
        {
            setRemoveAds4OpenAdsNative(isRemove);
        }

        [DllImport("__Internal")] private static extern void configAppOpenAdNative(int typeShow, int showat, int isShowFirstOpen, int deltime, string adid);
        public static void configAppOpenAd(int typeShow, int showat, int isShowFirstOpen, int deltime, string adid)
        {
            configAppOpenAdNative(typeShow, showat, isShowFirstOpen, deltime, adid);
        }
        [DllImport("__Internal")] private static extern void showOpenAdsNative(bool isShowFirst);
        public static void showOpenAds(bool isShowFirst)
        {
            showOpenAdsNative(isShowFirst);
        }

        [DllImport("__Internal")] private static extern bool isOpenAdLoadedNative();
        public static bool isOpenAdLoaded()
        {
            return isOpenAdLoadedNative();
        }
        [DllImport("__Internal")] private static extern void setAllowShowOpenNative(bool isAllow);
        public static void setAllowShowOpen(bool isAllow)
        {
            setAllowShowOpenNative(isAllow);
        }

        [DllImport("__Internal")] private static extern bool appReviewNative();
        public static bool appReview()
        {
            return appReviewNative();
        }

        [DllImport("__Internal")] private static extern bool requestIDFANative(int isallversion);
        public static void requestIDFA(int isallversion)
        {
            requestIDFANative(isallversion);
        }

        [DllImport("__Internal")] private static extern void showCMPNative();
        public static void showCMP()
        {
            showCMPNative();
        }
    }
}

#endif
