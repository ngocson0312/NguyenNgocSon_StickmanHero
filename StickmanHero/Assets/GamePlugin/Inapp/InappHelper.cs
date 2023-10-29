//#define ENABLE_INAPP
//#define ENABLE_ADJUST

using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Myapi;
#if ENABLE_ADJUST
using com.adjust.sdk;
#endif
using System.Linq;
#if ENABLE_INAPP
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Security;

#endif


namespace mygame.sdk
{
#if ENABLE_INAPP
    public class InappHelper : MonoBehaviour, IStoreListener
    {
#else
    public class InappHelper : MonoBehaviour
    {
#endif
        public static InappHelper Instance { get; private set; }

        public static event Action<string, int> SubCallback = null;

        public static event Action<string, int> FakeSubDailyCallback = null;

#if ENABLE_INAPP
        private IStoreController m_StoreController; // The Unity Purchasing system.
        private IExtensionProvider m_StoreExtensionProvider; // The store-specific Purchasing subsystems.
        private IAppleExtensions m_AppleExtensions;
        private CrossPlatformValidator validator;
        private Product productValidbyAppsflyer = null;
#endif
        public HandleSub _handleSub;
        public InappCountryOb listCurr { get; private set; }
        public Dictionary<string, InappCountryOb> listAll = new Dictionary<string, InappCountryOb>();
        private string buyWhere = "";
        private DateTime mPurchaseDate;
        int countTryCheckFakeSub = 0;
        public static int isPurchase
        {
            get { return PlayerPrefs.GetInt("iap_isPurchase", 0); }
        }

        private Action<PurchaseCallback> _callback;
        int CurrStatePurchase = 0;//0-none, 1-purchasing
        long tCurrPurchase = 0;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                CurrStatePurchase = 0;
                tCurrPurchase = 0;
                string data = "";
                string path = Application.persistentDataPath + "/files/cf_ina_ga.txt";
                if (File.Exists(path))
                {
                    data = File.ReadAllText(path);
                }

                if (data == null || data.Length < 10)
                {
#if UNITY_ANDROID
                    TextAsset txt = (TextAsset)Resources.Load("Inapp/Android/data", typeof(TextAsset));
#else
                    TextAsset txt = (TextAsset)Resources.Load("Inapp/iOS/data", typeof(TextAsset));
#endif
                    data = txt.text;
                }

                InappUtil.parserDataSkus(data, listAll);
                string countrycode = PlayerPrefs.GetString("mem_countryCode", "");
                countrycode = CountryCodeUtil.convertToCountryCode(countrycode);
                if (listAll.ContainsKey(countrycode))
                {
                    listCurr = listAll[countrycode];
                }
                else
                {
                    listCurr = listAll[GameHelper.CountryDefault];
                }

                //getMemPrice();
            }
            else
            {
                // if (this != Instance) Destroy(gameObject);
            }
        }

        private void Start()
        {
#if ENABLE_INAPP
            Debug.Log("mysdk: IAP Start1");
            ConfigurationBuilder configurationBuilder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
            configurationBuilder.Configure<IMicrosoftConfiguration>().useMockBillingSystem = true;
            //configurationBuilder.Configure<IGooglePlayConfiguration>().SetPublicKey(lincenseKey);
            foreach (var skuitem in listCurr.listWithId)
            {
                ProductType tpr = ProductType.Consumable;
                if (skuitem.Value.typeInapp == 0)
                {
                    tpr = ProductType.Consumable;
                }
                else if (skuitem.Value.typeInapp == 1)
                {
                    tpr = ProductType.NonConsumable;
                }
                else if (skuitem.Value.typeInapp == 2)
                {
                    tpr = ProductType.Subscription;
                }
                Debug.Log("mysdk: IAP Start1 sku=" + skuitem.Value.sku);

                configurationBuilder.AddProduct(skuitem.Value.sku, tpr);
            }
            Debug.Log("mysdk: IAP Start2");
            UnityPurchasing.Initialize(this, configurationBuilder);
            Debug.Log("mysdk: IAP Start3");
            string appidentifier = Application.identifier;
#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS || UNITY_IPHONE)
            validator = new CrossPlatformValidator(GooglePlayTangle.Data(),
                AppleTangle.Data(),
                appidentifier);
#endif

#if !UNITY_EDITOR && ENABLE_AppsFlyer
#if UNITY_ANDROID
            //Debug.Log("mysdk: IAP Start41");
            //AppsFlyerSDK.AppsFlyerAndroid.initInAppPurchaseValidatorListener(this);
#elif UNITY_IOS || UNITY_IPHONE
Debug.Log("mysdk: IAP Start42");
#endif
#endif

#endif
            Debug.Log("mysdk: IAP Start4");
        }

        void savePrice()
        {
            try
            {
                string dataPrice = "{";
                string pathPrice = Application.persistentDataPath + "/files/mem_iap_price.txt";
                bool isbegin = true;
                foreach (var item in listCurr.listWithSku)
                {
                    if (isbegin)
                    {
                        isbegin = false;
                        dataPrice += ",";
                    }
                    dataPrice += $"\"{item.Value.sku}\":\"{item.Value.price}\"";
                }
                dataPrice += "}";
                File.WriteAllText(pathPrice, dataPrice);
            }
            catch (Exception ex)
            {
                Debug.Log("mysdk: IAP ex=" + ex.ToString());
            }
        }

