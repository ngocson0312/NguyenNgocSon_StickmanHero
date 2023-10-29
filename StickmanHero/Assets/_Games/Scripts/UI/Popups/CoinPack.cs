using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using mygame.sdk;
using UnityEngine.UI;

namespace SuperFight
{
    public class CoinPack : MonoBehaviour
    {
        [Header("Skud")]
        [SerializeField] string skudPack = "goldpack1";
        [Header("Price")]
        [SerializeField] TextMeshProUGUI textMeshPrice;
        [Header("Recive")]
        [SerializeField] TextMeshProUGUI textMeshRecive;
        [Header("Button")]
        [SerializeField] Button ButtonBuy;
        private void Awake()
        {
            ButtonBuy.onClick.AddListener(ClickButtonBuy);
        }
        private void Start()
        {
            InitPack();
        }
        public void InitPack()
        {
            textMeshPrice.text = InappHelper.Instance.getPrice(skudPack + "sale");
            textMeshRecive.text = InappHelper.Instance.getMoneyRcv(skudPack, "gold") + "";
        }
        void ClickButtonBuy()
        {
            InappHelper.Instance.BuyPackage(skudPack + "sale", "shop_in_app", delegate (PurchaseCallback callback) {
                if (callback.status == 1)
                {
                    DataManager.Instance.AddCoin(InappHelper.Instance.getMoneyRcv(skudPack, "gold"), 0, "iap_purchase_gold");
                    //GameManager.Instance.shopPopup.Hide();
                }
            });
        }
    }
}

