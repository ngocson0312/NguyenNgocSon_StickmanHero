//#define ENABLE_ADS_FB

using System;
using UnityEngine;

#if ENABLE_ADS_FB
using AudienceNetwork;
#endif

namespace mygame.sdk
{
    public class AdsFB : AdsBase
    {
#if ENABLE_ADS_FB
        private AdView banner;
        private InterstitialAd full = null;
        private RewardedVideoAd gift = null;
#endif

        public override void InitAds()
        {
#if ENABLE_ADS_FB
            isEnable = true;
#endif
        }

        public override void AdsAwake()
        {
#if ENABLE_ADS_FB
            isEnable = true;
#endif
        }

        public override string getname()
        {
            return "fb";
        }

        protected override void tryLoadBanner(int type)
        {
#if ENABLE_ADS_FB
            SdkUtil.logd("fb tryLoadBanner = " + bannerId);
            if (BNTryLoad >= toTryLoad)
            {
                SdkUtil.logd("fb tryLoadBanner over trycount cbBanner="+ cbBanner);
                if (cbBanner != null)
                {
                    var tmpcb = cbBanner;
                    cbBanner = null;
                    tmpcb(AD_State.AD_LOAD_FAIL);
                }
                return;
            }
            if (banner != null)
            {
                SdkUtil.logd("fb tryLoadBanner destroy before load");
                banner.Dispose();
                banner = null;
            }

            isBNloaded = false;
            isBNLoading = false;
            this.banner = new AdView(bannerId.Trim(), AdSize.BANNER_HEIGHT_50);
            this.banner.Register(this.gameObject);
            banner.AdViewDidLoad = HandleBannerAdLoaded;
            banner.AdViewDidFailWithError = HandleBannerAdFailedToLoad;
            banner.LoadAd();
            SdkUtil.logd("fb tryLoadBanner 3");
#else
            if (cbBanner != null)
            {
                SdkUtil.logd("fb tryLoadBanner not enable");
                var tmpcb = cbBanner;
                cbBanner = null;
                tmpcb(AD_State.AD_LOAD_FAIL);
            }
#endif
        }

        public override void loadBanner(int type, AdCallBack cb)
        {
            SdkUtil.logd("fb loadBanner");
            cbBanner = cb;
            if (!dicBanner[type].isBNLoading)
            {
                BNTryLoad = 0;
                tryLoadBanner(type);
            }
            else
            {
                SdkUtil.logd("fb loadBanner isloading");
            }
        }
        public override bool showBanner(int type, int pos, int width, AdCallBack cb, float dxCenter)
        {
            SdkUtil.logd("fb showBanner");
            dicBanner[type].isBNShow = true;
#if ENABLE_ADS_FB
            if (banner != null && isBNloaded)
            {
                SdkUtil.logd("fb showBanner 1");
                AdPosition adposbn;
                if (posBanner == 0)
                {
                    adposbn = AdPosition.TOP;
                }
                else if (posBanner == 1)
                {
                    adposbn = AdPosition.BOTTOM;
                }
                else
                {
                    adposbn = AdPosition.BOTTOM;
                }
                banner.Show(adposbn);
                return true;
            }
            else
            {
                SdkUtil.logd("fb showBanner 10");
                posBanner = pos;
                loadBanner(cb);
                return false;
            }
#else
            if (cb != null)
            {
                SdkUtil.logd("iron tryLoadBanner not enable");
                cb(AD_State.AD_LOAD_FAIL);
            }
            return false;
#endif
        }

        public override bool showBanner(int type, Rect poscustom, AdCallBack cb)
        {
            if (cb != null)
            {
                SdkUtil.logd("iron tryLoadBanner not enable");
                cb(AD_State.AD_LOAD_FAIL);
            }
            return false;
        }
        public override void hideBanner(int type)
        {
            SdkUtil.logd("fb hideBanner");
            dicBanner[type].isBNShow = false;
#if ENABLE_ADS_FB
            if (banner != null)
            {
                banner.Dispose();
                banner = null;
                isBNloaded = false;
            }
#endif
        }
        public override void destroyBanner(int type)
        {
#if ENABLE_ADS_FB
            if (banner != null && isBNloaded)
            {
                banner.Dispose();
                banner = null;
            }
#endif
        }

