//#define ENABLE_ANAHELPER_LOG_CLIENT

using System;
using System.Collections.Generic;
using UnityEngine;

#if  ENABLE_ADJUST
using com.adjust.sdk;
#endif

namespace mygame.sdk
{
    [Serializable]
    public class MyAdjustEvent
    {
        public string name;
        public string TokenAndroid;
        public string TokenIOS;

        public MyAdjustEvent(string nameev)
        {
            this.name = nameev;
        }

        public MyAdjustEvent(string nameev, string tkAnd, string tkiOS)
        {
            this.name = nameev;
            TokenAndroid = tkAnd;
            TokenIOS = tkiOS;
        }

        public string getEventToken()
        {
#if UNITY_IOS || UNITY_IPHONE
            return TokenIOS;
#else
            return TokenAndroid;
#endif
        }
    }
    public class AdjustHelper : MonoBehaviour
    {
        private const string errorMsgEditor = "[Adjust]: SDK can not be used in Editor.";
        private const string errorMsgStart = "[Adjust]: SDK not started. Start it manually using the 'start' method.";
        private const string errorMsgPlatform = "[Adjust]: SDK can only be used in Android, iOS, Windows Phone 8.1, Windows Store or Universal Windows apps.";

        public static AdjustHelper Instance = null;

        public bool eventBuffering = false;
        public bool sendInBackground = false;
        public bool launchDeferredDeeplink = true;
        public string appTokenAndroid = "{Your App Token}";
        public string appTokeniOS = "{Your App Token}";
#if ENABLE_ADJUST

        public AdjustLogLevel logLevel = AdjustLogLevel.Info;
        public AdjustEnvironment environment = AdjustEnvironment.Production;

#if UNITY_IOS || UNITY_IPHONE
        // Delegate references for iOS callback triggering
        private static List<Action<int>> authorizationStatusDelegates = null;
        private static Action<string> deferredDeeplinkDelegate = null;
        private static Action<AdjustEventSuccess> eventSuccessDelegate = null;
        private static Action<AdjustEventFailure> eventFailureDelegate = null;
        private static Action<AdjustSessionSuccess> sessionSuccessDelegate = null;
        private static Action<AdjustSessionFailure> sessionFailureDelegate = null;
        private static Action<AdjustAttribution> attributionChangedDelegate = null;
#endif

#endif
        public List<MyAdjustEvent> listEvent = new List<MyAdjustEvent>(new MyAdjustEvent[] { new MyAdjustEvent("AdsFull"), new MyAdjustEvent("AdsGift"), new MyAdjustEvent("AdsTotal") });

