#if UNITY_ANDROID

using System;
using UnityEngine;

namespace mygame.plugin.Android
{
    public class DeviceUtilClient : AndroidJavaProxy
    {
        private const string DeviceUtilName = "mygame.plugin.util.DeviceUtil";
        private const string IFGetCountryName = "mygame.plugin.util.IFGetCountry";
        private AndroidJavaObject deviceUtil;

        public DeviceUtilClient() : base(IFGetCountryName)
        {
            Debug.Log("mysdk: DeviceUtilClient 1");
            AndroidJavaClass playerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject activity =
                    playerClass.GetStatic<AndroidJavaObject>("currentActivity");
            this.deviceUtil = new AndroidJavaObject(
                DeviceUtilName, activity, this);
            Debug.Log("mysdk: DeviceUtilClient 2");
        }

        public event EventHandler<GetCountryCodeArgs> onGetCountryCode;

        #region IGoogleMobileAdsInterstitialClient implementation
        // get country code.
        public string getCountryCode(bool isRequestPermission)
        {
            string re = this.deviceUtil.Call<string>("getCountryCode", isRequestPermission);
            return re;
        }

        public string getLanguageCode()
        {
            string re = this.deviceUtil.Call<string>("getLanguageCode");
            return re;
        }

        public void getAdsIdentify()
        {
            this.deviceUtil.Call("getAdsIdentify");
        }
        #endregion

        #region Callbacks from UnityInterstitialAdListener.
        public void ongetCountry(string countryCode)
        {
            if (this.onGetCountryCode != null)
            {
                GetCountryCodeArgs args = new GetCountryCodeArgs()
                {
                    CountryCode = countryCode
                };
                this.onGetCountryCode(this, args);
            }
        }
        #endregion
    }
}

#endif