        //
        protected override void tryLoadNative()
        { }
        public override void loadNative(AdCallBack cb)
        { }
        public override bool showNative(Rect pos, AdCallBack cb)
        { return false; }
        public override void hideNative()
        { }

        //
        public override void clearCurrFull()
        {
            if (getFullLoaded(false) == 1)
            {
#if ENABLE_ADS_FB
            full.Destroy();
            full = null;
            isFullLoaded = false;
#endif
            }
        }
        public override int getFullLoaded(bool isOpenads)
        {
#if ENABLE_ADS_FB
            if (isFullLoaded || (full != null && full.IsValid())) {
                return 1;
            } else
            {
                return 0;
            }
#else
            return 0;
#endif
        }
        protected override void tryLoadFull(bool isSplash)
        {
#if ENABLE_ADS_FB
            SdkUtil.logd("fb tryLoadFull =" + fullId);
            if (FullTryLoad >= toTryLoad)
            {
                SdkUtil.logd("fb tryLoadFull over try");
                if (cbFullLoad != null)
                {
                    var tmpcb = cbFullLoad;
                    cbFullLoad = null;
                    tmpcb(AD_State.AD_LOAD_FAIL);
                }
                return;
            }
            SdkUtil.logd("fb tryLoadFull load");
            isFullLoading = true;
            isFullLoaded = false;
            full = new InterstitialAd(fullId.Trim());
            full.Register(gameObject);
            full.InterstitialAdDidLoad = HandleInterstitialAdDidLoad;
            full.InterstitialAdDidFailWithError = HandleInterstitialAdDidFailWithError;
            full.InterstitialAdDidClose = HandleInterstitialAdDidClose;
            full.LoadAd();
#else
            if (cbFullLoad != null)
            {
                SdkUtil.logd("fb tryLoadFull not enable");
                var tmpcb = cbFullLoad;
                cbFullLoad = null;
                tmpcb(AD_State.AD_LOAD_FAIL);
            }
#endif
        }
        public override void loadFull(bool isSplash, AdCallBack cb)
        {
            SdkUtil.logd("fb loadFull");
            cbFullLoad = cb;
            if (!isFullLoading && !isFullLoaded)
            {
                FullTryLoad = 0;
                this.isSplash = isSplash;
                tryLoadFull(isSplash);
            }
            else
            {
                SdkUtil.logd("fb loadFull isloading or isloaded");
            }
        }
        public override bool showFull(bool isOpenads, AdCallBack cb)
        {
            SdkUtil.logd("fb showFull");
            cbFullShow = null;
            if (getFullLoaded(isOpenads) > 0)
            {
                FullTryLoad = 0;
                isFullLoaded = false;
#if ENABLE_ADS_FB
                cbFullShow = cb;
                full.Show();
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
#if ENABLE_ADS_FB
                gift.Destroy();
                gift = null;
                isGiftLoaded = false;
#endif
            }
        }
        public override bool getGiftLoaded()
        {
#if ENABLE_ADS_FB
            return (isGiftLoaded || (gift != null && gift.IsValid()));
#else
            return false;
#endif
        }
        protected override void tryloadGift()
        {
#if ENABLE_ADS_FB
            SdkUtil.logd("fb tryloadGift =" + giftId);
            if (GiftTryLoad >= toTryLoad)
            {
                SdkUtil.logd("fb tryloadGift over try");
                if (cbGiftLoad != null)
                {
                    var tmpcb = cbGiftLoad;
                    cbGiftLoad = null;
                    tmpcb(AD_State.AD_LOAD_FAIL);
                }
                return;
            }
            SdkUtil.logd("fb tryloadGift load");
            isGiftLoading = true;
            isGiftLoaded = false;
            gift = new RewardedVideoAd(giftId.Trim());
            gift.Register(gameObject);
            gift.RewardedVideoAdDidLoad = HandleRewardedVideoAdDidLoad;
            gift.RewardedVideoAdDidFailWithError = HandleRewardedVideoAdDidFailWithError;
            gift.RewardedVideoAdComplete = HandleRewardedVideoAdComplete;
            gift.RewardedVideoAdDidSucceed = HandleRewardedVideoAdDidSucceed;
            gift.RewardedVideoAdDidClose = HandleRewardedVideoAdDidClose;
            gift.LoadAd();
#else
            if (cbGiftLoad != null)
            {
                SdkUtil.logd("fb tryloadGift not enable");
                var tmpcb = cbGiftLoad;
                cbGiftLoad = null;
                tmpcb(AD_State.AD_LOAD_FAIL);
            }
#endif
        }
        public override void loadGift(AdCallBack cb)
        {
            SdkUtil.logd("fb loadGift");
            cbGiftLoad = cb;
            if (!isGiftLoading && !isGiftLoaded)
            {
                GiftTryLoad = 0;
                tryloadGift();
            }
            else
            {
                SdkUtil.logd("fb loadGift is loading or is loaded");
            }
        }
        public override bool showGift(AdCallBack cb)
        {
            SdkUtil.logd("fb showGift");
            if (getGiftLoaded())
            {
                cbGiftShow = cb;
                isGiftLoaded = false;
#if ENABLE_ADS_FB
                gift.Show();
                return true;
#endif
            }
            return false;
        }