        // private
        private bool isInit = false;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                isInit = false;
                if (IsEditor())
                {
                    return;
                }

#if ENABLE_ADJUST
                string appToken;
#if UNITY_IOS || UNITY_IPHONE
                appToken = appTokeniOS;
#else
                appToken = appTokenAndroid;
#endif
                AdjustConfig adjustConfig = new AdjustConfig(appToken, this.environment, (this.logLevel == AdjustLogLevel.Suppress));
                adjustConfig.setLogLevel(this.logLevel);
                adjustConfig.setSendInBackground(this.sendInBackground);
                adjustConfig.setEventBufferingEnabled(this.eventBuffering);
                adjustConfig.setLaunchDeferredDeeplink(this.launchDeferredDeeplink);
                Adjust.start(adjustConfig);
                isInit = true;
#endif
            }
            else
            {
                //if (this != Instance) Destroy(gameObject);
            }
        }

        void OnApplicationPause(bool pauseStatus)
        {
            if (IsEditor()) { return; }
#if ENABLE_ADJUST

#if UNITY_IOS || UNITY_IPHONE
            // No action, iOS SDK is subscribed to iOS lifecycle notifications.
#elif UNITY_ANDROID
            if (pauseStatus)
            {
                AdjustAndroid.OnPause();
            }
            else
            {
                AdjustAndroid.OnResume();
            }
#elif (UNITY_WSA || UNITY_WP8)
                if (pauseStatus)
                {
                    AdjustWindows.OnPause();
                }
                else
                {
                    AdjustWindows.OnResume();
                }
#else
                Debug.Log(errorMsgPlatform);
#endif

#endif//#if ENABLE_ADJUST
        }

        private static bool IsEditor()
        {
#if UNITY_EDITOR
            Debug.Log(errorMsgEditor);
            return true;
#else
            return false;
#endif
        }

        //===========================================================
        public static void LogEvent(AdjustEventName nameEvent, Dictionary<string, string> dicparam = null)
        {
            if (Instance == null || !Instance.isInit)
            {
                return;
            }
#if ENABLE_ADJUST
            var Inappevent = new AdjustEvent(Instance.listEvent[(int)nameEvent].getEventToken());
            // Inappevent.addPartnerParameter("Count_level_lose", AnalyticCommonParam.Instance.levelLose.ToString());
            // Inappevent.addPartnerParameter("Count_battle_lose", AnalyticCommonParam.Instance.battleLose.ToString());
            // Inappevent.addPartnerParameter("day_return", AnalyticCommonParam.Instance.dayReturn.ToString());
            // Inappevent.addPartnerParameter("Count_gift_get_money_endgame", AnalyticCommonParam.Instance.countGiftGetMoneyEndgame.ToString());
            // Inappevent.addPartnerParameter("Count_gift_get_key", AnalyticCommonParam.Instance.countGiftGetMoreKey.ToString());
            // Inappevent.addPartnerParameter("Count_gift_get_box", AnalyticCommonParam.Instance.countGiftGetMoreBox.ToString());
            // Inappevent.addPartnerParameter("Count_gift_get_money_in_skin", AnalyticCommonParam.Instance.countGiftGetMoneyInSkin.ToString());
            // Inappevent.addPartnerParameter("Count_gift_upgrade", AnalyticCommonParam.Instance.countGiftUpgrade.ToString());
            // Inappevent.addPartnerParameter("Spend_money_upgrade_atk", AnalyticCommonParam.Instance.spendMoneyUpgradeAtk.ToString());
            // Inappevent.addPartnerParameter("Spend_money_lv_money", AnalyticCommonParam.Instance.spendMoneyUpgradeLvMoney.ToString());
            // Inappevent.addPartnerParameter("Spend_money_upgrade_def", AnalyticCommonParam.Instance.spendMoneyUpgradeDef.ToString());
            // Inappevent.addPartnerParameter("Spend_money_buy_skin", AnalyticCommonParam.Instance.spendMoneyBuySkin.ToString());
            // Inappevent.addPartnerParameter("Total_spend_moeny", AnalyticCommonParam.Instance.totalSpendMoney.ToString());
            // Inappevent.addPartnerParameter("Total_eran_money", AnalyticCommonParam.Instance.totalEarnMoney.ToString());
            // Inappevent.addPartnerParameter("Current_level", PlayerPrefsUtil.Level.ToString());
            // Inappevent.addPartnerParameter("Current_momey", PlayerPrefsUtil.CurrentCoinPlayer.ToString());
            // Inappevent.addPartnerParameter("Atk_lv", PlayerPrefsUtil.ATKLevel.ToString());
            // Inappevent.addPartnerParameter("Def_lv", PlayerPrefsUtil.DEFLevel.ToString());
            // Inappevent.addPartnerParameter("Coin_havest_lv", PlayerPrefsUtil.CoinHavestLevel.ToString());

            Inappevent.addPartnerParameter("Count_show_full", AnalyticCommonParam.Instance.countShowAdsFull.ToString());
            Inappevent.addPartnerParameter("Count_show_gift", AnalyticCommonParam.Instance.countShowAdsGift.ToString());
            Inappevent.addPartnerParameter("gcm_key", PlayerPrefs.GetString("get_gcm_id", ""));
            if (dicparam != null && dicparam.Count > 0)
            {
                foreach (KeyValuePair<string, string> kvp in dicparam)
                {
                    Inappevent.addPartnerParameter(kvp.Key, kvp.Value);
                }
            }
            Adjust.trackEvent(Inappevent);
#if ENABLE_ANAHELPER_LOG_CLIENT
            Debug.Log("mysdk: adjust log event=" + nameEvent.ToString() + ", token=" + Inappevent.eventToken);
#endif

#endif
        }
    }
}
