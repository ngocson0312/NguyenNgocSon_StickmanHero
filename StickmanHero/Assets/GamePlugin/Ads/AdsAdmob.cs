//#define ENABLE_ADS_ADMOB

using System;
using UnityEngine;

#if ENABLE_ADS_ADMOB && !USE_ADSMOB_MY
using GoogleMobileAds.Api;
using mygame.plugin.Android;
#endif

namespace mygame.sdk
{
    public class AdsAdmob : AdsBase
    {

#if ENABLE_ADS_ADMOB && !USE_ADSMOB_MY
        private BannerView banner = null;
        private BannerView bannerRect = null;
        private InterstitialAd full = null;
        private RewardedAd gift = null;
#endif

        public override void AdsAwake()
        {
#if ENABLE_ADS_ADMOB && !USE_ADSMOB_MY
            isEnable = true;
#endif
        }

        public override void InitAds()
        {
            // dicBanner[0].banner = banner;
            // dicBanner[1].banner = bannerRect;

#if ENABLE_ADS_ADMOB && !USE_ADSMOB_MY
            isEnable = true;
            MobileAds.Initialize(initStatus => { });
#endif
        }

        public override string getname()
        {
            return "adsmob";
        }

        protected override void tryLoadBanner(int type)
        {
#if ENABLE_ADS_ADMOB && !USE_ADSMOB_MY
#if ENABLE_MYLOG
            SdkUtil.logd("ads admob" + adsType + " tryLoadBanner = " + bannerId);
#endif
            if (BNTryLoad >= toTryLoad)
            {
#if ENABLE_MYLOG
                SdkUtil.logd("ads admob" + adsType + " tryLoadBanner over trycount");
#endif
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
                return;
            }
            if (dicBanner[type].banner != null)
            {
#if ENABLE_MYLOG
                SdkUtil.logd("ads admob" + adsType + " tryLoadBanner destroy before load");
#endif
                ((BannerView)dicBanner[type].banner).Hide();
                ((BannerView)dicBanner[type].banner).Destroy();
                dicBanner[type].banner = null;
            }

            dicBanner[type].isBNLoading = true;
            dicBanner[type].isBNloaded = false;
            AdPosition adposbn;
            if (dicBanner[type].posBanner == 0)
            {
                adposbn = AdPosition.Top;
            }
            else if (dicBanner[type].posBanner == 1)
            {
                adposbn = AdPosition.Bottom;
            }
            else
            {
                adposbn = AdPosition.Bottom;
            }

            BannerView _bn;
            if (type == 0)
            {
                if (dicBanner[type].typeSize == 0)
                {
#if ENABLE_MYLOG
                    SdkUtil.logd("ads admob" + adsType + " tryLoadBanner banner size ");
#endif
                    if (SdkUtil.isiPad())
                    {
                        banner = new BannerView(bannerId.Trim(), AdSize.Leaderboard, adposbn);
                    }
                    else
                    {
                        banner = new BannerView(bannerId.Trim(), AdSize.Banner, adposbn);
                    }
                }
                else if (dicBanner[type].typeSize == 1)
                {
#if ENABLE_MYLOG
                    SdkUtil.logd("ads admob" + adsType + " tryLoadBanner smart ");
#endif
                    banner = new BannerView(bannerId.Trim(), AdSize.SmartBanner, adposbn);
                }
                else
                {
#if ENABLE_MYLOG
                    SdkUtil.logd("ads admob" + adsType + " tryLoadBanner banner adaptive ");
#endif
                    float widthInPixels = Screen.safeArea.width > 0 ? Screen.safeArea.width : Screen.width;
                    int width = (int)(widthInPixels / MobileAds.Utils.GetDeviceScale());
                    AdSize adaptiveSize = AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(width);

                    banner = new BannerView(bannerId.Trim(), adaptiveSize, adposbn);
                }
                _bn = banner;
            }
            else
            {
                bannerRect = new BannerView(bannerId.Trim(), AdSize.MediumRectangle, adposbn);
                _bn = bannerRect;
            }
            if (dicBanner[type].banner != null)
            {
                ((BannerView)dicBanner[type].banner).Destroy();
                dicBanner[type].banner = null;
            }
            dicBanner[type].banner = _bn;
            _bn.OnAdLoaded += HandleBannerAdLoaded;
            _bn.OnAdFailedToLoad += HandleBannerAdFailedToLoad;
            _bn.OnAdLoaded += HandleBannerAdOpened;
            _bn.OnAdClosed += HandleBannerAdClosed;
            _bn.OnPaidEvent += HandleBannerAdPaidEvent;
            AdRequest request = new AdRequest.Builder().Build();
            if (request != null)
            {
                _bn.LoadAd(request);
            }
            else
            {
                request = new AdRequest.Builder().Build();
                if (request != null)
                {
                    _bn.LoadAd(request);
                }
            }
#if ENABLE_MYLOG
            SdkUtil.logd("ads admob" + adsType + " tryLoadBanner 3");
#endif
#else
            if (cbBanner != null)
            {
#if ENABLE_MYLOG
                SdkUtil.logd("ads admob" + adsType + " tryLoadBanner not enable");
#endif
                var tmpcb = cbBanner;
                cbBanner = null;
                tmpcb(AD_State.AD_LOAD_FAIL);
            }
#endif
        }
        public override void loadBanner(int type, AdCallBack cb)
        {
#if ENABLE_MYLOG
            SdkUtil.logd("ads admob" + adsType + " loadBanner");
#endif
            cbBanner = cb;
            if (!dicBanner[type].isBNLoading)
            {
                BNTryLoad = 0;
                tryLoadBanner(type);
            }
            else
            {
#if ENABLE_MYLOG
                SdkUtil.logd("ads admob" + adsType + " loadBanner isloading");
#endif
            }
        }

