#define Nho_TrTh_Tao
#define Xem_Nga_Tao

using System;
using System.Globalization;
using Myapi;
using UnityEngine;

namespace mygame.sdk
{
    public enum Type_vibreate
    {
        Vib_Max = 0,
        Vib_Error,
        Vib_Success,
        Vib_Waring,
        Vib_Light,
        Vib_Medium,
        Vib_Heavy,
        Vib_Min,
    }

    public class GameHelper : MonoBehaviour
    {
        public static GameHelper Instance { get; private set; }

        public const string CountryDefault = "default";

        public string deviceid { get; set; }
        public string countryCode { get; set; }
        public string languageCode { get; set; }
        public string AdsIdentify { get; set; }
        public bool isAlowShowOpen { get; set; }

        private const long Day_len_Luc = 1649819784;
        private const int So_nga_xem = 2;
        static bool isShowCMP = false;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;

                deviceid = SystemInfo.deviceUniqueIdentifier;
                countryCode = PlayerPrefs.GetString("mem_countryCode", "");
                languageCode = PlayerPrefs.GetString("mem_languageCode", MutilLanguage.langDefault);
                AdsIdentify = PlayerPrefs.GetString("key_ads_identify", "");
                isAlowShowOpen = true;
                if (AdsIdentify.StartsWith("00000000-0000"))
                {
                    AdsIdentify = "";
                }
                // if (languageCode == null || languageCode.Length <= 0 || string.Compare(languageCode, MutilLanguage.langDefault) == 0 || string.Compare(languageCode, "en") == 0)
                // {
                getLanguageCode();
                // }
                // if (languageCode == null || languageCode.Length <= 0 || string.Compare(languageCode, MutilLanguage.langDefault) == 0 || string.Compare(languageCode, "en") == 0)
                // {
                //     languageCode = CountryCodeUtil.getLanguageCodeFromCountryCode(countryCode);
                // }
                if (languageCode == null || languageCode.Length <= 0)
                {
                    languageCode = MutilLanguage.langDefault;
                }

