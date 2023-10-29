//#define ENABLE_ADS_Mopub

using System;
using UnityEngine;
using System.Collections;

#if ENABLE_ADS_Mopub

#endif

namespace mygame.sdk
{
    public class AdsMoPub : AdsBase
    {
#if ENABLE_ADS_Mopub

#endif
        bool isWaitLoadBanner = false;
        bool isWaitLoadFull = false;
        bool isWaitLoadGift = false;
        bool isInitSdk = false;
        public override void InitAds()
        {
#if ENABLE_ADS_Mopub
            isEnable = true;
#endif
        }

        public override void AdsAwake()
        {
#if ENABLE_ADS_Mopub
            isEnable = true;
#endif
        }

        public override string getname()
        {
            return "MoPub";
        }

        private void Start()
        {
#if ENABLE_ADS_Mopub
            MoPubManager.OnAdLoadedEvent += OnBannerAdLoadedEvent;
            MoPubManager.OnAdFailedEvent += OnBannerAdLoadFailedEvent;

            MoPubManager.OnInterstitialLoadedEvent += OnInterstitialLoadedEvent;
            MoPubManager.OnInterstitialFailedEvent += OnInterstitialFailedEvent;
            MoPubManager.OnInterstitialShownEvent += OnInterstitialDisplayedEvent;
            MoPubManager.OnInterstitialExpiredEvent += OnInterstitialExpiredEvent;
            MoPubManager.OnInterstitialDismissedEvent += OnInterstitialDismissedEvent;

            MoPubManager.OnRewardedVideoLoadedEvent += OnRewardedAdLoadedEvent;
            MoPubManager.OnRewardedVideoFailedEvent += OnRewardedAdFailedEvent;
            MoPubManager.OnRewardedVideoExpiredEvent += OnRewardedVideoExpiredEvent;
            MoPubManager.OnRewardedVideoFailedToPlayEvent += OnRewardedAdFailedToDisplayEvent;
            MoPubManager.OnRewardedVideoShownEvent += OnRewardedAdDisplayedEvent;
            MoPubManager.OnRewardedVideoClickedEvent += OnRewardedAdClickedEvent;
            MoPubManager.OnRewardedVideoClosedEvent += OnRewardedAdDismissedEvent;
            MoPubManager.OnRewardedVideoReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;

            MoPub.LoadBannerPluginsForAdUnits(new string[] { bannerId });
            MoPub.LoadInterstitialPluginsForAdUnits(new string[] { fullId });
            MoPub.LoadRewardedVideoPluginsForAdUnits(new string[] { giftId });
#endif
        }

        public void OnSdkInitializedEvent(string adUnitId)
        {
            SdkUtil.logd("Mopub OnSdkInitializedEvent = " + adUnitId);
            isInitSdk = true;
            if (isWaitLoadBanner)
            {
                isWaitLoadBanner = false;
            }
            if (isWaitLoadFull)
            {
                isWaitLoadFull = false;
                loadFull(false, null);
            }
            if (isWaitLoadGift)
            {
                isWaitLoadGift = false;
                loadGift(null);
            }
            // The SDK is initialized here. Ready to make ad requests.
        }

