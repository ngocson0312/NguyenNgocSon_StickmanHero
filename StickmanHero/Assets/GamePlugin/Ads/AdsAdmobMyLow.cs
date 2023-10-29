//#define ENABLE_ADS_ADMOB

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if ENABLE_ADS_ADMOB && USE_ADSMOB_MY
#endif

namespace mygame.sdk
{
    public class AdsAdmobMyLow : AdsBase
    {

#if ENABLE_ADS_ADMOB && USE_ADSMOB_MY

#endif
        float _membnDxCenter;

        public override void InitAds()
        {
#if ENABLE_ADS_ADMOB && USE_ADSMOB_MY
            isEnable = true;
#endif
        }

        public override void AdsAwake()
        {
#if ENABLE_ADS_ADMOB && USE_ADSMOB_MY
            isEnable = true;
#endif  
        }

        private void Start()
        {
#if ENABLE_ADS_ADMOB && USE_ADSMOB_MY
            AdsAdmobMyLowBridge.onBNLoaded += OnBannerAdLoadedEvent;
            AdsAdmobMyLowBridge.onBNLoadFail += OnBannerAdLoadFailedEvent;
            AdsAdmobMyLowBridge.onBNPaid += OnBannerAdPaidEvent;

            AdsAdmobMyLowBridge.onInterstitialLoaded += OnInterstitialLoadedEvent;
            AdsAdmobMyLowBridge.onInterstitialLoadFail += OnInterstitialFailedEvent;
            AdsAdmobMyLowBridge.onInterstitialShowed += OnInterstitialDisplayedEvent;
            AdsAdmobMyLowBridge.onInterstitialFailedToShow += onInterstitialFailedToShow;
            AdsAdmobMyLowBridge.onInterstitialDismissed += OnInterstitialDismissedEvent;
            AdsAdmobMyLowBridge.onInterstitialPaid += OnInterstitialAdPaidEvent;

            AdsAdmobMyLowBridge.onRewardLoaded += OnRewardedAdLoadedEvent;
            AdsAdmobMyLowBridge.onRewardLoadFail += OnRewardedAdFailedEvent;
            AdsAdmobMyLowBridge.onRewardFailedToShow += OnRewardedAdFailedToDisplayEvent;
            AdsAdmobMyLowBridge.onRewardShowed += OnRewardedAdDisplayedEvent;
            AdsAdmobMyLowBridge.onRewardDismissed += OnRewardedAdDismissedEvent;
            AdsAdmobMyLowBridge.onRewardReward += OnRewardedAdReceivedRewardEvent;
            AdsAdmobMyLowBridge.onRewardPaid += OnRewardedAdPaidEvent;
#endif
        }

        public override string getname()
        {
            return "adsmobMyLow";
        }