        void getMemPrice()
        {
            string dataPrice = "";
            string pathPrice = Application.persistentDataPath + "/files/mem_iap_price.txt";
            if (File.Exists(pathPrice))
            {
                dataPrice = File.ReadAllText(pathPrice);
                var listmemPrice = (IDictionary<string, object>)MyJson.JsonDecoder.DecodeText(dataPrice);
                if (listmemPrice != null || listmemPrice.Count > 0)
                {
                    foreach (KeyValuePair<string, object> itemdata in listmemPrice)
                    {
                        if (listCurr.listWithSku.ContainsKey(itemdata.Key))
                        {
                            listCurr.listWithSku[itemdata.Key].price = (string)itemdata.Value;
                        }
                    }
                }
            }
        }

        public void onclickPolicy()
        {
#if UNITY_ANDROID
            Application.OpenURL("https://dinoproduct.com/privacypolicy");
#elif UNITY_IOS || UNITY_IPHONE
            Application.OpenURL("https://habuistudio.com/privacypolicy");
#endif
        }

        public void onclickTermsofuse()
        {
#if UNITY_ANDROID
            Application.OpenURL("https://dinoproduct.com/termsofuse");
#elif UNITY_IOS || UNITY_IPHONE
            Application.OpenURL("https://habuistudio.com/privacypolicy");
#endif
        }

        #region PUBLIC METHODS