        protected override void tryLoadBanner(int type)
        {
            type = 0;
#if ENABLE_ADS_Mopub
            SdkUtil.logd("Mopub tryLoadBanner = " + bannerId);
            if (BNTryLoad >= toTryLoad)
            {
                SdkUtil.logd("Mopub tryLoadBanner over trycount");
                if (cbBanner != null)
                {
                    var tmpcb = cbBanner;
                    cbBanner = null;
                    tmpcb(AD_State.AD_LOAD_FAIL);
                }
                return;
            }

            dicBanner[type].isBNLoading = false;
            dicBanner[type].isBNloaded = true;
            MoPub.AdPosition adposbn;
            if (dicBanner[type].posBanner == 0)
            {
                adposbn = MoPub.AdPosition.TopCenter;
            }
            else if (dicBanner[type].posBanner == 1)
            {
                adposbn = MoPub.AdPosition.BottomCenter;
            }
            else
            {
                adposbn = MoPub.AdPosition.BottomCenter;
            }
            MoPub.RequestBanner(bannerId, adposbn, MoPub.MaxAdSize.Width300Height50);
            SdkUtil.logd("Mopub tryLoadBanner 3");
            BNTryLoad = 0;
#else
            if (cbBanner != null)
            {
                SdkUtil.logd("Mopub tryLoadBanner not enable");
                var tmpcb = cbBanner;
                cbBanner = null;
                tmpcb(AD_State.AD_LOAD_FAIL);
            }
#endif
        }
        public override void loadBanner(int type, AdCallBack cb)
        {
            SdkUtil.logd("Mopub loadBanner");
            if (!isInitSdk)
            {
                SdkUtil.logd("Mopub loadBanner not init sdk");
                isWaitLoadBanner = true;
                if (cb != null)
                {
                    cb(AD_State.AD_LOAD_FAIL);
                }
                return;
            }
            cbBanner = cb;
            type = 0;
            if (!dicBanner[type].isBNLoading)
            {
                BNTryLoad = 0;
                tryLoadBanner(type);
            }
            else
            {
                SdkUtil.logd("Mopub loadBanner isloading");
            }
        }
        public override bool showBanner(int type, int pos, int width, AdCallBack cb, float dxCenter)
        {
            SdkUtil.logd("Mopub showBanner");
            type = 0;
#if ENABLE_ADS_Mopub
            dicBanner[type].isBNShow = true;
            if (dicBanner[type].isBNloaded)
            {
                SdkUtil.logd("Mopub showBanner 1");
                MoPub.ShowBanner(bannerId, true);
                return true;
            }
            else
            {
                SdkUtil.logd("Mopub showBanner 10");
                dicBanner[type].posBanner = pos;
                loadBanner(type, cb);
                return false;
            }
#else
            if (cb != null)
            {
                SdkUtil.logd("Mopub tryLoadBanner not enable");
                cb(AD_State.AD_LOAD_FAIL);
            }
            return false;
#endif
        }
        public override bool showBanner(int type, Rect poscustom, AdCallBack cb)
        {
            type = 0;
            if (cb != null)
            {
                SdkUtil.logd("Mopub tryLoadBanner not enable");
                cb(AD_State.AD_LOAD_FAIL);
            }
            return false;
        }
        public override void hideBanner(int type)
        {
            SdkUtil.logd("Mopub hideBanner");
            type = 0;
            dicBanner[type].isBNShow = false;
#if ENABLE_ADS_Mopub
            MoPub.ShowBanner(bannerId, false);
#endif
        }
        public override void destroyBanner(int type)
        {
            SdkUtil.logd("Mopub destroyBanner");
            type = 0;
            hideBanner(0);
#if ENABLE_ADS_Mopub
            //MoPub.DestroyBanner (bannerId);
#endif
            dicBanner[type].isBNLoading = false;
            dicBanner[type].isBNloaded = false;
        }

        // Native
        protected override void tryLoadNative()
        { }
        public override void loadNative(AdCallBack cb)
        {
            if (cb != null)
            {
                cb(AD_State.AD_LOAD_FAIL);
            }
        }
        public override bool showNative(Rect pos, AdCallBack cb)
        {
            if (cb != null)
            {
                SdkUtil.logd("Mopub tryLoadBanner not enable");
                cb(AD_State.AD_LOAD_FAIL);
            }
            return false;
        }
        public override void hideNative()
        { }

