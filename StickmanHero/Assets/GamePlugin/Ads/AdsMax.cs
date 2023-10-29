//#define ENABLE_ADS_MAX
//#define ENABLE_ADS_AMAZON

using System;
using UnityEngine;
using System.Collections;

#if ENABLE_ADS_MAX

#if ENABLE_ADS_AMAZON
using AmazonAds;
#endif

#endif

namespace mygame.sdk
{
    public class AdsMax : AdsBase
    {
#if ENABLE_ADS_MAX

#endif
        private bool isAdsInited = false;
        private long timeLoadFull4Wait = 0;
        private long timeLoadGift4Wait = 0;
        private int fullReloadWithApplovin = 1;
        private int giftReloadWithApplovin = 1;

        string nameFullCurr = "";
        string nameGiftCurr = "";

        int timewaitload = 60000;
#if ENABLE_ADS_MAX
        MaxSdkBase.BannerPosition bnCurrPos;
#endif
        bool isBnCreate = false;
        bool isFullFirstLoad = true;
        bool isGiftFirstLoad = true;

        [Header("Amazon")]
        public string amazonAppId = "";
        public string amazonBnSlotId = "";
        public string amazonBnLeaderSlotId = "";
        public string amazonInterSlotId = "";
        public string amazonVideoRewardedSlotId = "";//AMAZON_VIDEO_REWARDED_SLOT_ID

        public override string getname()
        {
            return "max";
        }

        public override void InitAds()
        {
#if ENABLE_ADS_MAX
            isEnable = true;
            isAdsInited = false;
#if ENABLE_ADS_AMAZON
            Amazon.Initialize(amazonAppId);
            Amazon.SetAdNetworkInfo(new AdNetworkInfo(DTBAdNetwork.MAX));
            Amazon.EnableLogging(true);
            Amazon.EnableTesting(true);
#endif

            MaxSdkCallbacks.OnSdkInitializedEvent += (MaxSdkBase.SdkConfiguration sdkConfiguration) =>
            {
                SdkUtil.logd($"ads max{adsType} init suc");
                isAdsInited = true;
                startAds();

                if (adsType == 6)
                {
                    advhelper.onAdsInitSuc();
                    AdsProcessCB.Instance().Enqueue(() =>
                    {
                        advhelper.loadFull4ThisTurn(false, 99, false, null);
                        advhelper.loadGift4ThisTurn(99, null);
                    }, 0.1f);
                    if (AdCanvasHelper.Instance != null && PlayerPrefs.GetInt("user_allow_track_ads", 0) != -1)
                    {
                        AdCanvasHelper.Instance.ShowCMPIOS();
                    }
                }
            };
            if (adsType == 6)
            {
                SdkUtil.logd($"ads max{adsType} init begin");
                MaxSdk.SetSdkKey(appId);
                MaxSdk.InitializeSdk();
            }
#endif
        }

        public override void AdsAwake()
        {
#if ENABLE_ADS_MAX
            isEnable = true;
#endif
        }

        private void Start()
        {
#if ENABLE_ADS_MAX

#endif
        }

        private void startAds()
        {
#if ENABLE_ADS_MAX
#if UNITY_IOS || UNITY_IPHONE
            if (GameHelper.Instance.countryCode.CompareTo("cn") == 0 || GameHelper.Instance.countryCode.CompareTo("chn") == 0
                    || GameHelper.Instance.countryCode.CompareTo("zh") == 0 || GameHelper.Instance.countryCode.CompareTo("zh0") == 0)
            {
                if (ios_bn_cn_id.Length > 3)
                {
                    Debug.Log($"mysdk: ads max{adsType} change bn to china");
                    bannerId = ios_bn_cn_id;
                }
                if (ios_full_cn_id.Length > 3)
                {
                    Debug.Log($"mysdk: ads max{adsType} change full to china");
                    fullId = ios_full_cn_id;
                }
                if (ios_gift_cn_id.Length > 3)
                {
                    Debug.Log($"mysdk: ads max{adsType} change gift to china");
                    giftId = ios_gift_cn_id;
                }
            }
#endif

            MaxSdkCallbacks.Banner.OnAdLoadedEvent += OnBannerAdLoadedEvent;
            MaxSdkCallbacks.Banner.OnAdLoadFailedEvent += OnBannerAdLoadFailedEvent;
            MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent += OnBannerAdRevenuePaidEvent;

            MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += OnInterstitialLoadedEvent;
            MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += OnInterstitialFailedEvent;
            MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent += OnInterstitialDisplayedEvent;
            MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += InterstitialFailedToDisplayEvent;
            MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += OnInterstitialDismissedEvent;
            MaxSdkCallbacks.Interstitial.OnAdRevenuePaidEvent += OnInterstitialAdRevenuePaidEvent;

            MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnRewardedAdLoadedEvent;
            MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += OnRewardedAdFailedEvent;
            MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += OnRewardedAdFailedToDisplayEvent;
            MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += OnRewardedAdDisplayedEvent;
            MaxSdkCallbacks.Rewarded.OnAdClickedEvent += OnRewardedAdClickedEvent;
            MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;
            MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += OnRewardedAdDismissedEvent;
            MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += OnRewardedAdRevenuePaidEvent;
#endif
        }