        protected override void tryLoadBanner(int typeb)
        {
#if ENABLE_ADS_ADMOB && USE_ADSMOB_MY
            SdkUtil.logd("ads admobmyLow tryLoadBanner = " + bannerId);
            // if (BNTryLoad >= toTryLoad)
            // {
            //     SdkUtil.logd("ads admobmyLow tryLoadBanner over trycount");
            //     if (cbBanner != null)
            //     {
            //         var tmpcb = cbBanner;
            //         cbBanner = null;
            //         tmpcb(AD_State.AD_LOAD_FAIL);
            //     }
            //     if (advhelper != null)
            //     {
            //         advhelper.onBannerLoadFail(adsType);
            //     }
            //     return;
            // }
            dicBanner[0].isBNLoading = true;
            dicBanner[0].isBNloaded = false;
            if (AppConfig.isBannerIpad)
            {
                AdsAdmobMyLowBridge.Instance.showBanner(bannerId, dicBanner[0].posBanner, (int)advhelper.bnOrien, SdkUtil.isiPad(), _membnDxCenter);
            }
            else
            {
                AdsAdmobMyLowBridge.Instance.showBanner(bannerId, dicBanner[0].posBanner, (int)advhelper.bnOrien, false, _membnDxCenter);
            }
            SdkUtil.logd("ads admobmyLow tryLoadBanner 3 _membnDxCenter=" + _membnDxCenter);
#else
            if (cbBanner != null)
            {
                SdkUtil.logd("ads admobmyLow tryLoadBanner not enable");
                var tmpcb = cbBanner;
                cbBanner = null;
                tmpcb(AD_State.AD_LOAD_FAIL);
            }
#endif
        }
        public override void loadBanner(int typeb, AdCallBack cb)
        {
            SdkUtil.logd("ads admobmyLow loadBanner");
            cbBanner = cb;
            if (!dicBanner[0].isBNLoading)
            {
                BNTryLoad = 0;
                tryLoadBanner(0);
            }
            else
            {
                SdkUtil.logd("ads admobmyLow loadBanner isProcessShow");
            }
        }
        public override bool showBanner(int typeb, int pos, int width, AdCallBack cb, float dxCenter)
        {
            SdkUtil.logd("ads admobmyLow showBanner typeb=" + typeb + ", pos=" + pos + ", dxCenter=" + dxCenter);
            if (bannerId == null || !bannerId.Contains("ca-app-pub"))
            {
#if ENABLE_MYLOG
                SdkUtil.logd("ads admobmyLow showBanner id incorrect");
#endif
                if (cb != null)
                {
                    cb(AD_State.AD_LOAD_FAIL);
                }
                return false;
            }
            dicBanner[0].isBNShow = true;
            dicBanner[0].posBanner = pos;
            bnWidth = width;
            _membnDxCenter = dxCenter;
#if ENABLE_ADS_ADMOB && USE_ADSMOB_MY
            if (dicBanner[0].banner != null && dicBanner[0].isBNloaded)
            {
                dicBanner[0].isBNHide = false;
                if (AppConfig.isBannerIpad)
                {
                    AdsAdmobMyLowBridge.Instance.showBanner(bannerId, dicBanner[0].posBanner, (int)advhelper.bnOrien, SdkUtil.isiPad(), dxCenter);
                }
                else
                {
                    AdsAdmobMyLowBridge.Instance.showBanner(bannerId, dicBanner[0].posBanner, (int)advhelper.bnOrien, false, dxCenter);
                }
                if (cb != null)
                {
                    cb(AD_State.AD_SHOW);
                }
                return true;
            }
            else
            {
                SdkUtil.logd("ads admobmyLow showBanner isprocess show dxCenter=" + dxCenter);
                loadBanner(0, cb);
                return false;
            }
#else
            if (cb != null)
            {
                SdkUtil.logd("ads admobmyLow tryLoadBanner not enable");
                cb(AD_State.AD_LOAD_FAIL);
            }
            return false;
#endif
        }
        public override bool showBanner(int typeb, Rect poscustom, AdCallBack cb)
        {
            if (cb != null)
            {
                SdkUtil.logd("ads admobmyLow tryLoadBanner not enable");
                cb(AD_State.AD_LOAD_FAIL);
            }
            return false;
        }
        public override void hideBanner(int typeb)
        {
            SdkUtil.logd("ads admobmyLow hideBanner");
            dicBanner[0].isBNShow = false;
#if ENABLE_ADS_ADMOB && USE_ADSMOB_MY
            AdsAdmobMyLowBridge.Instance.hideBanner();
#endif
        }
        public override void destroyBanner(int typeb)
        {
#if ENABLE_ADS_ADMOB && USE_ADSMOB_MY
            dicBanner[0].isBNLoading = false;
            dicBanner[0].isBNloaded = false;
            dicBanner[0].isBNHide = true;
            AdsAdmobMyLowBridge.Instance.hideBanner();
#endif
        }

        //
        protected override void tryLoadNative()
        { }
        public override void loadNative(AdCallBack cb)
        { }
        public override bool showNative(Rect pos, AdCallBack cb)
        {
            return false;
        }
        public override void hideNative()
        {
        }

