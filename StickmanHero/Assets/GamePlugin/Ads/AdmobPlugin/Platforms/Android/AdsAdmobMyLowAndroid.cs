#if UNITY_ANDROID
using System;
using UnityEngine;
using mygame.sdk;

namespace mygame.plugin.Android
{
    public class AdsAdmobMyLowAndroid : AndroidJavaProxy, AdsAdmobMyIF
    {
        private const string AdsAdmobMyName = "mygame.plugin.myads.adsmob.AdsAdmobMy";
        private const string IFAdsAdmobMyName = "mygame.plugin.myads.adsmob.IFAdsAdmobMy";
        private AndroidJavaObject adsAdmobMy;

        public AdsAdmobMyLowAndroid() : base(IFAdsAdmobMyName)
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
            if (AdsAdmobMyLowBridge.Instance != null) AdsAdmobMyLowBridge.Instance.onBannerLoaded();
        }
        public void onBannerLoadFail(string err)
        {
            if (AdsAdmobMyLowBridge.Instance != null) AdsAdmobMyLowBridge.Instance.onBannerLoadFail(err);
        }
        public void onBannerClose()
        {
            if (AdsAdmobMyLowBridge.Instance != null) AdsAdmobMyLowBridge.Instance.onBannerClose();
        }
        public void onBannerOpen()
        {
            if (AdsAdmobMyLowBridge.Instance != null) AdsAdmobMyLowBridge.Instance.onBannerOpen();
        }
        public void onBannerClick()
        {
            if (AdsAdmobMyLowBridge.Instance != null) AdsAdmobMyLowBridge.Instance.onBannerClick();
        }
        public void onBannerImpression()
        {
            if (AdsAdmobMyLowBridge.Instance != null) AdsAdmobMyLowBridge.Instance.onBannerImpression();
        }
        public void onBannerPaidEvent(int precisionType, string currencyCode, long valueMicros)
        {
            if (AdsAdmobMyLowBridge.Instance != null) AdsAdmobMyLowBridge.Instance.onBannerPaid(precisionType, currencyCode, valueMicros);
        }

        //Full
        public void onFullLoaded()
        {
            if (AdsAdmobMyLowBridge.Instance != null) AdsAdmobMyLowBridge.Instance.onFullLoaded();
        }
        public void onFullLoadFail(string err)
        {
            if (AdsAdmobMyLowBridge.Instance != null) AdsAdmobMyLowBridge.Instance.onFullLoadFail(err);
        }
        public void onFullFailedToShow(string err)
        {
            if (AdsAdmobMyLowBridge.Instance != null) AdsAdmobMyLowBridge.Instance.onFullFailedToShow(err);
        }
        public void onFullShowed()
        {
            if (AdsAdmobMyLowBridge.Instance != null) AdsAdmobMyLowBridge.Instance.onFullShowed();
        }
        public void onFullDismissed()
        {
            if (AdsAdmobMyLowBridge.Instance != null) AdsAdmobMyLowBridge.Instance.onFullDismissed();
        }
        public void onFullImpresstion()
        {
            if (AdsAdmobMyLowBridge.Instance != null) AdsAdmobMyLowBridge.Instance.onFullImpresstion();
        }
        public void onFullPaidEvent(int precisionType, string currencyCode, long valueMicros)
        {
            if (AdsAdmobMyLowBridge.Instance != null) AdsAdmobMyLowBridge.Instance.onFullPaid(precisionType, currencyCode, valueMicros);
        }

        //gift
        public void onGiftLoaded()
        {
            if (AdsAdmobMyLowBridge.Instance != null) AdsAdmobMyLowBridge.Instance.onGiftLoaded();
        }
        public void onGiftLoadFail(string err)
        {
            if (AdsAdmobMyLowBridge.Instance != null) AdsAdmobMyLowBridge.Instance.onGiftLoadFail(err);
        }
        public void onGiftFailedToShow(string err)
        {
            if (AdsAdmobMyLowBridge.Instance != null) AdsAdmobMyLowBridge.Instance.onGiftFailedToShow(err);
        }
        public void onGiftShowed()
        {
            if (AdsAdmobMyLowBridge.Instance != null) AdsAdmobMyLowBridge.Instance.onGiftShowed();
        }
        public void onGiftDismissed()
        {
            if (AdsAdmobMyLowBridge.Instance != null) AdsAdmobMyLowBridge.Instance.onGiftDismissed();
        }
        public void onGiftImpresstion()
        {
            if (AdsAdmobMyLowBridge.Instance != null) AdsAdmobMyLowBridge.Instance.onGiftImpresstion();
        }
        public void onGiftReward()
        {
            if (AdsAdmobMyLowBridge.Instance != null) AdsAdmobMyLowBridge.Instance.onGiftReward();
        }
        public void onGiftPaidEvent(int precisionType, string currencyCode, long valueMicros)
        {
            if (AdsAdmobMyLowBridge.Instance != null) AdsAdmobMyLowBridge.Instance.onGiftPaid(precisionType, currencyCode, valueMicros);
        }
        #endregion
    }
}

#endif
