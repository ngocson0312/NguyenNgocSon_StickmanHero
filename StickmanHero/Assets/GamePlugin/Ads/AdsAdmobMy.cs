//#define ENABLE_ADS_ADMOB
//#define ENABLE_TEST_ADMOB

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if ENABLE_ADS_ADMOB && USE_ADSMOB_MY
#endif

namespace mygame.sdk
{
    public class AdsECPM
    {
        public string adsid;
        public bool isLoaded;

        public AdsECPM(string _adsid)
        {
            adsid = _adsid;
            isLoaded = false;
        }
    }
    public class AdsAdmobMy : AdsBase
    {

#if ENABLE_ADS_ADMOB && USE_ADSMOB_MY

#endif
        List<AdsECPM> listBannerEcpm = new List<AdsECPM>();
        int idxCurrEcpmBN = 0;
        bool isProcessShowBN = false;
        List<AdsECPM> listFullEcpm = new List<AdsECPM>();
        int idxCurrEcpmFull = 0;

        long timeShowBanner = 0;
        float _membnDxCenter;

        public override void InitAds()
        {
#if ENABLE_ADS_ADMOB && USE_ADSMOB_MY
            AdsAdmobMyBridge.Instance.Initialize();
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
            initBanner();
            initFull();

            AdsAdmobMyBridge.onBNLoaded += OnBannerAdLoadedEvent;
            AdsAdmobMyBridge.onBNLoadFail += OnBannerAdLoadFailedEvent;
            AdsAdmobMyBridge.onBNPaid += OnBannerAdPaidEvent;

            AdsAdmobMyBridge.onInterstitialLoaded += OnInterstitialLoadedEvent;
            AdsAdmobMyBridge.onInterstitialLoadFail += OnInterstitialFailedEvent;
            AdsAdmobMyBridge.onInterstitialShowed += OnInterstitialDisplayedEvent;
            AdsAdmobMyBridge.onInterstitialFailedToShow += onInterstitialFailedToShow;
            AdsAdmobMyBridge.onInterstitialDismissed += OnInterstitialDismissedEvent;
            AdsAdmobMyBridge.onInterstitialPaid += OnInterstitialAdPaidEvent;

            AdsAdmobMyBridge.onRewardLoaded += OnRewardedAdLoadedEvent;
            AdsAdmobMyBridge.onRewardLoadFail += OnRewardedAdFailedEvent;
            AdsAdmobMyBridge.onRewardFailedToShow += OnRewardedAdFailedToDisplayEvent;
            AdsAdmobMyBridge.onRewardShowed += OnRewardedAdDisplayedEvent;
            AdsAdmobMyBridge.onRewardDismissed += OnRewardedAdDismissedEvent;
            AdsAdmobMyBridge.onRewardReward += OnRewardedAdReceivedRewardEvent;
            AdsAdmobMyBridge.onRewardPaid += OnRewardedAdPaidEvent;
#endif
        }