        //
        public override void clearCurrFull()
        {
            if (getFullLoaded(false) == 1)
            {
#if ENABLE_ADS_ADMOB && USE_ADSMOB_MY
                AdsAdmobMyBridge.Instance.clearCurrFull();
                isFullLoaded = false;
#endif
            }
        }
        public override int getFullLoaded(bool _isSplash)
        {
            SdkUtil.logd("ads admobmyLow getFullLoaded issplash=" + _isSplash + ", isFullLoaded=" + isFullLoaded);
            if (_isSplash)
            {
                if (splashId == null || splashId.Length < 10)
                {
                    _isSplash = false;
                    SdkUtil.logd("ads admobmyLow getFullLoaded change is splash");
                }
            }
#if ENABLE_ADS_ADMOB && USE_ADSMOB_MY
            if (!_isSplash)
            {
                if (isFullLoaded)
                {
                    return 1;
                }
            }
            else
            {
                if (isFullLoaded)
                {
                    return 1;
                }
            }
#endif
            return 0;
        }
        protected override void tryLoadFull(bool _isSplash)
        {
#if ENABLE_ADS_ADMOB && USE_ADSMOB_MY
#if ENABLE_MYLOG
            SdkUtil.logd("ads admobmyLow tryLoadFull=" + fullId);
#endif
            int tryload = FullTryLoad;
            if (tryload >= toTryLoad)
            {
#if ENABLE_MYLOG
                SdkUtil.logd("ads admobmyLow tryLoadFull over try");
#endif
                if (cbFullLoad != null)
                {
                    var tmpcb = cbFullLoad;
                    cbFullLoad = null;
                    tmpcb(AD_State.AD_LOAD_FAIL);
                }

                return;
            }
#if ENABLE_MYLOG
            SdkUtil.logd("ads admobmyLow tryLoadFull loading");
#endif
            if (fullId != null && fullId.Contains("ca-app-pub"))
            {
                isFullLoading = true;
                isFullLoaded = false;
                AdsAdmobMyLowBridge.Instance.loadFull(fullId);
            }
            else
            {
#if ENABLE_MYLOG
                SdkUtil.logd("ads admobmyLow tryLoadFull id not correct");
#endif
                isFullLoading = false;
                isFullLoaded = false;
                if (cbFullLoad != null)
                {
                    var tmpcb = cbFullLoad;
                    cbFullLoad = null;
                    tmpcb(AD_State.AD_LOAD_FAIL);
                }
            }
#else
            if (cbFullLoad != null)
                {
#if ENABLE_MYLOG
                    SdkUtil.logd("ads admobmyLow tryLoadFull not enable isSplash=" + _isSplash);
#endif
                    var tmpcb = cbFullLoad;
                    cbFullLoad = null;
                    tmpcb(AD_State.AD_LOAD_FAIL);
                }
#endif
        }
        public override void loadFull(bool _isSplash, AdCallBack cb)
        {
            SdkUtil.logd("ads admobmyLow loadFull type=" + adsType + ", isSplash=" + _isSplash);
            if (_isSplash)
            {
                if (splashId == null || splashId.Length < 10)
                {
                    _isSplash = false;
                    SdkUtil.logd("ads admobmyLow loadFull change is splash");
                }
            }

            bool isLoaded = false;
            bool isLoadding = false;
            cbFullLoad = cb;
            isLoaded = isFullLoaded;
            isLoadding = isFullLoading;
            if (!isLoadding && !isLoaded)
            {
                FullTryLoad = 0;
                tryLoadFull(_isSplash);
            }
            else
            {
                SdkUtil.logd("ads admobmyLow loadFull isloading or isloaded");
            }
        }
        public override bool showFull(bool _isSplash, AdCallBack cb)
        {
            SdkUtil.logd("ads admobmyLow showFull type=" + adsType + ", isspalsh=" + _isSplash);
            if (_isSplash)
            {
                if (splashId == null || splashId.Length < 10)
                {
                    _isSplash = false;
                    SdkUtil.logd("ads admobmyLow showFull change is splash");
                }
            }
            cbFullShow = null;
            int ss = getFullLoaded(_isSplash);
            if (ss > 0)
            {
                SdkUtil.logd("ads admobmyLow showFull type=" + adsType);
                FullTryLoad = 0;
                isFullLoaded = false;
#if ENABLE_ADS_ADMOB && USE_ADSMOB_MY
                cbFullShow = cb;
                AdsAdmobMyLowBridge.Instance.showFull();
                return true;
#endif
            }
            return false;
        }

