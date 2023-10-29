using System.Collections;
using TMPro;
using mygame.sdk;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace SuperFight
{
    public class SubscriptionPopup : PopupUI
    {
        public Button subPackBtn1;
        public Button subPackBtn2;
        public Button subPackBtn3;
        public Button closeBtn;
        public InappData[] subPacks;
        public TextMeshProUGUI[] price;
        public Button privacy;
        public override void Initialize(PopupManager popupManager)
        {
            base.Initialize(popupManager);
            popupName = PopupName.SUBSCRIPTION;
            subPackBtn1.onClick.AddListener(() => BuySubPack(0));
            subPackBtn2.onClick.AddListener(() => BuySubPack(1));
            subPackBtn3.onClick.AddListener(() => BuySubPack(2));
            closeBtn.onClick.AddListener(Hide);
            privacy.onClick.AddListener(OnClickPrivacy);
            for (int i = 0; i < price.Length; i++)
            {
                price[i].text = InappHelper.Instance.getPrice(subPacks[i].packageName);
            }
        }
        public override void Show()
        {
            base.Show();
            AdsHelper.Instance.hideBanner(0);
        }
        void OnClickPrivacy()
        {
#if UNITY_IOS || UNITY_IPHONE
            Application.OpenURL("https://sortpuzzle.live/policy");
#else
            Application.OpenURL("https://galaxyshooter.club/privacy/privacy.html");
#endif

        }
        void BuySubPack(int index)
        {
            InappHelper.Instance.BuySubscription(subPacks[index].packageName, "subscription_popup_" + subPacks[index].packName, delegate (PurchaseCallback callback)
            {
                if (callback.status == 1)
                {
                    Hide();
                }
            });
        }
        public override void Hide()
        {
            base.Hide();
            AdsHelper.Instance.showBanner(AD_BANNER_POS.TOP, App_Open_ad_Orien.Orien_Landscape, 320);
        }
    }
}
