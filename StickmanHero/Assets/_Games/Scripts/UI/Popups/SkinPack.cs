using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using mygame.sdk;
using UnityEngine.UI;

namespace SuperFight
{
    public class SkinPack : MonoBehaviour
    {
        [SerializeField] string skudPack;
        [SerializeField] TextMeshProUGUI textMeshPrice;
        [SerializeField] TextMeshProUGUI textMeshOriginPrice;
        [SerializeField] InappData inappData;
        [SerializeField] Button ButtonBuy;
        [SerializeField] Sprite[] bgBuyBtn;
        private int status
        {
            get { return PlayerPrefs.GetInt("sp_" + skudPack, 0); }
            set { PlayerPrefs.SetInt("sp_" + skudPack, value); }
        }
        private void Awake()
        {
            ButtonBuy.onClick.AddListener(ClickButtonBuy);
        }
        private void Start()
        {
            InitPack();
        }
        void InitPack()
        {
            skudPack = inappData.packageName;
            string _price = InappHelper.Instance.getPrice(skudPack);
            string _priceSale = InappHelper.Instance.getPrice(skudPack + "sale");
            textMeshPrice.text = _priceSale;
            textMeshOriginPrice.text = _price;
            UpdateStatus();
        }

        void UpdateStatus()
        {
            ButtonBuy.image.sprite = bgBuyBtn[0];
            if (DataManager.Instance.SoldOut(inappData.packageName + "sale", inappData.packType))
            {
                textMeshPrice.text = "SOLD";
                textMeshOriginPrice.gameObject.SetActive(false);
                ButtonBuy.image.sprite = bgBuyBtn[1];
                ButtonBuy.interactable = false;
            }
        }

        void ClickButtonBuy()
        {
            if (DataManager.Instance.SoldOut(inappData.packageName + "sale", inappData.packType)) return;
            InappHelper.Instance.BuyPackage(skudPack + "sale", "shop_in_app_" + inappData.packName + "_sale", delegate (PurchaseCallback callback)
              {
                  if (callback.status == 1)
                  {
                      inappData.ReceiveReward();
                      status = 1;
                      DataManager.Instance.OnPurchasePack(inappData);
                      UpdateStatus();
                      PopupManager.Instance.ShowPopup<RewardPopup>(PopupName.REWARDPOPUP).ShowReward(inappData);
                      PopupManager.Instance.HideCurrentPopup();
                    //GameManager.Instance.shopPopup.Hide();
                }
              });
        }
    }
}