        //
        public override void clearCurrGift()
        {
            if (getGiftLoaded())
            {
#if ENABLE_ADS_ADMOB && USE_ADSMOB_MY
                AdsAdmobMyBridge.Instance.clearCurrGift();
                isGiftLoaded = false;
#endif
            }
        }
        public override bool getGiftLoaded()
        {
#if ENABLE_ADS_ADMOB && USE_ADSMOB_MY
            return isGiftLoaded;
#endif
            return false;
        }
        protected override void tryloadGift()
        {
#if ENABLE_ADS_ADMOB && USE_ADSMOB_MY
#if ENABLE_MYLOG
            SdkUtil.logd("ads admobmyLow tryloadGift =" + giftId);
#endif
            if (GiftTryLoad >= toTryLoad)
            {
#if ENABLE_MYLOG
                SdkUtil.logd("ads admobmyLow tryloadGift over try");
#endif
                if (cbGiftLoad != null)
                {
                    var tmpcb = cbGiftLoad;
                    cbGiftLoad = null;
                    tmpcb(AD_State.AD_LOAD_FAIL);
                }
                return;
            }
            isGiftLoading = true;
            isGiftLoaded = false;
            AdsAdmobMyLowBridge.Instance.loadGift(giftId);
#else
            if (cbGiftLoad != null)
            {
#if ENABLE_MYLOG
                SdkUtil.logd("ads admobmyLow tryloadGift not enable");
#endif
                var tmpcb = cbGiftLoad;
                cbGiftLoad = null;
                tmpcb(AD_State.AD_LOAD_FAIL);
            }
#endif
        }
        public override void loadGift(AdCallBack cb)
        {
            SdkUtil.logd("ads admobmyLow loadGift");
            if (giftId == null || !giftId.Contains("ca-app-pub"))
            {
#if ENABLE_MYLOG
                SdkUtil.logd("ads admobmyLow loadGift id incorrect");
#endif
                if (cbGiftLoad != null)
                {
                    var tmpcb = cbGiftLoad;
                    cbGiftLoad = null;
                    tmpcb(AD_State.AD_LOAD_FAIL);
                }
                return;
            }
            cbGiftLoad = cb;
            if (!isGiftLoading && !isGiftLoaded)
            {
                GiftTryLoad = 0;
                tryloadGift();
            }
            else
            {
                SdkUtil.logd($"ads admobmyLow loadGift isloading={isGiftLoading} or isloaded={isGiftLoaded}");
            }
        }
        public override bool showGift(AdCallBack cb)
        {
            SdkUtil.logd("ads admobmyLow showGift");
            cbGiftShow = null;
            if (getGiftLoaded())
            {
                isGiftLoaded = false;
#if ENABLE_ADS_ADMOB && USE_ADSMOB_MY
                cbGiftShow = cb;
                AdsAdmobMyLowBridge.Instance.showGift();
                return true;
#endif
            }
            return false;
        }

        //-------------------------------------------------------------------------------
#if ENABLE_ADS_ADMOB && USE_ADSMOB_MY

        #region BANNER AD EVENTS

        public void OnBannerAdLoadedEvent()
        {
#if ENABLE_MYLOG
            SdkUtil.logd("ads admobmyLow HandleBannerAdLoaded");
#endif
            dicBanner[0].isBNloaded = true;
            dicBanner[0].isBNLoading = false;
            BNTryLoad = 0;

            if (cbBanner != null)
            {
                var tmpcb = cbBanner;
                cbBanner = null;
                tmpcb(AD_State.AD_LOAD_OK);
            }
            if (advhelper != null)
            {
                advhelper.onBannerLoadOk(adsType);
            }
        }

        private void OnBannerAdLoadFailedEvent(string err)
        {
            SdkUtil.logd("ads admobmyLow OnBannerAdLoadFailedEvent=" + err);
            if (dicBanner[0].isBNLoading)
            {
                dicBanner[0].isBNLoading = false;
                BNTryLoad++;
                tryLoadBanner(0);
            }
        }
        private void OnBannerAdPaidEvent(int precisionType, string currencyCode, long valueMicros)
        {
            FIRhelper.logEventAdsPaidAdmob(0, giftId, precisionType, currencyCode, valueMicros);
        }
        #endregion

