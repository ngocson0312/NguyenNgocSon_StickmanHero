#if UNITY_ANDROID

using System;
using UnityEngine;

namespace mygame.plugin.Android
{
    public class AdsAdmobMy : AndroidJavaProxy
    {
        private const string AdsAdmobMyName = "mygame.plugin.util.AdsAdmobMy";
        private const string IFAdsAdmobMyName = "mygame.plugin.util.IFAdsAdmobMy";
        //private AndroidJavaObject AdsAdmobMy;

        public AdsAdmobMy() : base(IFAdsAdmobMyName)
        {
            //Debug.Log("mysdk: AdsAdmobMy 1");
            //AndroidJavaClass playerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            //AndroidJavaObject activity =
            //        playerClass.GetStatic<AndroidJavaObject>("currentActivity");
            //this.AdsAdmobMy = new AndroidJavaObject(
            //    AdsAdmobMyName, activity, this);
            //Debug.Log("mysdk: AdsAdmobMy 2");
        }

        public event EventHandler<EventArgs> HandleOnNativeLoaded;
        public event EventHandler<EventArgs> HandleOnNativeLoadFail;

        #region IGoogleMobileAdsInterstitialClient implementation
        public void showNative(string nativeId, float x, float y, float w, float h)
        {
            //this.AdsAdmobMy.Call("showNative", nativeId, x, y, w, h);
        }

        public void hideNative()
        {
            //this.AdsAdmobMy.Call("hideNative");
        }
        #endregion

        #region Callbacks from UnityInterstitialAdListener.

        public void onNativeLoaded()
        {
            if (HandleOnNativeLoaded != null)
            {
                EventArgs args = new EventArgs();
                this.HandleOnNativeLoaded(this, args);
            }
        }

        public void onNativeLoadFail(string err)
        {
            if (HandleOnNativeLoadFail != null)
            {
                EventArgs args = new EventArgs();
                this.HandleOnNativeLoadFail(this, args);
            }
        }

        #endregion
    }
}

#endif