        //-------------------------------------------------------------------------------
#if ENABLE_ADS_FB

        #region BANNER AD EVENTS

        private void HandleBannerAdLoaded()
        {
            SdkUtil.logd("fb HandleBannerAdLoaded");
            isBNloaded = true;
            isBNLoading = false;
            BNTryLoad = 0;

            if (banner != null)
            {
                if (isBNShow)
                {
                    SdkUtil.logd("fb BannerAdLoaded show");
                    AdPosition adposbn;
                    if (posBanner == 0)
                    {
                        adposbn = AdPosition.TOP;
                    }
                    else if (posBanner == 1)
                    {
                        adposbn = AdPosition.BOTTOM;
                    }
                    else
                    {
                        adposbn = AdPosition.BOTTOM;
                    }
                    banner.Show(adposbn);
                }
                else
                {
                    SdkUtil.logd("fb BannerAdLoaded hide");
                    banner.Dispose();
                    banner = null;
                }
            }

            if (cbBanner != null)
            {
                var tmpcb = cbBanner;
                cbBanner = null;
                tmpcb(AD_State.AD_LOAD_OK);
            }
        }

        private void HandleBannerAdFailedToLoad(string error)
        {
            SdkUtil.logd("fb HandleBannerAdFailedToLoad");
            isBNloaded = false;
            isBNLoading = false;
            BNTryLoad++;
            tryLoadBanner();
        }
        #endregion

        #region native ads
        private void HandleNativeLoaded(object sender, EventArgs e)
        {
            SdkUtil.logd("fb HandleNativeLoaded");
            if (cbNative != null)
            {
                var tmpcb = cbNative;
                cbNative = null;
                tmpcb(AD_State.AD_LOAD_OK);
            }
        }

        private void HandleNativeLoadFail(object sender, EventArgs e)
        {
            SdkUtil.logd("fb HandleNativeLoadFail");
            if (cbNative != null)
            {
                var tmpcb = cbNative;
                cbNative = null;
                tmpcb(AD_State.AD_LOAD_FAIL);
            }
        }
        #endregion

        #region INTERSTITIAL AD EVENTS

        private void HandleInterstitialAdDidLoad()
        {
            SdkUtil.logd("fb HandleInterstitialAdDidLoad");
            FullTryLoad = 0;
            isFullLoading = false;
            isFullLoaded = true;
            if (cbFullLoad != null)
            {
                var tmpcb = cbFullLoad;
                cbFullLoad = null;
                tmpcb(AD_State.AD_LOAD_OK);
            }
            SdkUtil.logd("fb HandleInterstitialAdDidLoad1");
        }

        private void HandleInterstitialAdDidFailWithError(string err)
        {
            SdkUtil.logd("fb HandleInterstitialAdDidFailWithError");
            isFullLoading = false;
            isFullLoaded = false;
            AdsProcessCB.Instance().Enqueue(() => {
                FullTryLoad++;
                tryLoadFull();
            });
            
            SdkUtil.logd("fb HandleInterstitialAdDidFailWithError1");
        }

