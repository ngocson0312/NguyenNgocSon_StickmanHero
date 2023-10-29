#if UNITY_IOS

using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace mygame.sdk
{
    public class AdsAdmobMyiOS : AdsAdmobMyIF
    {
#region IGoogleMobileAdsInterstitialClient implementation
        public void Initialize()
        {
            AdsAdmobMyiOSBridge.Initialize();
        }
        public void setBannerPos(int pos, float dxCenter)
        {
            AdsAdmobMyiOSBridge.setBannerPos(pos, dxCenter);
        }
        public void showBanner(string adsId, int pos, int orien, bool iPad, float dxCenter)
        {
            AdsAdmobMyiOSBridge.showBanner(adsId, pos, orien, iPad, dxCenter);
        }
        public void hideBanner()
        {
            AdsAdmobMyiOSBridge.hideBanner();
        }

        public void clearCurrFull()
        {
            AdsAdmobMyiOSBridge.clearCurrFull();
        }
        public void loadFull(string adsId)
        {
            AdsAdmobMyiOSBridge.loadFull(adsId);
        }
        public bool showFull()
        {
            bool re = AdsAdmobMyiOSBridge.showFull();
            return re;
        }

        public void clearCurrGift()
        {
            AdsAdmobMyiOSBridge.clearCurrGift();
        }
        public void loadGift(string adsId)
        {
            AdsAdmobMyiOSBridge.loadGift(adsId);
        }
        public bool showGift()
        {
            bool re = AdsAdmobMyiOSBridge.showGift();
            return re;
        }

#endregion
    }
}

#endif