        public void initBanner()
        {
            if (adsType != 0)
            {
                return;
            }
            listBannerEcpm.Clear();
            try
            {
                SdkUtil.logd("ads admobmy stepFloorECPMBanner=" + advhelper.currConfig.stepFloorECPMBanner);
                if (advhelper.currConfig.stepFloorECPMBanner.Length > 0)
                {
                    string[] arrlv = advhelper.currConfig.stepFloorECPMBanner.Split(new char[] { ';' });
                    foreach (string item in arrlv)
                    {
                        if (item.StartsWith("ca-app"))
                        {
                            listBannerEcpm.Add(new AdsECPM(item));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Log("mysdk: ex=" + ex.ToString());
            }
            AdsECPM bnc = new AdsECPM(bannerId);
            listBannerEcpm.Add(bnc);
            idxCurrEcpmBN = 0;
        }

        public void initFull()
        {
            if (adsType != 0)
            {
                return;
            }
            listFullEcpm.Clear();
            try
            {
                SdkUtil.logd("ads admobmy stepFloorECPMFull=" + advhelper.currConfig.stepFloorECPMFull);
                if (advhelper.currConfig.stepFloorECPMFull.Length > 0)
                {
                    string[] arrlv = advhelper.currConfig.stepFloorECPMFull.Split(new char[] { ';' });
                    foreach (string item in arrlv)
                    {
                        if (item.StartsWith("ca-app"))
                        {
                            listFullEcpm.Add(new AdsECPM(item));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Log("mysdk: ex=" + ex.ToString());
            }
            AdsECPM bnc = new AdsECPM(fullId);
            listFullEcpm.Add(bnc);
            idxCurrEcpmFull = 0;
            SdkUtil.logd("ads admobmy stepFloorECPMFull count=" + listFullEcpm.Count);
        }

        public override string getname()
        {
            return "adsmobMy";
        }

        protected override void tryLoadBanner(int typeb)
        {
#if ENABLE_ADS_ADMOB && USE_ADSMOB_MY
            string idload = listBannerEcpm[idxCurrEcpmBN].adsid;
#if ENABLE_TEST_ADMOB
            idload = "ca-app-pub-3940256099942544/6300978111";
#endif
            SdkUtil.logd("ads admobmy tryLoadBanner = " + idload + ", idxCurrEcpm=" + idxCurrEcpmBN);
            // if (BNTryLoad >= toTryLoad)
            // {
            //     SdkUtil.logd("ads admobmy tryLoadBanner over trycount");
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
            isProcessShowBN = true;
            dicBanner[0].isBNLoading = true;
            dicBanner[0].isBNloaded = false;
            if (AppConfig.isBannerIpad)
            {
                AdsAdmobMyBridge.Instance.showBanner(idload, dicBanner[0].posBanner, (int)advhelper.bnOrien, SdkUtil.isiPad(), _membnDxCenter);
            }
            else
            {
                AdsAdmobMyBridge.Instance.showBanner(idload, dicBanner[0].posBanner, (int)advhelper.bnOrien, false, _membnDxCenter);
            }
            SdkUtil.logd("ads admobmy tryLoadBanner 3 _membnDxCenter=" + _membnDxCenter);
#else
            if (cbBanner != null)
            {
                SdkUtil.logd("ads admobmy tryLoadBanner not enable");
                var tmpcb = cbBanner;
                cbBanner = null;
                tmpcb(AD_State.AD_LOAD_FAIL);
            }
#endif
        }
        public override void loadBanner(int typeb, AdCallBack cb)
        {
            SdkUtil.logd("ads admobmy loadBanner");
            cbBanner = cb;
            if (!isProcessShowBN)
            {
                BNTryLoad = 0;
                tryLoadBanner(0);
            }
            else
            {
                SdkUtil.logd("ads admobmy loadBanner isProcessShow");
            }
        }
        public override bool showBanner(int typeb, int pos, int width, AdCallBack cb, float dxCenter)
        {
            if (!isProcessShowBN)
            {
                idxCurrEcpmBN = 0;
            }
            SdkUtil.logd("ads admobmy showBanner typeb=" + typeb + ", pos=" + pos + ", dxCenter=" + dxCenter + ", idxCurrEcpm=" + idxCurrEcpmBN + ", countecpm=" + listBannerEcpm.Count);
            dicBanner[0].isBNShow = true;
            dicBanner[0].posBanner = pos;
            bnWidth = width;
            _membnDxCenter = dxCenter;
#if ENABLE_ADS_ADMOB && USE_ADSMOB_MY
            if (!isProcessShowBN)
            {
                int idxsh = -10;
                for (int i = 0; i < listBannerEcpm.Count; i++)
                {
                    AdsECPM bnec = listBannerEcpm[i];
                    if (bnec.isLoaded)
                    {
                        string idload = bnec.adsid;
#if ENABLE_TEST_ADMOB
                        idload = "ca-app-pub-3940256099942544/6300978111";
#endif
                        SdkUtil.logd("ads admobmy showBanner show pre loaded adsid=" + bnec.adsid + ", idx=" + i + ", dxCenter=" + dxCenter);
                        if (AppConfig.isBannerIpad)
                        {
                            AdsAdmobMyBridge.Instance.showBanner(idload, dicBanner[0].posBanner, (int)advhelper.bnOrien, SdkUtil.isiPad(), dxCenter);
                        }
                        else
                        {
                            AdsAdmobMyBridge.Instance.showBanner(idload, dicBanner[0].posBanner, (int)advhelper.bnOrien, false, dxCenter);
                        }
                        if (cb != null)
                        {
                            cb(AD_State.AD_SHOW);
                        }
                        idxsh = i;
                        break;
                    }
                }

                if (idxsh != -10)
                {
                    if (cb != null)
                    {
                        cb(AD_State.AD_SHOW);
                    }
                    return true;
                }
                else
                {
                    loadBanner(0, cb);
                    return false;
                }
            }
            else
            {
                SdkUtil.logd("ads admobmy showBanner isprocess show dxCenter=" + dxCenter);
                AdsAdmobMyBridge.Instance.setBannerPos(pos, dxCenter);
                return false;
            }
#else
            if (cb != null)
            {
                SdkUtil.logd("ads admobmy tryLoadBanner not enable");
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
        public override void hideBanner(int typeb)
        {
            SdkUtil.logd("ads admobmy hideBanner");
            dicBanner[0].isBNShow = false;
#if ENABLE_ADS_ADMOB && USE_ADSMOB_MY
            AdsAdmobMyBridge.Instance.hideBanner();
#endif
        }
        public override void destroyBanner(int typeb)
        {
#if ENABLE_ADS_ADMOB && USE_ADSMOB_MY
            dicBanner[0].isBNLoading = false;
            dicBanner[0].isBNloaded = false;
            dicBanner[0].isBNHide = true;
            AdsAdmobMyBridge.Instance.hideBanner();
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
            SdkUtil.logd("ads admobmy getFullLoaded issplash=" + _isSplash + ", isFullLoaded=" + isFullLoaded);
            if (_isSplash)
            {
                if (splashId == null || splashId.Length < 10)
                {
                    _isSplash = false;
                    SdkUtil.logd("ads admobmy getFullLoaded change is splash");
                }
            }
#if ENABLE_ADS_ADMOB && USE_ADSMOB_MY
            if (isFullLoaded)
            {
                return 1;
            }
#endif
            return 0;
        }
        protected override void tryLoadFull(bool _isSplash)
        {
#if ENABLE_ADS_ADMOB && USE_ADSMOB_MY
            string idLoad = "";
            if (_isSplash)
            {
                if (splashId == null || splashId.Length < 10)
                {
                    _isSplash = false;
#if ENABLE_MYLOG
                    SdkUtil.logd("ads admobmy tryLoadFull change splash to no");
#endif
                }
                else
                {
                    idLoad = splashId;
#if ENABLE_MYLOG
                    SdkUtil.logd("ads admobmy tryLoadFull splash =" + splashId);
#endif
                }

            }
            if (!_isSplash)
            {
                idLoad = listFullEcpm[idxCurrEcpmFull].adsid;
#if ENABLE_TEST_ADMOB
                idLoad = "ca-app-pub-3940256099942544/1033173712";
#endif

#if ENABLE_MYLOG
                SdkUtil.logd("ads admobmy tryLoadFull=" + idLoad + ", idxCurrEcpmFull=" + idxCurrEcpmFull);
#endif
            }
            int tryload = FullTryLoad;
            if (tryload >= toTryLoad)
            {
#if ENABLE_MYLOG
                SdkUtil.logd("ads admobmy tryLoadFull over try");
#endif
                if (cbFullLoad != null)
                {
                    var tmpcb = cbFullLoad;
                    cbFullLoad = null;
                    tmpcb(AD_State.AD_LOAD_FAIL);
                }

                return;
            }
            if (advhelper.isChangeToStaticAds && splashId != null && splashId.Length > 5)
            {
#if ENABLE_MYLOG
                SdkUtil.logd("ads admobmy tryLoadFull load id static ads");
#endif
                idLoad = splashId;
            }
#if ENABLE_MYLOG
            SdkUtil.logd("ads admobmy tryLoadFull load idLoad=" + idLoad);
#endif
            if (idLoad != null && idLoad.Contains("ca-app-pub"))
            {
                isFullLoading = true;
                isFullLoaded = false;
                AdsAdmobMyBridge.Instance.loadFull(idLoad);
            }
            else
            {
#if ENABLE_MYLOG
                SdkUtil.logd("ads admobmy tryLoadFull id not correct");
#endif
                isFullLoading = false;
                isFullLoaded = false;

            }

#else
            if (cbFullLoad != null)
            {
#if ENABLE_MYLOG
                SdkUtil.logd("ads admobmy tryLoadFull not enable");
#endif
                var tmpcb = cbFullLoad;
                cbFullLoad = null;
                tmpcb(AD_State.AD_LOAD_FAIL);
            }
#endif
        }
        public override void loadFull(bool _isSplash, AdCallBack cb)
        {
            SdkUtil.logd("ads admobmy loadFull type=" + adsType + ", isSplash=" + _isSplash);
            if (_isSplash)
            {
                if (splashId == null || splashId.Length < 10)
                {
                    _isSplash = false;
                    SdkUtil.logd("ads admobmy loadFull change is splash");
                }
            }

            bool isLoaded = false;
            bool isLoadding = false;
            idxCurrEcpmFull = 0;
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
                SdkUtil.logd("ads admobmy loadFull isloading or isloaded");
            }
        }
        public override bool showFull(bool _isSplash, AdCallBack cb)
        {
            SdkUtil.logd("ads admobmy showFull type=" + adsType + ", isspalsh=" + _isSplash);
            if (_isSplash)
            {
                if (splashId == null || splashId.Length < 10)
                {
                    _isSplash = false;
                    SdkUtil.logd("ads admobmy showFull change is splash");
                }
            }
            cbFullShow = null;
            int ss = getFullLoaded(_isSplash);
            if (ss > 0)
            {
                SdkUtil.logd("ads admobmy showFull type=" + adsType);
                FullTryLoad = 0;
                isFullLoaded = false;
#if ENABLE_ADS_ADMOB && USE_ADSMOB_MY
                cbFullShow = cb;
                AdsAdmobMyBridge.Instance.showFull();
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
            string idLoad = giftId;
#if ENABLE_TEST_ADMOB
            idLoad = "ca-app-pub-3940256099942544/5354046379";
#endif
#if ENABLE_MYLOG
            SdkUtil.logd("ads admobmy tryloadGift =" + idLoad);
#endif
            if (GiftTryLoad >= toTryLoad)
            {
#if ENABLE_MYLOG
                SdkUtil.logd("ads admobmy tryloadGift over try");
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
            AdsAdmobMyBridge.Instance.loadGift(idLoad);
#else
            if (cbGiftLoad != null)
            {
#if ENABLE_MYLOG
                SdkUtil.logd("ads admobmy tryloadGift not enable");
#endif
                var tmpcb = cbGiftLoad;
                cbGiftLoad = null;
                tmpcb(AD_State.AD_LOAD_FAIL);
            }
#endif
        }
        public override void loadGift(AdCallBack cb)
        {
            SdkUtil.logd("ads admobmy loadGift");
            cbGiftLoad = cb;
            if (!isGiftLoading && !isGiftLoaded)
            {
                GiftTryLoad = 0;
                tryloadGift();
            }
            else
            {
                SdkUtil.logd($"ads admobmy loadGift isloading={isGiftLoading} or isloaded={isGiftLoaded}");
            }
        }
        public override bool showGift(AdCallBack cb)
        {
            SdkUtil.logd("ads admobmy showGift");
            cbGiftShow = null;
            if (getGiftLoaded())
            {
                isGiftLoaded = false;
#if ENABLE_ADS_ADMOB && USE_ADSMOB_MY
                cbGiftShow = cb;
                AdsAdmobMyBridge.Instance.showGift();
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
            SdkUtil.logd("ads admobmy HandleBannerAdLoaded idxCurrEcpm=" + idxCurrEcpmBN);
#endif
            if (isProcessShowBN)
            {
                listBannerEcpm[idxCurrEcpmBN].isLoaded = true;
            }
            isProcessShowBN = false;
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
            SdkUtil.logd("ads admobmy OnBannerAdLoadFailedEvent=" + err + ", isProcessShow=" + isProcessShowBN);
            if (isProcessShowBN)
            {
                if (dicBanner[0].isBNShow)
                {
                    if (idxCurrEcpmBN < (listBannerEcpm.Count - 1))
                    {
                        idxCurrEcpmBN++;
                        if (!listBannerEcpm[idxCurrEcpmBN].isLoaded)
                        {
                            tryLoadBanner(0);
                        }
                        else
                        {
                            idxCurrEcpmBN = 0;
                            isProcessShowBN = false;
                            AdsProcessCB.Instance().Enqueue(() =>
                            {
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
                            });
                        }
                    }
                    else
                    {
                        idxCurrEcpmBN = 0;
                        isProcessShowBN = false;
                        AdsProcessCB.Instance().Enqueue(() =>
                        {
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
                        });
                    }
                }
                else
                {
                    idxCurrEcpmBN = 0;
                    isProcessShowBN = false;
                    AdsProcessCB.Instance().Enqueue(() =>
                    {
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
                    });
                }
            }
        }
        private void OnBannerAdPaidEvent(int precisionType, string currencyCode, long valueMicros)
        {
            FIRhelper.logEventAdsPaidAdmob(0, bannerId, precisionType, currencyCode, valueMicros);
        }
        #endregion

        #region INTERSTITIAL AD EVENTS
        private void OnInterstitialLoadedEvent()
        {
#if ENABLE_MYLOG
            SdkUtil.logd("ads admobmy HandleInterstitialLoaded idxCurrEcpmFull=" + idxCurrEcpmFull);
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
            SdkUtil.logd("ads admobmy HandleInterstitialLoaded 1");
#endif
        }

        private void OnInterstitialFailedEvent(string err)
        {
#if ENABLE_MYLOG
            SdkUtil.logd("ads admobmy HandleInterstitialFailedToLoad=" + err);
#endif
            isFullLoading = false;
            isFullLoaded = false;
            if (idxCurrEcpmFull < (listFullEcpm.Count - 1))
            {
#if ENABLE_MYLOG
                SdkUtil.logd("ads admobmy HandleInterstitialFailedToLoad load other ecpm idxCurrEcpmFull=" + idxCurrEcpmFull + ", count=" + listFullEcpm.Count);
#endif
                idxCurrEcpmFull++;
                tryLoadFull(false);
            }
            else
            {
                AdsProcessCB.Instance().Enqueue(() =>
                {
                    FullTryLoad++;
                    tryLoadFull(false);
                }, 1.0f);
            }

#if ENABLE_MYLOG
            SdkUtil.logd("ads admobmy HandleInterstitialFailedToLoad 1");
#endif
        }

        private void OnInterstitialDisplayedEvent()
        {
#if ENABLE_MYLOG
            SdkUtil.logd("ads admobmy HandleInterstitialOpened");
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
            SdkUtil.logd("ads admobmy onInterstitialFailedToShow=" + err);
#endif
            isFullLoading = false;
            isFullLoaded = false;
            if (cbFullShow != null)
            {
                AdCallBack tmpcb = cbFullShow;
#if ENABLE_MYLOG
                SdkUtil.logd("ads admobmy full InterstitialAdShowFailedEvent 1");
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
                SdkUtil.logd("ads admobmy full HandleInterstitialClosed");
#endif
                AdsProcessCB.Instance().Enqueue(() => { tmpcb(AD_State.AD_CLOSE); });
            }
            onFullClose();

            FullTryLoad = 0;
            cbFullShow = null;
#if ENABLE_MYLOG
            SdkUtil.logd("ads admobmy full HandleInterstitialClosed 1");
#endif
        }
        private void OnInterstitialAdPaidEvent(int precisionType, string currencyCode, long valueMicros)
        {
            FIRhelper.logEventAdsPaidAdmob(1, fullId, precisionType, currencyCode, valueMicros);
        }
        #endregion

        #region REWARDED VIDEO AD EVENTS

        private void OnRewardedAdLoadedEvent()
        {
#if ENABLE_MYLOG
            SdkUtil.logd("ads admobmy rw HandleRewardBasedVideoLoaded");
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
            SdkUtil.logd("ads admobmy rw HandleRewardBasedVideoLoaded 1");
#endif
        }

        private void OnRewardedAdFailedEvent(string err)
        {
#if ENABLE_MYLOG
            SdkUtil.logd("ads admobmy rw HandleRewardBasedVideoFailedToLoad=" + err);
#endif
            isGiftLoading = false;
            isGiftLoaded = false;
            AdsProcessCB.Instance().Enqueue(() =>
            {
                GiftTryLoad++;
                tryloadGift();
            }, 1.0f);
#if ENABLE_MYLOG
            SdkUtil.logd("ads admobmy rw HandleRewardBasedVideoFailedToLoad 1");
#endif
        }

        private void OnRewardedAdDisplayedEvent()
        {
#if ENABLE_MYLOG
            SdkUtil.logd("ads admobmy rw HandleRewardBasedVideoOpened");
#endif
            GiftTryLoad = 0;
            if (cbGiftShow != null)
            {
                var tmpcb = cbGiftShow;
#if ENABLE_MYLOG
                SdkUtil.logd("ads admobmy rw HandleRewardBasedVideoClosed2");
#endif
                AdsProcessCB.Instance().Enqueue(() => { tmpcb(AD_State.AD_SHOW); });
            }
#if ENABLE_MYLOG
            SdkUtil.logd("ads admobmy rw HandleRewardBasedVideoOpened 1");
#endif
        }

        private void OnRewardedAdFailedToDisplayEvent(string err)
        {
#if ENABLE_MYLOG
            SdkUtil.logd("ads admobmy rw HandleRewardBasedVideoFailToShow=" + err);
#endif
            isGiftLoading = false;
            isGiftLoaded = false;
            if (cbGiftShow != null)
            {
                AdCallBack tmpcb = cbGiftShow;
#if ENABLE_MYLOG
                SdkUtil.logd("ads admobmy rw _cbAD fail");
#endif
                AdsProcessCB.Instance().Enqueue(() => { tmpcb(AD_State.AD_REWARD_FAIL); });
                AdsProcessCB.Instance().Enqueue(() => { tmpcb(AD_State.AD_SHOW_FAIL); });
            }
            onGiftClose();
        }

        private void OnRewardedAdReceivedRewardEvent()
        {
#if ENABLE_MYLOG
            SdkUtil.logd("ads admobmy rw HandleRewardBasedVideoRewarded");
#endif
            isRewardCom = true;
        }

        private void OnRewardedAdDismissedEvent()
        {
#if ENABLE_MYLOG
            SdkUtil.logd("ads admobmy rw HandleRewardBasedVideoClosed");
#endif
            isGiftLoading = false;
            isGiftLoaded = false;
            if (cbGiftShow != null)
            {
                AdCallBack tmpcb = cbGiftShow;
#if ENABLE_MYLOG
                SdkUtil.logd("ads admobmy rw _cbAD != null");
#endif
                if (isRewardCom)
                {
#if ENABLE_MYLOG
                    SdkUtil.logd("ads admobmy rw _cbAD reward");
#endif
                    AdsProcessCB.Instance().Enqueue(() => { tmpcb(AD_State.AD_REWARD_OK); });
                }
                else
                {
#if ENABLE_MYLOG
                    SdkUtil.logd("ads admobmy rw _cbAD fail");
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
            SdkUtil.logd("ads admobmy rw HandleRewardBasedVideoClosed 3");
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