        //
        public override void clearCurrFull()
        {
            if (getFullLoaded(false) == 1)
            {
#if ENABLE_ADS_Mopub
            isFullLoaded = false;123
#endif
            }
        }
        public override int getFullLoaded(bool _isSplash)
        {
            SdkUtil.logd("Mopub getFullLoaded issplash=" + _isSplash);
            if (_isSplash)
            {
                if (splashId == null || splashId.Length < 10)
                {
                    _isSplash = false;
                    SdkUtil.logd("Mopub getFullLoaded change is splash");
                }
            }
#if ENABLE_ADS_Mopub
            int re = 0;
            if (_isSplash)
            {
                if (isFullLoaded || MoPub.IsInterstitialReady(splashId))
                {
                    re = 2;
                } 
                else if (isFullLoaded || MoPub.IsInterstitialReady(fullId))
                {
                    re = 1;
                }
            }
            else
            {
                if (isFullLoaded || MoPub.IsInterstitialReady(fullId))
                {
                    re = 1;
                }
                else if (isFullLoaded || MoPub.IsInterstitialReady(splashId))
                {
                    re = 2;
                }
            }
#else
            return 0;
#endif
        }
        protected override void tryLoadFull(bool _isSplash)
        {
#if ENABLE_ADS_Mopub
            SdkUtil.logd("Mopub tryLoadFull issplash=" + _isSplash);
            if (_isSplash)
            {
                if (splashId == null || splashId.Length < 10)
                {
                    _isSplash = false;
                    SdkUtil.logd("Mopub tryLoadFull change is splash");
                }
                else
                {
                    SdkUtil.logd("Mopub tryLoadFull splash=" + splashId);
                }
            }
            if (!_isSplash)
            {
                SdkUtil.logd("Mopub tryLoadFull=" + fullId);
            }
            int tryload;
            if (_isSplash)
            {
                tryload = FullSplashTryLoad;
            }
            else
            {
                tryload = FullTryLoad;
            }

            if (tryload >= toTryLoad)
            {
                if (_isSplash)
                {
                    SdkUtil.logd("Mopub tryLoadFull splash over try");
                    if (cbSplashLoad != null)
                    {
                        var tmpcb = cbSplashLoad;
                        cbSplashLoad = null;
                        tmpcb(AD_State.AD_LOAD_FAIL);
                    }
                }
                else
                {
                    SdkUtil.logd("Mopub tryLoadFull over try");
                    if (cbFullLoad != null)
                    {
                        var tmpcb = cbFullLoad;
                        cbFullLoad = null;
                        tmpcb(AD_State.AD_LOAD_FAIL);
                    }
                }
                return;
            }
            if (_isSplash)
            {
                isSplashLoading = true;
                isSplashLoaded = false;
                MoPub.RequestInterstitialAd(splashId);
            }
            else
            {
                isFullLoading = true;
                isFullLoaded = false;
                MoPub.RequestInterstitialAd(fullId);
            }
#else
            if (cbFullLoad != null)
            {
                SdkUtil.logd("Mopub tryLoadFull not enable");
                var tmpcb = cbFullLoad;
                cbFullLoad = null;
                tmpcb(AD_State.AD_LOAD_FAIL);
            }
#endif
        }
        public override void loadFull(bool _isSplash, AdCallBack cb)
        {
            SdkUtil.logd("Mopub loadFull isSplash=" + _isSplash);
            if (!isInitSdk)
            {
                SdkUtil.logd("Mopub loadFull not init sdk isSplash=" + _isSplash);
                isWaitLoadFull = true;
                if (cb != null)
                {
                    cb(AD_State.AD_LOAD_FAIL);
                }
                return;
            }
            if (_isSplash)
            {
                if (splashId == null || splashId.Length < 10)
                {
                    _isSplash = false;
                    SdkUtil.logd("Mopub loadFull change is splash");
                }
            }
            cbFullLoad = cb;
            if (!isFullLoading && !isFullLoaded)
            {
                FullTryLoad = 0;
                tryLoadFull(_isSplash);
            }
            else
            {
                SdkUtil.logd("Mopub loadFull loading=" + isFullLoading + " or loaded=" + isFullLoaded);
            }
        }
        public override bool showFull(bool _isSplash, AdCallBack cb)
        {
            SdkUtil.logd("Mopub showFull isspalsh=" + _isSplash);
            if (_isSplash)
            {
                if (splashId == null || splashId.Length < 10)
                {
                    _isSplash = false;
                    SdkUtil.logd("Mopub showFull change is splash");
                }
            }
            cbFullShow = null;
            int ss = getFullLoaded(_isSplash);
            if (ss > 0)
            {
                FullTryLoad = 0;
                isFullLoaded = false;
#if ENABLE_ADS_Mopub
                cbFullShow = cb;
                if (_isSplash)
                {
                    MoPub.ShowInterstitialAd(splashId);
                }
                else
                {
                    MoPub.ShowInterstitialAd(fullId);
                }

                return true;
#endif
            }
            return false;
        }