        public bool BuyPackage(string skuid, string where, Action<PurchaseCallback> cb)
        {
#if CHECK_4INAPP
            int ss2 = PlayerPrefsBase.Instance().getInt("mem_kt_jvpirakt", 0);
            int ss1 = PlayerPrefsBase.Instance().getInt("mem_kt_cdtgpl", 0);
            int rsesss = PlayerPrefsBase.Instance().getInt("mem_procinva_gema", 3);
            if (rsesss != 1 && rsesss != 2 && rsesss != 3 && rsesss != 101 && rsesss != 102 && rsesss != 103 && rsesss != 1985)
            {
                rsesss = 103;
            }
            if ((rsesss == 3 && (ss1 == 1 || ss2 == 1)) || (rsesss == 1 && ss1 == 1) || (rsesss == 2 && ss2 == 1))
            {
                SDKManager.Instance.showNotSupportIAP();
                FIRhelper.logEvent($"game_invalid_iap1_{ss1}_{ss2}");
                return false;
            }
#endif
            string realSku = "";
            buyWhere = where;
            if (listCurr.listWithId.ContainsKey(skuid))
            {
                realSku = listCurr.listWithId[skuid].sku;
            }
            else
            {
                if (skuid != null)
                {
                    Debug.LogError("mysdk: IAP err " + skuid);
                }

                return false;
            }

            long t = SdkUtil.systemCurrentMiliseconds() / 1000;
            if (CurrStatePurchase != 0)
            {
                if ((t - tCurrPurchase) < 1200)
                {
                    Debug.LogError("mysdk: IAP err in pre processing");
                    return false;
                }
            }
            _callback = cb;

#if ENABLE_TEST_INAPP
        if (realSku.Length > 0) {
            Debug.Log("mysdk: IAP test inapp" + realSku);
            PlayerPrefs.SetInt("iap_isPurchase", 1);
            getRewardInapp(realSku);
            if (_callback != null)
            {
                PurchaseCallback pcb = new PurchaseCallback(1, realSku);
                _callback(pcb);
            }
            return true;
        } else {
            return false;
        }
        
#endif
#if ENABLE_INAPP
            string skulog = realSku.Replace('.', '_');
            if (skulog.Length > 25)
            {
                skulog = skulog.Substring(skulog.Length - 25);
            }
            FIRhelper.logEvent($"IAP_click_{skulog}");
            if (IsInitialized())
            {
                Product product = m_StoreController.products.WithID(realSku);
                if (product != null && product.availableToPurchase)
                {
                    Debug.Log($"mysdk: IAP Purchasing product asychronously: {product.definition.id}");
                    tCurrPurchase = t;
                    CurrStatePurchase = 1;
                    SDKManager.Instance.showWaitCommon();
                    m_StoreController.InitiatePurchase(product);
                    return true;
                }
                else
                {
                    Debug.LogError(
                        $"mysdk: IAP BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
                    return false;
                }
            }
            else
            {
                Debug.LogError($"mysdk: IAP BuyProductID FAIL. Not initialized.");
                return false;
            }
#else
            return false;
#endif
        }

        public bool BuySubscription(string skuid, string where, Action<PurchaseCallback> cb)
        {
            if (!_handleSub.hasSub(skuid))
            {
                return BuyPackage(skuid, where, cb);
            }
            else
            {
                return false;
            }
        }

        public void RestorePurchases()
        {
            Debug.Log("mysdk: IAP RestorePurchases Click ...");
            if (!IsInitialized())
            {
                Debug.Log("mysdk: IAP RestorePurchases FAIL. Not initialized.");
            }
            else if (Application.platform == RuntimePlatform.IPhonePlayer ||
                     Application.platform == RuntimePlatform.OSXPlayer)
            {
                Debug.Log("mysdk: IAP RestorePurchases started ...");
#if ENABLE_INAPP
                IAppleExtensions extension = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
                extension.RestoreTransactions(delegate (bool result)
                {
                    if (result)
                    {
                        for (int i = 0; i < m_StoreController.products.all.Length; i++)
                        {
                            if (m_StoreController.products.all[i].hasReceipt)
                            {
                                Debug.Log("mysdk: IAP Receipt " + m_StoreController.products.all[i].receipt);
                                if (listCurr.listWithSku.ContainsKey(m_StoreController.products.all[i].definition.id))
                                {
                                    PkgObject obsku =
                                        listCurr.listWithSku[m_StoreController.products.all[i].definition.id];
                                    if (obsku.rcv.removeads != 0)
                                    {
                                        PlayerPrefs.SetInt("RemoveAdsNew", 1);
                                        mygame.sdk.AdsHelper.setRemoveAds();
                                    }

                                    getRewardInapp(m_StoreController.products.all[i]);
                                    //TODO VVV
                                }
                            }
                        }
                    }

                    Debug.Log("mysdk: IAP RestorePurchases continuing: " + result +
                              ". If no further messages, no purchases available to restore.");
                });
#endif
            }
            else
            {
                Debug.Log("mysdk: IAP RestorePurchases FAIL. Not supported on this platform. Current = " +
                          Application.platform);
            }
        }

        public int GetPurchaseDate(string sku)
        {
            return listCurr.listWithId[sku].dayPurchased;
        }

        public void handleSub(string skuid, int dayRcv)
        {
            if (SubCallback != null)
            {
                SubCallback(skuid, dayRcv);
            }
        }

        void handleFakeSub(string skuid, int dayRcv)
        {
            if (FakeSubDailyCallback != null)
            {
                FakeSubDailyCallback(skuid, dayRcv);
            }
        }

        public bool isExpireSub(string skuid)
        {
#if ENABLE_INAPP
            if (listCurr.listWithId.ContainsKey(skuid))
            {
                if (listCurr.listWithId[skuid].typeInapp == 2)
                {
                    return _handleSub.isExpired(skuid);
                }
            }
            return true;
#else
            return true;
#endif
        }
        public string getPrice(string skuid)
        {
#if UNITY_EDITOR
            if (listCurr.listWithId.ContainsKey(skuid))
            {
                return listCurr.listWithId[skuid].price;
            }
#elif ENABLE_INAPP
            if (m_StoreController != null)
            {
                if (listCurr.listWithId.ContainsKey(skuid))
                {
                    string realSku = listCurr.listWithId[skuid].sku;
                    if (m_StoreController.products.WithID(realSku) != null)
                    {
                        return m_StoreController.products.WithID(realSku).metadata.localizedPriceString;
                    } 
                    else 
                    {
                        if (listCurr.listWithId.ContainsKey(skuid))
                        {
                            return listCurr.listWithId[skuid].price;
                        }
                    }
                }
            }
            else 
            {
                if (listCurr.listWithId.ContainsKey(skuid))
                {
                    return listCurr.listWithId[skuid].price;
                }
            }
#endif
            return "";
        }

        public int getDayReward(string skuid)
        {
            if (listCurr.listWithId.ContainsKey(skuid))
            {
                return listCurr.listWithId[skuid].periodSub;
            }
            return 0;
        }

        void checkTime4FakeSub()
        {
            Debug.Log("mysdk: IAP checkTime4FakeSub");
            countTryCheckFakeSub++;
            foreach (var skuitem in listCurr.listWithId)
            {
                if (skuitem.Value.typeInapp == 0 && skuitem.Value.periodSub > 0 && skuitem.Value.dayPurchased > 0)
                {
                    if (SDKManager.Instance.timeOnline <= 0)
                    {
                        Myapi.ApiManager.Instance.getTimeOnline((status, time) =>
                        {
                            if (status)
                            {
                                SDKManager.Instance.timeOnline = (int)(time / 60000);
                                SDKManager.Instance.timeWhenGetOnline = (int)(SdkUtil.systemCurrentMiliseconds() / 60000);
                                checkFakeSub();
                            }
                            else
                            {
                                if (countTryCheckFakeSub <= 3)
                                {
                                    Invoke("checkTime4FakeSub", 60);
                                }
                            }
                        });
                    }
                    else
                    {
                        checkFakeSub();
                    }
                    break;
                }
            }
        }

        void checkFakeSub()
        {
            DateTime nd = SdkUtil.DateTimeFromTimeStamp((long)SDKManager.Instance.timeOnline * 60);
            int ncday = nd.Year * 365 + nd.DayOfYear;
            Debug.Log($"mysdk: IAP checkFakeSub curr day={ncday}:{nd}");
            foreach (var skuitem in listCurr.listWithId)
            {
                if (skuitem.Value.typeInapp == 0 && skuitem.Value.periodSub > 0 && skuitem.Value.dayPurchased > 0)
                {
                    int dd = ncday - skuitem.Value.dayPurchased;
                    if (dd >= skuitem.Value.periodSub)
                    {
                        Debug.Log($"mysdk: IAP checkFakeSub pack:{skuitem.Value.id} daybuy={skuitem.Value.dayPurchased} out of date");
                        skuitem.Value.dayPurchased = 0;
                        PlayerPrefsBase.Instance().setInt($"iap_{skuitem.Value.id}_daypur", 0);
                    }
                    else
                    {
                        int drcvmem = PlayerPrefsBase.Instance().getInt($"iap_{skuitem.Value.id}_dayrcvpur", 0);
                        if (drcvmem < ncday)
                        {
                            Debug.Log($"mysdk: IAP checkFakeSub pack:{skuitem.Value.id} daybuy={skuitem.Value.dayPurchased} rcv day={dd}");
                            skuitem.Value.countRcvDaily++;
                            skuitem.Value.dayRcvFakeSub = ncday;
                            PlayerPrefsBase.Instance().setInt($"iap_{skuitem.Value.id}_dayrcv", skuitem.Value.countRcvDaily);
                            PlayerPrefsBase.Instance().setInt($"iap_{skuitem.Value.id}_dayrcvpur", ncday);

                            handleFakeSub(skuitem.Value.id, dd + 1);
                        }
                        else
                        {
                            Debug.Log($"mysdk: IAP checkFakeSub pack:{skuitem.Value.id} daybuy={skuitem.Value.dayPurchased} dayrcv={drcvmem} has rcv");
                        }
                    }
                }
            }
        }

        public InappRcvObject getReceiver(string skuId)
        {
            if (listCurr.listWithId.ContainsKey(skuId))
            {
                InappRcvObject rcv = listCurr.listWithId[skuId].rcv;
                return rcv;
            }
            return null;
        }
        public int getMoneyRcv(string skuId, string key)
        {
            if (listCurr.listWithId.ContainsKey(skuId))
            {
                InappRcvObject rcv = listCurr.listWithId[skuId].rcv;
                return rcv.getMoney(key);
            }
            return 0;
        }
        public int getItemRcv(string skuId, string key)
        {
            if (listCurr.listWithId.ContainsKey(skuId))
            {
                InappRcvObject rcv = listCurr.listWithId[skuId].rcv;
                return rcv.getItem(key);
            }
            return 0;
        }
        public InappRcvItemsObject getEquipmentRcv(string skuId, string key)
        {
            if (listCurr.listWithId.ContainsKey(skuId))
            {
                InappRcvObject rcv = listCurr.listWithId[skuId].rcv;
                return rcv.getEquipment(key);
            }
            return null;
        }

        #endregion

        #region PRIVATE METHODS

        public bool IsInitialized()
        {
#if ENABLE_INAPP
            // Only say we are initialized if both the Purchasing references are set.
            return (m_StoreController != null && m_StoreExtensionProvider != null);
#else
            return false;
#endif
        }

        #endregion

#if ENABLE_INAPP

        #region IMPLEMENTION IStoreListener

        public void OnInitializeFailed(InitializationFailureReason error)
        {
            Debug.Log("mysdk: IAP OnInitializeFailed " + error);
            //this.InitializePurchasing();
        }

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            Debug.Log("mysdk: IAP OnInitialized 1");
            countTryCheckFakeSub = 0;
            _handleSub.checkStartSub();
            checkTime4FakeSub();
            // Purchasing has succeeded initializing. Collect our Purchasing references.
            //Debug.Log("OnInitialized: PASS");

            // Overall Purchasing system, configured with products for this application.
            m_StoreController = controller;
            // Store specific subsystem, for accessing device-specific store features.
            m_StoreExtensionProvider = extensions;
            m_AppleExtensions = extensions.GetExtension<IAppleExtensions>();
            Dictionary<string, string> introductoryPriceDictionary = m_AppleExtensions.GetIntroductoryPriceDictionary();

            Product[] all = controller.products.all;
            bool isHanSub = false;
            foreach (Product product in all)
            {
                if (!product.availableToPurchase)
                {
                    continue;
                }

                if (listCurr.listWithSku.ContainsKey(product.definition.id))
                {
                    if (listCurr.listWithSku[product.definition.id].memPrice == 1)
                    {
                        PlayerPrefs.SetString(product.definition.id, product.metadata.localizedPriceString);
                    }
                }

                if (product.receipt != null)
                {
                    if (product.definition.type == ProductType.Subscription)
                    {
                        if (checkIfProductIsAvailableForSubscriptionManager(product.receipt))
                        {
                            string intro_json =
                                (introductoryPriceDictionary != null &&
                                 introductoryPriceDictionary.ContainsKey(product.definition.storeSpecificId))
                                    ? introductoryPriceDictionary[product.definition.storeSpecificId]
                                    : null;
                            SubscriptionManager subscriptionManager = new SubscriptionManager(product, intro_json);
                            SubscriptionInfo subscriptionInfo = subscriptionManager.getSubscriptionInfo();
                            Debug.Log("mysdk: IAP product id is: " + subscriptionInfo.getProductId());
                            Debug.Log("mysdk: IAP purchase date is: " + subscriptionInfo.getPurchaseDate());
                            Debug.Log("mysdk: IAP subscription next billing date is: " +
                                      subscriptionInfo.getExpireDate());
                            Debug.Log("mysdk: IAP is subscribed? " + subscriptionInfo.isSubscribed());
                            Debug.Log("mysdk: IAP is expired? " + subscriptionInfo.isExpired());
                            Debug.Log("mysdk: IAP is cancelled? " + subscriptionInfo.isCancelled());
                            Debug.Log("mysdk: IAP product is in free trial peroid? " +
                                      subscriptionInfo.isFreeTrial());
                            Debug.Log("mysdk: IAP product is auto renewing? " + subscriptionInfo.isAutoRenewing());
                            Debug.Log("mysdk: IAP subscription remaining valid time until next billing date is: " +
                                      subscriptionInfo.getRemainingTime());
                            Debug.Log("mysdk: IAP is this product in introductory price period? " +
                                      subscriptionInfo.isIntroductoryPricePeriod());
                            Debug.Log("mysdk: IAP the product introductory localized price is: " +
                                      subscriptionInfo.getIntroductoryPrice());
                            Debug.Log("mysdk: IAP the product introductory price period is: " +
                                      subscriptionInfo.getIntroductoryPricePeriod());
                            Debug.Log("mysdk: IAP the product introductory price period is: " +
                                      subscriptionInfo.getIntroductoryPricePeriod());
                            Debug.Log("mysdk: IAP the number of product introductory price period cycles is: " +
                                      subscriptionInfo.getIntroductoryPricePeriodCycles());
                            isHanSub = true;
                            _handleSub.ReceiveSubscriptionProduct(subscriptionInfo, listCurr.listWithSku[product.definition.id]);
                        }
                        else
                        {
                            Debug.Log(
                                "mysdk: IAP This product is not available for SubscriptionManager class, only products that are purchase by 1.19+ SDK can use this class." +
                                product.definition.id);
                        }
                    }
                    else
                    {
                        Debug.Log("mysdk: IAP the product is not a subscription product" + product.definition.id);
                    }
                }
                else
                {
                    Debug.Log("mysdk: IAP valid sku=" + product.definition.id + ", price=" + product.metadata.localizedPriceString);
                }
            }
            if (!isHanSub)
            {
                _handleSub.checkHandSub();
            }
        }