        protected override void tryLoadBanner(int type)
        {
            SdkUtil.logd($"ads max{adsType} tryLoadBanner = " + bannerId);
            type = 0;
#if ENABLE_ADS_MAX
            if (!isAdsInited)
            {
                SdkUtil.logd($"ads max{adsType} tryLoadBanner not init");
                var tmpcb = cbBanner;
                cbBanner = null;
                tmpcb(AD_State.AD_LOAD_FAIL);
                return;
            }
            if (BNTryLoad >= toTryLoad)
            {
                SdkUtil.logd($"ads max{adsType} tryLoadBanner over trycount");
                if (cbBanner != null)
                {
                    var tmpcb = cbBanner;
                    cbBanner = null;
                    tmpcb(AD_State.AD_LOAD_FAIL);
                }
                return;
            }
            if (bannerId == null || bannerId.Length < 5)
            {
                SdkUtil.logd($"ads max{adsType} tryLoadBanner bannerid is empty");
                if (cbBanner != null)
                {
                    var tmpcb = cbBanner;
                    cbBanner = null;
                    tmpcb(AD_State.AD_LOAD_FAIL);
                }
                return;
            }
#if ENABLE_ADS_AMAZON
            if (isBnCreate)
            {

            }
#else
            if (isBnCreate)
            {
                MaxSdk.DestroyBanner(bannerId);
            }
#endif

            dicBanner[type].isBNLoading = false;
            dicBanner[type].isBNloaded = true;
            
            if (dicBanner[type].posBanner == 0)
            {
                bnCurrPos = MaxSdkBase.BannerPosition.TopCenter;
            }
            else if (dicBanner[type].posBanner == 1)
            {
                bnCurrPos = MaxSdkBase.BannerPosition.BottomCenter;
            }
            else
            {
                bnCurrPos = MaxSdkBase.BannerPosition.BottomCenter;
            }
            if (bnWidth > 0)
            {
                MaxSdk.SetBannerWidth(bannerId, bnWidth);
            }
            string adaptivebn = "true";
            if (bnWidth > 0)
            {
                adaptivebn = "false";
            }
            else
            {
                adaptivebn = "true";
            }
#if ENABLE_ADS_AMAZON
            if (amazonBnSlotId != null && amazonBnSlotId.Length > 3)
            {
                SdkUtil.logd($"ads max{adsType} tryLoadBanner amazon load");
                int widtham;
                int heightam;
                string slotId;
                if (MaxSdkUtils.IsTablet() && bnWidth <= 0)
                {
                    widtham = 728;
                    heightam = 90;
                    slotId = amazonBnLeaderSlotId;
                }
                else
                {
                    widtham = 320;
                    heightam = 50;
                    slotId = amazonBnSlotId;
                }
                var apsBanner = new APSBannerAdRequest(widtham, heightam, slotId);
                apsBanner.onSuccess += (adResponse) =>
                {
                    MaxSdk.SetBannerLocalExtraParameter(bannerId, "amazon_ad_response", adResponse.GetResponse());
                    createBannerAndShow(type, adaptivebn);
                };
                apsBanner.onFailedWithError += (adError) =>
                {
                    MaxSdk.SetBannerLocalExtraParameter(bannerId, "amazon_ad_error", adError.GetAdError());
                    createBannerAndShow(type, adaptivebn);
                };

                apsBanner.LoadAd();
            }
            else
            {
                createBannerAndShow(type, adaptivebn);
            }
#else
            createBannerAndShow(type, adaptivebn);
#endif

#else
            if (cbBanner != null)
            {
                SdkUtil.logd($"ads max{adsType} tryLoadBanner not enable");
                var tmpcb = cbBanner;
                cbBanner = null;
                tmpcb(AD_State.AD_LOAD_FAIL);
            }
#endif
        }

#if ENABLE_ADS_MAX
        void createBannerAndShow(int type, string adaptivebn)
        {
            MaxSdk.CreateBanner(bannerId, bnCurrPos);
            isBnCreate = true;
            MaxSdk.SetBannerExtraParameter(bannerId, "adaptive_banner", adaptivebn);
            BNTryLoad = 0;
            if (dicBanner[type].isBNShow)
            {
                SdkUtil.logd($"ads max{adsType} tryLoadBanner CreateBanner show");
                if (bnWidth > 0)
                {
                    MaxSdk.SetBannerWidth(bannerId, bnWidth);
                }
                if (!dicBanner[0].isShowing)
                {
                    dicBanner[type].isShowing = true;
                    MaxSdk.ShowBanner(bannerId);
                }
                else
                {
                    MaxSdk.UpdateBannerPosition(bannerId, bnCurrPos);
                    MaxSdk.ShowBanner(bannerId);
                }
            }
            else
            {
                SdkUtil.logd($"ads max{adsType} tryLoadBanner CreateBanner hide");
                dicBanner[type].isShowing = false;
                MaxSdk.HideBanner(bannerId);
            }
        }
#endif