        //------------------------------------------------
        public override void clearCurrGift()
        {
            if (getGiftLoaded())
            {
#if ENABLE_ADS_Mopub
                isGiftLoaded = false;123
#endif
            }
        }
        public override bool getGiftLoaded()
        {
#if ENABLE_ADS_Mopub
            return (isGiftLoaded || (MoPub.HasRewardedVideo(giftId)));
#endif
            return false;
        }
        protected override void tryloadGift()
        {
#if ENABLE_ADS_Mopub
            SdkUtil.logd("Mopub Request gift =" + giftId);
            if (GiftTryLoad >= toTryLoad)
            {
                SdkUtil.logd("Mopub tryLoadgift over try");
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
            MoPub.RequestRewardedVideo(giftId);
            timeLoadGift = SdkUtil.systemCurrentMiliseconds();
            StartCoroutine(WaitGiftErr());
#else
            if (cbGiftLoad != null)
            {
                var tmpcb = cbGiftLoad;
                cbGiftLoad = null;
                tmpcb(AD_State.AD_LOAD_FAIL);
            }
#endif      
        }

        IEnumerator WaitGiftErr()
        {
            for (int i = 0; i < tLoadGiftErr; i++)
            {
                yield return new WaitForSeconds(1.0f);
                if (isGiftLoading)
                {
                    if (i == (tLoadGiftErr - 1) && timeLoadGift > 0)
                    {
                        SdkUtil.logd("Mopub WaitGiftErr overtime");
                        timeLoadGift = 0;
                        tLoadGiftErr = 45;
                        isGiftLoading = false;
                        isGiftLoaded = false;
                        GiftTryLoad++;
                        tryloadGift();
                        yield break;
                    }
                }
                else
                {
                    i = 1000;
                    tLoadGiftErr = 45;
                    yield break;
                }
            }
        }

        public override void loadGift(AdCallBack cb)
        {
            SdkUtil.logd("Mopub loadGift");
            if (!isInitSdk)
            {
                SdkUtil.logd("Mopub loadGift not init sdk");
                isWaitLoadGift = true;
                if (cb != null)
                {
                    cb(AD_State.AD_LOAD_FAIL);
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
                SdkUtil.logd("Mopub loadGift loading=" + isGiftLoading + " or loaded=" + isGiftLoaded);
            }
        }
        public override bool showGift(AdCallBack cb)
        {
            SdkUtil.logd("Mopub showGift");
            if (getGiftLoaded())
            {
                cbGiftShow = cb;
                isGiftLoaded = false;
#if ENABLE_ADS_Mopub
                MoPub.ShowRewardedVideo(giftId);
                return true;
#endif
            }
            return false;
        }

        //-------------------------------------------------------------------------------
#if ENABLE_ADS_Mopub
        #region BANNER AD EVENT
        private void OnBannerAdLoadedEvent(string adUnitId, float heigh)
        {
            SdkUtil.logd("Mopub OnBannerAdLoadedEvent=" + adUnitId);
            dicBanner[0].isBNloaded = true;
            dicBanner[0].isBNLoading = false;
            BNTryLoad = 0;

            if (dicBanner[0].isBNShow)
            {
                SdkUtil.logd("Mopub BannerAdLoaded show");
                if (!dicBanner[0].isShowing)
                {
                    dicBanner[0].isShowing = true;
                    MoPub.ShowBanner(adUnitId, true);
                }
            }
            else
            {
                SdkUtil.logd("Mopub BannerAdLoaded hide");
                dicBanner[0].isShowing = false;
                MoPub.ShowBanner(adUnitId, false);
            }

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
        private void OnBannerAdLoadFailedEvent(string adUnitId, string err)
        {
            SdkUtil.logd("Mopub OnBannerAdLoadFailedEvent=" + adUnitId + ", err=" + err);
            if (cbBanner != null)
            {
                var tmpcb = cbBanner;
                cbBanner = null;
                tmpcb(AD_State.AD_LOAD_FAIL);
            }
            if (advhelper != null)
            {
                advhelper.onBannerLoadFail(adsType);
            }
        }
        #endregion

        #region INTERSTITIAL AD EVENTS
        private void OnInterstitialLoadedEvent(string adUnitId)
        {
            SdkUtil.logd("Mopub HandleInterstitialAdDidLoad = " + adUnitId);
            if (fullId.CompareTo(adUnitId) == 0)
            {
                FullTryLoad = 0;
                isFullLoading = false;
                isFullLoaded = true;
                if (cbFullLoad != null)
                {
                    var tmpcb = cbFullLoad;
                    cbFullLoad = null;
                    tmpcb(AD_State.AD_LOAD_OK);
                }
            }
            else
            {
                FullSplashTryLoad = 0;
                isSplashLoading = false;
                isSplashLoaded = true;
                if (cbSplashLoad != null)
                {
                    var tmpcb = cbSplashLoad;
                    cbSplashLoad = null;
                    tmpcb(AD_State.AD_LOAD_OK);
                }
            }

            SdkUtil.logd("Mopub HandleInterstitialAdDidLoad1");
        }

        private void OnInterstitialFailedEvent(string adUnitId, string errorCode)
        {
            SdkUtil.logd("Mopub HandleInterstitialAdDidFailWithError adid=" + adUnitId + ", err=" + errorCode);
            if (fullId.CompareTo(adUnitId) == 0)
            {
                isFullLoading = false;
                isFullLoaded = false;
                AdsProcessCB.Instance().Enqueue(() =>
                {
                    FullTryLoad++;
                    tryLoadFull(false);
                }, 0.1f);
            }
            else
            {
                isSplashLoaded = false;
                isSplashLoading = false;
                AdsProcessCB.Instance().Enqueue(() =>
                {
                    FullSplashTryLoad++;
                    tryLoadFull(true);
                }, 0.1f);
            }

            SdkUtil.logd("Mopub HandleInterstitialAdDidFailWithError1");
        }

        private void OnInterstitialDisplayedEvent(string adUnitId)
        {
            SdkUtil.logd("Mopub OnInterstitialDisplayedEvent=" + adUnitId);
            if (cbFullShow != null)
            {
                AdCallBack tmpcb = cbFullShow;
                AdsProcessCB.Instance().Enqueue(() =>
                {
                    tmpcb(AD_State.AD_SHOW);
                });
            }
        }

        private void OnInterstitialExpiredEvent(string adUnitId)
        {
            SdkUtil.logd("Mopub OnInterstitialExpiredEvent adid=" + adUnitId);
            isFullLoading = false;
            isFullLoaded = false;
            if (cbFullShow != null)
            {
                AdCallBack tmpcb = cbFullShow;
                SdkUtil.logd("Mopub full OnInterstitialExpiredEvent 1");
                AdsProcessCB.Instance().Enqueue(() =>
                {
                    tmpcb(AD_State.AD_SHOW_FAIL);
                    onFullClose();
                });
            }
        }

        private void OnInterstitialDismissedEvent(string adUnitId)
        {
            SdkUtil.logd("Mopub HandleInterstitialClosed = " + adUnitId);
            isFullLoading = false;
            isFullLoaded = false;
            if (cbFullShow != null)
            {
                AdCallBack tmpcb = cbFullShow;
                SdkUtil.logd("Mopub full HandleInterstitialClosed 1");
                AdsProcessCB.Instance().Enqueue(() =>
                {
                    tmpcb(AD_State.AD_CLOSE);
                    onFullClose();
                });
            }
            else
            {
                AdsProcessCB.Instance().Enqueue(() =>
                {
                    onFullClose();
                });
            }

            FullTryLoad = 0;
            cbFullShow = null;
            SdkUtil.logd("Mopub HandleInterstitialClosed 2");
        }
        #endregion

        #region REWARDED VIDEO AD EVENTS

        private void OnRewardedAdLoadedEvent(string adUnitId)
        {
            SdkUtil.logd("Mopub rw OnRewardedAdLoadedEvent=" + adUnitId);
            GiftTryLoad = 0;
            isGiftLoading = false;
            isGiftLoaded = true;
            timeLoadGift = 0;
            tLoadGiftErr = 45;
        }

        private void OnRewardedAdFailedEvent(string adUnitId, string errorCode)
        {
            SdkUtil.logd("Mopub rw OnRewardedAdFailedEvent=" + adUnitId + ", err=" + errorCode);
            isGiftLoading = false;
            isGiftLoaded = false;
            timeLoadGift = 0;
            tLoadGiftErr = 45;
            AdsProcessCB.Instance().Enqueue(() =>
            {
                GiftTryLoad++;
                tryloadGift();
            }, 0.1f);
            SdkUtil.logd("Mopub rw OnRewardedAdFailedEvent 1");
        }
        private void OnRewardedVideoExpiredEvent(string adUnitId)
        {
            SdkUtil.logd("Mopub rw OnRewardedVideoExpiredEvent=" + adUnitId);
        }
        private void OnRewardedAdDisplayedEvent(string adUnitId)
        {
            SdkUtil.logd("Mopub rw OnRewardedAdDisplayedEvent=" + adUnitId);
            if (cbGiftShow != null)
            {
                AdCallBack tmpcb = cbGiftShow;
                AdsProcessCB.Instance().Enqueue(() => { tmpcb(AD_State.AD_SHOW); });
            }
        }

        private void OnRewardedAdClickedEvent(string adUnitId)
        {
            SdkUtil.logd("Mopub rw OnRewardedAdClickedEvent=" + adUnitId);
            isRewardCom = true;
        }

        private void OnRewardedAdReceivedRewardEvent(string adUnitId, string label, float amount)
        {
            SdkUtil.logd("Mopub rw OnRewardedAdReceivedRewardEvent=" + adUnitId);
            isRewardCom = true;
        }

        private void OnRewardedAdFailedToDisplayEvent(string adUnitId, string errorCode)
        {
            SdkUtil.logd("Mopub rw OnRewardedAdFailedToDisplayEvent adid=" + adUnitId + ", err=" + errorCode);
            isGiftLoading = false;
            isGiftLoaded = false;
            if (cbGiftShow != null)
            {
                AdCallBack tmpcb = cbGiftShow;
                SdkUtil.logd("Mopub rw _cbAD fail");
                AdsProcessCB.Instance().Enqueue(() => { tmpcb(AD_State.AD_REWARD_FAIL); });
            }

            if (cbGiftShow != null)
            {
                AdCallBack tmpcb = cbGiftShow;
                SdkUtil.logd("Mopub rw OnRewardedAdFailedToDisplayEvent2");
                AdsProcessCB.Instance().Enqueue(() =>
                {
                    tmpcb(AD_State.AD_SHOW_FAIL);
                    onGiftClose();
                });
            }
        }

        private void OnRewardedAdDismissedEvent(string adUnitId)
        {
            SdkUtil.logd("Mopub rw OnRewardedAdDismissedEvent=" + adUnitId);
            isGiftLoading = false;
            isGiftLoaded = false;
            if (cbGiftShow != null)
            {
                AdCallBack tmpcb = cbGiftShow;
                SdkUtil.logd("Mopub rw _cbAD != null");
                if (isRewardCom)
                {
                    SdkUtil.logd("Mopub rw _cbAD reward");
                    AdsProcessCB.Instance().Enqueue(() => { tmpcb(AD_State.AD_REWARD_OK); });
                }
                else
                {
                    SdkUtil.logd("Mopub rw _cbAD fail");
                    AdsProcessCB.Instance().Enqueue(() => { tmpcb(AD_State.AD_REWARD_FAIL); });
                }
            }

            SdkUtil.logd("Mopub rw OnRewardedAdDismissedEvent1");
            if (cbGiftShow != null)
            {
                AdCallBack tmpcb = cbGiftShow;
                SdkUtil.logd("Mopub rw OnRewardedAdDismissedEvent2");
                AdsProcessCB.Instance().Enqueue(() =>
                {
                    tmpcb(AD_State.AD_CLOSE);
                    onGiftClose();
                });
            }
            else
            {
                AdsProcessCB.Instance().Enqueue(() =>
                {
                    onGiftClose();
                });
            }

            isRewardCom = false;
            GiftTryLoad = 0;
            cbGiftShow = null;
            SdkUtil.logd("Mopub rw OnRewardedAdDismissedEvent3");
        }

        #endregion

#endif

    }
}