        #region INTERSTITIAL AD EVENTS
        private void OnInterstitialLoadedEvent()
        {
#if ENABLE_MYLOG
            SdkUtil.logd("ads admobmyLow HandleInterstitialLoaded");
#endif
            FullTryLoad = 0;
            isFullLoading = false;
            isFullLoaded = true;
            if (cbFullLoad != null)
            {
                var tmpcb = cbFullLoad;
                cbFullLoad = null;
                tmpcb(AD_State.AD_LOAD_OK);
            }
#if ENABLE_MYLOG
            SdkUtil.logd("ads admobmyLow HandleInterstitialLoaded 1");
#endif
        }

        private void OnInterstitialFailedEvent(string err)
        {
#if ENABLE_MYLOG
            SdkUtil.logd("ads admobmyLow HandleInterstitialFailedToLoad=" + err);
#endif
            isFullLoading = false;
            isFullLoaded = false;
            AdsProcessCB.Instance().Enqueue(() =>
            {
                FullTryLoad++;
                tryLoadFull(false);
            }, 1.0f);

#if ENABLE_MYLOG
            SdkUtil.logd("ads admobmyLow HandleInterstitialFailedToLoad 1");
#endif
        }

        private void OnInterstitialDisplayedEvent()
        {
#if ENABLE_MYLOG
            SdkUtil.logd("ads admobmyLow HandleInterstitialOpened");
#endif
            if (cbFullShow != null)
            {
                AdCallBack tmpcb = cbFullShow;
                AdsProcessCB.Instance().Enqueue(() => { tmpcb(AD_State.AD_SHOW); });
            }
        }

        private void onInterstitialFailedToShow(string err)
        {
#if ENABLE_MYLOG
            SdkUtil.logd("ads admobmyLow onInterstitialFailedToShow=" + err);
#endif
            isFullLoading = false;
            isFullLoaded = false;
            if (cbFullShow != null)
            {
                AdCallBack tmpcb = cbFullShow;
#if ENABLE_MYLOG
                SdkUtil.logd("ads admobmyLow full InterstitialAdShowFailedEvent 1");
#endif
                AdsProcessCB.Instance().Enqueue(() => { tmpcb(AD_State.AD_SHOW_FAIL); });
            }
            onFullClose();
        }

        private void OnInterstitialDismissedEvent()
        {
            isFullLoading = false;
            if (cbFullShow != null)
            {
                AdCallBack tmpcb = cbFullShow;
#if ENABLE_MYLOG
                SdkUtil.logd("ads admobmyLow full HandleInterstitialClosed");
#endif
                AdsProcessCB.Instance().Enqueue(() => { tmpcb(AD_State.AD_CLOSE); });
            }
            onFullClose();

            FullTryLoad = 0;
            cbFullShow = null;
#if ENABLE_MYLOG
            SdkUtil.logd("ads admobmyLow full HandleInterstitialClosed 1");
#endif
        }
        private void OnInterstitialAdPaidEvent(int precisionType, string currencyCode, long valueMicros)
        {
            FIRhelper.logEventAdsPaidAdmob(1, giftId, precisionType, currencyCode, valueMicros);
        }
        #endregion

        #region REWARDED VIDEO AD EVENTS

        private void OnRewardedAdLoadedEvent()
        {
#if ENABLE_MYLOG
            SdkUtil.logd("ads admobmyLow rw HandleRewardBasedVideoLoaded");
#endif
            GiftTryLoad = 0;
            isGiftLoading = false;
            isGiftLoaded = true;
            if (cbGiftLoad != null)
            {
                var tmpcb = cbGiftLoad;
                cbGiftLoad = null;
                tmpcb(AD_State.AD_LOAD_OK);
            }
#if ENABLE_MYLOG
            SdkUtil.logd("ads admobmyLow rw HandleRewardBasedVideoLoaded 1");
#endif
        }

