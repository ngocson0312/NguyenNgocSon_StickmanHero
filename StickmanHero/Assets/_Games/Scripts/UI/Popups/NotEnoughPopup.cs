using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SuperFight;
using mygame.sdk;

public class NotEnoughPopup : MonoBehaviour
{
    public Button watchAd;
    public Button inapp1;
    public Button inapp2;
    public Button closeButton;

    int stateShowAds = 0;

    public UpgradePopupCtrl upgradePopupCtrl;

    private void Awake()
    {
        watchAd.onClick.AddListener(OnClickWatchAd);
        inapp1.onClick.AddListener(OnClickInapp1);
        inapp2.onClick.AddListener(OnClickInapp2);
        closeButton.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
        });
    }

    private void OnEnable()
    {
        stateShowAds = 0;
        LoadPrice();
        LoadRecive();
    }

    void LoadPrice()
    {
        inapp1.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = InappHelper.Instance.getPrice("goldpack1sale");//priceSale
        inapp1.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = InappHelper.Instance.getPrice("goldpack1");//price

        inapp2.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = InappHelper.Instance.getPrice("goldpack3sale");
        inapp2.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = InappHelper.Instance.getPrice("goldpack3");
    }
    void LoadRecive()
    {
        inapp1.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = InappHelper.Instance.getMoneyRcv("goldpack1sale", "gold").ToString();
        inapp2.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = InappHelper.Instance.getMoneyRcv("goldpack3sale", "gold").ToString();
    }

    void OnClickInapp1()
    {
        InappHelper.Instance.BuyPackage("goldpack1sale", "not_enough_popup", delegate (PurchaseCallback callback)
        {
            if (callback.status == 1)
            {
                DataManager.Instance.AddCoin(InappHelper.Instance.getMoneyRcv("goldpack1sale", "gold"), 0, "iap_purchase_gold");
            }
        });
    }

    void OnClickInapp2()
    {
        InappHelper.Instance.BuyPackage("goldpack3sale", "not_enough_popup", delegate (PurchaseCallback callback)
        {
            if (callback.status == 1)
            {
                DataManager.Instance.AddCoin(InappHelper.Instance.getMoneyRcv("goldpack3sale", "gold"), 0, "iap_purchase_gold");
            }
        });
    }

    void OnClickWatchAd()
    {
        SoundManager.Instance.playSoundFx(SoundManager.Instance.effClickUI);
        stateShowAds = 0;
        int res = AdsHelper.Instance.showGift(GameRes.LevelCommon(), "update_main", state =>
        {
            if (state == AD_State.AD_SHOW)
            {
                SoundManager.Instance.enableSoundInAds(false);
            }
            else if (state == AD_State.AD_CLOSE || state == AD_State.AD_SHOW_FAIL)
            {
                SoundManager.Instance.enableSoundInAds(true);
                if (stateShowAds == 2)
                {
                    upgradePopupCtrl.UpgradePlayer();
                    gameObject.SetActive(false);
                }
                //canWatchAd = 0;
            }
            else if (state == AD_State.AD_REWARD_FAIL)
            {
                stateShowAds = 0;
            }
            else if (state == AD_State.AD_REWARD_OK)
            {
                stateShowAds = 2;
            }
        });

        if (res == 0)
        {
            stateShowAds = 1;
            SoundManager.Instance.enableSoundInAds(false);
        }
    }
}