        public override bool showBanner(int type, int pos, int width, AdCallBack cb, float dxCenter)
        {
#if ENABLE_MYLOG
            SdkUtil.logd("ads admob" + adsType + " showBanner");
#endif

            dicBanner[type].isBNShow = true;
            dicBanner[type].posBanner = pos;
            bnWidth = width;
#if ENABLE_ADS_ADMOB && !USE_ADSMOB_MY
            if (dicBanner[type].banner != null && dicBanner[type].isBNloaded)
            {
#if ENABLE_MYLOG
                SdkUtil.logd("ads admob" + adsType + " showBanner0 1");
#endif
                dicBanner[type].isBNHide = false;
                ((BannerView)dicBanner[type].banner).Show();
                if (cb != null)
                {
                    cb(AD_State.AD_SHOW);
                }
                return true;
            }
            else
            {
#if ENABLE_MYLOG
                SdkUtil.logd("ads admob" + adsType + " showBanner 10");
#endif
                loadBanner(type, cb);
                return false;
            }
#else
            if (cb != null)
            {
#if ENABLE_MYLOG
                SdkUtil.logd("ads admob" + adsType + " tryLoadBanner not enable");
#endif
                cb(AD_State.AD_LOAD_FAIL);
            }
            return false;
#endif
        }
        public override bool showBanner(int typeb, Rect poscustom, AdCallBack cb)
        {
            if (cb != null)
            {
                SdkUtil.logd("ads admobmy tryLoadBanner not enable");
                cb(AD_State.AD_LOAD_FAIL);
            }
            return false;
        }
        public override void hideBanner(int type)
        {
#if ENABLE_MYLOG
            SdkUtil.logd("ads admob" + adsType + " hideBanner");
#endif
            dicBanner[type].isBNShow = false;
#if ENABLE_ADS_ADMOB && !USE_ADSMOB_MY
            if (dicBanner[type].banner != null && dicBanner[type].isBNloaded)
            {
                dicBanner[type].isBNHide = true;
                ((BannerView)dicBanner[type].banner).Hide();
            }
#endif
        }
        public override void destroyBanner(int type)
        {
#if ENABLE_ADS_ADMOB && !USE_ADSMOB_MY
            if (dicBanner[type].banner != null)
            {
                ((BannerView)dicBanner[type].banner).Hide();
                ((BannerView)dicBanner[type].banner).Destroy();
                dicBanner[type].banner = null;
                dicBanner[type].isBNLoading = false;
                dicBanner[type].isBNloaded = false;
            }
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
        public override int getFullLoaded(bool _isSplash)
        {
#if ENABLE_MYLOG
            SdkUtil.logd("ads admob getFullLoaded issplash=" + _isSplash);
#endif
            if (_isSplash)
            {
                if (splashId == null || splashId.Length < 10)
                {
                    _isSplash = false;
#if ENABLE_MYLOG
                    SdkUtil.logd("ads admob getFullLoaded change is splash");
#endif
                }
            }
            int re = 0;
#if ENABLE_ADS_ADMOB && !USE_ADSMOB_MY
            if (full != null && (isFullLoaded || full.IsLoaded()))
            {
                re = 1;
            }
#endif
            return re;
        }

        public override void clearCurrFull()
        {
            if (getFullLoaded(false) == 1)
            {
#if ENABLE_ADS_ADMOB && !USE_ADSMOB_MY
                full.Destroy();
                full = null;
                isFullLoaded = false;
#endif
            }
        }

        protected override void tryLoadFull(bool _isSplash)
        {
#if ENABLE_ADS_ADMOB && !USE_ADSMOB_MY
            if (_isSplash)
            {
                if (splashId == null || splashId.Length < 10)
                {
                    _isSplash = false;
#if ENABLE_MYLOG
                    SdkUtil.logd("ads admob tryLoadFull change is splash");
#endif
                }
                else
                {
#if ENABLE_MYLOG
                    SdkUtil.logd("ads admob" + adsType + " tryLoadFull splash =" + splashId);
#endif
                }

            }
            if (!_isSplash)
            {
#if ENABLE_MYLOG
                SdkUtil.logd("ads admob" + adsType + " tryLoadFull =" + fullId);
#endif
            }

            if (FullTryLoad >= toTryLoad)
            {
#if ENABLE_MYLOG
                SdkUtil.logd("ads admob" + adsType + " tryLoadFull over try _isSplash=" + _isSplash);
#endif
                if (cbFullLoad != null)
                {
                    var tmpcb = cbFullLoad;
                    cbFullLoad = null;
                    tmpcb(AD_State.AD_LOAD_FAIL);
                }

                return;
            }
            isFullLoading = true;
            isFullLoaded = false;
            if (full != null)
            {
                full.Destroy();
                full = null;
            }
            string idload = fullId;
            if (advhelper.isChangeToStaticAds && splashId != null && splashId.Length > 5)
            {
#if ENABLE_MYLOG
                SdkUtil.logd("ads admob tryLoadFull load id static ads");
#endif
                idload = splashId;
            }
            full = new InterstitialAd(idload.Trim());
            full.OnAdLoaded += HandleInterstitialLoaded;
            full.OnAdFailedToLoad += HandleInterstitialFailedToLoad;
            full.OnAdOpening += HandleInterstitialOpened;
            full.OnAdFailedToShow += HandleInterstitialFailedToShow;
            full.OnAdClosed += HandleInterstitialClosed;
            full.OnPaidEvent += HandleInterstitialPaidEvent;
            AdRequest request = new AdRequest.Builder().Build();
            if (request != null)
            {
                full.LoadAd(request);
            }
#else
            if (cbFullLoad != null)
            {
#if ENABLE_MYLOG
                SdkUtil.logd("ads admob" + adsType + " tryLoadFull not enable");
#endif
                var tmpcb = cbFullLoad;
                cbFullLoad = null;
                tmpcb(AD_State.AD_LOAD_FAIL);
            }
#endif
        }
        public override void loadFull(bool _isSplash, AdCallBack cb)
        {
#if ENABLE_MYLOG
            SdkUtil.logd("ads admob loadFull type=" + adsType + ", isSplash=" + _isSplash);
#endif
            if (_isSplash)
            {
                if (splashId == null || splashId.Length < 10)
                {
                    _isSplash = false;
#if ENABLE_MYLOG
                    SdkUtil.logd("ads admob loadFull change is splash");
#endif
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
#if ENABLE_MYLOG
                SdkUtil.logd("ads admob" + adsType + " loadFull isFullLoading=" + isFullLoading + " isFullLoaded=" + isFullLoaded);
#endif
            }
        }
        public override bool showFull(bool _isSplash, AdCallBack cb)
        {
#if ENABLE_MYLOG
            SdkUtil.logd("ads admob showFull type=" + adsType + ", isspalsh=" + _isSplash);
#endif
            if (_isSplash)
            {
                if (splashId == null || splashId.Length < 10)
                {
                    _isSplash = false;
#if ENABLE_MYLOG
                    SdkUtil.logd("ads admob showFull change is splash");
#endif
                }
            }
            cbFullShow = null;
            int ss = getFullLoaded(_isSplash);
            if (ss > 0)
            {
#if ENABLE_MYLOG
                SdkUtil.logd("ads admob showFull type=" + adsType);
#endif
                FullTryLoad = 0;
                isFullLoaded = false;
#if ENABLE_ADS_ADMOB && !USE_ADSMOB_MY
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
#if ENABLE_ADS_ADMOB && !USE_ADSMOB_MY
                gift.Destroy();
                gift = null;
                isGiftLoaded = false;
#endif
            }
        }
        public override bool getGiftLoaded()
        {
#if ENABLE_ADS_ADMOB && !USE_ADSMOB_MY
            return (isGiftLoaded || (gift != null && gift.IsLoaded()));
#endif
            return false;
        }
        protected override void tryloadGift()
        {
#if ENABLE_ADS_ADMOB && !USE_ADSMOB_MY
#if ENABLE_MYLOG
            SdkUtil.logd("ads admob" + adsType + " tryloadGift =" + giftId);
#endif
            if (GiftTryLoad >= toTryLoad)
            {
#if ENABLE_MYLOG
                SdkUtil.logd("ads admob" + adsType + " tryloadGift over try");
#endif
                if (cbGiftLoad != null)
                {
                    var tmpcb = cbGiftLoad;
                    cbGiftLoad = null;
                    tmpcb(AD_State.AD_LOAD_FAIL);
                }
                return;
            }
            if (gift != null)
            {
                gift.Destroy();
                gift = null;
            }
            isGiftLoading = true;
            isGiftLoaded = false;
            gift = new RewardedAd(giftId.Trim());
            gift.OnAdLoaded += HandleRewardBasedVideoLoaded;
            gift.OnAdOpening += HandleRewardBasedVideoOpened;
            gift.OnAdFailedToShow += HandleRewardBasedVideoFailToShow;
            gift.OnUserEarnedReward += HandleRewardBasedVideoRewarded;
            gift.OnAdClosed += HandleRewardBasedVideoClosed;
            gift.OnPaidEvent += HandleRewardBasedPaidEvent;
            AdRequest request = new AdRequest.Builder().Build();
            if (request != null)
            {
                gift.LoadAd(request);
            }
#else
            if (cbGiftLoad != null)
            {
#if ENABLE_MYLOG
                SdkUtil.logd("ads admob" + adsType + " tryloadGift not enable");
#endif
                var tmpcb = cbGiftLoad;
                cbGiftLoad = null;
                tmpcb(AD_State.AD_LOAD_FAIL);
            }
#endif
        }
        public override void loadGift(AdCallBack cb)
        {
#if ENABLE_MYLOG
            SdkUtil.logd("ads admob" + adsType + " loadGift");
#endif
            cbGiftLoad = cb;
            if (!isGiftLoading && !isGiftLoaded)
            {
                GiftTryLoad = 0;
                tryloadGift();
            }
            else
            {
#if ENABLE_MYLOG
                SdkUtil.logd("ads admob" + adsType + " loadGift loading=" + isGiftLoading + " or loaded=" + isGiftLoaded);
#endif
            }
        }
        public override bool showGift(AdCallBack cb)
        {
#if ENABLE_MYLOG
            SdkUtil.logd("ads admob" + adsType + " showGift");
#endif
            cbGiftShow = null;
            if (getGiftLoaded())
            {
                isGiftLoaded = false;
#if ENABLE_ADS_ADMOB && !USE_ADSMOB_MY
                cbGiftShow = cb;
                gift.Show();
                return true;
#endif
            }
            return false;
        }

        //-------------------------------------------------------------------------------
#if ENABLE_ADS_ADMOB && !USE_ADSMOB_MY

        #region BANNER AD EVENTS

        private void HandleBannerAdLoaded(object sender, EventArgs e)
        {
            int type = 0;
            if (sender.Equals(dicBanner[1].banner))
            {
                type = 1;
            }
#if ENABLE_MYLOG
            SdkUtil.logd("ads admob" + adsType + " HandleBannerAdLoaded=" + type);
#endif
            dicBanner[type].isBNloaded = true;
            dicBanner[type].isBNLoading = false;
            BNTryLoad = 0;
            if (dicBanner[type].banner != null)
            {
                if (dicBanner[type].isBNShow)
                {
#if ENABLE_MYLOG
                    SdkUtil.logd("ads admob" + adsType + " BannerAdLoaded show");
#endif
                    //bannerView.Show();
                    if (dicBanner[type].isBNHide)
                    {
                        dicBanner[type].isBNHide = false;
                        ((BannerView)dicBanner[type].banner).Show();
                    }
                }
                else
                {
#if ENABLE_MYLOG
                    SdkUtil.logd("ads admob" + adsType + " BannerAdLoaded hide");
#endif
                    dicBanner[type].isBNHide = true;
                    ((BannerView)dicBanner[type].banner).Hide();
                }
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

        private void HandleBannerAdFailedToLoad(object sender, AdFailedToLoadEventArgs e)
        {
            int type = 0;
            if (sender.Equals(dicBanner[1].banner))
            {
                type = 1;
            }
#if ENABLE_MYLOG
            SdkUtil.logd("ads admob" + adsType + " HandleBannerAdFailedToLoad=" + type);
#endif
            if (dicBanner[type].isBNLoading)
            {
                dicBanner[type].isBNLoading = false;
                BNTryLoad++;
                AdsProcessCB.Instance().Enqueue(() => { tryLoadBanner(type); }, 1.0f);
            }
        }

        private void HandleBannerAdOpened(object sender, EventArgs e)
        {
#if ENABLE_MYLOG
            SdkUtil.logd("ads admob" + adsType + " HandleBannerAdOpened");
#endif
        }

        private void HandleBannerAdClosed(object sender, EventArgs e)
        {
#if ENABLE_MYLOG
            SdkUtil.logd("ads admob" + adsType + " HandleBannerAdClosed");
#endif
            BNTryLoad = 0;
        }

        #endregion

        #region native ads
        private void HandleNativeLoaded(object sender, EventArgs e)
        {
#if ENABLE_MYLOG
            SdkUtil.logd("ads admob" + adsType + " HandleNativeLoaded");
#endif
            if (cbNative != null)
            {
                var tmpcb = cbNative;
                cbNative = null;
                tmpcb(AD_State.AD_LOAD_OK);
            }
        }

        private void HandleBannerAdPaidEvent(object sender, AdValueEventArgs e)
        {
#if ENABLE_MYLOG
            SdkUtil.logd($"ads admob{adsType} HandleBannerAdPaidEvent v={e.AdValue.Value}-{e.AdValue.CurrencyCode}-{e.AdValue.Precision}");
#endif
            FIRhelper.logEventAdsPaidAdmob(0, bannerId, (int)e.AdValue.Precision, e.AdValue.CurrencyCode, e.AdValue.Value);
        }

        private void HandleNativeLoadFail(object sender, EventArgs e)
        {
#if ENABLE_MYLOG
            SdkUtil.logd("ads admob" + adsType + " HandleNativeLoadFail");
#endif
            if (cbNative != null)
            {
                var tmpcb = cbNative;
                cbNative = null;
                tmpcb(AD_State.AD_LOAD_FAIL);
            }
        }
        #endregion

        #region INTERSTITIAL AD EVENTS
        private void HandleInterstitialLoaded(object sender, EventArgs e)
        {
#if ENABLE_MYLOG
            SdkUtil.logd("ads admob" + adsType + " HandleInterstitialLoaded");
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
            SdkUtil.logd("ads admob" + adsType + " HandleInterstitialLoaded 1");
#endif
        }

        private void HandleInterstitialFailedToLoad(object sender, AdFailedToLoadEventArgs e)
        {
#if ENABLE_MYLOG
            SdkUtil.logd("ads admob" + adsType + " HandleInterstitialFailedToLoad");
#endif
            isFullLoading = false;
            isFullLoaded = false;
            AdsProcessCB.Instance().Enqueue(() =>
            {
                FullTryLoad++;
                tryLoadFull(false);
            }, 1.0f);
#if ENABLE_MYLOG
            SdkUtil.logd("ads admob" + adsType + " HandleInterstitialFailedToLoad 1");
#endif
        }

        private void HandleInterstitialOpened(object sender, EventArgs e)
        {
#if ENABLE_MYLOG
            SdkUtil.logd("ads admob" + adsType + " HandleInterstitialOpened");
#endif
            if (cbFullShow != null)
            {
                AdCallBack tmpcb = cbFullShow;
                AdsProcessCB.Instance().Enqueue(() => { tmpcb(AD_State.AD_SHOW); });
            }
        }

        private void HandleInterstitialFailedToShow(object sender, AdErrorEventArgs e)
        {
#if ENABLE_MYLOG
            SdkUtil.logd("ads admob" + adsType + " HandleInterstitialFailedToShow=" + e.ToString());
#endif
            isFullLoading = false;
            isFullLoaded = false;
            if (cbFullShow != null)
            {
                AdCallBack tmpcb = cbFullShow;
                AdsProcessCB.Instance().Enqueue(() => { tmpcb(AD_State.AD_SHOW_FAIL); });
            }
            onFullClose();

            FullTryLoad = 0;
            cbFullShow = null;
        }

        private void HandleInterstitialClosed(object sender, EventArgs e)
        {
#if ENABLE_MYLOG
            SdkUtil.logd("ads admob" + adsType + " full HandleInterstitialClosed");
#endif
            isFullLoading = false;
            if (cbFullShow != null)
            {
                AdCallBack tmpcb = cbFullShow;
                AdsProcessCB.Instance().Enqueue(() =>
                {
                    tmpcb(AD_State.AD_CLOSE);
                });
            }
            onFullClose();

            FullTryLoad = 0;
            cbFullShow = null;
#if ENABLE_MYLOG
            SdkUtil.logd("ads admob" + adsType + " full HandleInterstitialClosed 1");
#endif
        }

        private void HandleInterstitialPaidEvent(object sender, AdValueEventArgs e)
        {
#if ENABLE_MYLOG
            SdkUtil.logd($"ads admob{adsType} HandleInterstitialPaidEvent v={e.AdValue.Value}-{e.AdValue.CurrencyCode}-{e.AdValue.Precision}");
#endif
            FIRhelper.logEventAdsPaidAdmob(1, fullId, (int)e.AdValue.Precision, e.AdValue.CurrencyCode, e.AdValue.Value);
        }

        private void HandleInterstitialLeftApplication(object sender, EventArgs e)
        {
#if ENABLE_MYLOG
            SdkUtil.logd("ads admob" + adsType + " full HandleInterstitialLeftApplication 1");
#endif
        }

        #endregion

        #region REWARDED VIDEO AD EVENTS

        private void HandleRewardBasedVideoLoaded(object sender, EventArgs e)
        {
#if ENABLE_MYLOG
            SdkUtil.logd("ads admob" + adsType + " rw HandleRewardBasedVideoLoaded");
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
            SdkUtil.logd("ads admob" + adsType + " rw HandleRewardBasedVideoLoaded 1");
#endif
        }

        private void HandleRewardBasedVideoFailedToLoad(object sender, AdErrorEventArgs e)
        {
#if ENABLE_MYLOG
            SdkUtil.logd("ads admob" + adsType + " rw HandleRewardBasedVideoFailedToLoad");
#endif
            isGiftLoading = false;
            isGiftLoaded = false;
            AdsProcessCB.Instance().Enqueue(() =>
            {
                GiftTryLoad++;
                tryloadGift();
            }, 1.0f);
#if ENABLE_MYLOG
            SdkUtil.logd("ads admob" + adsType + " rw HandleRewardBasedVideoFailedToLoad 1");
#endif
        }

        private void HandleRewardBasedVideoOpened(object sender, EventArgs e)
        {
#if ENABLE_MYLOG
            SdkUtil.logd("ads admob" + adsType + " rw HandleRewardBasedVideoOpened");
#endif
            GiftTryLoad = 0;
            if (cbGiftShow != null)
            {
                var tmpcb = cbGiftShow;
#if ENABLE_MYLOG
                SdkUtil.logd("ads admob" + adsType + " rw HandleRewardBasedVideoClosed2");
#endif
                AdsProcessCB.Instance().Enqueue(() => { tmpcb(AD_State.AD_SHOW); });
            }
#if ENABLE_MYLOG
            SdkUtil.logd("ads admob" + adsType + " rw HandleRewardBasedVideoOpened 1");
#endif
        }

        private void HandleRewardBasedVideoFailToShow(object sender, AdErrorEventArgs e)
        {
#if ENABLE_MYLOG
            SdkUtil.logd("ads admob" + adsType + " rw HandleRewardBasedVideoFailToShow");
#endif
            GiftTryLoad = 0;
            isGiftLoading = false;
            isGiftLoaded = false;
            if (cbGiftShow != null)
            {
                AdCallBack tmpcb = cbGiftShow;
                AdsProcessCB.Instance().Enqueue(() => { tmpcb(AD_State.AD_SHOW_FAIL); });
            }
            onGiftClose();

            isRewardCom = false;
            cbGiftShow = null;
        }

        private void HandleRewardBasedVideoStarted(object sender, EventArgs e)
        {
#if ENABLE_MYLOG
            SdkUtil.logd("ads admob" + adsType + " rw HandleRewardBasedVideoStarted");
#endif
            GiftTryLoad = 0;
        }

        private void HandleRewardBasedVideoRewarded(object sender, GoogleMobileAds.Api.Reward e)
        {
#if ENABLE_MYLOG
            SdkUtil.logd("ads admob" + adsType + " rw HandleRewardBasedVideoRewarded");
#endif
            isRewardCom = true;
        }

        private void HandleRewardBasedVideoClosed(object sender, EventArgs e)
        {
#if ENABLE_MYLOG
            SdkUtil.logd("ads admob" + adsType + " rw HandleRewardBasedVideoClosed");
#endif
            isGiftLoading = false;
            isGiftLoaded = false;
            if (cbGiftShow != null)
            {
                AdCallBack tmpcb = cbGiftShow;
#if ENABLE_MYLOG
                SdkUtil.logd("ads admob" + adsType + " rw _cbAD != null");
#endif
                if (isRewardCom)
                {
#if ENABLE_MYLOG
                    SdkUtil.logd("ads admob" + adsType + " rw _cbAD reward");
#endif
                    AdsProcessCB.Instance().Enqueue(() => { tmpcb(AD_State.AD_REWARD_OK); });
                }
                else
                {
#if ENABLE_MYLOG
                    SdkUtil.logd("ads admob" + adsType + " rw _cbAD fail");
#endif
                    AdsProcessCB.Instance().Enqueue(() => { tmpcb(AD_State.AD_REWARD_FAIL); });
                }
                AdsProcessCB.Instance().Enqueue(() =>
                {
                    tmpcb(AD_State.AD_CLOSE);
                });
            }
            onGiftClose();

            isRewardCom = false;
            GiftTryLoad = 0;
            cbGiftShow = null;
#if ENABLE_MYLOG
            SdkUtil.logd("ads admob" + adsType + " rw HandleRewardBasedVideoClosed 3");
#endif
        }

        private void HandleRewardBasedVideoLeftApplication(object sender, EventArgs e)
        {
#if ENABLE_MYLOG
            SdkUtil.logd("ads admob" + adsType + " rw HandleRewardBasedVideoLeftApplication");
#endif
        }

        private void HandleRewardBasedPaidEvent(object sender, AdValueEventArgs e)
        {
#if ENABLE_MYLOG
            SdkUtil.logd($"ads admob{adsType} HandleRewardBasedPaidEvent v={e.AdValue.Value}-{e.AdValue.CurrencyCode}-{e.AdValue.Precision}");
#endif
            FIRhelper.logEventAdsPaidAdmob(2, giftId, (int)e.AdValue.Precision, e.AdValue.CurrencyCode, e.AdValue.Value);
        }

        #endregion

#endif

    }
}