        public override void loadBanner(int type, AdCallBack cb)
        {
            SdkUtil.logd($"ads max{adsType} loadBanner");
            cbBanner = cb;
            type = 0;
            if (!dicBanner[type].isBNLoading)
            {
                BNTryLoad = 0;
                tryLoadBanner(type);
            }
            else
            {
                SdkUtil.logd($"ads max{adsType} loadBanner isloading");
            }
        }
        public override bool showBanner(int type, int pos, int width, AdCallBack cb, float dxCenter)
        {
            SdkUtil.logd($"ads max{adsType} showBanner");
            type = 0;
#if ENABLE_ADS_MAX
            dicBanner[type].isBNShow = true;
            dicBanner[type].posBanner = pos;
            bnWidth = width;
            if (dicBanner[type].isBNloaded)
            {
                SdkUtil.logd($"ads max{adsType} showBanner loaded and show");
                if (dicBanner[type].posBanner == 0)
                {
                    bnCurrPos = MaxSdkBase.BannerPosition.TopCenter;
                }
                else if (dicBanner[type].posBanner == 1)
                {
                    bnCurrPos = MaxSdkBase.BannerPosition.BottomCenter;
                }
                else
                {
                    bnCurrPos = MaxSdkBase.BannerPosition.BottomCenter;
                }
                if (bnWidth > 0)
                {
                    MaxSdk.SetBannerExtraParameter(bannerId, "adaptive_banner", "false");
                    MaxSdk.SetBannerWidth(bannerId, bnWidth);
                }
                else
                {
                    MaxSdk.SetBannerExtraParameter(bannerId, "adaptive_banner", "true");
                }
                MaxSdk.UpdateBannerPosition(bannerId, bnCurrPos);
                MaxSdk.ShowBanner(bannerId);
                return true;
            }
            else
            {
                SdkUtil.logd($"ads max{adsType} showBanner not load and load");
                loadBanner(type, cb);
                return false;
            }
#else
            if (cb != null)
            {
                SdkUtil.logd($"ads max{adsType} tryLoadBanner not enable");
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
                SdkUtil.logd($"ads max{adsType} tryLoadBanner not enable");
                cb(AD_State.AD_LOAD_FAIL);
            }
            return false;
        }
        public override void hideBanner(int type)
        {
            SdkUtil.logd($"ads max{adsType} hideBanner");
            type = 0;
            dicBanner[type].isBNShow = false;
#if ENABLE_ADS_MAX
            if (bannerId != null && bannerId.Length > 3)
            {
                MaxSdk.HideBanner(bannerId);
            }
#endif
        }
        public override void destroyBanner(int type)
        {
            type = 0;
            hideBanner(0);
            dicBanner[0].isBNLoading = false;
            dicBanner[0].isBNloaded = false;
#if ENABLE_ADS_MAX
            MaxSdk.DestroyBanner(bannerId);
#endif
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
#if ENABLE_MYLOG
                SdkUtil.logd($"ads max{adsType} tryLoadBanner not enable");
#endif
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
#if ENABLE_ADS_MAX
                isFullLoaded = false;
#endif
            }
        }
        public override int getFullLoaded(bool _isSplash)
        {
            long tc = SdkUtil.systemCurrentMiliseconds();
            SdkUtil.logd($"ads max{adsType} getFullLoaded issplash=" + _isSplash + ", stateShowAppLlovin=" + advhelper.currConfig.stateShowAppLlovin);
            if (_isSplash)
            {
                if (splashId == null || splashId.Length < 10)
                {
                    _isSplash = false;
                    SdkUtil.logd($"ads max{adsType} getFullLoaded change is splash");
                }
            }
            int re = 0;
#if ENABLE_ADS_MAX
            if (_isSplash)
            {
                if (isFullLoaded && splashId != null && splashId.Length > 0 && MaxSdk.IsInterstitialReady(splashId))
                {
                    re = 2;
                }
                else if (isFullLoaded && fullId != null && fullId.Length > 0 && MaxSdk.IsInterstitialReady(fullId))
                {
                    re = 1;
                }
                if (re > 0 && advhelper.currConfig.stateShowAppLlovin == 1 && advhelper.level4ApplovinFull < advhelper.currConfig.levelShowAppLovin)
                {
                    SdkUtil.logd($"ads max{adsType} getFullLoaded 1 check reject applovin");
                    long t = SdkUtil.systemCurrentMiliseconds();
                    if ((t - timeLoadFull4Wait) < timewaitload)
                    {
                        SdkUtil.logd($"ads max{adsType} getFullLoaded 1 reject applovin");
                        re = 0;
                    }
                }
                SdkUtil.logd($"ads max{adsType} getFullLoaded re={re}, dt={(SdkUtil.systemCurrentMiliseconds() - tc)}");
                return re;
            }
            else
            {
                if (isFullLoaded && fullId != null && fullId.Length > 0 && MaxSdk.IsInterstitialReady(fullId))
                {
                    re = 1;
                }
                else if (isFullLoaded && splashId != null && splashId.Length > 0 && MaxSdk.IsInterstitialReady(splashId))
                {
                    re = 2;
                }
                if (re > 0 && advhelper.currConfig.stateShowAppLlovin == 1 && advhelper.level4ApplovinFull < advhelper.currConfig.levelShowAppLovin)
                {
                    SdkUtil.logd($"ads max{adsType} getFullLoaded 2 check reject applovin");
                    long t = SdkUtil.systemCurrentMiliseconds();
                    if ((t - timeLoadFull4Wait) < timewaitload)
                    {
                        SdkUtil.logd($"ads max{adsType} getFullLoaded 1 reject applovin");
                        re = 0;
                    }
                }
                SdkUtil.logd($"ads max{adsType} getFullLoaded1 re={re}, dt={(SdkUtil.systemCurrentMiliseconds() - tc)}");
                return re;
            }
#else
            return 0;
#endif
        }
        protected override void tryLoadFull(bool _isSplash)
        {
#if ENABLE_ADS_MAX
#if ENABLE_MYLOG
            SdkUtil.logd($"ads max{adsType} tryLoadFull issplash=" + _isSplash);
#endif
            if (!isAdsInited)
            {
#if ENABLE_MYLOG
                SdkUtil.logd($"ads max{adsType} tryLoadFull not init");
#endif
                if (cbFullLoad != null)
                {
                    var tmpcb = cbFullLoad;
                    cbFullLoad = null;
                    tmpcb(AD_State.AD_LOAD_FAIL);
                }
                return;
            }
            if (_isSplash)
            {
                if (splashId == null || splashId.Length < 10)
                {
                    _isSplash = false;
#if ENABLE_MYLOG
                    SdkUtil.logd($"ads max{adsType} tryLoadFull change is splash");
#endif
                }
                else
                {
#if ENABLE_MYLOG
                    SdkUtil.logd($"ads max{adsType} tryLoadFull splash=" + splashId);
#endif
                }
            }
            if (!_isSplash)
            {
#if ENABLE_MYLOG
                SdkUtil.logd($"ads max{adsType} tryLoadFull=" + fullId);
#endif
            }

            if (FullTryLoad >= toTryLoad)
            {
#if ENABLE_MYLOG
                SdkUtil.logd($"ads max{adsType} tryLoadFull over try");
#endif
                if (cbFullLoad != null)
                {
                    var tmpcb = cbFullLoad;
                    cbFullLoad = null;
                    tmpcb(AD_State.AD_LOAD_FAIL);
                }
                return;
            }
            if (fullId == null || fullId.Length < 5)
            {
#if ENABLE_MYLOG
                SdkUtil.logd($"ads max{adsType} tryLoadFull fullid is empty");
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
            adsFullSubType = -1;
            string fullloadid = "";
            if (_isSplash)
            {
                fullloadid = splashId;
            }
            else
            {
                fullloadid = fullId;
            }
#if ENABLE_ADS_AMAZON
            if (isFullFirstLoad && amazonInterSlotId != null && amazonInterSlotId.Length > 3)
            {
                isFullFirstLoad = false;
                

                var interstitialAd = new APSInterstitialAdRequest(amazonInterSlotId);
                interstitialAd.onSuccess += (adResponse) =>
                {
                    MaxSdk.SetInterstitialLocalExtraParameter(fullloadid, "amazon_ad_response", adResponse.GetResponse());
                    MaxSdk.LoadInterstitial(fullloadid);
                };
                interstitialAd.onFailedWithError += (adError) =>
                {
                    MaxSdk.SetInterstitialLocalExtraParameter(fullloadid, "amazon_ad_error", adError.GetAdError());
                    MaxSdk.LoadInterstitial(fullloadid);
                };

                interstitialAd.LoadAd();
            }
            else
            {
                MaxSdk.LoadInterstitial(fullloadid);
            }
#else
            MaxSdk.LoadInterstitial(fullloadid);
#endif

#else
            if (cbFullLoad != null)
            {
#if ENABLE_MYLOG
                SdkUtil.logd($"ads max{adsType} tryLoadFull not enable");
#endif
                var tmpcb = cbFullLoad;
                cbFullLoad = null;
                tmpcb(AD_State.AD_LOAD_FAIL);
            }
#endif
        }
        public override void loadFull(bool _isSplash, AdCallBack cb)
        {
            SdkUtil.logd($"ads max{adsType} loadFull isSplash=" + _isSplash + ", stateShowAppLlovin=" + advhelper.currConfig.stateShowAppLlovin);
            if (_isSplash)
            {
                if (splashId == null || splashId.Length < 10)
                {
                    _isSplash = false;
                    SdkUtil.logd($"ads max{adsType} loadFull change is splash");
                }
            }
            fullReloadWithApplovin = advhelper.currConfig.tryloadApplovin;
            cbFullLoad = cb;
            if (!isFullLoading && !isFullLoaded)
            {
                FullTryLoad = 0;
                tryLoadFull(_isSplash);
            }
            else
            {
                SdkUtil.logd($"ads max{adsType} loadFull loading=" + isFullLoading + " or loaded=" + isFullLoaded);
                if (isFullLoaded)
                {
                    bool isok = true;
                    if (advhelper.currConfig.stateShowAppLlovin == 1 && advhelper.level4ApplovinFull < advhelper.currConfig.levelShowAppLovin)
                    {
                        long t = SdkUtil.systemCurrentMiliseconds();
                        if ((t - timeLoadFull4Wait) < timewaitload)
                        {
                            SdkUtil.logd($"ads max{adsType} loadFull load ok and wait -> call cb AD_LOAD_OK_WAIT");
                            isok = false;
                            AdsProcessCB.Instance().Enqueue(() =>
                            {
                                if (cb != null)
                                {
                                    var tmpcb = cb;
                                    cb = null;
                                    tmpcb(AD_State.AD_LOAD_OK_WAIT);
                                }
                            });
                        }
                        else
                        {
                            SdkUtil.logd($"ads max{adsType} loadFull load ok and overtime wait -> call cb AD_LOAD_OK");
                        }
                    }
                    else
                    {
                        SdkUtil.logd($"ads max{adsType} loadFull load ok -> call cb AD_LOAD_OK");
                    }
                    if (isok)
                    {
                        if (getFullLoaded(_isSplash) > 0)
                        {
                            SdkUtil.logd($"ads max{adsType} loadFull loaded and ready");
                            if (cb != null)
                            {
                                var tmpcb = cb;
                                cb = null;
                                tmpcb(AD_State.AD_LOAD_OK);
                            }
                        }
                        else
                        {
                            SdkUtil.logd($"ads max{adsType} loadFull loaded and not ready");
                            AdsProcessCB.Instance().Enqueue(() =>
                            {
                                if (cb != null)
                                {
                                    var tmpcb = cb;
                                    cb = null;
                                    tmpcb(AD_State.AD_LOAD_OK_WAIT);
                                }
                            });
                        }

                    }
                }
            }
        }
        public override bool showFull(bool _isSplash, AdCallBack cb)
        {
            SdkUtil.logd($"ads max{adsType} showFull isspalsh=" + _isSplash);
            if (_isSplash)
            {
                if (splashId == null || splashId.Length < 10)
                {
                    SdkUtil.logd($"ads max{adsType} showFull change is splash");
                    _isSplash = false;
                }
            }
            cbFullShow = null;
            int ss = getFullLoaded(_isSplash);
            if (ss > 0)
            {
                FullTryLoad = 0;
                isFullLoaded = false;
#if ENABLE_ADS_MAX
                cbFullShow = cb;
                if (_isSplash)
                {
                    SdkUtil.logd($"ads max{adsType} showFull isspalsh netname=" + nameFullCurr);
                    MaxSdk.ShowInterstitial(splashId);
                }
                else
                {
                    SdkUtil.logd($"ads max{adsType} showFull isspalsh netname=" + nameFullCurr);
                    MaxSdk.ShowInterstitial(fullId);
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
#if ENABLE_ADS_MAX
                isGiftLoaded = false;
#endif
            }
        }
        public override bool getGiftLoaded()
        {
            long tc = SdkUtil.systemCurrentMiliseconds();
            SdkUtil.logd($"ads max{adsType} getGiftLoaded getGiftLoaded stateShowAppLlovin=" + advhelper.currConfig.stateShowAppLlovin);
#if ENABLE_ADS_MAX
            bool re = (isGiftLoaded && giftId != null && giftId.Length > 0 && MaxSdk.IsRewardedAdReady(giftId));
            if (re && advhelper.currConfig.stateShowAppLlovin == 1 && advhelper.level4ApplovinGift < advhelper.currConfig.levelShowAppLovin)
            {
                SdkUtil.logd($"ads max{adsType} getGiftLoaded 1 check reject applovin");
                long t = SdkUtil.systemCurrentMiliseconds();
                if ((t - timeLoadGift4Wait) < timewaitload)
                {
                    SdkUtil.logd($"ads max{adsType} getGiftLoaded 1 reject applovin");
                    re = false;
                }
            }
            if (re && advhelper.currConfig.stateShowAppLlovin == 2 && advhelper.level4ApplovinGift < advhelper.currConfig.levelShowAppLovin)
            {
                SdkUtil.logd($"ads max{adsType} getGiftLoaded 2 reject applovin");
                re = false;
            }
            SdkUtil.logd($"ads max{adsType} getGiftLoaded re={re}, dt={(SdkUtil.systemCurrentMiliseconds() - tc)}");
            return re;
#endif
            return false;
        }
        protected override void tryloadGift()
        {
#if ENABLE_ADS_MAX
#if ENABLE_MYLOG
            SdkUtil.logd($"ads max{adsType} Request gift =" + giftId);
#endif
            if (!isAdsInited)
            {
#if ENABLE_MYLOG
                SdkUtil.logd($"ads max{adsType} tryloadGift not init");
#endif
                if (cbGiftLoad != null)
                {
                    var tmpcb = cbGiftLoad;
                    cbGiftLoad = null;
                    tmpcb(AD_State.AD_LOAD_FAIL);
                }
                return;
            }
            if (GiftTryLoad >= toTryLoad)
            {
#if ENABLE_MYLOG
                SdkUtil.logd($"ads max{adsType} tryLoadgift over try");
#endif
                if (cbGiftLoad != null)
                {
                    var tmpcb = cbGiftLoad;
                    cbGiftLoad = null;
                    tmpcb(AD_State.AD_LOAD_FAIL);
                }
                return;
            }
            if (giftId == null || giftId.Length < 5)
            {
#if ENABLE_MYLOG
                SdkUtil.logd($"ads max{adsType} tryLoadgift giftId is empty");
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
#if ENABLE_ADS_AMAZON
            if (isGiftFirstLoad && amazonVideoRewardedSlotId != null && amazonVideoRewardedSlotId.Length > 3)
            {
                isGiftFirstLoad = false;

                var rewardedVideoAd = new APSVideoAdRequest(320, 480, amazonVideoRewardedSlotId);
                rewardedVideoAd.onSuccess += (adResponse) =>
                {
                    MaxSdk.SetRewardedAdLocalExtraParameter(giftId, "amazon_ad_response", adResponse.GetResponse());
                    MaxSdk.LoadRewardedAd(giftId);
                    timeLoadGift = SdkUtil.systemCurrentMiliseconds();
                };
                rewardedVideoAd.onFailedWithError += (adError) =>
                {
                    MaxSdk.SetRewardedAdLocalExtraParameter(giftId, "amazon_ad_error", adError.GetAdError());
                    MaxSdk.LoadRewardedAd(giftId);
                    timeLoadGift = SdkUtil.systemCurrentMiliseconds();
                };

                rewardedVideoAd.LoadAd();
            }
            else
            {
                MaxSdk.LoadRewardedAd(giftId);
                timeLoadGift = SdkUtil.systemCurrentMiliseconds();
            }
#else
            MaxSdk.LoadRewardedAd(giftId);
            timeLoadGift = SdkUtil.systemCurrentMiliseconds();
            // StartCoroutine(WaitGiftErr());
#endif

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
#if ENABLE_MYLOG
                        SdkUtil.logd($"ads max{adsType} WaitGiftErr overtime");
#endif
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
            SdkUtil.logd($"ads max{adsType} loadGift stateShowAppLlovin=" + advhelper.currConfig.stateShowAppLlovin);
            cbGiftLoad = cb;
            giftReloadWithApplovin = advhelper.currConfig.tryloadApplovin;
            if (!isGiftLoading && !isGiftLoaded)
            {
                GiftTryLoad = 0;
                tryloadGift();
            }
            else
            {
                SdkUtil.logd($"ads max{adsType} loadGift loading=" + isGiftLoading + " or loaded=" + isGiftLoaded);
                if (isGiftLoaded)
                {
                    if (advhelper.currConfig.stateShowAppLlovin == 1 && advhelper.level4ApplovinGift < advhelper.currConfig.levelShowAppLovin)
                    {
                        long t = SdkUtil.systemCurrentMiliseconds();
                        if ((t - timeLoadGift4Wait) < timewaitload)
                        {
                            SdkUtil.logd($"ads max{adsType} loadGift load ok and wait -> call cb AD_LOAD_OK_WAIT");
                            AdsProcessCB.Instance().Enqueue(() =>
                            {
                                if (cbGiftLoad != null)
                                {
                                    var tmpcb = cbGiftLoad;
                                    cbGiftLoad = null;
                                    tmpcb(AD_State.AD_LOAD_OK_WAIT);
                                }
                            });
                        }
                        else
                        {
                            SdkUtil.logd($"ads max{adsType} loadGift load ok and overtime wait -> call cb AD_LOAD_OK");
                            if (cbGiftLoad != null)
                            {
                                var tmpcb = cbGiftLoad;
                                cbGiftLoad = null;
                                tmpcb(AD_State.AD_LOAD_OK);
                            }
                        }
                    }
                    else
                    {
                        SdkUtil.logd($"ads max{adsType} loadGift load ok -> call cb AD_LOAD_OK");
                        if (cbGiftLoad != null)
                        {
                            var tmpcb = cbGiftLoad;
                            cbGiftLoad = null;
                            tmpcb(AD_State.AD_LOAD_OK);
                        }
                    }
                }
            }
        }
        public override bool showGift(AdCallBack cb)
        {
            SdkUtil.logd($"ads max{adsType} showGift");
            if (getGiftLoaded())
            {
                cbGiftShow = cb;
                isGiftLoaded = false;
#if ENABLE_ADS_MAX
                SdkUtil.logd($"ads max{adsType} showGift netname=" + nameGiftCurr);
                MaxSdk.ShowRewardedAd(giftId); ;
                return true;
#endif
            }
            return false;
        }

        private int getAdsType(string adsname)
        {
            if (adsname != null)
            {
                if (adsname.Contains("Google AdMob"))
                {
                    return 0;
                }
                else if (adsname.Contains("Facebook"))
                {
                    return 1;
                }
                else if (adsname.Contains("Unity Ads"))
                {
                    return 2;
                }
                else if (adsname.Contains("AdColony"))
                {
                    return 40;
                }
                else if (adsname.Contains("Chartboost"))
                {
                    return 41;
                }
                else if (adsname.Contains("Fyber"))
                {
                    return 42;
                }
                else if (adsname.Contains("Google Ad Manager"))
                {
                    return 43;
                }
                else if (adsname.Contains("InMobis"))
                {
                    return 44;
                }
                else if (adsname.Contains("IronSource"))
                {
                    return 3;
                }
                else if (adsname.Contains("Mintegral"))
                {
                    return 45;
                }
                else if (adsname.Contains("Tapjoy"))
                {
                    return 46;
                }
                else if (adsname.Contains("Pangle"))
                {
                    return 47;
                }
            }
            return 6;
        }

        //-------------------------------------------------------------------------------
#if ENABLE_ADS_MAX
        #region BANNER AD EVENT
        private void OnBannerAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adinfo)
        {
            if (adUnitId != null && adUnitId.CompareTo(bannerId) == 0)
            {
                SdkUtil.logd($"ads max{adsType} OnBannerAdLoadedEvent={adUnitId}");
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
                if (dicBanner[0].isBNShow)
                {
                    SdkUtil.logd($"ads max{adsType} OnBannerAdLoadedEvent show");
                    if (bnWidth > 0)
                    {
                        MaxSdk.SetBannerWidth(bannerId, bnWidth);
                    }
                    if (!dicBanner[0].isShowing)
                    {
                        dicBanner[0].isShowing = true;
                        MaxSdk.ShowBanner(bannerId);
                    }
                    else
                    {
                        MaxSdk.UpdateBannerPosition(bannerId, bnCurrPos);
                        MaxSdk.ShowBanner(bannerId);
                    }
                }
                else
                {
                    SdkUtil.logd($"ads max{adsType} OnBannerAdLoadedEvent hide");
                    dicBanner[0].isShowing = false;
                    MaxSdk.HideBanner(bannerId);
                }
            }
            else
            {
#if ENABLE_MYLOG
                SdkUtil.logd($"ads max{adsType} OnBannerAdLoadedEvent={adUnitId} but not match");
#endif
            }
        }
        private void OnBannerAdLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo err)
        {
            if (adUnitId != null && adUnitId.CompareTo(bannerId) == 0)
            {
                SdkUtil.logd($"ads max{adsType} OnBannerAdLoadFailedEvent={adUnitId}, err={err.ToString()}");
                if (cbBanner != null)
                {
                    var tmpcb = cbBanner;
                    cbBanner = null;
                    tmpcb(AD_State.AD_LOAD_FAIL);
                }
                if (advhelper != null)
                {
                    AdsProcessCB.Instance().Enqueue(() => { advhelper.onBannerLoadFail(adsType); }, 1.0f);
                }
                hideBanner(0);
            }
            else
            {
#if ENABLE_MYLOG
                SdkUtil.logd($"ads max{adsType} OnBannerAdLoadFailedEvent={adUnitId}, but not match");
#endif
            }
        }
        private void OnBannerAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adinfo)
        {
            if (adUnitId != null && adUnitId.CompareTo(bannerId) == 0)
            {
                FIRhelper.logEventAdsPaidMax(adUnitId, adinfo.AdUnitIdentifier, adinfo.NetworkName, adinfo.AdFormat, adinfo.Revenue, MaxSdk.GetSdkConfiguration().CountryCode, adinfo.NetworkPlacement);
            }
        }
        #endregion

