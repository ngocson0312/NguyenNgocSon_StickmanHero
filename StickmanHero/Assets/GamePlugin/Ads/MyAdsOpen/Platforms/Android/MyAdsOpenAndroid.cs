#if UNITY_ANDROID

using System;
using UnityEngine;
using mygame.sdk;

namespace mygame.plugin.Android
{
    // public class MyAdsOpenAndroid : AndroidJavaProxy, MyAdsOpenIF
    public class MyAdsOpenAndroid : MyAdsOpenIF
    {
        private const string MyAdsOpenName = "mygame.plugin.myopenads.MyAdsOpen";
        private const string IFMyAdsOpenName = "mygame.plugin.myopenads.IFMyAdsOpen";
        // private AndroidJavaObject myOpenAds;

        // public MyAdsOpenAndroid() : base(IFMyAdsOpenName)
        public MyAdsOpenAndroid()
        {
            // using(AndroidJavaClass playerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            // {
            //     using(AndroidJavaObject activity = playerClass.GetStatic<AndroidJavaObject>("currentActivity"))
            //     {
            //         this.myOpenAds = new AndroidJavaObject(MyAdsOpenName, activity, this);
            //     }
            // }
        }

        #region IGoogleMobileAdsInterstitialClient implementation
        public void load(string path, bool isCache)
        {
            // this.myOpenAds.Call("load", path, isCache);
        }
        public bool show(string path, int flagBtNo, int isFull)
        {
            // return this.myOpenAds.Call<bool>("show", path, flagBtNo, isFull);
            return false;
        }
        public void loadAndShow(string path, bool isCache, int flagBtNo, int isFull)
        {
            // this.myOpenAds.Call("loadAndShow", path, isCache, flagBtNo, isFull);
        }

        #endregion

        #region Callbacks from UnityInterstitialAdListener.
        public void onLoaded()
        {
            if (MyAdsOpenBridge.Instance != null) MyAdsOpenBridge.Instance.CBLoaded();
        }
        public void onLoadFail(string err)
        {
            if (MyAdsOpenBridge.Instance != null) MyAdsOpenBridge.Instance.CBLoadFail(err);
        }
        public void onStart()
        {
            if (MyAdsOpenBridge.Instance != null) MyAdsOpenBridge.Instance.CBOpen();
        }
        public void onClose()
        {
            if (MyAdsOpenBridge.Instance != null) MyAdsOpenBridge.Instance.CBClose();
        }
        #endregion
    }
}

#endif
