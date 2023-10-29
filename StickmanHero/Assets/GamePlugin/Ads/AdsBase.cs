//#define TEST_ADS

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace mygame.sdk
{
    public abstract class AdsBase : MonoBehaviour
    {
        public AdsHelper advhelper;

        protected int toTryLoad = 1;
        public int adsType = 0;

        [Header("Android")]
        public string android_app_id = "";
        public string android_bn_id = "";
        public string android_native_id = "";
        public string android_splash_id = "";
        public string android_full_id = "";
        public string android_gift_id = "";

        [Header("iOS")]
        public string ios_app_id = "";
        public string ios_bn_id = "";
        public string ios_native_id = "";
        public string ios_splash_id = "";
        public string ios_full_id = "";
        public string ios_gift_id = "";
#if UNITY_IOS || UNITY_IPHONE
        [Header("iOS China")]
        public string ios_bn_cn_id = "";
        public string ios_full_cn_id = "";
        public string ios_gift_cn_id = "";
#endif

        protected string appId = "";
        protected string bannerId = "";
        protected string nativeId = "";
        protected string splashId = "";
        protected string fullId = "";
        protected string giftId = "";
        public bool isEnable { get; set; }

        protected long timeLoadBN;
        protected int BNTryLoad;
        protected int bnWidth;
        protected Dictionary<int, BannerType> dicBanner = new Dictionary<int, BannerType>();
        protected AdCallBack cbBanner;

        public abstract string getname();
        protected abstract void tryLoadBanner(int type);
        public abstract void loadBanner(int type, AdCallBack cb);
        public abstract bool showBanner(int type, int pos, int width, AdCallBack cb, float dxCenter);
        public abstract bool showBanner(int type, Rect poscustom, AdCallBack cb);
        public abstract void hideBanner(int type);
        public abstract void destroyBanner(int type);
        //native
        protected bool isNTLoading;
        protected bool isNTloaded;
        protected int NTTryLoad;
        protected AdCallBack cbNative;
        protected abstract void tryLoadNative();
        public abstract void loadNative(AdCallBack cb);
        public abstract bool showNative(Rect pos, AdCallBack cb);
        public abstract void hideNative();
        //full
        protected bool isFullLoading;
        public bool isFullLoaded { get; protected set; }
        protected long timeLoadFull;
        protected int FullTryLoad;
        protected bool isSplash = false;
        protected AdCallBack cbFullLoad;
        protected AdCallBack cbFullShow;
        public int adsFullSubType { get; protected set; }
        public abstract void clearCurrFull();
        protected abstract void tryLoadFull(bool isSplashAds);
        public abstract void loadFull(bool isSplashAds, AdCallBack cb);
        public abstract bool showFull(bool isSplashAds, AdCallBack cb);

        //Gift
        protected bool isGiftLoading;
        public bool isGiftLoaded { get; protected set; }
        protected long timeLoadGift;
        protected int tLoadGiftErr = 15;
        protected int GiftTryLoad;
        protected bool isRewardCom;
        protected AdCallBack cbGiftLoad;
        protected AdCallBack cbGiftShow;
        public abstract void clearCurrGift();
        protected abstract void tryloadGift();
        public abstract void loadGift(AdCallBack cb);
        public abstract bool showGift(AdCallBack cb);

        public abstract void InitAds();
        public abstract void AdsAwake();

        private void Awake()
        {
#if UNITY_ANDROID
            appId = android_app_id;
            bannerId = android_bn_id;
            nativeId = android_native_id;
            splashId = android_splash_id;
            fullId = android_full_id;
            giftId = android_gift_id;
#elif UNITY_IOS || UNITY_IPHONE
            appId = ios_app_id;
            bannerId = ios_bn_id;
            nativeId = ios_native_id;
            splashId = ios_splash_id;
            fullId = ios_full_id;
            giftId = ios_gift_id;
#endif
            timeLoadBN = 0;

            timeLoadFull = 0;
            isFullLoading = false;
            isFullLoaded = false;

            timeLoadGift = 0;
            isGiftLoading = false;
            isGiftLoaded = false;

            dicBanner.Add(0, new BannerType());
            dicBanner.Add(1, new BannerType());
            isEnable = false;
            adsFullSubType = adsType;

            AdsAwake();
        }

        public string getSplashId()
        {
            return splashId;
        }
        public string getFullId()
        {
            return fullId;
        }
        public string getGiftId()
        {
            return giftId;
        }
        public virtual int getFullLoaded(bool isSplashAds)
        {
            int re = 0;
            if (isFullLoaded)
            {
                re = 1;
            }

            return re;
        }
        public virtual bool getGiftLoaded()
        {
            return isGiftLoaded;
        }
        long timecall = 0;
        public void onFullClose()
        {
            if (advhelper.isFullLoadWhenClose)
            {
                AdsProcessCB.Instance().Enqueue(() =>
                {
#if ENABLE_MYLOG
                    timecall = SdkUtil.systemCurrentMiliseconds();
#endif
                    advhelper.loadFull4ThisTurn(false, advhelper.levelCurr4Full, false);
#if ENABLE_MYLOG
                    SdkUtil.logd("ads base onFullClose1=" + (SdkUtil.systemCurrentMiliseconds() - timecall));
#endif
                }, 2.0f);
            }
        }

        public void onGiftClose()
        {
            if (advhelper.isGiftLoadWhenClose)
            {
                AdsProcessCB.Instance().Enqueue(() =>
                {
#if ENABLE_MYLOG
                    timecall = SdkUtil.systemCurrentMiliseconds();
#endif
                    advhelper.loadGift4ThisTurn(advhelper.levelCurr4Gift, null);
#if ENABLE_MYLOG
                    SdkUtil.logd("ads base onGiftClose1=" + (SdkUtil.systemCurrentMiliseconds() - timecall));
#endif
                }, 2.0f);
            }
        }
    }

    public class BannerType
    {
        public bool isBNLoading;
        public bool isBNloaded;
        public bool isBNShow;
        public bool isBNHide;
        public bool isBNHasLoaded;
        public int posBanner;
        public object banner;
        public int typeSize;
        public bool isShowing = false;
    }
}
