#if UNITY_ANDROID
using System;
using UnityEngine;
using mygame.sdk;

namespace mygame.plugin.Android
{
    public class AdsAdmobMyAndroid : AndroidJavaProxy, AdsAdmobMyIF
    {
        private const string AdsAdmobMyName = "mygame.plugin.myads.adsmob.AdsAdmobMy";
        private const string IFAdsAdmobMyName = "mygame.plugin.myads.adsmob.IFAdsAdmobMy";
        private AndroidJavaObject adsAdmobMy;

        public AdsAdmobMyAndroid() : base(IFAdsAdmobMyName)
        {
            using (AndroidJavaClass playerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                using (AndroidJavaObject activity = playerClass.GetStatic<AndroidJavaObject>("currentActivity"))
                {
                    this.adsAdmobMy = new AndroidJavaObject(AdsAdmobMyName, activity, this);
                }
            }
        }

        #region IGoogleMobileAdsInterstitialClient implementation
        public void Initialize()
        {
            this.adsAdmobMy.Call("Initialize");
        }

        public void setLog(bool isLog)
        {
            this.adsAdmobMy.Call("Initialize", isLog);
        }

        public void setBannerPos(int pos, float dxCenter)
        {
            this.adsAdmobMy.Call("setBannerPos", pos, dxCenter);
        }

        public void showBanner(string adsId, int pos, int orien, bool iPad, float dxCenter)
        {
            this.adsAdmobMy.Call("showBanner", adsId, pos, orien, iPad, dxCenter);
        }
        public void hideBanner()
        {
            this.adsAdmobMy.Call("hideBanner");
        }

        public void clearCurrFull()
        {
            this.adsAdmobMy.Call("clearCurrFull");
        }

        public void loadFull(string adsId)
        {
            this.adsAdmobMy.Call("loadFull", adsId);
        }
        public bool showFull()
        {
            bool re = this.adsAdmobMy.Call<bool>("showFull");
            return re;
        }

        public void clearCurrGift()
        {
            this.adsAdmobMy.Call("clearCurrGift");
        }

        public void loadGift(string adsId)
        {
            this.adsAdmobMy.Call("loadGift", adsId);
        }
        public bool showGift()
        {
            bool re = this.adsAdmobMy.Call<bool>("showGift");
            return re;
        }

        #endregion

        #region Callbacks from UnityInterstitialAdListener.
        public void onBannerLoaded()
        {
            if (AdsAdmobMyBridge.Instance != null) AdsAdmobMyBridge.Instance.onBannerLoaded();
        }
        public void onBannerLoadFail(string err)
        {
            if (AdsAdmobMyBridge.Instance != null) AdsAdmobMyBridge.Instance.onBannerLoadFail(err);
        }
        public void onBannerClose()
        {
            if (AdsAdmobMyBridge.Instance != null) AdsAdmobMyBridge.Instance.onBannerClose();
        }
        public void onBannerOpen()
        {
            if (AdsAdmobMyBridge.Instance != null) AdsAdmobMyBridge.Instance.onBannerOpen();
        }
        public void onBannerClick()
        {
            if (AdsAdmobMyBridge.Instance != null) AdsAdmobMyBridge.Instance.onBannerClick();
        }
        public void onBannerImpression()
        {
            if (AdsAdmobMyBridge.Instance != null) AdsAdmobMyBridge.Instance.onBannerImpression();
        }
        public void onBannerPaidEvent(int precisionType, string currencyCode, long valueMicros)
        {
            if (AdsAdmobMyBridge.Instance != null) AdsAdmobMyBridge.Instance.onBannerPaid(precisionType, currencyCode, valueMicros);
        }

        //Full
        public void onFullLoaded()
        {
            if (AdsAdmobMyBridge.Instance != null) AdsAdmobMyBridge.Instance.onFullLoaded();
        }
        public void onFullLoadFail(string err)
        {
            if (AdsAdmobMyBridge.Instance != null) AdsAdmobMyBridge.Instance.onFullLoadFail(err);
        }
        public void onFullFailedToShow(string err)
        {
            if (AdsAdmobMyBridge.Instance != null) AdsAdmobMyBridge.Instance.onFullFailedToShow(err);
        }
        public void onFullShowed()
        {
            if (AdsAdmobMyBridge.Instance != null) AdsAdmobMyBridge.Instance.onFullShowed();
        }
        public void onFullDismissed()
        {
            if (AdsAdmobMyBridge.Instance != null) AdsAdmobMyBridge.Instance.onFullDismissed();
        }
        public void onFullImpresstion()
        {
            if (AdsAdmobMyBridge.Instance != null) AdsAdmobMyBridge.Instance.onFullImpresstion();
        }
        public void onFullPaidEvent(int precisionType, string currencyCode, long valueMicros)
        {
            if (AdsAdmobMyBridge.Instance != null) AdsAdmobMyBridge.Instance.onFullPaid(precisionType, currencyCode, valueMicros);
        }

        //gift
        public void onGiftLoaded()
        {
            if (AdsAdmobMyBridge.Instance != null) AdsAdmobMyBridge.Instance.onGiftLoaded();
        }
        public void onGiftLoadFail(string err)
        {
            if (AdsAdmobMyBridge.Instance != null) AdsAdmobMyBridge.Instance.onGiftLoadFail(err);
        }
        public void onGiftFailedToShow(string err)
        {
            if (AdsAdmobMyBridge.Instance != null) AdsAdmobMyBridge.Instance.onGiftFailedToShow(err);
        }
        public void onGiftShowed()
        {
            if (AdsAdmobMyBridge.Instance != null) AdsAdmobMyBridge.Instance.onGiftShowed();
        }
        public void onGiftDismissed()
        {
            if (AdsAdmobMyBridge.Instance != null) AdsAdmobMyBridge.Instance.onGiftDismissed();
        }
        public void onGiftImpresstion()
        {
            if (AdsAdmobMyBridge.Instance != null) AdsAdmobMyBridge.Instance.onGiftImpresstion();
        }
        public void onGiftReward()
        {
            if (AdsAdmobMyBridge.Instance != null) AdsAdmobMyBridge.Instance.onGiftReward();
        }
        public void onGiftPaidEvent(int precisionType, string currencyCode, long valueMicros)
        {
            if (AdsAdmobMyBridge.Instance != null) AdsAdmobMyBridge.Instance.onGiftPaid(precisionType, currencyCode, valueMicros);
        }
        #endregion
    }
}

#endif