        private bool checkIfProductIsAvailableForSubscriptionManager(string receipt)
        {
            // Debug.Log("mysdk: IAP checkIfProductIsAvailableForSubscriptionManager");

            // var listAndroidReceipt = (IDictionary<string, object>)MyJson.JsonDecoder.DecodeText(receipt);
            // if (listAndroidReceipt != null || listAndroidReceipt.Count > 0)
            // {
            //     foreach (KeyValuePair<string, object> itemReceipt in listAndroidReceipt)
            //     {
            //         Debug.Log("mysdk: IAP itemReceipt key={itemReceipt.Key}");
            //         Debug.Log("mysdk: IAP itemReceipt val={itemReceipt.Value}");
            //         try
            //         {
            //             var listAndPayload = (IDictionary<string, object>)MyJson.JsonDecoder.DecodeText((string)itemReceipt.Value);
            //             if (listAndPayload != null || listAndPayload.Count > 0)
            //             {
            //                 foreach (KeyValuePair<string, object> itemPayload in listAndPayload)
            //                 {
            //                     Debug.Log("mysdk: IAP itemPayload key={itemPayload.Key}");
            //                     Debug.Log("mysdk: IAP itemPayload val={itemPayload.Value}");
            //                 }
            //             }
            //         }
            //         catch (Exception ex)
            //         {

            //         }
            //     }
            // }
            // Debug.Log("mysdk: IAP checkIfProductIsAvailableForSubscriptionManager 1111");

            var dicReceipt = (IDictionary<string, object>)MyJson.JsonDecoder.DecodeText(receipt);
            if (!dicReceipt.ContainsKey("Store") || !dicReceipt.ContainsKey("Payload"))
            {
                Debug.Log("mysdk: IAP The product receipt does not contain enough information ");
                return false;
            }

            string txtStore = (string)dicReceipt["Store"];
            string textPayload = (string)dicReceipt["Payload"];
            if (txtStore != null && textPayload != null)
            {
                if (txtStore == "GooglePlay")
                {
                    var dicPayload = (IDictionary<string, object>)MyJson.JsonDecoder.DecodeText(textPayload);
                    if (!dicPayload.ContainsKey("json"))
                    {
                        Debug.Log("mysdk: IAP The product receipt does not contain enough information, the 'json' field is missing");
                        return false;
                    }
                    // string txtjson = (string)dicPayload["json"];
                    // Debug.Log("mysdk: IAP txtjson={txtjson}");
                    // var dicJson = (IDictionary<string, object>)MyJson.JsonDecoder.DecodeText(txtjson);
                    // if (dicJson == null || !dicJson.ContainsKey("developerPayload"))
                    // {
                    //     Debug.Log("mysdk: IAP The product receipt does not contain enough information, the 'developerPayload' field is missing");
                    //     return false;
                    // }

                    // string txtDevPlayload = (string)dicJson["developerPayload"];
                    // Debug.Log("mysdk: IAP txtDevPlayload={txtDevPlayload}");
                    // var dicDevPay = (IDictionary<string, object>)MyJson.JsonDecoder.DecodeText(txtDevPlayload);
                    // if (dicDevPay == null || !dicDevPay.ContainsKey("is_free_trial") || !dicDevPay.ContainsKey("has_introductory_price_trial"))
                    // {
                    //     Debug.Log("mysdk: IAP The product receipt does not contain enough information, the product is not purchased using 1.19 or later");
                    //     return false;
                    // }

                    return true;
                }
                else if (txtStore == "AppleAppStore" || txtStore == "AmazonApps" || txtStore == "MacAppStore")
                {
                    return true;
                }

                return false;
            }

            return false;
        }