                MutilLanguage.Instance().initRes();
            }
            else
            {
                //if (this != Instance) Destroy(gameObject);
            }
        }

        private void Start()
        {
            AdsHelper.Instance.configWithRegion();
            if (countryCode == null || countryCode.Length <= 0 || string.Compare(countryCode, CountryDefault) == 0)
            {
                getCountryCode();
            }
            if (AdsIdentify.Length < 2)
            {
                getAdsIdentify();
            }
        }

        //===================================================================================================
        public void getLanguageCode()
        {
#if UNITY_EDITOR
            languageCode = MutilLanguage.langDefault;
#elif UNITY_ANDROID
            languageCode = mygame.plugin.Android.GameHelperAndroid.getLanguageCode();
#elif UNITY_IOS || UNITY_IPHONE
            languageCode = GameHelperIos.getLanguageCode();
#endif
            languageCode = languageCode.ToLower();
            Debug.Log("mysdk: getLanguageCode=" + languageCode);
            PlayerPrefs.SetString("mem_languageCode", languageCode);
        }
        public void getCountryCode()
        {
#if UNITY_EDITOR
            countryCode = CountryDefault;
            checkCountryNoInapp();
#elif UNITY_ANDROID
            mygame.plugin.Android.GameHelperAndroid.getCountryCode(false);
#elif UNITY_IOS || UNITY_IPHONE
            countryCode = GameHelperIos.GetCountryCode();
            countryCode = countryCode.ToLower();
            Debug.Log("mysdk: countryCode=" + countryCode);
            PlayerPrefs.SetString("mem_countryCode", countryCode);
            checkCountryNoInapp();
#endif
        }

        public void getAdsIdentify()
        {
            Debug.Log("mysdk: call getAdsIdentify");
#if UNITY_EDITOR
#elif UNITY_ANDROID
            mygame.plugin.Android.GameHelperAndroid.getAdsIdentify();
#elif UNITY_IOS || UNITY_IPHONE
            AdsIdentify = GameHelperIos.GetAdsIdentify();
            if (AdsIdentify.StartsWith("00000000-0000"))
            {
                AdsIdentify = "";
            } 
            else 
            {
                Debug.Log("mysdk: AdsIdentify=" + AdsIdentify);
                PlayerPrefs.SetString("key_ads_identify", AdsIdentify);
                //SingleTonApi<LogInstallApi, LogInstallOb>.Instance.check();
            }
#endif
        }

        public void rate()
        {
            //isAlowShowOpen = false;
            appReview();
            // int memverrate = PlayerPrefs.GetInt("mem_ver_rate", -1);
            // #if UNITY_ANDROID
            //             if (memverrate != AppConfig.verapp)
            //             {
            //                 appReview();
            //                 PlayerPrefs.SetInt("mem_ver_rate", AppConfig.verapp);
            //             }
            //             else
            //             {
            //                 gotoStore(Application.identifier);
            //             }
            // #elif UNITY_IOS || UNITY_IPHONE
            //             gotoStore(AppConfig.appid);
            // #endif      
        }

        public void gotoStore(string idapp)
        {
#if UNITY_ANDROID
            Application.OpenURL(string.Format(AppConfig.urlstore, idapp));
#elif UNITY_IOS || UNITY_IPHONE
            Application.OpenURL(string.Format(AppConfig.urlstore, idapp));
#endif
        }

        public void gotoLink(string url)
        {
            Application.OpenURL(url);
        }

        public string getlinkStore()
        {
#if UNITY_ANDROID
            return string.Format(AppConfig.urlstore, Application.identifier);
#elif UNITY_IOS || UNITY_IPHONE
            return string.Format(AppConfig.urlstore, AppConfig.appid);
#else
            return "";
#endif
        }

        public string getlinkHttpStore()
        {
#if UNITY_ANDROID
            return string.Format(AppConfig.urlstorehttp, Application.identifier);
#elif UNITY_IOS || UNITY_IPHONE
            return string.Format(AppConfig.urlstore, AppConfig.appid);
#else
            return "";
#endif
        }

        public void openFanpage(string pageid)
        {
#if UNITY_EDITOR
            Application.OpenURL("https://www.facebook.com/" + pageid);

#elif UNITY_IOS || UNITY_IPHONE
            Application.OpenURL("fb://profile/" + pageid);

#elif UNITY_ANDROID
            if(checkPackageAppIsPresent("com.facebook.katana")) {
                Application.OpenURL("fb://page/" + pageid);
            } else {
                Application.OpenURL("https://www.facebook.com/" + pageid); // no Facebook app - use built-in web browser
            }

#else
            Application.OpenURL("https://www.facebook.com/" + pageid);

#endif
        }

        public bool checkPackageAppIsPresent(string package)
        {
#if UNITY_ANDROID
            return mygame.plugin.Android.GameHelperAndroid.checkPackageAppIsPresent(package);
#else
            return true;
#endif
        }

        public void pushLocalNotify(NotifyObject notify)
        {

        }

        /*
         * amply: type of vibrate
         *      = 0: Vibrate
         *      = 1: haptic error
         *      = 2: haptic success
         *      = 3: haptic warning
         *      = 4: haptic impactOccurred light
         *      = 5: haptic impactOccurred medium
         *      = 6: haptic impactOccurred heavy
         *      = 7: haptic impactOccurred selectionChanged
        */
        public void Vibrate(Type_vibreate type)
        {
            if (PlayerPrefs.GetInt("key_config_vibrate", 1) == 0)
            {
                return;
            }
            // Debug.Log("mysdk Vibrate 1");
            try
            {
#if UNITY_EDITOR
                Handheld.Vibrate();
#elif UNITY_ANDROID
                int amply = 70;
                int lenght = 40;
                if (type == Type_vibreate.Vib_Max) {
                    amply = 100;
                    lenght = 200;
                } else if (type == Type_vibreate.Vib_Error) {
                    amply = -1;
                    lenght = 70;
                } else if (type == Type_vibreate.Vib_Success) {
                    amply = -1;
                    lenght = 60;
                } else if (type == Type_vibreate.Vib_Waring) {
                    amply = -1;
                    lenght = 50;
                } else if (type == Type_vibreate.Vib_Light) {
                    amply = 70;
                    lenght = 40;
                } else if (type == Type_vibreate.Vib_Medium) {
                    amply = 85;
                    lenght = 55;
                } else if (type == Type_vibreate.Vib_Heavy) {
                    amply = 100;
                    lenght = 70;
                } else {
                    amply = 60;
                    lenght = 35;
                }
                mygame.plugin.Android.GameHelperAndroid.Vibrate(amply, lenght);
#elif UNITY_IOS || UNITY_IPHONE
                GameHelperIos.vibrate((int)type);
#endif
            }
            catch (Exception)
            {

            }
        }

        public void pushNotify(int when, string title, string msg)
        {
            Debug.Log("mysdk: pushNotify when=" + when + ", title=" + title + ", msg=" + msg);
#if UNITY_ANDROID && ! UNITY_EDITOR
            // mygame.plugin.Android.GameHelperAndroid.pushNotify(when, title, msg);
#elif UNITY_IOS || UNITY_IPHONE

#else
            Debug.Log("No sharing set up for this platform.");
#endif
        }

        public void configAppOpenAd(int typeShow, int showat, int isShowFirstOpen, int deltime, string adid)
        {
#if UNITY_EDITOR
            Debug.Log("not impl");
#elif UNITY_ANDROID
            mygame.plugin.Android.GameHelperAndroid.configAppOpenAd(typeShow, showat, isShowFirstOpen, deltime, adid);
#elif UNITY_IOS || UNITY_IPHONE
            GameHelperIos.configAppOpenAd(typeShow, showat, isShowFirstOpen, deltime, adid);
#endif
        }

        public static void setRemoveAds4OpenAds(int isRemove)
        {
#if UNITY_EDITOR
            Debug.Log("not impl");
#elif UNITY_ANDROID
            mygame.plugin.Android.GameHelperAndroid.setRemoveAds4OpenAds(isRemove);
#elif UNITY_IOS || UNITY_IPHONE
            GameHelperIos.setRemoveAds4OpenAds(isRemove);
#endif
        }

        public void loadAppOpenAdFull4First()
        {
            if (AdsHelper.Instance != null && AdsHelper.Instance.isShowOpenAds(true) > 0)
            {
#if UNITY_EDITOR
                Debug.Log("showAppOpenAd not impl");
#elif UNITY_ANDROID
                int openadhowtype = PlayerPrefs.GetInt("cf_open_ad_type", 0);
                if (openadhowtype != 0) {
                    AdsHelper.Instance.loadFull4ThisTurn(true, 99, false);
                }
#elif UNITY_IOS || UNITY_IPHONE
                int openadhowtype = PlayerPrefs.GetInt("cf_open_ad_type", 0);
                if (openadhowtype != 0) {
                    AdsHelper.Instance.loadFull4ThisTurn(true, 99, false);
                }
#endif
            }
        }

        public bool showAppOpenAdFirst()
        {
            Debug.Log($"mysdk: showAppOpenAdFirst isAlowShowOpen={isAlowShowOpen}");
            if (isAlowShowOpen && AdsHelper.Instance != null && AdsHelper.Instance.isShowOpenAds(true) > 0)
            {
#if UNITY_EDITOR
                Debug.Log("showAppOpenAd not impl");
                return false;
#elif UNITY_ANDROID
                int openadhowtype = PlayerPrefs.GetInt("cf_open_ad_type", 0);
                if (openadhowtype == 0 || openadhowtype == 2 || openadhowtype == 4) {
                    mygame.plugin.Android.GameHelperAndroid.showAppOpenAd(true);
                    return false;
                } else {
                    bool re = AdsHelper.Instance.showFull(true, 99, false, true, "OpenAds_first", false, false, (satead) =>
                    {
                        if (satead == AD_State.AD_SHOW)
                        {
                            SDKManager.Instance.PopupShowFirstAds.gameObject.SetActive(false);
                        }
                        if (satead == AD_State.AD_CLOSE || satead == AD_State.AD_SHOW_FAIL)
                        {
                            SDKManager.Instance.PopupShowFirstAds.gameObject.SetActive(false);
                            SDKManager.Instance.flagTimeScale = 0;
                            Time.timeScale = 1;
                            if (SDKManager.Instance.CBPauseGame != null) {
                                SDKManager.Instance.CBPauseGame.Invoke(false);
                            }
                        }
                    });
                    if (!re && openadhowtype == 3)
                    {
                        mygame.plugin.Android.GameHelperAndroid.showAppOpenAd(true);
                    }
                    return re;
                }
#elif UNITY_IOS || UNITY_IPHONE
                int openadhowtype = PlayerPrefs.GetInt("cf_open_ad_type", 0);
                if (openadhowtype == 0 || openadhowtype == 2 || openadhowtype == 4) {
                    GameHelperIos.showOpenAds(true);
                    return false;
                } else {
                    bool re = AdsHelper.Instance.showFull(true, 99, false, true, "OpenAds_first", false, false, (satead) =>
                    {
                        if (satead == AD_State.AD_SHOW)
                        {
                            SDKManager.Instance.PopupShowFirstAds.gameObject.SetActive(false);
                        }
                        if (satead == AD_State.AD_CLOSE || satead == AD_State.AD_SHOW_FAIL)
                        {
                            SDKManager.Instance.PopupShowFirstAds.gameObject.SetActive(false);
                            SDKManager.Instance.flagTimeScale = 0;
                            Time.timeScale = 1;
                            if (SDKManager.Instance.CBPauseGame != null) {
                                SDKManager.Instance.CBPauseGame.Invoke(false);
                            }
                        }
                    });
                    if (!re && openadhowtype == 3)
                    {
                        GameHelperIos.showOpenAds(true);
                    }
                    return re;
                }
#endif
            }
            else
            {
                isAlowShowOpen = true;
                return false;
            }
        }

        public bool isOpenAdLoaded()
        {
#if UNITY_EDITOR
            return false;
#endif
#if UNITY_ANDROID
            bool re = mygame.plugin.Android.GameHelperAndroid.isOpenAdLoaded();
            return re;
#elif UNITY_IOS || UNITY_IPHONE
            bool re = GameHelperIos.isOpenAdLoaded();
            return re;
#endif
            return false;
        }

        public void setFlagAdShowing()
        {
            bool isShowopen = true;
            if (AdsHelper.Instance != null)
            {
                if (AdsHelper.Instance.isAdsShowing || !isAlowShowOpen)
                {
                    isShowopen = false;
                }
            }
            setAllowShowOpenAd(isShowopen);
        }

        public void setAllowShowOpenAd(bool isShowopen)
        {
#if UNITY_EDITOR
            return;
#endif
#if UNITY_ANDROID
            mygame.plugin.Android.GameHelperAndroid.setAllowShowOpen(isShowopen);
#elif UNITY_IOS || UNITY_IPHONE
            GameHelperIos.setAllowShowOpen(isShowopen);
#endif 
        }

        public bool appReview()
        {
#if UNITY_EDITOR
            return false;
#endif
            try
            {
#if UNITY_ANDROID
                mygame.plugin.Android.GameHelperAndroid.appReview();
                return true;
#elif UNITY_IOS || UNITY_IPHONE
                bool re = GameHelperIos.appReview();
                return re;
#endif
            }
            catch (Exception)
            {

            }
            return false;
        }

        public static bool requestIDFA()
        {
            int lvshowrequest = PlayerPrefs.GetInt("lv_show_request_idfa", 0);
            Debug.Log("mysdk: requestIDFA lvshowrequest=" + lvshowrequest);
            if (lvshowrequest < 0)
            {
                return false;
            }
            if (GameRes.LevelCommon() >= lvshowrequest)
            {
                PlayerPrefs.SetInt("lv_show_request_idfa", -1);
#if (UNITY_IOS || UNITY_IPHONE) && !UNITY_EDITOR
                int isallversion = PlayerPrefs.GetInt("cf_ver_os_show_idfa", 1);
                int vergameshowidfa = PlayerPrefs.GetInt("cf_ver_game_show_idfa", 0);
                if (AppConfig.verapp < vergameshowidfa)
                {
                    isallversion = 0;
                }
                Debug.Log("mysdk: requestIDFA call isallversion=" + isallversion + ", vergameshowidfa=" + vergameshowidfa);
                GameHelperIos.requestIDFA(isallversion);
                return true;
#endif
            }
            return false;
        }

        public static void showCMP()
        {
            if (isShowCMP)
            {
                return;
            }
            isShowCMP = true;
#if (UNITY_IOS || UNITY_IPHONE) && !UNITY_EDITOR
            mygame.sdk.GameHelperIos.showCMP();
#elif UNITY_ANDROID && !UNITY_EDITOR
            mygame.plugin.Android.GameHelperAndroid.showCMP();
#endif
        }

        public static bool deviceIsRooted()
        {
#if UNITY_EDITOR
            return false;
#elif UNITY_ANDROID
            return mygame.plugin.Android.GameHelperAndroid.deviceIsRooted();
#endif
            return false;
        }

        public static float getScreenWidth()
        {
#if (UNITY_IOS || UNITY_IPHONE) && !UNITY_EDITOR
            return GameHelperIos.getScreenWidth();
#else
            return Screen.width;
#endif
        }

        public static bool checkLvXaDu()
        {
#if UNITY_IOS || UNITY_IPHONE
            int memsen = 0;
            int TT_Xadu = PlayerPrefs.GetInt("cf_trathai_xadu", 0);
#if Nho_TrTh_Tao
            if(TT_Xadu != 100)
            {
                memsen = PlayerPrefs.GetInt("nho_tt_ktro", 0);
            }
#endif
            if (memsen == 1001)
            {
                return true;
            }
            else
            {
                int deban = AppConfig.verapp - 1;
                long dt = -1000;
                int lvban = PlayerPrefs.GetInt("cf_lv_ktro", deban);
                Debug.Log($"mysdk: checkLvXaDu ban={lvban}");
#if Xem_Nga_Tao
                if(TT_Xadu != 100 && TT_Xadu != 99)
                {
                    long tcurr = SdkUtil.systemCurrentMiliseconds() / 1000;
                    dt = tcurr - Day_len_Luc;
                    Debug.Log($"mysdk: checkLvXaDu dt={dt}");
                }
#endif
                if (dt >= So_nga_xem * 24 * 3600 || lvban >= (deban + 1))
                {
                    PlayerPrefs.SetInt("nho_tt_ktro", 1001);
                    return true;
                }
                else
                {
                    return false;
                }
            }
#else
            return true;
#endif
        }

        //================================================================================================
        public void checkCountryNoInapp()
        {
            string listctry = PlayerPrefs.GetString("key_country_noinapp", "");
        }

        //================================================================================================
#if UNITY_ANDROID
        public void HandleGetCountryCode(string description)
        {
            countryCode = description;
            countryCode = countryCode.ToLower();
            UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {
                Debug.Log("mysdk: countryCode=" + countryCode);
                PlayerPrefs.SetString("mem_countryCode", countryCode);
                if (FIRhelper.Instance != null)
                {
                    FIRhelper.Instance.checkcounty();
                }
                AdsHelper.Instance.configWithRegion();
                checkCountryNoInapp();
            });
        }

        public void HandleGetAdsIdentify(string description)
        {
            AdsIdentify = description;
            Debug.Log("mysdk: AdsIdentify=" + AdsIdentify);
            UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {
                PlayerPrefs.SetString("key_ads_identify", AdsIdentify);
            });

        }
#endif
    }
}
