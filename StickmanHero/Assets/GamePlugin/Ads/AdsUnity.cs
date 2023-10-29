//#define ENABLE_ADS_UNITY

using System;
using UnityEngine;
using System.Collections;

#if ENABLE_ADS_UNITY
using UnityEngine.Advertisements;
#endif

namespace mygame.sdk
{
#if ENABLE_ADS_UNITY
    public class AdsUnity : AdsBase, IUnityAdsListener
#else
    public class AdsUnity : AdsBase
#endif

    {
#if ENABLE_ADS_UNITY

#endif

        public override void InitAds()
        {
#if ENABLE_ADS_UNITY
            isEnable = true;
#endif
        }

        public override string getname()
        {
            return "unity";
        }

        public override void AdsAwake()
        {
#if ENABLE_ADS_UNITY
            isEnable = true;
#endif
        }

        private void Start()
        {
#if ENABLE_ADS_UNITY
            Advertisement.Initialize(appId, false);
            Advertisement.AddListener(this);
#endif

        }

        protected override void tryLoadBanner(int type)
        {
#if ENABLE_ADS_UNITY
            SdkUtil.logd("unity RequestBanner = " + bannerId);
            isBNLoading = true;
            isBNloaded = false;
            Advertisement.Banner.Load();
            StartCoroutine(waitBannerReady());
#else
            dicBanner[type].isBNloaded = false;
            if (cbBanner != null)
            {
                SdkUtil.logd("unity not avaiable");
                var tmpcb = cbBanner;
                cbBanner = null;
                tmpcb(AD_State.AD_LOAD_FAIL);
            }
#endif
        }
        IEnumerator waitBannerReady()
        {
#if ENABLE_ADS_UNITY
            int count = 0;
            while (!Advertisement.Banner.isLoaded && count < 10)
            {
                yield return new WaitForSeconds(0.5f);
                count++;
            }
            isBNLoading = false;
            SdkUtil.logd("unity waitBannerReady cb=" + cbBanner);
            if (cbBanner != null)
            {
                if (Advertisement.Banner.isLoaded)
                {
                    isBNloaded = true;
                    var tmpcb = cbBanner;
                    cbBanner = null;
                    tmpcb(AD_State.AD_LOAD_OK);
                    BannerPosition adposbn;
                    if (posBanner == 0)
                    {
                        adposbn = BannerPosition.TOP_CENTER;
                    }
                    else if (posBanner == 1)
                    {
                        adposbn = BannerPosition.BOTTOM_CENTER;
                    }
                    else
                    {
                        adposbn = BannerPosition.BOTTOM_CENTER;
                    }
                    Advertisement.Banner.SetPosition(adposbn);
                    Advertisement.Banner.Show(bannerId);
                    SdkUtil.logd("unity waitBannerReady Call show");
                }
                else
                {
                    isBNloaded = false;
                    var tmpcb = cbBanner;
                    cbBanner = null;
                    tmpcb(AD_State.AD_LOAD_FAIL);
                }
            }
#endif
            yield break;
        }
        public override void loadBanner(int type, AdCallBack cb)
        {
            SdkUtil.logd("unity loadBanner");
            cbBanner = cb;
            if (!dicBanner[type].isBNLoading)
            {
                BNTryLoad = 0;
                tryLoadBanner(type);
            }
            else
            {
                SdkUtil.logd("unity loadBanner isloading");
            }
        }
        public override bool showBanner(int type, int pos, int width, AdCallBack cb, float dxCenter)
        {
            SdkUtil.logd("unity showBanner");
            dicBanner[type].isBNShow = true;
#if ENABLE_ADS_UNITY
            if (isBNloaded)
            {
                SdkUtil.logd("unity showBanner 1");
                BannerPosition adposbn;
                if (posBanner == 0)
                {
                    adposbn = BannerPosition.TOP_CENTER;
                }
                else if (posBanner == 1)
                {
                    adposbn = BannerPosition.BOTTOM_CENTER;
                }
                else
                {
                    adposbn = BannerPosition.BOTTOM_CENTER;
                }
                Advertisement.Banner.SetPosition(adposbn);
                Advertisement.Banner.Show(bannerId);
                return true;
            }
            else
            {
                SdkUtil.logd("unity showBanner 10");
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
                SdkUtil.logd("unity tryLoadBanner not enable");
                cb(AD_State.AD_LOAD_FAIL);
            }
            return false;
        }
        public override void hideBanner(int type)
        {
            SdkUtil.logd("unity hideBanner");
            dicBanner[type].isBNShow = false;
            dicBanner[type].isBNloaded = false;
#if ENABLE_ADS_UNITY
            Advertisement.Banner.Hide();
#endif
        }
        public override void destroyBanner(int type)
        {
            hideBanner(0);
        }

        //
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
                SdkUtil.logd("unity tryLoadBanner not enable");
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
#if ENABLE_ADS_UNITY
            isFullLoaded = false;123
#endif
            }
        }
        public override int getFullLoaded(bool isOpenads)
        {
#if ENABLE_ADS_UNITY
            if (isFullLoaded || Advertisement.IsReady(fullId))
            {
                return 1;
            }
            else 
            {
                return 0;
            }
#endif
            return 0;
        }
        protected override void tryLoadFull(bool isSplash)
        {
#if ENABLE_ADS_UNITY
            SdkUtil.logd("unity RequestInterstitial =" + fullId);
            isFullLoading = true;
            isFullLoaded = false;
            StartCoroutine(waitFullReady());
#else
            if (cbFullLoad != null)
            {
                var tmpcb = cbFullLoad;
                cbFullLoad = null;
                tmpcb(AD_State.AD_LOAD_FAIL);
            }
#endif
        }
        IEnumerator waitFullReady()
        {
#if ENABLE_ADS_UNITY
            int count = 0;
            while (!Advertisement.IsReady(fullId) && count < 6)
            {
                yield return new WaitForSeconds(0.5f);
                count++;
            }
            
            isFullLoading = false;
            if (cbFullLoad != null)
            {
                if (Advertisement.IsReady(fullId))
                {
                    isFullLoaded = true;
                    var tmpcb = cbFullLoad;
                    cbFullLoad = null;
                    tmpcb(AD_State.AD_LOAD_OK);
                }
                else
                {
                    isFullLoaded = false;
                    var tmpcb = cbFullLoad;
                    cbFullLoad = null;
                    tmpcb(AD_State.AD_LOAD_FAIL);
                }
            }
            SdkUtil.logd("unity waitFullReady isFullLoaded= " + isFullLoaded);
#endif
            yield break;
        }
        public override void loadFull(bool isSplash, AdCallBack cb)
        {
            SdkUtil.logd("unity loadFull");
            cbFullLoad = cb;
            if (!isFullLoading && !isFullLoaded)
            {
                FullTryLoad = 0;
                tryLoadFull(isSplash);
            }
            else
            {
                SdkUtil.logd("unity loadFull isloading or isloaded");
            }
        }
        public override bool showFull(bool isOpenads, AdCallBack cb)
        {
            SdkUtil.logd("unity showFull");
            cbFullShow = null;
            if (getFullLoaded(isOpenads) > 0)
            {
                FullTryLoad = 0;
                isFullLoaded = false;
#if ENABLE_ADS_UNITY
                cbFullShow = cb;
                Advertisement.Show(fullId);
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
#if ENABLE_ADS_UNITY
                isGiftLoaded = false;123
#endif
            }
        }
        public override bool getGiftLoaded()
        {
#if ENABLE_ADS_UNITY
            return (isGiftLoaded || Advertisement.IsReady(giftId));
#endif
            return false;
        }
        protected override void tryloadGift()
        {
#if ENABLE_ADS_UNITY
            SdkUtil.logd("unity Request gift =" + giftId);
            isGiftLoading = true;
            isGiftLoaded = false;
            StartCoroutine(waitGiftReady());
#else
            if (cbGiftLoad != null)
            {
                cbGiftLoad(AD_State.AD_LOAD_FAIL);
                cbGiftLoad = null;
            }
#endif
        }
        IEnumerator waitGiftReady()
        {
#if ENABLE_ADS_UNITY
            int count = 0;
            while (!Advertisement.IsReady(giftId) && count < 6)
            {
                yield return new WaitForSeconds(0.5f);
                count++;
            }
            isGiftLoading = false;
            if (cbGiftLoad != null)
            {
                if (Advertisement.IsReady(giftId))
                {
                    isGiftLoaded = true;
                    var tmpcb = cbGiftLoad;
                    cbGiftLoad = null;
                    tmpcb(AD_State.AD_LOAD_OK);
                }
                else
                {
                    isGiftLoaded = false;
                    var tmpcb = cbGiftLoad;
                    cbGiftLoad = null;
                    tmpcb(AD_State.AD_LOAD_FAIL);
                }
            }
            SdkUtil.logd("unity waitGiftReady = " + isGiftLoaded);
#endif
            yield break;
        }
        public override void loadGift(AdCallBack cb)
        {
            SdkUtil.logd("unity loadGift");
            cbGiftLoad = cb;
            if (!isGiftLoading && !isGiftLoaded)
            {
                GiftTryLoad = 0;
                tryloadGift();
            }
            else
            {
                SdkUtil.logd("unity loadGift isloading or isloaded");
            }
        }
        public override bool showGift(AdCallBack cb)
        {
            SdkUtil.logd("unity showGift");
            cbGiftShow = null;
            if (getGiftLoaded())
            {
                GiftTryLoad = 0;
                isGiftLoaded = false;
#if ENABLE_ADS_UNITY
                cbGiftShow = cb;
                Advertisement.Show(giftId);
                return true;
#endif
            }
            return false;
        }

        //-------------------------------------------------------------------------------
