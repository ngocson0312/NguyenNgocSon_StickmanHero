#if UNITY_IOS

using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace mygame.sdk
{
    public class AdsAdmobMyLowiOS : AdsAdmobMyIF
    {
#region IGoogleMobileAdsInterstitialClient implementation
        public void Initialize()
        {
            AdsAdmobMyLowiOSBridge.Initialize();
        }
        public void setBannerPos(int pos, float dxCenter)
        {
            AdsAdmobMyLowiOSBridge.setBannerPos(pos, dxCenter);
        }
        public void showBanner(string adsId, int pos, int orien, bool iPad, float dxCenter)
        {
            AdsAdmobMyLowiOSBridge.showBanner(adsId, pos, orien, iPad, dxCenter);
        }
        public void hideBanner()
        {
            AdsAdmobMyLowiOSBridge.hideBanner();
        }

        public void clearCurrFull()
        {
            AdsAdmobMyLowiOSBridge.clearCurrFull();
        }
        public void loadFull(string adsId)
        {
            AdsAdmobMyLowiOSBridge.loadFull(adsId);
        }
        public bool showFull()
        {
            bool re = AdsAdmobMyLowiOSBridge.showFull();
            return re;
        }

        public void clearCurrGift()
        {
            AdsAdmobMyLowiOSBridge.clearCurrGift();
        }
        public void loadGift(string adsId)
        {
            AdsAdmobMyLowiOSBridge.loadGift(adsId);
        }
        public bool showGift()
        {
            bool re = AdsAdmobMyLowiOSBridge.showGift();
            return re;
        }

#endregion
    }
}

#endif
