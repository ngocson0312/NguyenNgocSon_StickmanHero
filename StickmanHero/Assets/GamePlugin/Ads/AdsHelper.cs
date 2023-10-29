using System;
using System.Collections;
using System.Collections.Generic;
#if ENABLE_ADJUST
#endif
using UnityEngine;
using UnityEngine.UI;

namespace mygame.sdk
{
    public delegate void AdCallBack(AD_State state);

    public enum AD_State
    {
        AD_NONE,
        AD_LOAD_FAIL,
        AD_LOAD_OK,
        AD_LOAD_OK_WAIT,
        AD_SHOW_FAIL,
        AD_SHOW,
        AD_SHOW_MISS_CB,
        AD_REWARD_FAIL,
        AD_REWARD_OK,
        AD_CLOSE
    }

    public enum AD_TYPE
    {
        TYPE_ADMODE,
        TYPE_FB,
        TYPE_IRON,
        TYPE_UNITY,
        TYPE_START
    }

    public enum AD_BANNER_POS
    {
        TOP = 0,
        BOTTOM
    }

    public enum ADJUST_ADS_event
    {
        Event_ShowFUll = 0,
        Event_ShowGift
    }

    public class AdsHelper : MonoBehaviour
    {
        //-------------AppOpenAd-------------
        [Header("App Open Ad")]
        public string AdmobAppID4Android;
        public string AdmobAppID4iOS;
        public string OpenAdIdAndroid;
        public string OpenAdIdiOS;
        //----------------------------
        [Header("App Open Ad add gradle")]
        public bool iSCommentAndroidSupport = false;
        public string gradleVersion = "4.0.1";
        public string AdsVersion;
        public string AdsIdentifierVersion;
        private int LowPoint4LogVipAds = 15;
        private int StepPoint4LogVipAds = 5;
        public static AdsHelper Instance;
        private AdCallBack _cbFull;
        private AdCallBack _cbGift;
        [Header("--------------------||-----------------")]
        public AdsBase adsAdmob;
        public AdsBase adsAdmobMy;
        public AdsBase adsAdmobLower;
        public AdsBase adsAdmobMyLower;
        public AdsBase adsFb;
        public AdsBase adsUnity;
        public AdsBase adsIron;
        public AdsBase adsApplovinMax;
        public AdsBase adsApplovinMaxLow;
        public AdsBase adsMopb;

        private List<AdsBase> listStepShowNative;

        private Dictionary<int, AdsBase> listAds = new Dictionary<int, AdsBase>();
        public IDictionary<string, ObjectAdsCf> adsConfigAllRegion { get; set; }
        public ObjectAdsCf currConfig;

        [SerializeField] Image ImgBgBanner;
        [SerializeField] GameObject bgFullGift;
        private bool isShowBanner = false;
        private int currBannerShow = -1;
        private AD_BANNER_POS bnLocation;
        private int idxBNLoad;
        private int stepBNLoad;
        private int countBNLoad;
        private int idxBNShowCircle;
        public int typeBn;
        public int typeBnShowing;
        public App_Open_ad_Orien bnOrien { get; private set; }
        private bool isCallReloadBanner = false;
        private int statusLoadBanner = 0;
        private bool isLoadBannerok = false;
        private long tbnLoadFail = 0;
        private long tbnLoadOk = 0;
        private int bannerWidth;
        private float dxCenter;
        private AdCallBack _cbBNLoad;
        private AdCallBack _cbBNShow;
        private List<AdsBase> listStepShowBNCircle;
        private List<AdsBase> listStepShowBNRe;
        private string imgBanner = "";
        private string actionBanner = "";
        public int stateMybanner = 0;
        public bool isShowBNAdsMob { get; set; }

        private Rect posNative;
        private bool isShowNatie = false;
        private int idxNative = 0;
        //-------------full------------------
        private bool fullIsloadWhenErr = false;
        private long tFullShow;
        private int deltaloverCountFull = 100;
        private int countFullShowOfDay;
        private int idxFullLoad;
        private int stepFullLoad;
        private int countFullLoad;
        private int idxFullShowCircle;
        public bool isFullLoadStart = false;
        public bool isFullLoadWhenClose = false;
        private bool isFullCallMissCB = false;
        public int level4ApplovinFull { get; private set; }
        private AdCallBack _cbFullLoad;
        private AdCallBack _cbFullShow;
        private List<AdsBase> listStepShowFullCircle;
        private List<AdsBase> listStepShowFullRe;
        public bool isShowFulled { get; private set; }

        //-------------gift----------------------------
        private bool giftIsloadWhenErr = false;
        private long tGiftShow;
        private int countGiftShowOfDay;
        private int idxGiftLoad;
        private int stepGiftLoad;
        private int countGiftLoad;
        private int idxGiftShowCircle;
        public bool isGiftLoadStart = false;
        public bool isGiftLoadWhenClose = false;
        private bool isGiftCallMissCB = false;
        private AdCallBack _cbGiftLoad;
        private AdCallBack _cbGiftShow;
        private List<AdsBase> listStepShowGiftCircle;
        private List<AdsBase> listStepShowGiftRe;

        public bool isAdsShowing { get; private set; }
        public long tShowAdsCheckContinue = 0;
        private bool isPauseAudio = false;
        public int statuschekAdsFullErr = 0;
        public int statuschekAdsGiftErr = 0;
        public int level4ApplovinGift { get; private set; }

        public bool isChangeToStaticAds { get; private set; }
        public int levelCurr4Full { get; private set; }
        public int levelCurr4Gift { get; private set; }
        public int cfStatusRemoveAdsInterval { get; set; }
        private bool isinitafteridfa = true;

        private static int countTotalShowAds = 0;
        private int typeFullGift = 0;
        private bool isInitSuc = false;
        private bool isWaitShowBanner = false;
        private long timeOpenGame = 0;
        private int countTryCheckRemoveInterval = 0;

#if UNITY_EDITOR
        [Header("Ad Editor")]
        public GameObject adsEditorPrefab;
        public AdsEditorCtr adsEditorCtr;
#endif


        public static int isRemoveAds
        {
            get
            {
                return PlayerPrefs.GetInt("key_mem_rm_ads", 0);
            }
        }
        public static void setRemoveAds()
        {
            Debug.Log("mysdk: ads helper setRemoveAds");
            PlayerPrefs.SetInt("key_mem_rm_ads", 1);
            GameHelper.setRemoveAds4OpenAds(1);
            if (Instance != null)
            {
                Instance.hideBanner(0);
                Instance.hideBanner(1);
            }
        }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;

                if (adsAdmob != null)
                {
#if USE_ADSMOB_MY
                    listAds.Add(0, adsAdmobMy);
#else
                    listAds.Add(0, adsAdmob);
#endif
                }
                if (adsAdmobLower != null)
                {
#if USE_ADSMOB_MY
                    listAds.Add(10, adsAdmobMyLower);
#else
                    listAds.Add(10, adsAdmobLower);
#endif
                }
                if (adsFb != null)
                {
                    listAds.Add(1, adsFb);
                }
                if (adsUnity != null)
                {
                    listAds.Add(2, adsUnity);
                }
                if (adsIron != null)
                {
                    listAds.Add(3, adsIron);
                }
                if (adsApplovinMax != null)
                {
                    listAds.Add(6, adsApplovinMax);
                }
                if (adsApplovinMaxLow != null)
                {
                    listAds.Add(11, adsApplovinMaxLow);
                }
                if (adsMopb != null)
                {
                    listAds.Add(7, adsMopb);
                }

                cfStatusRemoveAdsInterval = 1;

                int nop = PlayerPrefs.GetInt("key_count_open_game", 0);
                PlayerPrefs.SetInt("key_count_open_game", nop++);
                countTotalShowAds = PlayerPrefs.GetInt("mem_count_to_show", 0);

                adsConfigAllRegion = new Dictionary<string, ObjectAdsCf>();
                currConfig = new ObjectAdsCf();
                currConfig.loadFromPlayerPrefs();
                adsConfigAllRegion.Add(currConfig.countrycode, currConfig);
                checkUseAds();

                typeBnShowing = -1;
                setIsAdsShowing(false);
                isChangeToStaticAds = false;
                isShowBNAdsMob = false;
                isShowFulled = false;
                isInitSuc = false;
                isWaitShowBanner = false;
                timeOpenGame = PlayerPrefs.GetInt("mem_topen_game", -1);
                if (timeOpenGame < 0)
                {
                    timeOpenGame = SdkUtil.systemCurrentMiliseconds();
                    PlayerPrefs.SetInt("mem_topen_game", (int)(timeOpenGame / 1000));
                }
                else
                {
                    timeOpenGame = timeOpenGame * 1000;
                }

                listStepShowBNCircle = new List<AdsBase>();
                listStepShowBNRe = new List<AdsBase>();
                listStepShowNative = new List<AdsBase>();
                listStepShowFullCircle = new List<AdsBase>();
                listStepShowFullRe = new List<AdsBase>();
                listStepShowGiftCircle = new List<AdsBase>();
                listStepShowGiftRe = new List<AdsBase>();

                countFullShowOfDay = currConfig.fullTotalOfday;
                countGiftShowOfDay = currConfig.giftTotalOfday;

