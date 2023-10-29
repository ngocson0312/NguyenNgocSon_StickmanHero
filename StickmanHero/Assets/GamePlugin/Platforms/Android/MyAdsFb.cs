#if UNITY_ANDROID

using System;
using UnityEngine;

namespace mygame.plugin.Android
{
    public class MyAdsFb : AndroidJavaProxy
    {
        private const string MyAdsFbName = "mygame.plugin.util.MyAdsFb";
        private const string IFMyAdsFbName = "mygame.plugin.util.IFMyAdsFb";
        private AndroidJavaObject myAdsFb;

        public MyAdsFb() : base(IFMyAdsFbName)
        {
            Debug.Log("mysdk: MyAdsFb 1");
            AndroidJavaClass playerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject activity =
                    playerClass.GetStatic<AndroidJavaObject>("currentActivity");
            this.myAdsFb = new AndroidJavaObject(
                MyAdsFbName, activity, this);
            Debug.Log("mysdk: MyAdsFb 2");
        }

        public event EventHandler<EventArgs> HandleOnNativeLoaded;
        public event EventHandler<EventArgs> HandleOnNativeLoadFail;

        #region IGoogleMobileAdsInterstitialClient implementation
        public void showNative(string nativeId, float x, float y, float w, float h)
        {
            this.myAdsFb.Call("showNative", nativeId, x, y, w, h);
        }

        public void hideNative()
        {
            this.myAdsFb.Call("hideNative");
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