        private void HandleInterstitialAdDidClose()
        {
            SdkUtil.logd("fb HandleInterstitialClosed 0");
            isFullLoading = false;
            isFullLoaded = false;
            if (cbFullShow != null)
            {
                AdCallBack tmpcb = cbFullShow;
                SdkUtil.logd("fb full HandleInterstitialClosed 1");
                AdsProcessCB.Instance().Enqueue(() => { 
                    tmpcb(AD_State.AD_CLOSE);
                    onFullClose();
                     });
            }
            AdsProcessCB.Instance().Enqueue(() => {
                onFullClose();
                });

            FullTryLoad = 0;
            cbFullShow = null;
            if (this.full != null)
            {
                this.full.Dispose();
                full = null;
            }
            SdkUtil.logd("fb HandleInterstitialClosed 2");
        }
        #endregion

        #region REWARDED VIDEO AD EVENTS

        private void HandleRewardedVideoAdDidLoad()
        {
            SdkUtil.logd("fb rw HandleRewardBasedVideoLoaded");
            GiftTryLoad = 0;
            isGiftLoading = false;
            isGiftLoaded = true;
            if (cbGiftLoad != null)
            {
                var tmpcb = cbGiftLoad;
                cbGiftLoad = null;
                tmpcb(AD_State.AD_LOAD_OK);
            }
            SdkUtil.logd("fb rw HandleRewardBasedVideoLoaded1");
        }

        private void HandleRewardedVideoAdDidFailWithError(string err)
        {
            SdkUtil.logd("fb rw HandleRewardBasedVideoFailedToLoad");
            isGiftLoading = false;
            isGiftLoaded = false;
            AdsProcessCB.Instance().Enqueue(() => {
                GiftTryLoad++;
                tryloadGift();
            });
            
            SdkUtil.logd("fb rw HandleRewardBasedVideoFailedToLoad1");
        }

        private void HandleRewardedVideoAdComplete()
        {
            SdkUtil.logd("fb rw HandleRewardedVideoAdComplete");
            isRewardCom = true;
        }

        private void HandleRewardedVideoAdDidSucceed()
        {
            SdkUtil.logd("fb rw HandleRewardBasedVideoRewarded");
            isRewardCom = true;
        }

        private void HandleRewardedVideoAdDidClose()
        {
            SdkUtil.logd("fb rw HandleRewardBasedVideoClosed");
            isGiftLoading = false;
            isGiftLoaded = false;
            if (cbGiftShow != null)
            {
                AdCallBack tmpcb = cbGiftShow;
                SdkUtil.logd("fb rw _cbAD != null");
                if (isRewardCom)
                {
                    SdkUtil.logd("fb rw _cbAD reward");
                    AdsProcessCB.Instance().Enqueue(() => { tmpcb(AD_State.AD_REWARD_OK); });
                }
                else
                {
                    SdkUtil.logd("fb rw _cbAD fail");
                    AdsProcessCB.Instance().Enqueue(() => { tmpcb(AD_State.AD_REWARD_FAIL); });
                }
            }

            SdkUtil.logd("fb rw HandleRewardBasedVideoClosed1");
            if (cbGiftShow != null)
            {
                AdCallBack tmpcb = cbGiftShow;
                SdkUtil.logd("fb rw HandleRewardBasedVideoClosed2");
                AdsProcessCB.Instance().Enqueue(() => { 
                    tmpcb(AD_State.AD_CLOSE); 
                    onGiftClose();
                    });
            }
            AdsProcessCB.Instance().Enqueue(() => {
                    onGiftClose();
                    });
            SdkUtil.logd("fb rw HandleRewardBasedVideoClosed3");
            isRewardCom = false;
            GiftTryLoad = 0;
            cbGiftShow = null;
            if (gift != null)
            {
                gift.Dispose();
                gift = null;
            }
            SdkUtil.logd("fb rw HandleRewardBasedVideoClosed3");
        }

        #endregion

#endif

    }
}