//#define TEST_ADS

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace mygame.sdk
{
    public interface AdsAdmobMyIF
    {
        #region IGoogleMobileAdsInterstitialClient implementation
        void Initialize();
        void setBannerPos(int pos, float dxCenter);
        void showBanner(string adsId, int pos, int orien, bool iPad, float dxCenter);
        void hideBanner();

        void clearCurrFull();
        void loadFull(string adsId);
        bool showFull();

        void clearCurrGift();
        void loadGift(string adsId);
        bool showGift();

        #endregion
    }
}