                if (!GameHelper.requestIDFA())
                {
                    isinitafteridfa = true;
                }
                else
                {
                    isinitafteridfa = false;
                }
                if (isRemoveAds == 1)
                {
                    GameHelper.setRemoveAds4OpenAds(1);
                }
            }
            else
            {

            }
        }

        private void Start()
        {
#if UNITY_EDITOR
            var ob = Instantiate(adsEditorPrefab, Vector3.zero, Quaternion.identity, SDKManager.Instance.transform);
            adsEditorCtr = ob.GetComponent<AdsEditorCtr>();
#endif
            idxBNLoad = 0;
            stepBNLoad = 0;
            countBNLoad = 0;
            idxBNShowCircle = 0;
            typeBnShowing = -1;
            typeBn = -1;

            idxFullLoad = 0;
            countFullLoad = 0;
            stepFullLoad = 0;
            idxFullShowCircle = 0;

            idxGiftLoad = 0;
            countGiftLoad = 0;
            stepGiftLoad = 0;
            idxGiftShowCircle = 0;

            checkTotalShowGiftFull();

            initListBN();
            initListNative();
            initListFull();
            initListGift();

            if (isinitafteridfa)
            {
                Debug.Log("mysdk: adshelper start isinitafteridfa");
                isinitafteridfa = false;
                initAds();
            }
            StartCoroutine(waitFlagInitSuc());
            countTryCheckRemoveInterval = 0;
            checkRemoveAdsInterval();
        }

        void checkRemoveAdsInterval()
        {
            int tinval = PlayerPrefs.GetInt("ads_remove_inval", 0);
            if (tinval > 0)
            {
                countTryCheckRemoveInterval++;
                Myapi.ApiManager.Instance.getTimeOnline((status, time) =>
                {
                    if (status)
                    {
                        SDKManager.Instance.timeOnline = (int)(time / 60000);
                        SDKManager.Instance.timeWhenGetOnline = (int)(SdkUtil.systemCurrentMiliseconds() / 60000);
                    }
                    else
                    {
                        if (countTryCheckRemoveInterval <= 3)
                        {
                            Invoke("checkRemoveAdsInterval", 60);
                        }
                    }
                });
            }
        }

        public bool isRemoveAdsInterval()
        {
            int tinval = PlayerPrefs.GetInt("ads_remove_inval", 0);
            if (tinval > 0 && SDKManager.Instance.timeOnline > 0 && cfStatusRemoveAdsInterval == 1)
            {
                long t = SdkUtil.systemCurrentMiliseconds() / 60000;
                if (t > SDKManager.Instance.timeWhenGetOnline)
                {
                    long tbg = PlayerPrefs.GetInt("ads_remove_inval_tbegin", 0);
                    long dt = tinval + tbg - SDKManager.Instance.timeOnline;
                    if (dt > 0)
                    {
                        if ((t - SDKManager.Instance.timeWhenGetOnline) < dt)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public void initAds()
        {
            Debug.Log("mysdk: adshelper init ads");
            foreach (var item in listAds)
            {
                item.Value.InitAds();
            }
            StartCoroutine(loadStart());
        }
        IEnumerator loadStart()
        {
            if (isShowOpenAds(true) > 0)
            {
                GameHelper.Instance.loadAppOpenAdFull4First();
            }
            yield return new WaitForSeconds(1.1f);
            if (isFullLoadStart)
            {
                loadFull4ThisTurn(false, 99, false);
            }
            if (isGiftLoadStart)
            {
                loadGift4ThisTurn(99, null);
            }
        }

        IEnumerator waitFlagInitSuc()
        {
            yield return new WaitForSeconds(7);
            onAdsInitSuc();
        }

        void showTransFullGift()
        {
            bgFullGift.SetActive(false);
            bgFullGift.transform.GetChild(0).gameObject.SetActive(false);
            AdsProcessCB.Instance().Enqueue(() =>
            {
                if (bgFullGift.activeInHierarchy)
                {
                    bgFullGift.transform.GetChild(0).gameObject.SetActive(true);
                }
            }, 1.0f);
        }

        public void onAdsInitSuc()
        {
            isInitSuc = true;
            if (isWaitShowBanner)
            {
                isWaitShowBanner = false;
                showBanner(bnLocation, bnOrien, bannerWidth, dxCenter);
            }
        }

        private void checkResumeAudio()
        {
#if UNITY_IOS || UNITY_IPHONE
            if (isPauseAudio)
            {
                SdkUtil.logd($"ads helper checkResumeAudio");
                isPauseAudio = false;
                AudioListener.pause = false;
            }
#endif
        }
        public void reinit()
        {
            try
            {
                if (listAds != null && listStepShowFullCircle != null && listStepShowFullRe != null)
                {
                    initListBN();
                    initListNative();
                    initListFull();
                    initListGift();
                }
            }
            catch (Exception)
            {

            }
        }
        public void removeAdsWithTimeInterval(int timeIntervalInHour, Action<bool> result)
        {
            Myapi.ApiManager.Instance.getTimeOnline((status, time) =>
            {
                result?.Invoke(status);
                if (status)
                {
                    SDKManager.Instance.timeOnline = (int)(time / 60000);
                    SDKManager.Instance.timeWhenGetOnline = (int)(SdkUtil.systemCurrentMiliseconds() / 60000);
                    PlayerPrefs.SetInt("ads_remove_inval", timeIntervalInHour * 60);
                    PlayerPrefs.SetInt("ads_remove_inval_tbegin", SDKManager.Instance.timeOnline);
                }
            });
        }
        public void setStartTimeNoAds()
        {
            long ts = SdkUtil.systemCurrentMiliseconds() / 60000;
            PlayerPrefs.SetInt("time_start_noads_fi", (int)ts);
        }
        public bool isNoAdsTime()
        {
            long tcurr = SdkUtil.systemCurrentMiliseconds() / 60000;
            int ts = PlayerPrefs.GetInt("time_start_noads_fi", 0);
            if ((tcurr - ts) <= 240)
            {
                Debug.Log($"mysdk: isNoAdsTime true");
                return true;
            }

            return false;
        }

        private void setIsAdsShowing(bool isshow)
        {
            isAdsShowing = isshow;
            if (GameHelper.Instance != null)
            {
                GameHelper.Instance.setFlagAdShowing();
            }
        }

        public void clearCurrAds()
        {
            foreach (var item in listAds)
            {
                item.Value.clearCurrFull();
                item.Value.clearCurrGift();
            }
        }

        public int isShowOpenAds(bool isShowFirst)
        {
            int cfcountopenshow = PlayerPrefs.GetInt("cf_open_ad_showat", -1);
            int countopenshow = PlayerPrefs.GetInt("mem_gl_count_play", 0);
            int cflvshow = PlayerPrefs.GetInt("cf_open_ad_level_show", 3);
            int cfFirst = PlayerPrefs.GetInt("cf_open_ad_show_firstopen", 0);
            Debug.Log($"mysdk: isShowOpenAds cfcountopenshow={cfcountopenshow}, countopenshow={countopenshow}, cflv={cflvshow}, lv={GameRes.LevelCommon()}, cfshowfirst={cfFirst}, isfirst={isShowFirst}");
            if (cfcountopenshow > 0 && countopenshow >= cfcountopenshow && GameRes.LevelCommon() >= cflvshow)
            {
                if (isShowFirst)
                {
                    if (cfFirst > 0)
                    {
                        Debug.Log($"mysdk: isShowOpenAds first true");
                        return cfFirst;
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    Debug.Log($"mysdk: isShowOpenAds true");
                    return 1;
                }
            }
            else
            {
                return 0;
            }
        }
        private void checkUseAds()
        {

        }
        private void checkLogVipAds()
        {
            int countfull4point = PlayerPrefs.GetInt("count_full_4_point", 0);
            int countGift4point = PlayerPrefs.GetInt("count_gift_4_point", 0);
            int point = countfull4point * 2 + countGift4point * 3;
            if (point > LowPoint4LogVipAds)
            {
                int lvvip = point / StepPoint4LogVipAds;
                point = lvvip * StepPoint4LogVipAds;
                int pointlastlog = PlayerPrefs.GetInt("point_last_log_vipads", 0);
                if (pointlastlog != point)
                {
                    PlayerPrefs.SetInt("point_last_log_vipads", point);
                    string eventname = $"SR_{point:000}";
                    FIRhelper.logEvent(eventname);
                }
            }
        }

        private void initListBN()
        {
            bool hasAdsEnable = false;
            foreach (var item in listAds)
            {
                if (item.Value.isEnable)
                {
                    hasAdsEnable = true;
                    break;
                }
            }
            listStepShowBNCircle.Clear();
            string steplog = "";
            for (int i = 0; i < currConfig.bnStepShowCircle.Count; i++)
            {
                if (listAds.ContainsKey(currConfig.bnStepShowCircle[i]))
                {
                    if (!hasAdsEnable || listAds[currConfig.bnStepShowCircle[i]].isEnable)
                    {
                        listStepShowBNCircle.Add(listAds[currConfig.bnStepShowCircle[i]]);
                        steplog += $"{currConfig.bnStepShowCircle[i]},";
                    }
                }
            }
            if (listStepShowBNCircle.Count == 0)
            {
                foreach (var item in listAds)
                {
                    if (item.Value.isEnable)
                    {
                        listStepShowBNCircle.Add(item.Value);
                        steplog += $"{item.Value.adsType},";
                    }
                }
            }

            listStepShowBNRe.Clear();
            for (int i = 0; i < currConfig.bnStepShowRe.Count; i++)
            {
                if (listAds.ContainsKey(currConfig.bnStepShowRe[i]))
                {
                    if (!hasAdsEnable || listAds[currConfig.bnStepShowRe[i]].isEnable)
                    {
                        listStepShowBNRe.Add(listAds[currConfig.bnStepShowRe[i]]);
                        steplog += $"_{currConfig.bnStepShowRe[i]}";
                    }
                }
            }
            if (idxBNShowCircle >= listStepShowBNCircle.Count)
            {
                idxBNShowCircle = 0;
            }
            Debug.Log("mysdk: adshelper listbn=" + steplog);

            initMyBanner();
        }

        public void initECPMFloorAdMobMy()
        {
            if (adsAdmobMy != null)
            {
                ((AdsAdmobMy)adsAdmobMy).initBanner();
                ((AdsAdmobMy)adsAdmobMy).initFull();
            }
        }
        private void initMyBanner()
        {
            string cfmybn = PlayerPrefs.GetString("key_my_banner", "");
            if (cfmybn.Length > 5)
            {
                string[] arrcfmbn = cfmybn.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                if (arrcfmbn != null && arrcfmbn.Length >= 2)
                {
                    imgBanner = arrcfmbn[0];
                    actionBanner = arrcfmbn[1];
                }
            }
        }
        private void initListNative()
        {
            listStepShowNative.Clear();
            for (int i = 0; i < currConfig.nativeStepShow.Count; i++)
            {
                if (listAds.ContainsKey(currConfig.nativeStepShow[i]))
                {
                    listStepShowNative.Add(listAds[currConfig.nativeStepShow[i]]);
                }
            }
            //vvvvv
            listStepShowNative.Clear();
            listStepShowNative.Add(listAds[0]);
        }
        private void initListFull()
        {
            bool hasAdsEnable = false;
            foreach (var item in listAds)
            {
                if (item.Value.isEnable)
                {
                    hasAdsEnable = true;
                    break;
                }
            }
            listStepShowFullCircle.Clear();
            AdsBase tmpads = null;
            string steplog = "";
            if (isChangeToStaticAds && listAds[0].isEnable)
            {
                Debug.Log("mysdk: ads helper add static Ads");
                listStepShowFullCircle.Add(listAds[0]);
                steplog += $"0,";
            }
            for (int i = 0; i < currConfig.fullStepShowCircle.Count; i++)
            {
                if (listAds.ContainsKey(currConfig.fullStepShowCircle[i]))
                {
                    if (!hasAdsEnable || listAds[currConfig.fullStepShowCircle[i]].isEnable)
                    {
                        if (isChangeToStaticAds && listAds[0].isEnable)
                        {
                            if (tmpads == null && currConfig.fullStepShowCircle[i] != 0)
                            {
                                tmpads = listAds[currConfig.fullStepShowCircle[i]];
                            }
                        }
                        else
                        {
                            listStepShowFullCircle.Add(listAds[currConfig.fullStepShowCircle[i]]);
                            steplog += $"{currConfig.fullStepShowCircle[i]},";
                        }
                    }
                }
            }
            if (listStepShowFullCircle.Count == 0)
            {
                foreach (var item in listAds)
                {
                    if (item.Value.isEnable)
                    {
                        listStepShowFullCircle.Add(item.Value);
                        steplog += $"{item.Value.adsType},";
                    }
                }
            }

            listStepShowFullRe.Clear();
            if (tmpads != null)
            {
                listStepShowFullRe.Add(tmpads);
                steplog += $"_{tmpads.adsType}";
            }
            bool isAdmoblowadd = false;
            for (int i = 0; i < currConfig.fullStepShowRe.Count; i++)
            {
                if (listAds.ContainsKey(currConfig.fullStepShowRe[i]))
                {
                    if (!hasAdsEnable || listAds[currConfig.fullStepShowRe[i]].isEnable)
                    {
                        if (listAds[currConfig.fullStepShowRe[i]].adsType == 10)
                        {
                            isAdmoblowadd = true;
                        }
                        listStepShowFullRe.Add(listAds[currConfig.fullStepShowRe[i]]);
                        steplog += $"_{currConfig.fullStepShowRe[i]}";
                    }
                }
            }
            if (currConfig.fullLoadAdsMobStatic == 1)
            {
#if ENABLE_ADS_ADMOB && USE_ADSMOB_MY
                if (!isAdmoblowadd && listAds.ContainsKey(10))
                {
#if UNITY_IOS || UNITY_IPHONE
                    if (listAds[10].ios_full_id != null && listAds[10].ios_full_id.Contains("ca-app-pub"))
                    {
                        listStepShowFullRe.Add(listAds[10]);
                        steplog += $"_10";
                    }
#elif UNITY_ANDROID
                    if (listAds[10].android_full_id != null && listAds[10].android_full_id.Contains("ca-app-pub"))
                    {
                        listStepShowFullRe.Add(listAds[10]);
                        steplog += $"_10";
                    }
#endif
                }
#endif
            }
            Debug.Log("mysdk: adshelper listfull=" + steplog);
            if (idxFullShowCircle >= listStepShowFullCircle.Count)
            {
                idxFullShowCircle = 0;
            }
        }
        private void initListGift()
        {
            bool hasAdsEnable = false;
            foreach (var item in listAds)
            {
                if (item.Value.isEnable)
                {
                    hasAdsEnable = true;
                    break;
                }
            }
            string steplog = "";
            listStepShowGiftCircle.Clear();
            for (int i = 0; i < currConfig.giftStepShowCircle.Count; i++)
            {
                if (listAds.ContainsKey(currConfig.giftStepShowCircle[i]))
                {
                    if (!hasAdsEnable || listAds[currConfig.giftStepShowCircle[i]].isEnable)
                    {
                        listStepShowGiftCircle.Add(listAds[currConfig.giftStepShowCircle[i]]);
                        steplog += $"{currConfig.giftStepShowCircle[i]},";
                    }
                }
            }
            if (listStepShowGiftCircle.Count == 0)
            {
                foreach (var item in listAds)
                {
                    if (item.Value.isEnable)
                    {
                        listStepShowGiftCircle.Add(item.Value);
                        steplog += $"{item.Value.adsType},";
                    }
                }
            }

            listStepShowGiftRe.Clear();
            for (int i = 0; i < currConfig.giftStepShowRe.Count; i++)
            {
                if (listAds.ContainsKey(currConfig.giftStepShowRe[i]))
                {
                    if (!hasAdsEnable || listAds[currConfig.giftStepShowRe[i]].isEnable)
                    {
                        listStepShowGiftRe.Add(listAds[currConfig.giftStepShowRe[i]]);
                        steplog += $"_{currConfig.giftStepShowRe[i]}";
                    }
                }
            }
            Debug.Log("mysdk: adshelper listgift=" + steplog);
            if (idxGiftShowCircle >= listStepShowGiftCircle.Count)
            {
                idxGiftShowCircle = 0;
            }
        }
        public void configWithRegion()
        {
            // if (adsConfigAllRegion.ContainsKey(GameHelper.CountryDefault))
            // {
            //     ObjectAdsCf cgdf = adsConfigAllRegion[GameHelper.CountryDefault];
            //     foreach (var item in adsConfigAllRegion)
            //     {
            //         if (!item.Key.Contains(GameHelper.CountryDefault))
            //         {
            //             item.Value.coppyFromOther(cgdf);
            //         }
            //     }
            // }

            Debug.Log("mysdk: ads helper cf ads=" + GameHelper.Instance.countryCode);
            if (adsConfigAllRegion.ContainsKey(GameHelper.Instance.countryCode))
            {
                currConfig = adsConfigAllRegion[GameHelper.Instance.countryCode];
                currConfig.saveAllConfig();
                initListBN();
                initListNative();
                initListFull();
                initListGift();
                checkUseAds();
            }
            else
            {
                Debug.Log("mysdk: ads helper cf2 ads=default");
                currConfig = adsConfigAllRegion[GameHelper.CountryDefault];
                currConfig.saveAllConfig();
                initListBN();
                initListNative();
                initListFull();
                initListGift();
                checkUseAds();
            }
            AdsHelper.Instance.initECPMFloorAdMobMy();
            if (currConfig.OpenAdShowat != -1)
            {
                string adopenid = "";
#if UNITY_IOS || UNITY_IPHONE
                adopenid = OpenAdIdiOS;
#elif UNITY_ANDROID
                adopenid = OpenAdIdAndroid;
#endif
                GameHelper.Instance.configAppOpenAd(currConfig.OpenAdShowtype, currConfig.OpenAdShowat, currConfig.OpenAdIsShowFirstOpen, currConfig.OpenAdDelTimeOpen, adopenid);
            }
        }
        private void checkTotalShowGiftFull()
        {
            countFullShowOfDay = PlayerPrefs.GetInt("mem_ads_count_showFull", currConfig.fullTotalOfday);
            countGiftShowOfDay = PlayerPrefs.GetInt("mem_ads_count_showGift", currConfig.giftTotalOfday);

            if (SDKManager.Instance.flagNewDay)
            {
                Debug.Log("mysdk: ads helper reset TotalShowGift");
                countGiftShowOfDay = currConfig.giftTotalOfday;
                PlayerPrefs.SetInt("mem_ads_count_showGift", countGiftShowOfDay);

                //for full
                countFullShowOfDay = currConfig.fullTotalOfday;
                PlayerPrefs.SetInt("mem_ads_count_showFull", countFullShowOfDay);

                //calculate day return game
                int dayreturn = PlayerPrefs.GetInt("analy_pr_dayReturn", -1);
                dayreturn++;
                PlayerPrefs.GetInt("analy_pr_dayReturn", dayreturn);
            }
        }
        private void subCountShowFull()
        {
            if (countFullShowOfDay > 0)
            {
                countFullShowOfDay--;
                PlayerPrefs.SetInt("mem_ads_count_showFull", countFullShowOfDay);
            }
        }
        private void subCountShowGift()
        {
            if (countGiftShowOfDay > 0)
            {
                countGiftShowOfDay--;
                PlayerPrefs.SetInt("mem_ads_count_showGift", countGiftShowOfDay);
            }
        }

        //==========================================================================
        public void showMyBanner(Button mybanner)
        {
            if (mybanner != null)
            {
                mybanner.onClick.RemoveAllListeners();
                mybanner.onClick.AddListener(() =>
                {
                    GameHelper.Instance.gotoLink(actionBanner);
                });
                mybanner.interactable = false;
                if (imgBanner == null || imgBanner.Length < 3)
                {
                    initMyBanner();
                }
                if (stateMybanner >= 0 && imgBanner.Length > 3)
                {
                    ImageLoader.LoadImageSprite(imgBanner, 640, 100, sprite =>
                    {
                        if (sprite != null)
                        {
                            mybanner.GetComponent<Image>().sprite = sprite;
                            mybanner.interactable = true;
                        }
                    });
                }
            }
        }
        public float getBannerSize(int bannersize)
        {
#if (UNITY_IOS || UNITY_IPHONE) && !UNITY_EDITOR
            float iosw = GameHelper.getScreenWidth();
            if (iosw > 0)
            {
                float rw = (Screen.width * (float)bannersize) / iosw;
                return rw;
            }
            else
            {
                return Screen.dpi * (float)bannersize / 160.0f;
            }
#else
            return Screen.dpi * (float)bannersize / 160.0f;
#endif
        }
        public void showBanner(AD_BANNER_POS location, App_Open_ad_Orien orien, int width, float _dxCenter = 0)
        {
            SdkUtil.logd($"ads helper showBanner location={location}, width={width}, dxCenter={_dxCenter}");
            if (isRemoveAds == 1 || isNoAdsTime()) //vvv
            {
                SdkUtil.logd("ads helper showBanner is removee ads");
                ImgBgBanner.gameObject.SetActive(false);
                return;
            }

#if UNITY_ANDROID
            int buildversionallowShowBanner = PlayerPrefs.GetInt("android_build_ver_show_bn", 0);
            if (SdkUtil.getAndroidBuildVersion() < buildversionallowShowBanner)
            {
                ImgBgBanner.gameObject.SetActive(false);
                return;
            }
#endif      
            if (width <= 0)
            {
                ImgBgBanner.gameObject.SetActive(false);
            }
            else
            {
                Debug.Log($"mysdk: ads helper showbanner w={width}, dpi={Screen.dpi}, screen width={Screen.width}");
                ImgBgBanner.gameObject.SetActive(true);
                float wbn = getBannerSize(width) + 4.0f;
                float hbn = getBannerSize(50);
                float pw = wbn / (Screen.width * 2.0f);
                float ph = (hbn + 2.0f) / Screen.height;
                RectTransform rc = ImgBgBanner.GetComponent<RectTransform>();
                if (location == AD_BANNER_POS.TOP)
                {
                    rc.anchorMin = new Vector2(0.5f - pw, 1.0f - ph);
                    rc.anchorMax = new Vector2(0.5f + pw, 1);
                }
                else
                {
                    rc.anchorMin = new Vector2(0.5f - pw, 0);
                    rc.anchorMax = new Vector2(0.5f + pw, ph);
                }
                rc.sizeDelta = new Vector2(0, 0);
                rc.anchoredPosition = Vector2.zero;
                rc.anchoredPosition3D = Vector3.zero;
                ImgBgBanner.gameObject.SetActive(false);//vvv
            }

            typeBn = 0;//vvv
            bnOrien = orien;
            isShowBanner = true;
            bnLocation = location;
            bannerWidth = width;
            dxCenter = _dxCenter;
            if (listStepShowBNCircle.Count == 0 && listStepShowBNRe.Count == 0)
            {
                initListBN();
            }
            if (idxBNShowCircle >= listStepShowBNCircle.Count)
            {
                idxBNShowCircle = 0;
            }
            idxBNLoad = 0;
            if (listStepShowBNCircle.Count > 0)
            {
                stepBNLoad = 0;
                countBNLoad = listStepShowBNCircle.Count;
            }
            else
            {
                stepBNLoad = 1;
                countBNLoad = listStepShowBNRe.Count;
            }
            if (!isInitSuc)
            {
                isWaitShowBanner = true;
                Debug.Log("mysdk: ads helper wait show banner");
                return;
            }
            isWaitShowBanner = false;
            showBNCircle(typeBn);
        }
        private void showBNCircle(int type)
        {
            SdkUtil.logd("ads helper showBNCircle 1");
            AdsBase tmads = null;
            int idxforerr1 = -3;
            int idxforerr2 = -3;
            if (countBNLoad > 0)
            {
                idxforerr1 = -2;
                idxforerr2 = -2;
                if (stepBNLoad == 0)
                {
                    idxforerr1 = -1;
                    if (idxBNShowCircle >= listStepShowBNCircle.Count)
                    {
                        idxBNShowCircle = 0;
                    }
                    int tmpidx = idxBNShowCircle;
                    idxBNShowCircle++;
                    if (idxBNShowCircle >= listStepShowBNCircle.Count)
                    {
                        idxBNShowCircle = 0;
                    }
                    countBNLoad--;
                    if (countBNLoad <= 0)
                    {
                        stepBNLoad = 1;
                        countBNLoad = listStepShowBNRe.Count;
                        idxBNLoad = 0;
                    }
                    SdkUtil.logd("ads helper showBNCircle cir tmpidx=" + tmpidx);
                    tmads = listStepShowBNCircle[tmpidx];
                    idxforerr1 = tmpidx;
                }
                else
                {
                    idxforerr2 = -1;
                    if (listStepShowBNRe.Count > 0)
                    {
                        idxforerr2 = 10;
                        if (idxBNLoad >= listStepShowBNRe.Count)
                        {
                            idxBNLoad = 0;
                        }
                        int tmpidx = idxBNLoad;
                        idxBNLoad++;
                        if (idxBNLoad >= listStepShowBNRe.Count)
                        {
                            idxBNLoad = 0;
                        }
                        SdkUtil.logd("ads helper showBNCircle re tmpidx=" + tmpidx);
                        tmads = listStepShowBNRe[tmpidx];
                        idxforerr2 = tmpidx;
                    }
                    countBNLoad--;
                }
            }
            if (tmads != null)
            {
                SdkUtil.logd("ads helper showBNCircle tmads=" + tmads.adsType);
                AdsBase fbn = tmads;
                if (isShowBNAdsMob && tmads.adsType != 0)
                {
                    SdkUtil.logd("ads helper showBNCircle isShowBNAdsMob");
                    tmads.hideBanner(0);
                    AdsProcessCB.Instance().Enqueue(() =>
                    {
                        if (isShowBanner)
                        {
                            showBNCircle(type);
                        }
                    }, 0.001f);
                }
                else
                {
                    tmads.showBanner(type, (int)bnLocation, bannerWidth, (AD_State state) =>
                    {
                        if (state == AD_State.AD_LOAD_FAIL)
                        {
                            if (isShowBanner)
                            {
                                showBNCircle(type);
                            }
                        }
                        else if (state == AD_State.AD_LOAD_OK)
                        {
                            stateMybanner = -1;
                        }
                        else if (state == AD_State.AD_SHOW)
                        {
                            stateMybanner = -1;
                        }
                    }, dxCenter);
                }
                if (!isCallReloadBanner)
                {
                    isCallReloadBanner = true;
                    tbnLoadOk = SdkUtil.systemCurrentMiliseconds();
                    tbnLoadFail = tbnLoadOk;
                    StartCoroutine(reloadBanner());
                }
            }
            else
            {
                SdkUtil.logd("ads helper err showbanner listStepShowBNCircle.Count=" + listStepShowBNCircle.Count + ", re=" + listStepShowBNRe.Count);
                SdkUtil.logd("ads helper err showbanner idxforerr1=" + idxforerr1 + ", idxforerr2=" + idxforerr2);
                string stepbn = "cir:";
                for (int i = 0; i < listStepShowBNCircle.Count; i++)
                {
                    stepbn += ("" + listStepShowBNCircle[i].adsType + ",");
                }
                stepbn += "#re:";
                for (int i = 0; i < listStepShowBNRe.Count; i++)
                {
                    stepbn += ("" + listStepShowBNRe[i].adsType + ",");
                }
                SdkUtil.logd("ads helper err showbanner stepbn=" + stepbn);
                FIRhelper.logEvent("ErrShowBanner");
            }

            //#endif
        }
        public void onBannerLoadFail(int adsType)
        {
#if ENABLE_MYLOG
            SdkUtil.logd("ads helper onBannerLoadFail 0");
#endif
            if (statusLoadBanner != -1)
            {
                tbnLoadFail = SdkUtil.systemCurrentMiliseconds();
                statusLoadBanner = -1;
            }
        }
        public void onBannerLoadOk(int adsType)
        {
#if ENABLE_MYLOG
            SdkUtil.logd("ads helper onBannerLoadOk 0");
#endif
            tbnLoadOk = SdkUtil.systemCurrentMiliseconds();
            tbnLoadFail = tbnLoadOk;
            statusLoadBanner = 1;
            isLoadBannerok = true;
        }
        private IEnumerator reloadBanner()
        {
            yield return new WaitForSeconds(20);
#if ENABLE_MYLOG
            SdkUtil.logd("ads helper reloadBanner 0");
#endif
            if (Application.internetReachability != NetworkReachability.NotReachable)
            {
#if ENABLE_MYLOG
                SdkUtil.logd("ads helper reloadBanner 01 statusLoadBanner=" + statusLoadBanner);
#endif
                if (statusLoadBanner == -1)
                {
                    long tcu = SdkUtil.systemCurrentMiliseconds();
                    if ((tcu - tbnLoadFail) >= 20000 && !isLoadBannerok)
                    {
#if ENABLE_MYLOG
                        SdkUtil.logd("ads helper reloadBanner 1");
#endif
                        statusLoadBanner = 0;
                        tbnLoadFail = tcu;
                        if (isShowBanner)
                        {
                            SdkUtil.logd("ads helper destroy banner and reshow");
                            destroyBanner(0);
                            showBanner(bnLocation, bnOrien, bannerWidth, dxCenter);
                        }
                    }
                }
                else if (statusLoadBanner == 1)
                {
                    long tcu = SdkUtil.systemCurrentMiliseconds();
                    if ((tcu - tbnLoadOk) > 60000)
                    {
#if ENABLE_MYLOG
                        SdkUtil.logd("ads helper reloadBanner 2");
#endif
                        //destroyBanner(0);
                        statusLoadBanner = 0;
                        tbnLoadFail = tcu;
                        tbnLoadOk = tcu;
                        //showBanner(typeBn, bnLocation);
                    }
                }
            }
            if (isRemoveAds == 0)
            {
                StartCoroutine(reloadBanner());
            }
        }
        public void hideBanner(int type)
        {
            SdkUtil.logd("ads helper hideBanner 1");
            ImgBgBanner.gameObject.SetActive(false);
            isShowBanner = false;
            isWaitShowBanner = false;
            foreach (var ads in listAds)
            {
                if (ads.Value != null)
                {
                    ads.Value.hideBanner(type);
                }
            }
        }
        public void destroyBanner(int type)
        {
            SdkUtil.logd("ads helper hideBanner 1");
            ImgBgBanner.gameObject.SetActive(false);
            isShowBanner = false;
            isWaitShowBanner = false;
            foreach (var ads in listAds)
            {
                if (ads.Value != null)
                {
                    ads.Value.hideBanner(type);
                    ads.Value.destroyBanner(type);
                }
            }
        }

        //----------------------
        public void showNative(Rect pos)
        {
            SdkUtil.logd("ads helper showNative 1");
            if (isRemoveAds == 1) //vvv
            {
                return;
            }

            SdkUtil.logd("ads helper showNative 2");
            if (listStepShowNative.Count == 0)
            {
                initListNative();
            }
            posNative = pos;
            isShowNatie = true;
            idxNative = 0;
            showNativeCircle();
        }
        private void showNativeCircle()
        {
#if !UNITY_EDITOR
            SdkUtil.logd("ads helper showNativeCircle");
            if (listStepShowNative.Count > 0)
            {
                AdsBase ads = null;
                if (idxNative < listStepShowNative.Count)
                {
                    ads = listStepShowNative[idxNative];
                }
                idxNative = 0;
                listStepShowNative.RemoveAt(0);
                if (ads != null)
                {
                    ads.showNative(posNative, (AD_State stateshow) =>
                    {
                        if (stateshow == AD_State.AD_LOAD_FAIL)
                        {
                            if (isShowNatie)
                            {
                                showNativeCircle();
                            }
                        }
                    });
                }
            }
#endif
        }
        public void hideNative()
        {
            isShowNatie = false;
            foreach (var ads in listAds)
            {
                if (ads.Value != null)
                {
                    ads.Value.hideNative();
                }
            }
        }
        //----------------------
        private void checkStaticAdsFull(int lv)
        {
            if (!isChangeToStaticAds)
            {
                if (listAds[0].isEnable && currConfig.fullLoadAdsMobStatic == 1)
                {
                    int lvcf = currConfig.cfLvStaticAds;
                    if (lv <= lvcf && listAds[0].getSplashId() != null && listAds[0].getSplashId().Length > 5)
                    {
                        isChangeToStaticAds = true;
                        initListFull();
                    }
                }
            }
            else
            {
                int lvcf = currConfig.cfLvStaticAds;
                if (lv > lvcf)
                {
                    isChangeToStaticAds = false;
                    initListFull();
                }
            }
        }
        public void loadFull4ThisTurn(bool isSplash, int level, bool ischecknumover = true, AdCallBack cb = null)
        {
#if ENABLE_MYLOG
            SdkUtil.logd("ads helper loadFull4ThisTurn issplash=" + isSplash + ", lv=" + level);
#endif
            checkResumeAudio();
            _cbFullLoad = null;
            if (isRemoveAds == 1 || isNoAdsTime()) //vvv
            {
                SdkUtil.logd("ads helper loadFull4ThisTurn is removee ads");
                if (cb != null)
                {
                    cb(AD_State.AD_LOAD_FAIL);
                }
                return;
            }
            checkStaticAdsFull(level);
            if (ischecknumover && !isSplash)
            {
                int count = deltaloverCountFull + 1;
                int overconfig = currConfig.fullDefaultNumover;
                for (int i = 0; i < currConfig.listIntervalShow.Count; i++)
                {
                    if (currConfig.listIntervalShow[i].startlevel <= level && currConfig.listIntervalShow[i].endLevel >= level)
                    {
                        overconfig = currConfig.listIntervalShow[i].deltal4Show;
                        break;
                    }
                }

                if (count < overconfig)
                {
#if ENABLE_MYLOG
                    SdkUtil.logd("ads helper loadFull4ThisTurn 2 check delta count full");
#endif
                    if (cb != null)
                    {
                        cb(AD_State.AD_LOAD_FAIL);
                    }
                    return;
                }
            }
            long dtstart = (SdkUtil.systemCurrentMiliseconds() - timeOpenGame) * 2;
            if ((level < currConfig.fullLevelStart || dtstart < currConfig.fullTimeStart) && !isSplash)
            {
#if ENABLE_MYLOG
                SdkUtil.logd("ads helper loadFull4ThisTurn lvstart: curr=" + level + ", lvs=" + currConfig.fullLevelStart + ", dtstart=" + dtstart + " dtcf=" + currConfig.fullTimeStart);
#endif
                if (cb != null)
                {
                    cb(AD_State.AD_LOAD_FAIL);
                }
                return;
            }
            if (countFullShowOfDay <= 0)
            {
#if ENABLE_MYLOG
                SdkUtil.logd("ads helper loadFull4ThisTurn over num show of day, to=" + currConfig.fullTotalOfday);
#endif
                if (cb != null)
                {
                    cb(AD_State.AD_LOAD_FAIL);
                }
                return;
            }

            // long t = SdkUtil.systemCurrentMiliseconds();
            // SdkUtil.logd("ads helper loadFull4ThisTurn + d:" + currConfig.fullDeltatime + " del_t:" + (t - tFullShow));
            // if ((t - tFullShow) < (currConfig.fullDeltatime - 300000) && !isSplash)
            // {
            //     if (cb != null)
            //     {
            //         cb(AD_State.AD_LOAD_FAIL);
            //     }
            //     return;
            // }
            // SdkUtil.logd("ads helper loadFull4ThisTurn 2");
            //if (isFull4Show(false)) return;
#if ENABLE_MYLOG
            SdkUtil.logd("ads helper loadFull4ThisTurn load");
#endif
            // if (isChangeToStaticAds) {
            //     isSplash = true;
            // }
            _cbFullLoad = cb;
            if (listStepShowFullCircle.Count == 0 && listStepShowFullRe.Count == 0)
            {
                initListFull();
            }
            if (idxFullShowCircle >= listStepShowFullCircle.Count)
            {
                idxFullShowCircle = 0;
            }

            if (listStepShowFullCircle.Count == 0)
            {
                stepFullLoad = 1;
                idxFullLoad = 0;
                countFullLoad = listStepShowFullRe.Count;
            }
            else
            {
                stepFullLoad = 0;
                idxFullLoad = idxFullShowCircle;
                countFullLoad = listStepShowFullCircle.Count;
            }

            //if (currConfig.fullLoadAdsMobStatic == 1)
            {
                if (adsAdmobMy != null && ((listStepShowFullCircle != null && listStepShowFullCircle.Contains(adsAdmobMy)) || (listStepShowFullRe != null && listStepShowFullRe.Contains(adsAdmobMy))))
                {
                    adsAdmobMy.loadFull(isSplash, null);
                }
                if (isSplash && adsAdmobLower != null && ((listStepShowFullCircle != null && listStepShowFullCircle.Contains(adsAdmobLower)) || (listStepShowFullRe != null && listStepShowFullRe.Contains(adsAdmobLower))))
                {
                    adsAdmobLower.loadFull(isSplash, null);
                }
            }
            if (adsApplovinMaxLow != null && (listStepShowFullCircle.Contains(adsApplovinMaxLow) || listStepShowFullRe.Contains(adsApplovinMaxLow)))
            {
                string adxfullid = adsApplovinMaxLow.getFullId();
                if (adxfullid != null && adxfullid.Length > 3 && currConfig.isLoadMaxLow == 1)
                {
                    adsApplovinMaxLow.loadFull(isSplash, null);
                }
            }

            loadFullCircle(isSplash, level);
        }
        private void loadFullCircle(bool isSplash, int lv)
        {
#if UNITY_EDITOR
            if (!adsEditorCtr.isFullEditorLoading && !adsEditorCtr.isFullEditorLoaded)
            {
                loadFullEditor();
            }
            return;
#endif      
#if ENABLE_MYLOG
            SdkUtil.logd("ads helper loadFullCircle idx=" + idxFullLoad);
#endif
            if (countFullLoad > 0)
            {
                level4ApplovinFull = lv;
                int idxcurr = idxFullLoad;
                idxFullLoad++;
                List<AdsBase> listcurr = null;
                if (stepFullLoad == 0)
                {
                    listcurr = listStepShowFullCircle;
                    if (idxFullLoad >= listStepShowFullCircle.Count)
                    {
                        idxFullLoad = 0;
                    }
                }
                else
                {
                    listcurr = listStepShowFullRe;
                    if (idxFullLoad >= listStepShowFullRe.Count)
                    {
                        idxFullLoad = 0;
                    }
                }
                countFullLoad--;
                if (countFullLoad <= 0 && stepFullLoad == 0)
                {
                    stepFullLoad = 1;
                    idxFullLoad = 0;
                    countFullLoad = listStepShowFullRe.Count;
                }

                if (listcurr != null && listcurr.Count > idxcurr && idxcurr >= 0)
                {
                    bool isNextLoad = false;
                    if (listcurr[idxcurr].adsType == 6 && currConfig.stateShowAppLlovin == 2 && currConfig.levelShowAppLovin >= lv && (listStepShowFullRe.Count > 0 || listStepShowFullCircle.Count > 1))
                    {
#if ENABLE_MYLOG
                        SdkUtil.logd("ads helper loadFullCircle Applovin Nextload");
#endif
                        isNextLoad = true;
                    }
                    if (isNextLoad)
                    {
                        AdsProcessCB.Instance().Enqueue(() =>
                        {
                            loadFullCircle(isSplash, lv);
                        });
                    }
                    else
                    {
                        listcurr[idxcurr].loadFull(isSplash, (AD_State state) =>
                        {
                            if (state == AD_State.AD_LOAD_OK)
                            {
                                if (_cbFullLoad != null)
                                {
                                    _cbFullLoad(AD_State.AD_LOAD_OK);
                                    _cbFullLoad = null;
                                }
                            }
                            else
                            {
                                loadFullCircle(isSplash, lv);
                            }
                        });
                    }
                }
                else
                {
                    if (_cbFullLoad != null)
                    {
                        _cbFullLoad(AD_State.AD_LOAD_FAIL);
                        _cbFullLoad = null;
                    }
                }
            }
            else
            {
                if (_cbFullLoad != null)
                {
                    _cbFullLoad(AD_State.AD_LOAD_FAIL);
                    _cbFullLoad = null;
                }
            }
        }
        public bool isFull4Show(bool isOpenAds, bool isAll, bool isExcluse)
        {
            checkResumeAudio();
            SdkUtil.logd($"ads helper isFull4Show isOpenAds={isOpenAds}, isAll={isAll}, isExcluse={isExcluse}");

#if UNITY_EDITOR
            return adsEditorCtr.isFullEditorLoaded;
#endif
            List<int> lex = null;
            if (isExcluse)
            {
                lex = currConfig.fullExcluseShowRunning;
            }
            for (int i = 0; i < listStepShowFullCircle.Count; i++)
            {
                if (listStepShowFullCircle[i].getFullLoaded(isOpenAds) > 0)
                {
                    if (lex != null && lex.Count > 0)
                    {
                        if (!lex.Contains(listStepShowFullCircle[i].adsType))
                        {
                            SdkUtil.logd("ads helper isFull4Show m true = " + listStepShowFullCircle[i].adsType);
                            return true;
                        }
                    }
                    else
                    {
                        SdkUtil.logd("ads helper isFull4Show m true = " + listStepShowFullCircle[i].adsType);
                        return true;
                    }
                }
            }
            if (isAll || listStepShowFullCircle.Count == 0)
            {
                for (int i = 0; i < listStepShowFullRe.Count; i++)
                {
                    if (listStepShowFullRe[i].getFullLoaded(isOpenAds) > 0)
                    {
                        if (lex != null && lex.Count > 0)
                        {
                            if (!lex.Contains(listStepShowFullRe[i].adsType))
                            {
                                SdkUtil.logd("ads helper isFull4Show re true = " + listStepShowFullRe[i].adsType);
                                return true;
                            }
                        }
                        else
                        {
                            SdkUtil.logd("ads helper isFull4Show re true = " + listStepShowFullRe[i].adsType);
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        private int checkShowFull(bool isSplash, int level)
        {
            if (isSplash)
            {
                long t1 = SdkUtil.systemCurrentMiliseconds();
                if (t1 - tFullShow <= 5000)
                {
                    return 0;
                }
                else
                {
                    return 1;
                }
            }
            long dtstart = SdkUtil.systemCurrentMiliseconds() - timeOpenGame;
            SdkUtil.logd("ads helper checkShowFull lvstart: curr=" + level + ", lvs=" + currConfig.fullLevelStart + ", dtstart=" + dtstart + ", dtcf=" + currConfig.fullTimeStart);
            if ((level < currConfig.fullLevelStart || dtstart < currConfig.fullTimeStart) && !isSplash)
            {
                return 0;
            }
            SdkUtil.logd("ads helper checkShowFull over num show of day, to=" + currConfig.fullTotalOfday);
            if (countFullShowOfDay <= 0)
            {
                return -1;
            }

            long t = SdkUtil.systemCurrentMiliseconds();
            SdkUtil.logd("ads helper checkShowFull + d:" + currConfig.fullDeltatime + " dt:" + (t - tFullShow));
            if (t - tFullShow < currConfig.fullDeltatime)
            {
                return 2;
            }
            return 1;
        }
        public void setTimeShowFull()
        {
            checkResumeAudio();
            tFullShow = SdkUtil.systemCurrentMiliseconds();
        }
        public bool checkFullWillShow(bool isSplash, int level, bool isShowOnPlaying, bool isExcluse, bool ischecknumover)
        {
            checkResumeAudio();
            SdkUtil.logd("ads helper checkFullWillShow issplash=" + isSplash + ", lv=" + level + ", isExcluse=" + isExcluse);
            if (isRemoveAds == 1 || isNoAdsTime()) //vvv
            {
                SdkUtil.logd("ads helper checkFullWillShow is removed ads");
                return false;
            }

            if (isShowOnPlaying)
            {
                if (currConfig.fullShowPlaying != 1)
                {
                    Debug.Log("mysdk: ads helper checkFullWillShow return when not allow show on playing");
                    return false;
                }
            }

            if (isAdsShowing)
            {
                long tc = SdkUtil.systemCurrentMiliseconds();
                if ((tc - tShowAdsCheckContinue) >= 30 * 1000)
                {
                    SdkUtil.logd("ads helper checkFullWillShow isAdsShowing and overtime -> reset isAdsShowing=false");
                    setIsAdsShowing(false);
                }
                else
                {
                    SdkUtil.logd("ads helper checkFullWillShow is ads showing");
                    return false;
                }
            }

            if (ischecknumover && !isSplash)
            {
                int countover = deltaloverCountFull + 1;
                int overconfig = currConfig.fullDefaultNumover;
                for (int i = 0; i < currConfig.listIntervalShow.Count; i++)
                {
                    if (currConfig.listIntervalShow[i].startlevel <= level && currConfig.listIntervalShow[i].endLevel >= level)
                    {
                        overconfig = currConfig.listIntervalShow[i].deltal4Show;
                        SdkUtil.logd("ads helper checkFullWillShow cf interval start= " + currConfig.listIntervalShow[i].startlevel + "; end = " +
                        currConfig.listIntervalShow[i].endLevel + ", num=" + overconfig);
                        break;
                    }
                }
                SdkUtil.logd($"ads helper checkFullWillShow count4ShowFull={countover}; numOverShowFull={overconfig}");
                if (countover < overconfig) return false;
            }

            SdkUtil.logd("ads helper checkFullWillShow checkShowFull");
            int scheck = checkShowFull(isSplash, level);
            if (scheck != 1)
            {
                return false;
            }

            return isFull4Show(isSplash, true, isExcluse);
        }
        public bool showFull(bool isSplash, int level, bool isShowOnPlaying, bool isExcluse, string showWhere, bool isShowLoad, bool ischecknumover = true, AdCallBack cb = null)
        {
            checkResumeAudio();
            SdkUtil.logd("ads helper showFull issplash=" + isSplash + ", lv=" + level + ", isExcluse=" + isExcluse);
            if (isRemoveAds == 1 || isNoAdsTime() || isRemoveAdsInterval()) //vvv
            {
                SdkUtil.logd("ads helper showFull is removed ads");
                return false;
            }
            if (isShowOnPlaying)
            {
                if (currConfig.fullShowPlaying != 1)
                {
                    Debug.Log("mysdk: showFull return when not allow show on playing");
                    return false;
                }
            }

            if (isAdsShowing)
            {
                long tc = SdkUtil.systemCurrentMiliseconds();
                if ((tc - tShowAdsCheckContinue) >= 30 * 1000)
                {
                    SdkUtil.logd("ads helper showFull isAdsShowing and overtime -> reset isAdsShowing=false");
                    setIsAdsShowing(false);
                }
                else
                {
                    SdkUtil.logd("ads helper showFull is ads showing");
                    return false;
                }
            }

            if (ischecknumover && !isSplash)
            {
                deltaloverCountFull++;
                int overconfig = currConfig.fullDefaultNumover;
                for (int i = 0; i < currConfig.listIntervalShow.Count; i++)
                {
                    if (currConfig.listIntervalShow[i].startlevel <= level && currConfig.listIntervalShow[i].endLevel >= level)
                    {
                        overconfig = currConfig.listIntervalShow[i].deltal4Show;
                        SdkUtil.logd("ads helper showfull cf interval start= " + currConfig.listIntervalShow[i].startlevel + "; end = " +
                          currConfig.listIntervalShow[i].endLevel + ", num=" + overconfig);
                        break;
                    }
                }

                SdkUtil.logd("ads helper showfull count4ShowFull = " + deltaloverCountFull + "; numOverShowFull = " + overconfig);
                if (deltaloverCountFull < overconfig) return false;
            }
            if (idxFullShowCircle >= listStepShowFullCircle.Count)
            {
                idxFullShowCircle = 0;
            }

            SdkUtil.logd("ads helper showFull checkShowFull");
            int scheck = checkShowFull(isSplash, level);
            if (scheck != 1)
            {
                if (scheck == 2)
                {
                    loadFull4ThisTurn(isSplash, level, ischecknumover, null);
                }
                return false;
            }
            FIRhelper.logEvent("show_ads_full_call");
            if (showWhere != null && showWhere.CompareTo("OpenAds") == 0)
            {
                FIRhelper.logEvent("show_ads_full_open_call");
            }
            SdkUtil.logd("ads helper showFull call");
            fullIsloadWhenErr = true;
            _cbFullShow = cb;
            levelCurr4Full = level;

#if UNITY_EDITOR
            if (isFull4Show(isSplash, false, isExcluse))
            {
                _cbFull = cb;
                tFullShow = SdkUtil.systemCurrentMiliseconds();
                deltaloverCountFull = 0;
                subCountShowFull();
                showFullEditor();
                int countfull4pointe = PlayerPrefs.GetInt("count_full_4_point", 0);
                countfull4pointe++;
                AnalyticCommonParam.Instance.countShowAdsFull = countfull4pointe;
                PlayerPrefs.SetInt("count_full_4_point", countfull4pointe);
                checkLogVipAds();
                return true;
            }

            loadFull4ThisTurn(isSplash, level, ischecknumover, null);
            return false;
#endif
            List<int> lex = null;
            if (isShowOnPlaying || isExcluse)
            {
                lex = currConfig.fullExcluseShowRunning;
            }
            if (!showFullCircle(isSplash, showWhere, lex, isShowLoad))
            {
                if (Application.internetReachability == NetworkReachability.NotReachable)
                {
                    FIRhelper.logEvent("show_ads_full_fail_not_con");
                }
                _cbFullShow = null;
                loadFull4ThisTurn(isSplash, level, ischecknumover, null);
                return false;
            }
            else
            {
                isFullCallMissCB = false;
                deltaloverCountFull = 0;
                int countfull4point = PlayerPrefs.GetInt("count_full_4_point", 0);
                countfull4point++;
                AnalyticCommonParam.Instance.countShowAdsFull = countfull4point;
                PlayerPrefs.SetInt("count_full_4_point", countfull4point);
                checkLogVipAds();
                AdjustHelper.LogEvent(AdjustEventName.AdsFull);
                AdjustHelper.LogEvent(AdjustEventName.AdsTotal);
                Myapi.LogEventApi.Instance().LogAds(Myapi.AdsTypeLog.Interstitial, showWhere);//vvv
#if UNITY_IOS || UNITY_IPHONE
                statuschekAdsFullErr = 1;
                statuschekAdsGiftErr = 0;
                Invoke("waitStatusAdsShowErr", 3);
                isPauseAudio = true;
                AudioListener.pause = true;
#elif UNITY_ANDROID
                Invoke("waitCheckCbFullErr", 3);
#endif
                return true;
            }
        }
        private void waitCheckCbFullErr()
        {
            SdkUtil.logd("ads helper waitCheckCbFullErr 0");
            if (!isFullCallMissCB)
            {
                SdkUtil.logd("ads helper waitCheckCbFullErr: AD_SHOW_MISS_CB");
                isFullCallMissCB = true;
                if (_cbFullShow != null)
                {
                    _cbFullShow(AD_State.AD_SHOW_MISS_CB);
                }
            }
        }
        private bool showFullCircle(bool isSplash, string where, List<int> listExcluse, bool isShowLoad)
        {
            SdkUtil.logd("ads helper showFullCircle idxFullShowCircle=" + idxFullShowCircle + ", idxFullShow=" + idxFullShowCircle);
            bool re = false;
            setIsAdsShowing(false);
            for (int ii = 0; ii < listStepShowFullCircle.Count; ii++)
            {
                int idxcurr = idxFullShowCircle;
                idxFullShowCircle++;
                if (idxFullShowCircle >= listStepShowFullCircle.Count)
                {
                    idxFullShowCircle = 0;
                }
                bool isShow = true;
                if (listExcluse != null && listExcluse.Count > 0)
                {
                    if (listExcluse.Contains(listStepShowFullCircle[idxcurr].adsType))
                    {
                        if (listStepShowFullCircle[idxcurr].adsType == 6)
                        {
                            if (listStepShowFullCircle[idxcurr].getFullLoaded(isSplash) > 0)
                            {
                                if (listExcluse.Contains(listStepShowFullCircle[idxcurr].adsFullSubType))
                                {
                                    isShow = false;
                                }
                            }
                            else
                            {
                                isShow = false;
                            }
                        }
                        else
                        {
                            isShow = false;
                        }
                    }
                }

                if (isShow)
                {
                    if (listStepShowFullCircle[idxcurr].getFullLoaded(isSplash) > 0)
                    {
                        subCountShowFull();
                        listStepShowFullCircle[idxcurr].showFull(isSplash, (stateshow) =>
                        {
                            SdkUtil.logd("ads showFullCircle callback ads=" + listStepShowFullCircle[idxcurr].adsType + ", state=" + stateshow.ToString());
                            if (stateshow == AD_State.AD_CLOSE || stateshow == AD_State.AD_SHOW_FAIL)
                            {
                                setIsAdsShowing(false);
                                isFullCallMissCB = true;
                                statuschekAdsFullErr = 0;
                                SDKManager.Instance.closeWaitShowFull();
                                onclickCloseFullGift(false, true);
#if UNITY_IOS || UNITY_IPHONE
                                isPauseAudio = false;
                                AudioListener.pause = false;
#endif
                            }
                            else if (stateshow == AD_State.AD_SHOW)
                            {
                                bgFullGift.SetActive(false);
                                SDKManager.Instance.closeWaitShowFull();
#if UNITY_IOS || UNITY_IPHONE
                                if (statuschekAdsFullErr == 1)
                                {
                                    statuschekAdsFullErr = 2;
                                    Invoke("waitStatusAdsCloseErr", 5);
                                }
#endif
                            }
                            if (_cbFullShow != null)
                            {
                                _cbFullShow(stateshow);
                            }
                        });
                        isShowFulled = true;
                        setIsAdsShowing(true);
                        typeFullGift = 0;
                        showTransFullGift();
                        tFullShow = SdkUtil.systemCurrentMiliseconds();
                        tShowAdsCheckContinue = tFullShow;
                        if (isShowLoad)
                        {
                            SDKManager.Instance.showWait4ShowFull();
                        }
                        re = true;
                        FIRhelper.logEvent("show_ads_full");
                        if (where != null && where.CompareTo("OpenAds") == 0)
                        {
                            FIRhelper.logEvent("show_ads_full_open");
                        }
                        FIRhelper.logEvent("show_ads_full_" + listStepShowFullCircle[idxcurr].adsType);
                        FIRhelper.logEvent("show_ads_total");
                        logCountTotal();
                        break;
                    }
                    else if (fullIsloadWhenErr)
                    {
                        //fullIsloadWhenErr = false;
                        if (listStepShowFullCircle[idxcurr].adsType != 6 || currConfig.stateShowAppLlovin != 2 || currConfig.levelShowAppLovin < levelCurr4Full || (listStepShowFullRe.Count == 0 && listStepShowFullCircle.Count <= 1))
                        {
                            SdkUtil.logd("ads helper showFullCircle fullIsloadWhenErr=" + idxcurr);
                            listStepShowFullCircle[idxcurr].loadFull(isSplash, null);
                        }
                    }
                }
            }
            if (!re)
            {
                for (int ii = 0; ii < listStepShowFullRe.Count; ii++)
                {
                    if (listStepShowFullRe[ii].getFullLoaded(isSplash) > 0)
                    {
                        subCountShowFull();

                        listStepShowFullRe[ii].showFull(isSplash, (stateshow) =>
                        {
                            SdkUtil.logd("ads showFullCircle re callback ads=" + listStepShowFullRe[ii].adsType + ", state=" + stateshow.ToString());
                            if (stateshow == AD_State.AD_CLOSE || stateshow == AD_State.AD_SHOW_FAIL)
                            {
                                setIsAdsShowing(false);
                                isFullCallMissCB = true;
                                statuschekAdsFullErr = 0;
                                SDKManager.Instance.closeWaitShowFull();
                                onclickCloseFullGift(false, true);
#if UNITY_IOS || UNITY_IPHONE
                                isPauseAudio = false;
                                AudioListener.pause = false;
#endif
                            }
                            else if (stateshow == AD_State.AD_SHOW)
                            {
                                bgFullGift.SetActive(false);
                                SDKManager.Instance.closeWaitShowFull();
#if UNITY_IOS || UNITY_IPHONE
                                if (statuschekAdsFullErr == 1)
                                {
                                    statuschekAdsFullErr = 2;
                                    Invoke("waitStatusAdsCloseErr", 5);
                                }
#endif
                            }
                            if (_cbFullShow != null)
                            {
                                _cbFullShow(stateshow);
                            }
                        });
                        isShowFulled = true;
                        setIsAdsShowing(true);
                        typeFullGift = 0;
                        showTransFullGift();
                        tFullShow = SdkUtil.systemCurrentMiliseconds();
                        tShowAdsCheckContinue = tFullShow;
                        if (isShowLoad)
                        {
                            SDKManager.Instance.showWait4ShowFull();
                        }
                        re = true;
                        FIRhelper.logEvent("show_ads_full");
                        if (where != null && where.CompareTo("OpenAds") == 0)
                        {
                            FIRhelper.logEvent("show_ads_full_open");
                        }
                        FIRhelper.logEvent("show_ads_full_" + listStepShowFullRe[ii].adsType);
                        FIRhelper.logEvent("show_ads_total");
                        logCountTotal();
                        break;
                    }
                }
            }
            return re;
        }

        public void onclickCloseFullGift(bool isCallCb, bool isFull)
        {
            bgFullGift.SetActive(false);
            if (isFull)
            {
                tFullShow = SdkUtil.systemCurrentMiliseconds();
            }
            else
            {
                tGiftShow = SdkUtil.systemCurrentMiliseconds();
            }
            if (isCallCb)
            {
                Debug.Log($"mysdk: adshelper onclickCloseFullGift typeFullGift={typeFullGift}");
                if (typeFullGift == 0)
                {
                    setIsAdsShowing(false);
                    isFullCallMissCB = true;
                    statuschekAdsFullErr = 0;
                    SDKManager.Instance.closeWaitShowFull();
#if UNITY_IOS || UNITY_IPHONE
                    isPauseAudio = false;
                    AudioListener.pause = false;
#endif
                    if (_cbFullShow != null)
                    {
                        _cbFullShow(AD_State.AD_SHOW_FAIL);
                    }
                }
                else
                {
                    setIsAdsShowing(false);
                    isGiftCallMissCB = true;
                    statuschekAdsGiftErr = 0;
#if UNITY_IOS || UNITY_IPHONE
                    isPauseAudio = false;
                    AudioListener.pause = false;
#endif
                    if (_cbGiftShow != null)
                    {
                        _cbGiftShow(AD_State.AD_SHOW_FAIL);
                    }
                }
            }
        }

        //-----------------------
        public void loadGift4ThisTurn(int lv, AdCallBack cb)
        {
            SdkUtil.logd("ads helper loadGift4ThisTurn 1");
            checkResumeAudio();
            _cbGiftLoad = null;

            if (countGiftShowOfDay <= 0)
            {
                SdkUtil.logd("ads helper loadGift4ThisTurn over num show of day, to=" + currConfig.giftTotalOfday);
                return;
            }

            // long t = SdkUtil.systemCurrentMiliseconds();
            // SdkUtil.logd("ads helper loadGift4ThisTurn + d:" + currConfig.giftDeltatime + " del_t:" + (t - tGiftShow));
            // if ((t - tGiftShow) < (currConfig.giftDeltatime - 30000)) return;
            // SdkUtil.logd("ads helper loadGift4ThisTurn 2");
            //if (isGift4Show(false)) return;
            SdkUtil.logd("ads helper loadGift4ThisTurn load");
            _cbGiftLoad = cb;
            if (listStepShowGiftCircle.Count == 0 && listStepShowGiftRe.Count == 0)
            {
                initListGift();
            }
            if (idxGiftShowCircle >= listStepShowGiftCircle.Count)
            {
                idxGiftShowCircle = 0;
            }

            if (listStepShowGiftCircle.Count == 0)
            {
                stepGiftLoad = 1;
                idxGiftLoad = 0;
                countGiftLoad = listStepShowGiftRe.Count;
            }
            else
            {
                stepGiftLoad = 0;
                idxGiftLoad = idxGiftShowCircle;
                countGiftLoad = listStepShowGiftCircle.Count;
            }
            if (adsAdmobMy != null && ((listStepShowGiftCircle != null && listStepShowGiftCircle.Contains(adsAdmobMy)) || (listStepShowGiftRe != null && listStepShowGiftRe.Contains(adsAdmobMy))))
            {
                adsAdmobMy.loadGift(null);
            }
            if (adsApplovinMaxLow != null && (listStepShowGiftCircle.Contains(adsApplovinMaxLow) || listStepShowGiftRe.Contains(adsApplovinMaxLow)))
            {
                string adxgiftid = adsApplovinMaxLow.getGiftId();
                if (adxgiftid != null && adxgiftid.Length > 3 && currConfig.isLoadMaxLow == 1)
                {
                    adsApplovinMaxLow.loadGift(null);
                }
            }
            loadGiftCircle(lv);
        }
        private void loadGiftCircle(int lv)
        {
#if UNITY_EDITOR
            if (!adsEditorCtr.isGiftEditorLoading && !adsEditorCtr.isGiftEditorLoaded)
            {
                loadGiftEditor();
            }
            return;
#endif
            SdkUtil.logd("ads helper loadGiftCircle idx=" + idxGiftLoad);
            if (countGiftLoad > 0)
            {
                level4ApplovinGift = lv;
                int idxcurr = idxGiftLoad;
                idxGiftLoad++;
                List<AdsBase> listcurr = null;
                if (stepGiftLoad == 0)
                {
                    listcurr = listStepShowGiftCircle;
                    if (idxGiftLoad >= listStepShowGiftCircle.Count)
                    {
                        idxGiftLoad = 0;
                    }
                }
                else
                {
                    listcurr = listStepShowGiftRe;
                    if (idxGiftLoad >= listStepShowGiftRe.Count)
                    {
                        idxGiftLoad = 0;
                    }
                }
                countGiftLoad--;
                if (countGiftLoad <= 0 && stepGiftLoad == 0)
                {
                    stepGiftLoad = 1;
                    idxGiftLoad = 0;
                    countGiftLoad = listStepShowGiftRe.Count;
                }

                if (listcurr != null && listcurr.Count > idxcurr && idxcurr >= 0)
                {
                    bool isNextLoad = false;
                    if (listcurr[idxcurr].adsType == 6 && currConfig.stateShowAppLlovin == 2 && currConfig.levelShowAppLovin >= lv && (listStepShowGiftRe.Count > 0 || listStepShowGiftCircle.Count > 1))
                    {
                        isNextLoad = true;
                    }
                    if (isNextLoad)
                    {
                        AdsProcessCB.Instance().Enqueue(() =>
                        {
                            loadGiftCircle(lv);
                        });
                    }
                    else
                    {
                        listcurr[idxcurr].loadGift((AD_State state) =>
                        {
                            if (state == AD_State.AD_LOAD_FAIL || state == AD_State.AD_LOAD_OK_WAIT)
                            {
                                loadGiftCircle(lv);
                            }
                            else
                            {
                                if (_cbGiftLoad != null)
                                {
                                    _cbGiftLoad(AD_State.AD_LOAD_OK);
                                    _cbGiftLoad = null;
                                }
                            }
                        });
                    }
                }
                else
                {
                    if (_cbGiftLoad != null)
                    {
                        _cbGiftLoad(AD_State.AD_LOAD_FAIL);
                        _cbGiftLoad = null;
                    }
                }
            }
            else
            {
                if (_cbGiftLoad != null)
                {
                    _cbGiftLoad(AD_State.AD_LOAD_FAIL);
                    _cbGiftLoad = null;
                }
            }
        }
        public bool isGift4Show(bool isAll)
        {
            SdkUtil.logd("ads helper isGift4Show 1");
            checkResumeAudio();
#if UNITY_EDITOR
            return adsEditorCtr.isGiftEditorLoaded;
#endif
            for (int i = 0; i < listStepShowGiftCircle.Count; i++)
            {
                if (listStepShowGiftCircle[i].getGiftLoaded())
                {
                    return true;
                }
            }
            if (isAll || listStepShowGiftCircle.Count == 0)
            {
                for (int i = 0; i < listStepShowGiftRe.Count; i++)
                {
                    if (listStepShowGiftRe[i].getGiftLoaded())
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        private int checkShowGift()
        {
            if (countGiftShowOfDay <= 0)
            {
                SdkUtil.logd("ads helper checkShowgift over num show of day, to=" + currConfig.giftTotalOfday);
                return -1;
            }

            long t = SdkUtil.systemCurrentMiliseconds();
            SdkUtil.logd("ads helper checkShowGift + d:" + currConfig.giftDeltatime + " dt:" + (t - tGiftShow));
            if (t - tGiftShow < currConfig.giftDeltatime)
            {
                return 2;
            }
            return 1;
        }
        public int showGift(int lv, string showWhere, AdCallBack cb)
        {
            checkResumeAudio();
            if (isAdsShowing)
            {
                long tc = SdkUtil.systemCurrentMiliseconds();
                if ((tc - tShowAdsCheckContinue) >= 30 * 1000)
                {
                    SdkUtil.logd("ads helper showGift isAdsShowing and overtime -> reset isAdsShowing=false");
                    setIsAdsShowing(false);
                }
                else
                {
                    SdkUtil.logd("ads helper showGift is ads showing");
                    return -3;
                }
            }
            SdkUtil.logd("ads helper showGift");

            if (idxGiftShowCircle >= listStepShowGiftCircle.Count)
            {
                idxGiftShowCircle = 0;
            }

            SdkUtil.logd("ads helper showGift 2");
            levelCurr4Gift = lv;
            int scheck = checkShowGift();
            if (scheck != 1)
            {
                if (scheck == 2)
                {
                    loadGift4ThisTurn(lv, null);
                }
                return -1;
            }
            SdkUtil.logd("ads helper showGift 3");
            FIRhelper.logEvent("show_ads_reward_call");
            giftIsloadWhenErr = true;
            _cbGiftShow = cb;
#if UNITY_EDITOR
            if (adsEditorCtr.isGiftEditorLoaded)
            {
                _cbGift = cb;
                tGiftShow = SdkUtil.systemCurrentMiliseconds();
                tShowAdsCheckContinue = tGiftShow;
                subCountShowGift();
                showGiftEditor();
                int countGift4pointe = PlayerPrefs.GetInt("count_gift_4_point", 0);
                countGift4pointe++;
                AnalyticCommonParam.Instance.countShowAdsGift = countGift4pointe;
                PlayerPrefs.SetInt("count_gift_4_point", countGift4pointe);
                checkLogVipAds();
                return 0;
            }

            loadGift4ThisTurn(lv, null);
            return -2;
#endif
            if (!showGiftCircle())
            {
                if (Application.internetReachability == NetworkReachability.NotReachable)
                {
                    FIRhelper.logEvent("show_ads_reward_fail_not_con");
                }
                _cbGiftShow = null;
                loadGift4ThisTurn(lv, null);
                return -2;
            }
            isGiftCallMissCB = false;
            int countGift4point = PlayerPrefs.GetInt("count_gift_4_point", 0);
            countGift4point++;
            AnalyticCommonParam.Instance.countShowAdsGift = countGift4point;
            PlayerPrefs.SetInt("count_gift_4_point", countGift4point);
            checkLogVipAds();
            AdjustHelper.LogEvent(AdjustEventName.AdsGift);
            AdjustHelper.LogEvent(AdjustEventName.AdsTotal);
            Myapi.LogEventApi.Instance().LogAds(Myapi.AdsTypeLog.Reward, showWhere);//vvv
#if UNITY_IOS || UNITY_IPHONE
            statuschekAdsFullErr = 0;
            statuschekAdsGiftErr = 1;
            Invoke("waitStatusAdsShowErr", 3);
            isPauseAudio = true;
            AudioListener.pause = true;
#elif UNITY_ANDROID
            Invoke("waitCheckCbGiftErr", 3);
#endif
            return 0;
        }
        private void waitStatusAdsShowErr()
        {
            SdkUtil.logd("ads helper waitStatusAdsShowErr");
            if (statuschekAdsFullErr == 1)
            {
                SdkUtil.logd("ads helper waitStatusAdsShowErr full");
                statuschekAdsFullErr = 0;
                if (!isFullCallMissCB)
                {
                    SdkUtil.logd("ads helper waitStatusAdsShowErr full: AD_SHOW_MISS_CB");
                    isFullCallMissCB = true;
                    if (_cbFullShow != null)
                    {
                        _cbFullShow(AD_State.AD_SHOW_MISS_CB);
                    }
                }
            }
            if (statuschekAdsGiftErr == 1)
            {
                SdkUtil.logd("ads helper waitStatusAdsShowErr gift");
                statuschekAdsGiftErr = 0;
                if (!isGiftCallMissCB)
                {
                    SdkUtil.logd("ads helper waitStatusAdsShowErr gift: AD_SHOW_MISS_CB");
                    isGiftCallMissCB = true;
                    if (_cbGiftShow != null)
                    {
                        _cbGiftShow(AD_State.AD_SHOW_MISS_CB);
                    }
                }

            }
        }
        public void waitStatusAdsCloseErr()
        {
            SdkUtil.logd("ads helper waitStatusAdsCloseErr");
            if (statuschekAdsFullErr == 2)
            {
                SdkUtil.logd("ads helper waitStatusAdsCloseErr full");
                statuschekAdsFullErr = 0;
                if (!isFullCallMissCB)
                {
                    SdkUtil.logd("ads helper waitStatusAdsCloseErr full: AD_SHOW_MISS_CB");
                    isFullCallMissCB = true;
                    if (_cbFullShow != null)
                    {
                        _cbFullShow(AD_State.AD_SHOW_MISS_CB);
                    }
                }
            }
            if (statuschekAdsGiftErr == 2)
            {
                SdkUtil.logd("ads helper waitStatusAdsCloseErr gift");
                statuschekAdsGiftErr = 0;
                if (!isGiftCallMissCB)
                {
                    SdkUtil.logd("ads helper waitStatusAdsCloseErr gift: AD_SHOW_MISS_CB");
                    isGiftCallMissCB = true;
                    if (_cbGiftShow != null)
                    {
                        _cbGiftShow(AD_State.AD_SHOW_MISS_CB);
                    }
                }
            }
        }
        private void waitCheckCbGiftErr()
        {
            SdkUtil.logd("ads helper waitCheckCbGiftErr 0");
            if (!isGiftCallMissCB)
            {
                SdkUtil.logd("ads helper waitCheckCbGiftErr: AD_SHOW_MISS_CB");
                isGiftCallMissCB = true;
                if (_cbGiftShow != null)
                {
                    _cbGiftShow(AD_State.AD_SHOW_MISS_CB);
                }
            }
        }
        private bool showGiftCircle()
        {
#if ENABLE_MYLOG
            SdkUtil.logd("ads helper showGiftCircle idxgiftShowCircle=" + idxGiftShowCircle);
#endif
            bool re = false;
            setIsAdsShowing(false);
            for (int ii = 0; ii < listStepShowGiftCircle.Count; ii++)
            {
                int idxcurr = idxGiftShowCircle;
                idxGiftShowCircle++;
                if (idxGiftShowCircle >= listStepShowGiftCircle.Count)
                {
                    idxGiftShowCircle = 0;
                }

                if (listStepShowGiftCircle[idxcurr].getGiftLoaded())
                {
                    subCountShowGift();

                    listStepShowGiftCircle[idxcurr].showGift((stateshow) =>
                    {
#if ENABLE_MYLOG
                        SdkUtil.logd("ads showGiftCircle callback ads=" + listStepShowGiftCircle[idxcurr].adsType + ", state=" + stateshow.ToString());
#endif
                        if (stateshow == AD_State.AD_CLOSE || stateshow == AD_State.AD_SHOW_FAIL)
                        {
                            setIsAdsShowing(false);
                            isGiftCallMissCB = true;
                            statuschekAdsGiftErr = 0;
                            onclickCloseFullGift(false, false);
#if UNITY_IOS || UNITY_IPHONE
                            isPauseAudio = false;
                            AudioListener.pause = false;
#endif
                        }
                        else if (stateshow == AD_State.AD_SHOW)
                        {
                            bgFullGift.SetActive(false);
#if UNITY_IOS || UNITY_IPHONE
                            if (statuschekAdsGiftErr == 1)
                            {
                                statuschekAdsGiftErr = 2;
                                Invoke("waitStatusAdsCloseErr", 5);
                            }
#endif
                        }
                        if (_cbGiftShow != null)
                        {
                            _cbGiftShow(stateshow);
                        }
                    });
                    setIsAdsShowing(true);
                    typeFullGift = 1;
                    showTransFullGift();
                    tGiftShow = SdkUtil.systemCurrentMiliseconds();
                    tShowAdsCheckContinue = tGiftShow;
                    FIRhelper.logEvent("show_ads_reward");
                    FIRhelper.logEvent("show_ads_reward_" + listStepShowGiftCircle[idxcurr].adsType);
                    FIRhelper.logEvent("show_ads_total");
                    logCountTotal();
                    re = true;
                    break;
                }
                else if (giftIsloadWhenErr)
                {
                    //giftIsloadWhenErr = false;
                    if (listStepShowFullCircle[idxcurr].adsType != 6 || currConfig.stateShowAppLlovin != 2 || currConfig.levelShowAppLovin < levelCurr4Gift || (listStepShowGiftRe.Count == 0 && listStepShowGiftCircle.Count <= 1))
                    {
                        SdkUtil.logd("ads helper showGiftCircle giftIsloadWhenErr=" + idxcurr);
                        listStepShowGiftCircle[idxcurr].loadGift(null);
                    }
                }
            }
            if (!re)
            {
                for (int ii = 0; ii < listStepShowGiftRe.Count; ii++)
                {
                    if (listStepShowGiftRe[ii].getGiftLoaded())
                    {
                        subCountShowGift();

                        listStepShowGiftRe[ii].showGift((stateshow) =>
                        {
                            SdkUtil.logd("ads showGiftCircle re callback ads=" + listStepShowGiftRe[ii].adsType + ", state=" + stateshow.ToString());
                            if (stateshow == AD_State.AD_CLOSE || stateshow == AD_State.AD_SHOW_FAIL)
                            {
                                setIsAdsShowing(false);
                                isGiftCallMissCB = true;
                                statuschekAdsGiftErr = 0;
                                onclickCloseFullGift(false, false);
#if UNITY_IOS || UNITY_IPHONE
                                isPauseAudio = false;
                                AudioListener.pause = false;
#endif
                            }
                            else if (stateshow == AD_State.AD_SHOW)
                            {
                                bgFullGift.SetActive(false);
#if UNITY_IOS || UNITY_IPHONE
                                if (statuschekAdsGiftErr == 1)
                                {
                                    statuschekAdsGiftErr = 2;
                                    Invoke("waitStatusAdsCloseErr", 5);
                                }
#endif
                            }
                            if (_cbGiftShow != null)
                            {
                                _cbGiftShow(stateshow);
                            }
                        });
                        setIsAdsShowing(true);
                        typeFullGift = 1;
                        showTransFullGift();
                        tGiftShow = SdkUtil.systemCurrentMiliseconds();
                        tShowAdsCheckContinue = tGiftShow;
                        re = true;
                        FIRhelper.logEvent("show_ads_reward");
                        FIRhelper.logEvent("show_ads_reward_" + listStepShowGiftRe[ii].adsType);
                        FIRhelper.logEvent("show_ads_total");
                        logCountTotal();
                        break;
                    }
                }
            }
            return re;
        }

        public static void logCountTotal()
        {
            countTotalShowAds++;
            PlayerPrefs.SetInt("mem_count_to_show", countTotalShowAds);
            string slog = $"show_ads_total_{countTotalShowAds:000}";
            FIRhelper.logEvent(slog);
            if (countTotalShowAds == 3
                || countTotalShowAds == 5
                || countTotalShowAds == 10
                || countTotalShowAds == 15
            )
            {
#if ENABLE_AppsFlyer
                Dictionary<string, string> additionalParameters = new Dictionary<string, string>();
                AppsFlyerSDK.AppsFlyer.sendEvent(slog, additionalParameters);
#endif
            }
            else if (countTotalShowAds % 10 == 0)
            {
#if ENABLE_AppsFlyer
                Dictionary<string, string> additionalParameters = new Dictionary<string, string>();
                AppsFlyerSDK.AppsFlyer.sendEvent(slog, additionalParameters);
#endif
            }
        }
        //===========================================================================================
        private void loadFullEditor()
        {
#if UNITY_EDITOR
            adsEditorCtr.fullEditor.loadAds();
#endif
        }
        private void showFullEditor()
        {
#if UNITY_EDITOR
            adsEditorCtr.showFullEditor();
#endif
        }
        private void loadGiftEditor()
        {
#if UNITY_EDITOR
            SdkUtil.logd("ads helper RequestRewardBasedVideo editor");
            adsEditorCtr.giftEditor.loadAds();
#endif
        }
        private void showGiftEditor()
        {
#if UNITY_EDITOR
            adsEditorCtr.showGiftEditor();
#endif
        }

#if UNITY_EDITOR
        public void EditorOnFullClose()
        {
            if (_cbFull != null)
            {
                SdkUtil.logd("ads helper onFullClose1");
                _cbFull(AD_State.AD_CLOSE);
            }
            if (isFullLoadWhenClose)
            {
                loadFull4ThisTurn(false, 100, false);
            }

        }
        public void EditorOnGiftClose(bool isrw)
        {
            if (isrw && _cbGift != null)
            {
                SdkUtil.logd("ads helper rw onGifClose1");
                _cbGift(AD_State.AD_REWARD_OK);
            }


            if (_cbGift != null)
            {
                SdkUtil.logd("ads helper rw onGifClose2");
                _cbGift(AD_State.AD_CLOSE);
            }

            if (isGiftLoadWhenClose)
            {
                loadGift4ThisTurn(GameRes.LevelCommon(), null);
            }

        }
#endif
    }
}