        #region INTERSTITIAL AD EVENTS
        private void OnInterstitialLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adinfo)
        {
            if (adUnitId != null && (adUnitId.CompareTo(fullId) == 0 || adUnitId.CompareTo(splashId) == 0))
            {
#if ENABLE_MYLOG
                SdkUtil.logd($"ads max{adsType} HandleInterstitialAdDidLoad = " + adUnitId);
                SdkUtil.logd($"ads max{adsType} HandleInterstitialAdDidLoad netName = " + adinfo.NetworkName);
#endif
                nameFullCurr = adinfo.NetworkName;
                adsFullSubType = getAdsType(nameFullCurr);
                timeLoadFull4Wait = 0;
                int status_tt_load = 0;
                if ("AppLovin".CompareTo(adinfo.NetworkName) == 0)
                {
#if ENABLE_MYLOG
                    SdkUtil.logd($"ads max{adsType} HandleInterstitialAdDidLoad ads is applovin stateShowAppLlovin=" + advhelper.currConfig.stateShowAppLlovin + ", cflv=" + advhelper.currConfig.levelShowAppLovin + ", lv=" + advhelper.level4ApplovinFull);
#endif
                    if (advhelper.currConfig.stateShowAppLlovin == 1 && advhelper.level4ApplovinFull < advhelper.currConfig.levelShowAppLovin)
                    {
#if ENABLE_MYLOG
                        SdkUtil.logd($"ads max{adsType} HandleInterstitialAdDidLoad = " + adUnitId + " stateShowAppLlovin=1");
#endif
                        timeLoadFull4Wait = SdkUtil.systemCurrentMiliseconds();
                        status_tt_load = 1;
                    }
                    else if (advhelper.currConfig.stateShowAppLlovin == 3 && advhelper.level4ApplovinFull < advhelper.currConfig.levelShowAppLovin)
                    {
#if ENABLE_MYLOG
                        SdkUtil.logd($"ads max{adsType} HandleInterstitialAdDidLoad = " + adUnitId + " stateShowAppLlovin=3 fullReloadWithApplovin=" + fullReloadWithApplovin);
#endif
                        if (fullReloadWithApplovin > 0)
                        {
                            fullReloadWithApplovin--;
                            status_tt_load = 3;
                        }
                    }
                }
                FullTryLoad = 0;
                isFullLoading = false;
                if (status_tt_load != 3)
                {
                    isFullLoaded = true;
                }
                else
                {
                    isFullLoaded = false;
                }
                AdsProcessCB.Instance().Enqueue(() =>
                {
                    if (cbFullLoad != null)
                    {
                        var tmpcb = cbFullLoad;
                        cbFullLoad = null;
                        if (status_tt_load == 1)
                        {
#if ENABLE_MYLOG
                            SdkUtil.logd($"ads max{adsType} HandleInterstitialAdDidLoad = " + adUnitId + " -> cb ok_wait");
#endif
                            tmpcb(AD_State.AD_LOAD_OK_WAIT);
                        }
                        else if (status_tt_load == 3)
                        {
#if ENABLE_MYLOG
                            SdkUtil.logd($"ads max{adsType} HandleInterstitialAdDidLoad = " + adUnitId + " -> tryload");
#endif
                            tryLoadFull(false);
                        }
                        else
                        {
#if ENABLE_MYLOG
                            SdkUtil.logd($"ads max{adsType} HandleInterstitialAdDidLoad = " + adUnitId + " -> cb ok");
#endif

                            if (advhelper.currConfig.loadOtherWhenLoadedApplovinButNotReady == 0 || MaxSdk.IsInterstitialReady(adUnitId))
                            {
                                tmpcb(AD_State.AD_LOAD_OK);
                            }
                            else
                            {
                                tmpcb(AD_State.AD_LOAD_OK_WAIT);
                            }
                        }
                    }
                });

#if ENABLE_MYLOG
                SdkUtil.logd($"ads max{adsType} HandleInterstitialAdDidLoad1");
#endif
            }
            else
            {
#if ENABLE_MYLOG
                SdkUtil.logd($"ads max{adsType} HandleInterstitialAdDidLoad={adUnitId} but not match");
#endif
            }
        }

        private void OnInterstitialFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo err)
        {
            if (adUnitId != null && (adUnitId.CompareTo(fullId) == 0 || adUnitId.CompareTo(splashId) == 0))
            {
#if ENABLE_MYLOG
                SdkUtil.logd($"ads max{adsType} HandleInterstitialAdDidFailWithError adid=" + adUnitId + ", err=" + err.ToString());
#endif
                isFullLoading = false;
                isFullLoaded = false;
                AdsProcessCB.Instance().Enqueue(() =>
                {
                    FullTryLoad++;
                    tryLoadFull(false);
                }, 1.0f);

#if ENABLE_MYLOG
                SdkUtil.logd($"ads max{adsType} HandleInterstitialAdDidFailWithError1");
#endif
            }
            else
            {
#if ENABLE_MYLOG
                SdkUtil.logd($"ads max{adsType} HandleInterstitialAdDidFailWithError adid=" + adUnitId + ", but not match");
#endif
            }
        }

        private void OnInterstitialDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adinfo)
        {
            if (adUnitId != null && (adUnitId.CompareTo(fullId) == 0 || adUnitId.CompareTo(splashId) == 0))
            {
#if ENABLE_MYLOG
                SdkUtil.logd($"ads max{adsType} OnInterstitialDisplayedEvent=" + adUnitId);
#endif
                if (cbFullShow != null)
                {
                    AdCallBack tmpcb = cbFullShow;
                    AdsProcessCB.Instance().Enqueue(() =>
                    {
                        tmpcb(AD_State.AD_SHOW);
                    });
                }
            }
            else
            {

            }
        }

        private void InterstitialFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo err, MaxSdkBase.AdInfo adinfo)
        {
            if (adUnitId != null && (adUnitId.CompareTo(fullId) == 0 || adUnitId.CompareTo(splashId) == 0))
            {
#if ENABLE_MYLOG
                SdkUtil.logd($"ads max{adsType} InterstitialAdShowFailedEvent adid=" + adUnitId + ", err=" + err.ToString());
#endif
                isFullLoading = false;
                isFullLoaded = false;
                if (cbFullShow != null)
                {
                    AdCallBack tmpcb = cbFullShow;
#if ENABLE_MYLOG
                    SdkUtil.logd($"ads max{adsType} full InterstitialAdShowFailedEvent 1");
#endif
                    AdsProcessCB.Instance().Enqueue(() => { tmpcb(AD_State.AD_SHOW_FAIL); });
                }
                onFullClose();
            }
        }

        private void OnInterstitialDismissedEvent(string adUnitId, MaxSdkBase.AdInfo adinfo)
        {
            if (adUnitId != null && (adUnitId.CompareTo(fullId) == 0 || adUnitId.CompareTo(splashId) == 0))
            {
#if ENABLE_MYLOG
                SdkUtil.logd($"ads max{adsType} HandleInterstitialClosed = " + adUnitId);
#endif
                isFullLoading = false;
                isFullLoaded = false;
                if (cbFullShow != null)
                {
                    AdCallBack tmpcb = cbFullShow;
#if ENABLE_MYLOG
                    SdkUtil.logd($"ads max{adsType} full HandleInterstitialClosed 1");
#endif
                    AdsProcessCB.Instance().Enqueue(() => { tmpcb(AD_State.AD_CLOSE); });
                }
                onFullClose();

                FullTryLoad = 0;
                cbFullShow = null;
#if ENABLE_MYLOG
                SdkUtil.logd($"ads max{adsType} HandleInterstitialClosed 2");
#endif
            }
        }
        private void OnInterstitialAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adinfo)
        {
            if (adUnitId != null && (adUnitId.CompareTo(fullId) == 0 || adUnitId.CompareTo(splashId) == 0))
            {
                FIRhelper.logEventAdsPaidMax(adUnitId, adinfo.AdUnitIdentifier, adinfo.NetworkName, adinfo.AdFormat, adinfo.Revenue, MaxSdk.GetSdkConfiguration().CountryCode, adinfo.NetworkPlacement);
            }
        }
        #endregion

        #region REWARDED VIDEO AD EVENTS

        private void OnRewardedAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adinfo)
        {
            if (adUnitId != null && adUnitId.CompareTo(giftId) == 0)
            {
#if ENABLE_MYLOG
                SdkUtil.logd($"ads max{adsType} rw OnRewardedAdLoadedEvent=" + adUnitId);
                SdkUtil.logd($"ads max{adsType} rw OnRewardedAdLoadedEvent netName = " + adinfo.NetworkName);
#endif
                nameGiftCurr = adinfo.NetworkName;
                GiftTryLoad = 0;
                isGiftLoading = false;
                isGiftLoaded = true;
                timeLoadGift = 0;
                tLoadGiftErr = 45;
                timeLoadGift4Wait = 0;
                int status_tt_load = 0;
                if ("AppLovin".CompareTo(adinfo.NetworkName) == 0)
                {
#if ENABLE_MYLOG
                    SdkUtil.logd($"ads max{adsType} rw OnRewardedAdLoadedEvent ads is applovin stateShowAppLlovin=" + advhelper.currConfig.stateShowAppLlovin + ", cflv=" + advhelper.currConfig.levelShowAppLovin + ", lv=" + advhelper.level4ApplovinGift);
#endif
                    if (advhelper.currConfig.stateShowAppLlovin == 1 && advhelper.level4ApplovinGift < advhelper.currConfig.levelShowAppLovin)
                    {
#if ENABLE_MYLOG
                        SdkUtil.logd($"ads max{adsType} rw OnRewardedAdLoadedEvent = " + adUnitId + " stateShowAppLlovin=1");
#endif
                        timeLoadGift4Wait = SdkUtil.systemCurrentMiliseconds();
                        status_tt_load = 1;
                    }
                    else if (advhelper.currConfig.stateShowAppLlovin == 3 && advhelper.level4ApplovinGift < advhelper.currConfig.levelShowAppLovin)
                    {
#if ENABLE_MYLOG
                        SdkUtil.logd($"ads max{adsType} rw OnRewardedAdLoadedEvent = " + adUnitId + " stateShowAppLlovin=3 giftReloadWithApplovin=" + giftReloadWithApplovin);
#endif
                        if (giftReloadWithApplovin > 0)
                        {
                            giftReloadWithApplovin--;
                            status_tt_load = 3;
                        }
                    }
                }
                AdsProcessCB.Instance().Enqueue(() =>
                {
                    if (cbGiftLoad != null)
                    {
                        var tmpcb = cbGiftLoad;
                        cbGiftLoad = null;
                        if (status_tt_load == 1)
                        {
#if ENABLE_MYLOG
                            SdkUtil.logd($"ads max{adsType} rw OnRewardedAdLoadedEvent = " + adUnitId + " -> cb ok_wait");
#endif
                            tmpcb(AD_State.AD_LOAD_OK_WAIT);
                        }
                        else if (status_tt_load == 3)
                        {
#if ENABLE_MYLOG
                            SdkUtil.logd($"ads max{adsType} rw OnRewardedAdLoadedEvent = " + adUnitId + " -> tryload");
#endif
                            tryloadGift();
                        }
                        else
                        {
#if ENABLE_MYLOG
                            SdkUtil.logd($"ads max{adsType} rw OnRewardedAdLoadedEvent = " + adUnitId + " -> cb ok");
#endif
                            if (advhelper.currConfig.loadOtherWhenLoadedApplovinButNotReady == 0 || MaxSdk.IsRewardedAdReady(adUnitId))
                            {
                                tmpcb(AD_State.AD_LOAD_OK);
                            }
                            else
                            {
                                tmpcb(AD_State.AD_LOAD_OK_WAIT);
                            }
                        }
                    }
                });
            }
            else
            {
#if ENABLE_MYLOG
                SdkUtil.logd($"ads max{adsType} rw OnRewardedAdLoadedEvent=" + adUnitId + " but not match");
#endif
            }
        }

        private void OnRewardedAdFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo err)
        {
            if (adUnitId != null && adUnitId.CompareTo(giftId) == 0)
            {
#if ENABLE_MYLOG
                SdkUtil.logd($"ads max{adsType} rw OnRewardedAdFailedEvent=" + adUnitId + ", err=" + err.ToString());
#endif
                isGiftLoading = false;
                isGiftLoaded = false;
                timeLoadGift = 0;
                tLoadGiftErr = 45;
                AdsProcessCB.Instance().Enqueue(() =>
                {
                    GiftTryLoad++;
                    tryloadGift();
                }, 1.0f);

#if ENABLE_MYLOG
                SdkUtil.logd($"ads max{adsType} rw OnRewardedAdFailedEvent 1");
#endif
            }
            else
            {
#if ENABLE_MYLOG
                SdkUtil.logd($"ads max{adsType} rw OnRewardedAdFailedEvent=" + adUnitId + ", but not match");
#endif
            }
        }

        private void OnRewardedAdDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adinfo)
        {
            if (adUnitId != null && adUnitId.CompareTo(giftId) == 0)
            {
#if ENABLE_MYLOG
                SdkUtil.logd($"ads max{adsType} rw OnRewardedAdDisplayedEvent=" + adUnitId);
#endif
                if (cbGiftShow != null)
                {
                    AdCallBack tmpcb = cbGiftShow;
                    AdsProcessCB.Instance().Enqueue(() => { tmpcb(AD_State.AD_SHOW); });
                }
            }
        }

        private void OnRewardedAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adinfo)
        {
            if (adUnitId != null && adUnitId.CompareTo(giftId) == 0)
            {
#if ENABLE_MYLOG
                SdkUtil.logd($"ads max{adsType} rw OnRewardedAdClickedEvent=" + adUnitId);
#endif
                isRewardCom = true;
            }
        }
