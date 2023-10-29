using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using mygame.sdk;
using System;

namespace SuperFight
{
    public class SubscriptionPopupCtl : MonoBehaviour
    {
        // [Header("Button")]
        // [SerializeField] Button ButtonSub1;
        // [SerializeField] Button ButtonSub2;
        // [SerializeField] Button ButtonSub3;
        // [SerializeField] Button ButtonClose;
        // [Header("Total Day")]
        // [SerializeField] TextMeshProUGUI[] textMeshTotalDayPack;
        // [Header("Price")]
        // [SerializeField] TextMeshProUGUI[] textMeshPrice;
        // [Header("Recive")]
        // [SerializeField] TextMeshProUGUI txtMeshRecive1;
        // #region Key
        
        // //type
        // string sword = "sword";
        // string bow = "bow";
        // string coin = "coin";
        // // benefits
        // string benefits = " every days";
        // #endregion

        // private void Start()
        // {
        //     ButtonSub1.onClick.AddListener(ClickButtonSub1);
        //     ButtonSub2.onClick.AddListener(ClickButtonSub2);
        //     ButtonSub3.onClick.AddListener(ClickButtonSub3);
        //     ButtonClose.onClick.AddListener(Hide);
        //     Initialize();
        // }
        
        // void Initialize()
        // {
        //     for (int i = 0; i < SubscriptionController.Instance.packageSubscription.Length; i++)
        //     {
        //         InitPack(i, textMeshPrice[i], textMeshTotalDayPack[i]);
        //     }
        //     var _package = SubscriptionController.Instance.packageSubscription[0];
        //     var _moneyRecive = InappHelper.Instance.getMoenyRcv(_package.skudPackage,"gold");
        //     //UpdateTextRecive(txtMeshRecive1, _moneyRecive, coin);
        // }
        // void InitPack(int id, TextMeshProUGUI _textMeshPrice, TextMeshProUGUI _textMeshTotalDay)
        // {
        //     var _package = SubscriptionController.Instance.packageSubscription[id];
        //     var _price = InappHelper.Instance.getPrice(_package.skudPackage);
        //     _textMeshPrice.text = _price + "";
        //     //_package.toltalDayPackage = InappHelper.Instance._handleSub.RcvDailyReward
        //     //_textMeshTotalDay.text = _package.toltalDayPackage +" days";
        // }
        // void UpdateTextRecive(TextMeshProUGUI _textMesh, int _values, string _type)
        // {
        //     _textMesh.text = "+ " + _values + " " + _type + benefits;
        // }
        // void ClickButtonSub1()
        // {
        //     var _package1 = SubscriptionController.Instance.packageSubscription[0];
        //     string _skud1 = _package1.skudPackage;
        //     InappHelper.Instance.BuyPackage(_skud1, "subscription_popup", delegate (PurchaseCallback callback) {
        //         if (callback.status == 1)
        //         {
        //             _package1.StartSubscription();
        //             Hide();
        //         }
        //     });
        // }
        // void ClickButtonSub2()
        // {
        //     var _package2 = SubscriptionController.Instance.packageSubscription[1];
        //     string _skud2 = _package2.skudPackage;
        //     InappHelper.Instance.BuyPackage(_skud2, "subscription_popup", delegate (PurchaseCallback callback) {
        //         if (callback.status == 1)
        //         {
        //             _package2.StartSubscription();
        //             Hide();
        //         }
        //     });
        // }
        // void ClickButtonSub3()
        // {
        //     var _package3 = SubscriptionController.Instance.packageSubscription[2];
        //     string _skud3 = _package3.skudPackage;
        //     InappHelper.Instance.BuyPackage(_skud3, "subscription_popup", delegate (PurchaseCallback callback) {
        //         if (callback.status == 1)
        //         {
        //             _package3.StartSubscription();
        //             Hide();
        //         }
        //     });
        // }
        
        // public void Hide()
        // {
        //     gameObject.SetActive(false);
        // }
    }
}