        private void OnRewardedAdFailedEvent(string err)
        {
#if ENABLE_MYLOG
            SdkUtil.logd("ads admobmyLow rw HandleRewardBasedVideoFailedToLoad=" + err);
#endif
            isGiftLoading = false;
            isGiftLoaded = false;
            AdsProcessCB.Instance().Enqueue(() =>
            {
                GiftTryLoad++;
                tryloadGift();
            }, 1.0f);
#if ENABLE_MYLOG
            SdkUtil.logd("ads admobmyLow rw HandleRewardBasedVideoFailedToLoad 1");
#endif
        }

        private void OnRewardedAdDisplayedEvent()
        {
#if ENABLE_MYLOG
            SdkUtil.logd("ads admobmyLow rw HandleRewardBasedVideoOpened");
#endif
            GiftTryLoad = 0;
            if (cbGiftShow != null)
            {
                var tmpcb = cbGiftShow;
#if ENABLE_MYLOG
                SdkUtil.logd("ads admobmyLow rw HandleRewardBasedVideoClosed2");
#endif
                AdsProcessCB.Instance().Enqueue(() => { tmpcb(AD_State.AD_SHOW); });
            }
#if ENABLE_MYLOG
            SdkUtil.logd("ads admobmyLow rw HandleRewardBasedVideoOpened 1");
#endif
        }

        private void OnRewardedAdFailedToDisplayEvent(string err)
        {
#if ENABLE_MYLOG
            SdkUtil.logd("ads admobmyLow rw HandleRewardBasedVideoFailToShow=" + err);
#endif
            isGiftLoading = false;
            isGiftLoaded = false;
            if (cbGiftShow != null)
            {
                AdCallBack tmpcb = cbGiftShow;
#if ENABLE_MYLOG
                SdkUtil.logd("ads admobmyLow rw _cbAD fail");
#endif
                AdsProcessCB.Instance().Enqueue(() => { tmpcb(AD_State.AD_REWARD_FAIL); });
                AdsProcessCB.Instance().Enqueue(() => { tmpcb(AD_State.AD_SHOW_FAIL); });
            }
            onGiftClose();
        }

        private void OnRewardedAdReceivedRewardEvent()
        {
#if ENABLE_MYLOG
            SdkUtil.logd("ads admobmyLow rw HandleRewardBasedVideoRewarded");
#endif
            isRewardCom = true;
        }

        private void OnRewardedAdDismissedEvent()
        {
#if ENABLE_MYLOG
            SdkUtil.logd("ads admobmyLow rw HandleRewardBasedVideoClosed");
#endif
            isGiftLoading = false;
            isGiftLoaded = false;
            if (cbGiftShow != null)
            {
                AdCallBack tmpcb = cbGiftShow;
#if ENABLE_MYLOG
                SdkUtil.logd("ads admobmyLow rw _cbAD != null");
#endif
                if (isRewardCom)
                {
#if ENABLE_MYLOG
                    SdkUtil.logd("ads admobmyLow rw _cbAD reward");
#endif
                    AdsProcessCB.Instance().Enqueue(() => { tmpcb(AD_State.AD_REWARD_OK); });
                }
                else
                {
#if ENABLE_MYLOG
                    SdkUtil.logd("ads admobmyLow rw _cbAD fail");
#endif
                    AdsProcessCB.Instance().Enqueue(() => { tmpcb(AD_State.AD_REWARD_FAIL); });
                }
                AdsProcessCB.Instance().Enqueue(() => { tmpcb(AD_State.AD_CLOSE); });
            }
            onGiftClose();

            isRewardCom = false;
            GiftTryLoad = 0;
            cbGiftShow = null;
#if ENABLE_MYLOG
            SdkUtil.logd("ads admobmyLow rw HandleRewardBasedVideoClosed 3");
#endif
        }
        private void OnRewardedAdPaidEvent(int precisionType, string currencyCode, long valueMicros)
        {
            FIRhelper.logEventAdsPaidAdmob(2, giftId, precisionType, currencyCode, valueMicros);
        }
        #endregion

#endif

    }
}