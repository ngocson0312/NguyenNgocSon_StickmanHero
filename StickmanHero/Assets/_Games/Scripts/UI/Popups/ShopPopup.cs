using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamePlugins;
using UnityEngine.UI;
using TMPro;
using mygame.sdk;

namespace SuperFight
{
    public class ShopPopup : PopupUI
    {
        [SerializeField] Button ButtonClose;

        //[Header("Pack In App")]
        //[SerializeField] Button ButtonPack1;
        //[SerializeField] Button ButtonPack2;
        //[SerializeField] Button ButtonPack3;
        //[SerializeField] Button ButtonRemoveAds;
        //[Header("Price Pack")]
        //public TextMeshProUGUI textPricePack1;
        //public TextMeshProUGUI textPricePack2;
        //public TextMeshProUGUI textPricePack3;
        //public TextMeshProUGUI textPriceRemoveAds;
        //[Header("TxtCoinEarn")]
        //[SerializeField] TextMeshProUGUI txtPack1;
        //[SerializeField] TextMeshProUGUI txtPack2;
        //[SerializeField] TextMeshProUGUI txtPack3;

        public Button[] tabButton;
        public GameObject[] tabPack;
        public Sprite[] tabBtnSprite;

        private void OnEnable()
        {
            AdsHelper.Instance.hideBanner(0);
        }
        public override void Initialize(PopupManager popupManager)
        {
            base.Initialize(popupManager);
            popupName = PopupName.SHOPCOIN;
            ButtonClose.onClick.AddListener(Hide);
            tabButton[0].onClick.AddListener(() => OnClickTab(0));
            tabButton[1].onClick.AddListener(() => OnClickTab(1));
            tabButton[2].onClick.AddListener(() => OnClickTab(2));
            OnClickTab(0);
        }
        void OnClickTab(int idx)
        {
            for (int i = 0; i < tabButton.Length; i++)
            {
                if (i == idx)
                {
                    tabButton[i].image.sprite = tabBtnSprite[0];
                    tabPack[i].SetActive(true);
                }
                else
                {
                    tabButton[i].image.sprite = tabBtnSprite[1];
                    tabPack[i].SetActive(false);
                }
            }
        }


        public void OnButtonBuyNoAds()
        {
            InappHelper.Instance.BuyPackage("removeads", "shop_in_app", delegate (PurchaseCallback callback)
            {
                if (callback.status == 1)
                {
                    Hide();
                }
            });
        }
        public void OnButtonBuyPack1()
        {
            InappHelper.Instance.BuyPackage("goldpack1", "shop_in_app", delegate (PurchaseCallback callback)
            {
                if (callback.status == 1)
                {
                    DataManager.Instance.AddCoin(InappHelper.Instance.getMoneyRcv("goldpack1", "gold"), 0, "iap_purchase_gold");
                    Hide();
                }
            });
        }
        public void OnButtonBuyPack2()
        {
            InappHelper.Instance.BuyPackage("goldpack2", "shop_in_app", delegate (PurchaseCallback callback)
            {
                if (callback.status == 1)
                {
                    DataManager.Instance.AddCoin(InappHelper.Instance.getMoneyRcv("goldpack2", "gold"), 0, "iap_purchase_gold");
                    Hide();
                }
            });
        }

        public void OnButtonBuyPack3()
        {
            InappHelper.Instance.BuyPackage("goldpack3", "shop_in_app", delegate (PurchaseCallback callback)
            {
                if (callback.status == 1)
                {
                    DataManager.Instance.AddCoin(InappHelper.Instance.getMoneyRcv("goldpack3", "gold"), 0, "iap_purchase_gold");
                    Hide();
                }
            });
        }
    }
}

