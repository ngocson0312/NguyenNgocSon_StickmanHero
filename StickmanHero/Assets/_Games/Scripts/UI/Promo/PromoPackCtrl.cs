using System.Collections;
using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using mygame.sdk;
using SuperFight;
using System;

namespace SuperFight
{
    public class PromoPackCtrl : MonoBehaviour
    {
        public Button closeButton;
        public Button buyButton;
        [SerializeField] private TextMeshProUGUI header;
        [SerializeField] private TextMeshProUGUI sale;
        [SerializeField] private TextMeshProUGUI oldPrice;
        [SerializeField] private TextMeshProUGUI txtTime;
        private InappData inappData;
        private PackManager packManager;
        public void Show(InappData inappData, PackManager packManager)
        {
            this.inappData = inappData;
            this.packManager = packManager;
            buyButton.GetComponentInChildren<TextMeshProUGUI>().text = InappHelper.Instance.getPrice(inappData.packageName+"sale");
            if (oldPrice!= null)
            {
                oldPrice.text = InappHelper.Instance.getPrice(inappData.packageName);
            }
            header.text = inappData.packName;
            buyButton.onClick.AddListener(OnClickBuy);
            closeButton.onClick.AddListener(Close);
        }
        private void Close()
        {
            Destroy(gameObject);
        }

       

        void OnClickBuy()
        {
            InappHelper.Instance.BuyPackage(inappData.packageName+"sale", "pack_"+inappData.packName + "_sale", delegate (PurchaseCallback callback)
            {
                if (callback.status == 1)
                {
                    DataManager.Instance.OnPurchasePack(inappData);
                    inappData.ReceiveReward();
                    PopupManager.Instance.ShowPopup<RewardPopup>(PopupName.REWARDPOPUP).ShowReward(inappData);
                    Close();
                }
            });
        }
    }

}