        public void OnPurchaseFailed(Product i, PurchaseFailureReason p)
        {
            Debug.Log(string.Format("mysdk: OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}",
                i.definition.storeSpecificId, p));
            handlePurchaseFaild(i, p.ToString());
        }
        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
        {
            bool validPurchase = true;

#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS || UNITY_STANDALONE_OSX)
            try {
                // On Google Play, result has a single product ID.
                // On Apple stores, receipts contain multiple products.
                var result = validator.Validate(args.purchasedProduct.receipt);
                // For informational purposes, we list the receipt(s)
                foreach (IPurchaseReceipt productReceipt in result) {
                    Debug.Log("mysdk: IAP Product ID: " + productReceipt.productID);
                    Debug.Log("mysdk: IAP Purchase date: " + productReceipt.purchaseDate);
                    Debug.Log("mysdk: IAP Receipt: " + productReceipt);
                    mPurchaseDate = productReceipt.purchaseDate;
                }
                
            } catch (IAPSecurityException) {
                Debug.Log("mysdk: IAP Invalid receipt, not unlocking content");
                validPurchase = false;
            }
#elif UNITY_EDITOR
            mPurchaseDate = SdkUtil.DateTimeFromTimeStamp(SdkUtil.systemCurrentMiliseconds() / 1000);
#endif

            Debug.Log("mysdk: IAP ProcessPurchase:1" + validPurchase);
            if (validPurchase)
            {
                Debug.Log("mysdk: IAP ProcessPurchase:2" + args.purchasedProduct.definition.id);
#if !UNITY_EDITOR && ENABLE_AppsFlyer && ENABLE_VALIDATE_IAP

#if UNITY_ANDROID
                Debug.Log("mysdk: IAP Start31");
                productValidbyAppsflyer = args.purchasedProduct;
                appsflyerValidateAndroid(args.purchasedProduct.receipt, args.purchasedProduct.metadata.localizedPriceString, args.purchasedProduct.metadata.isoCurrencyCode);
#elif UNITY_IOS || UNITY_IPHONE
                Debug.Log("mysdk: IAP Start32");
                handlePurchaseSuc(args.purchasedProduct);
#endif

#else
                handlePurchaseSuc(args.purchasedProduct);
#endif
            }
            else
            {
                handlePurchaseFaild(args.purchasedProduct, "validPurchase faild");
            }

            // Return a flag indicating whether this product has completely been received, or if the application needs 
            // to be reminded of this purchase at next app launch. Use PurchaseProcessingResult.Pending when still 
            // saving purchased products to the cloud, and when that save is delayed. 
            return PurchaseProcessingResult.Complete;
        }

#if UNITY_ANDROID && ENABLE_AppsFlyer
        bool appsflyerValidateAndroid(string androidReceipt, string price, string currency)
        {
            try
            {
                byte[] makey = { 64, 57, 57, 60, 57, 91, 56, 77, 53, 90, 94, 113, 95, 99, 97, 63, 49, 107, 48, 56, 65, 67, 55, 67, 63, 64, 78, 58, 56, 65, 45, 54, 66, 62, 63, 56, 65, 95, 56, 60, 55, 67, 62, 52, 98, 50, 82, 32, 97, 76, 64, 63, 48, 83, 50, 95, 67, 36, 108, 120, 87, 87, 112, 116, 113, 100, 117, 88, 71, 117, 107, 86, 36, 38, 47, 31, 60, 68, 99, 56, 91, 32, 75, 90, 82, 102, 84, 47, 81, 50, 87, 65, 52, 103, 49, 52, 65, 51, 104, 74, 82, 74, 66, 102, 105, 87, 102, 28, 101, 61, 83, 103, 53, 95, 57, 81, 71, 80, 118, 101, 113, 44, 78, 115, 97, 110, 48, 105, 48, 90, 121, 43, 52, 108, 81, 97, 99, 70, 111, 88, 119, 82, 77, 74, 95, 83, 37, 105, 56, 68, 42, 41, 96, 32, 115, 48, 100, 80, 46, 77, 63, 57, 94, 61, 64, 53, 85, 107, 41, 65, 31, 53, 42, 40, 104, 68, 29, 69, 113, 70, 90, 48, 101, 104, 37, 66, 82, 103, 55, 33, 65, 54, 114, 83, 102, 68, 99, 118, 36, 39, 81, 61, 68, 91, 95, 92, 62, 54, 68, 39, 33, 98, 39, 52, 99, 61, 48, 104, 56, 73, 59, 92, 39, 107, 81, 57, 77, 68, 38, 87, 40, 45, 59, 57, 90, 105, 81, 100, 33, 55, 94, 84, 44, 39, 108, 64, 38, 74, 37, 92, 110, 95, 93, 64, 58, 105, 52, 70, 85, 69, 96, 79, 47, 76, 106, 51, 97, 100, 116, 48, 74, 63, 62, 96, 76, 66, 59, 102, 116, 67, 87, 117, 54, 73, 102, 122, 81, 84, 66, 85, 117, 67, 81, 90, 107, 104, 52, 117, 87, 98, 106, 70, 115, 66, 109, 77, 114, 68, 69, 77, 122, 100, 77, 78, 112, 108, 122, 108, 52, 54, 66, 65, 76, 76, 105, 85, 115, 84, 56, 117, 75, 47, 112, 117, 99, 117, 112, 80, 74, 116, 71, 83, 76, 75, 55, 110, 72, 48, 116, 107, 48, 100, 52, 85, 75, 67, 100, 69, 102, 57, 78, 54, 55, 118, 120, 53, 54, 50, 101, 111, 74, 77, 103, 72, 68, 112, 82, 108, 122, 98, 108, 103, 53, 107, 73, 81, 73, 68, 65, 81, 65, 66 };
                int[] paskey = { -1, 1, 3, 0, 1, 6, 6, 3, 11, 6, 5, 17, 12, 12, 18, 15, 23, 19, 12, 21, 16, 29, 14, 34, 15, 32, 20, 33, 29, 22, 21, 20, 19, 28, 38, 31, 37, 20, 50, 29, 21, 55, 31, 47, 47, 66, 24, 38, 25, 52, 70, 58, 34, 32, 61, 82, 70, 56, 42, 36, 65, 63, 77, 54, 61, 87, 99, 39, 47, 73, 101, 100, 45, 81, 85, 46, 89, 92, 74, 58, 58, 65, 79, 40, 101, 69, 79, 111, 111, 68, 52, 135, 58, 115, 86, 69, 126, 144, 91, 117, 118, 81, 154, 155, 78, 143, 70, 56, 99, 56, 74, 104, 158, 79, 95, 173, 165, 95, 172, 166, 96, 152, 114, 64, 139, 179, 190, 110, 138, 165, 145, 119, 183, 75, 102, 118, 68, 74, 106, 132, 161, 132, 192, 155, 118, 169, 98, 103, 72, 137, 206, 103, 209, 103, 132, 115, 171, 98, 145, 203, 148, 127, 111, 236, 122, 181, 100, 190, 153, 206, 253, 196, 176, 178, 150, 187, 248, 137, 171, 168, 96, 262, 137, 98, 119, 153, 140, 122, 227, 183, 211, 111, 185, 256, 102, 257, 211, 257 };
                byte[] pasva = { 16, 13, 16, 6, 16, 15, 9, 1, 13, 0, 9, 8, 12, 0, 10, 0, 14, 3, 2, 1, 1, 9, 16, 11, 10, 10, 2, 8, 19, 7, 10, 14, 7, 13, 14, 3, 18, 17, 12, 12, 14, 7, 7, 6, 11, 0, 0, 10, 18, 1, 5, 15, 7, 14, 19, 18, 5, 7, 8, 5, 16, 1, 3, 16, 2, 18, 2, 1, 3, 8, 11, 7, 6, 12, 12, 19, 12, 4, 7, 5, 1, 5, 9, 17, 14, 10, 3, 4, 7, 3, 0, 0, 16, 19, 2, 10, 9, 1, 9, 18, 15, 11, 15, 4, 18, 8, 17, 10, 4, 17, 12, 4, 17, 5, 4, 12, 8, 8, 16, 14, 11, 7, 10, 14, 3, 15, 15, 12, 15, 1, 0, 0, 14, 7, 4, 12, 9, 7, 13, 15, 14, 7, 9, 5, 11, 18, 15, 14, 18, 9, 16, 2, 6, 1, 14, 18, 11, 11, 8, 4, 9, 17, 17, 12, 4, 17, 16, 10, 6, 5, 11, 12, 7, 14, 17, 17, 12, 5, 13, 0, 6, 4, 7, 15, 18, 2, 5, 7, 15, 12, 14, 16, 14, 2, 11, 5, 5, 7 };
                string pkey = SdkUtil.myGiaima(makey, paskey, pasva);
                string psig = "";
                string pdata = "";
                if (pkey != null && pkey.Length > 20)
                {
                    var listAndroidReceipt = (IDictionary<string, object>)MyJson.JsonDecoder.DecodeText(androidReceipt);
                    if (listAndroidReceipt != null || listAndroidReceipt.Count > 0)
                    {
                        foreach (KeyValuePair<string, object> itemReceipt in listAndroidReceipt)
                        {
                            if (itemReceipt.Key.Equals("Payload"))
                            {
                                var listAndPayload = (IDictionary<string, object>)MyJson.JsonDecoder.DecodeText((string)itemReceipt.Value);
                                if (listAndPayload != null || listAndPayload.Count > 0)
                                {
                                    foreach (KeyValuePair<string, object> itemPayload in listAndPayload)
                                    {
                                        if (itemPayload.Key.Equals("json"))
                                        {
                                            pdata = (string)itemPayload.Value;
                                        }
                                        else if (itemPayload.Key.Equals("signature"))
                                        {
                                            psig = (string)itemPayload.Value;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    AppsFlyerSDK.AppsFlyerAndroid.validateAndSendInAppPurchase(pkey, psig, pdata, price, currency, null, this);
                    //AppsFlyerSDK.AppsFlyer.validateAndSendInAppPurchase(pkey, psig, pdata, price, currency, null, this);
                    return true;
                }
                else
                {
                    handlePurchaseSuc(productValidbyAppsflyer);
                    return true;
                }
            }
            catch (Exception ex)
            {
                handlePurchaseFaild(null, "Exception validate appsflyer");
                return true;
            }
        }
#endif

        public void didFinishValidateReceipt(string description)
        {
            Debug.Log("mysdk: IAP appsflyer didFinishValidateReceipt=" + description);
            handlePurchaseSuc(productValidbyAppsflyer);
        }

        public void didFinishValidateReceiptWithError(string description)
        {
            Debug.Log("mysdk: IAP appsflyer didFinishValidateReceiptWithError=" + description);
            handlePurchaseFaild(null, description);
        }

        private void handlePurchaseFaild(Product p, string err)
        {
            Debug.Log(string.Format("mysdk: handlePurchaseFaild: Product: '{0}', PurchaseFailureReason: {1}",
                p.definition.storeSpecificId, err));
            SDKManager.Instance.PopupShowFirstAds.gameObject.SetActive(false);
            if (CurrStatePurchase == 1)
            {
                string skulog = p.definition.id.Replace('.', '_');
                if (skulog.Length > 25)
                {
                    skulog = skulog.Substring(skulog.Length - 25);
                }
                FIRhelper.logEvent($"IAP_fail_{skulog}");
            }
            CurrStatePurchase = 0;

            if (_callback != null)
            {
                PurchaseCallback pcb = new PurchaseCallback(0, p.definition.id);
                _callback(pcb);
            }
        }

        private void handlePurchaseSuc(Product p)
        {
            SDKManager.Instance.PopupShowFirstAds.gameObject.SetActive(false);
#if CHECK_4INAPP
            int ss2 = PlayerPrefsBase.Instance().getInt("mem_kt_jvpirakt", 0);
            int ss1 = PlayerPrefsBase.Instance().getInt("mem_kt_cdtgpl", 0);
            int rsesss = PlayerPrefsBase.Instance().getInt("mem_procinva_gema", 3);
            if (rsesss != 1 && rsesss != 2 && rsesss != 3 && rsesss != 101 && rsesss != 102 && rsesss != 103 && rsesss != 1985)
            {
                rsesss = 103;
            }
            if ((rsesss == 3 && (ss1 == 1 || ss2 == 1)) || (rsesss == 1 && ss1 == 1) || (rsesss == 2 && ss2 == 1))
            {
                SDKManager.Instance.showNotSupportIAP();
                if (CurrStatePurchase == 1)
                {
                    FIRhelper.logEvent($"game_invalid_iap2_{ss1}_{ss2}");
                }
                CurrStatePurchase = 0;
                return;
            }
#endif
            if (!SDKManager.Instance.isDeviceTest())
            {
                if (CurrStatePurchase == 1)
                {
#if FIRBASE_ENABLE && UNITY_ANDROID && !UNITY_EDITOR
                    Dictionary<string, object> iapAndParam = new Dictionary<string, object>();
                    iapAndParam.Add("product_id", p.definition.id);
                    iapAndParam.Add("quantity", p.definition.id);
                    iapAndParam.Add("price", (double)p.metadata.localizedPrice);
                    iapAndParam.Add("currency", p.metadata.isoCurrencyCode);
                    iapAndParam.Add("value", (double)p.metadata.localizedPrice);
                    FIRhelper.logEvent("in_app_purchase" , iapAndParam);
#endif
                    Dictionary<string, object> iapParam = new Dictionary<string, object>();
                    iapParam.Add("product_id", p.definition.id);
                    iapParam.Add("quantity", p.definition.id);
                    iapParam.Add("price", (double)p.metadata.localizedPrice);
                    iapParam.Add("currency", p.metadata.isoCurrencyCode);
                    iapParam.Add("value", (double)p.metadata.localizedPrice);
                    FIRhelper.logEvent($"IAP_suc_purchase", iapParam);

                    string skulog = p.definition.id.Replace('.', '_');
                    if (skulog.Length > 25)
                    {
                        skulog = skulog.Substring(skulog.Length - 25);
                    }
                    FIRhelper.logEvent($"IAP_suc_{skulog}");
                    if (SDKManager.Instance.mediaType.Equals(MediaSourceType.Facebook_Ads) && SDKManager.Instance.mediaCampain.Contains("_iap"))
                    {
                        FIRhelper.logEvent($"IAP_suc_fb_{skulog}");
                    }

                    AppsFlyerHelperScript.logPurchase(p.definition.id, p.metadata.localizedPrice.ToString(), p.metadata.isoCurrencyCode);
                    Myapi.LogEventApi.Instance().logInApp(p.definition.id, p.transactionID, "", p.metadata.isoCurrencyCode, (float)p.metadata.localizedPrice, buyWhere);
                    Dictionary<string, string> dicparam = new Dictionary<string, string>();
                    dicparam.Add("producId", p.definition.id);
                    AdjustHelper.LogEvent(AdjustEventName.Inapp, dicparam);
                }
            }
            CurrStatePurchase = 0;
            PlayerPrefs.SetInt("iap_isPurchase", 1);
            getRewardInapp(p);
            if (_callback != null)
            {
                PurchaseCallback pcb = new PurchaseCallback(1, p.definition.id);
                _callback(pcb);
            }
        }

        private void getRewardInapp(Product pro)
        {
            if (listCurr.listWithSku.ContainsKey(pro.definition.id))
            {
                PkgObject obPur = listCurr.listWithSku[pro.definition.id];
                if (obPur.typeInapp == 2)
                {
                    InappRcvObject rcv = obPur.rcv;
                    if (rcv.removeads == 1)
                    {
                        AdsHelper.setRemoveAds();
                    }
                    _handleSub.onBuySubSuccess(obPur, pro);
                }
                else
                {
                    if (obPur.periodSub > 0)
                    {
                        obPur.countRcvDaily = 1;
                        obPur.dayPurchased = mPurchaseDate.Year * 365 + mPurchaseDate.DayOfYear;
                        PlayerPrefsBase.Instance().setInt($"iap_{obPur.id}_dayrcv", obPur.countRcvDaily);
                        PlayerPrefsBase.Instance().setInt($"iap_{obPur.id}_daypur", obPur.dayPurchased);
                        PlayerPrefsBase.Instance().setInt($"iap_{obPur.id}_dayrcvpur", obPur.dayPurchased);
                        Debug.Log($"mysdk: IAP getRewardInapp day buy={mPurchaseDate} daycount={obPur.dayPurchased}");
                        handleFakeSub(obPur.id, 1);
                    }
                    InappRcvObject rcv = obPur.rcv;
                    if (rcv.removeads == 1)
                    {
                        AdsHelper.setRemoveAds();
                    }

                    int va = rcv.getMoney("gold");
                    if (va > 0)
                    {
                        //add gold
                    }

                    var items = rcv.getItems();
                    if (items != null)
                    {
                        //for (int i = 0; i < items.Count; i++)
                        //{
                        //    KeyValuePair<string, int> key = items.ElementAt(i);
                        //    ItemData.ItemName itemName = ItemData.ItemName.ItemIronHammer;
                        //    if (Enum.TryParse(key.Key, out itemName))
                        //    {
                        //        //success
                        //        PlayerInfo.SetItemAmount(itemName, key.Value);
                        //    }
                        //    else
                        //    {
                        //        Debug.Log($"Parse enum fail: {key.Key} : {key.Value}");
                        //        continue;  //fail
                        //    }
                        //}
                    }
                }
            }
            else
            {
                Debug.Log("mysdk: IAP buy success but list not contain sku = " + pro.definition.id);
            }
        }

        private string parIdItem(string pkg)
        {
            string idItem = "";
            int n = pkg.LastIndexOf('.');
            if (n >= 0 && n < (pkg.Length - 1))
            {
                idItem = pkg.Substring(n + 1);
            }
            else
            {
                n = pkg.Length;
                n = n - 15;
                if (n > 0)
                {
                    idItem = pkg.Substring(n);
                }
                else
                {
                    idItem = pkg;
                }
            }

            return idItem;
        }

        string getDateFromReceipt(string receipt)
        {
            string re = "";

            return re;
        }

        #endregion

#endif
    }
}