#if ENABLE_ADS_MAX
        private void OnRewardedAdReceivedRewardEvent(string adUnitId, MaxSdkBase.Reward reward, MaxSdkBase.AdInfo info)
        {
            if (adUnitId != null && adUnitId.CompareTo(giftId) == 0)
            {
#if ENABLE_MYLOG
                SdkUtil.logd($"ads max{adsType} rw OnRewardedAdReceivedRewardEvent=" + adUnitId);
#endif
                isRewardCom = true;
            }
        }
#endif

        private void OnRewardedAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo err, MaxSdkBase.AdInfo adinfo)
        {
            if (adUnitId != null && adUnitId.CompareTo(giftId) == 0)
            {
#if ENABLE_MYLOG
                SdkUtil.logd($"ads max{adsType} rw OnRewardedAdFailedToDisplayEvent adid=" + adUnitId + ", err=" + err.ToString());
#endif
                isGiftLoading = false;
                isGiftLoaded = false;
                if (cbGiftShow != null)
                {
                    AdCallBack tmpcb = cbGiftShow;
#if ENABLE_MYLOG
                    SdkUtil.logd($"ads max{adsType} rw _cbAD fail");
#endif
                    AdsProcessCB.Instance().Enqueue(() => { tmpcb(AD_State.AD_REWARD_FAIL); });
                    AdsProcessCB.Instance().Enqueue(() => { tmpcb(AD_State.AD_SHOW_FAIL); });
                }
                onGiftClose();
            }
        }

        private void OnRewardedAdDismissedEvent(string adUnitId, MaxSdkBase.AdInfo adinfo)
        {
            if (adUnitId != null && adUnitId.CompareTo(giftId) == 0)
            {
#if ENABLE_MYLOG
                SdkUtil.logd($"ads max{adsType} rw OnRewardedAdDismissedEvent=" + adUnitId);
#endif
                isGiftLoading = false;
                isGiftLoaded = false;
                if (cbGiftShow != null)
                {
                    AdCallBack tmpcb = cbGiftShow;
#if ENABLE_MYLOG
                    SdkUtil.logd($"ads max{adsType} rw _cbAD != null");
#endif
                    if (isRewardCom)
                    {
#if ENABLE_MYLOG
                        SdkUtil.logd($"ads max{adsType} rw _cbAD reward");
#endif
                        AdsProcessCB.Instance().Enqueue(() => { tmpcb(AD_State.AD_REWARD_OK); });
                    }
                    else
                    {
#if ENABLE_MYLOG
                        SdkUtil.logd($"ads max{adsType} rw _cbAD fail");
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
                SdkUtil.logd($"ads max{adsType} rw OnRewardedAdDismissedEvent3");
#endif
            }
        }
        private void OnRewardedAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adinfo)
        {
            if (adUnitId != null && adUnitId.CompareTo(giftId) == 0)
            {
                FIRhelper.logEventAdsPaidMax(adUnitId, adinfo.AdUnitIdentifier, adinfo.NetworkName, adinfo.AdFormat, adinfo.Revenue, MaxSdk.GetSdkConfiguration().CountryCode, adinfo.NetworkPlacement);
            }
        }
        #endregion

#endif

    }
}