#if ENABLE_ADS_UNITY

        // Implement IUnityAdsListener interface methods:
        public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
        {
            SdkUtil.logd("unity OnUnityAdsDidFinish placementId =" + placementId + ", showResult=" + showResult);
            // Define conditional logic for each ad completion status:
            if (placementId == giftId)
            {
                if (showResult == ShowResult.Finished)
                {
                    // Reward the user for watching the ad to completion.
                    if (cbGiftShow != null)
                    {
                        SdkUtil.logd("unity rw _cbAD != null");
                        AdCallBack tmpcb = cbGiftShow;
                        AdsProcessCB.Instance().Enqueue(() => { tmpcb(AD_State.AD_REWARD_OK); });
                    }
                } 
                else if (showResult == ShowResult.Skipped)
                {
                    // Do not reward the user for skipping the ad.
                }
                else if (showResult == ShowResult.Failed)
                {
                    // Debug.LogWarning("The ad did not finish due to an error.");
                }
                if (cbGiftShow != null)
                {
                    SdkUtil.logd("unity rw HandleRewardBasedVideoClosed2");
                    AdCallBack tmpcb = cbGiftShow;
                    AdsProcessCB.Instance().Enqueue(() => { 
                        tmpcb(AD_State.AD_CLOSE); 
                        onGiftClose();
                    });
                } 
                else 
                {
                    AdsProcessCB.Instance().Enqueue(() => {
                            onGiftClose();
                            });
                }

                isRewardCom = false;
                GiftTryLoad = 0;
                cbGiftShow = null;
            }
            else if (placementId == fullId) 
            {
                if (cbFullShow != null)
                {
                    SdkUtil.logd("unity full close2");
                    AdCallBack tmpcb = cbFullShow;
                    AdsProcessCB.Instance().Enqueue(() => { 
                                tmpcb(AD_State.AD_CLOSE); 
                                onFullClose();
                                });
                }
            }
        }

        public void OnUnityAdsReady(string placementId)
        {
            // If the ready Placement is rewarded, show the ad:
            SdkUtil.logd("unity OnUnityAdsReady placementId=" + placementId);
        }

        public void OnUnityAdsDidError(string message)
        {
            // Log the error.
            SdkUtil.logd("unity OnUnityAdsDidError msg=" + message);
        }

        public void OnUnityAdsDidStart(string placementId)
        {
            // Optional actions to take when the end-users triggers an ad.
            SdkUtil.logd("unity OnUnityAdsDidStart placementId=" + placementId);
        }

#endif

    }
}