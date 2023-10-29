using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using mygame.sdk;

namespace SuperFight
{
    public class X2RewardPopup : PopupUI
    {
        // [Header("Button")]
        // [SerializeField] Button ButtonGetX2;
        // [SerializeField] Button ButtonNoThank;
        // [SerializeField] Button ButtonGetSkin;
        // [Header("UI")]
        // [SerializeField] Reward reward;
        // [SerializeField] TextMeshProUGUI txtReward;
        // [SerializeField] Image imgReward;
        // [SerializeField] int today;
        
        // public override void Initialize(PopupManager popupManager)
        // {
        //     base.Initialize(popupManager);
        //     popupName = PopupName.X2REWARD;
        //     ButtonGetX2.onClick.AddListener(ClickButtonGetX2);
        //     ButtonNoThank.onClick.AddListener(ClickButtonNothank);
        //     ButtonGetSkin.onClick.AddListener(ClickButtonGetSkin);
        // }
        // public void DisplayUI(Reward _reward, int _today)
        // {
        //     today = _today;
        //     reward = _reward;
        //     imgReward.sprite = reward.sprReward;
        //     if (reward.coin > 0)
        //     {
        //         txtReward.text = "+" + reward.coin;
        //     }
        //     if (reward.heart > 0)
        //     {
        //         txtReward.text = "+" + reward.heart;
        //     }
        //     if (reward.skinName != "")
        //     {
        //         txtReward.text = "New Skin";
        //         SetButtonGetSkin();
        //     }
        //     else
        //     {
        //         SetButtonClaimCoinHeart();
        //     }
        // }
        // void SetButtonClaimCoinHeart()
        // {
        //     ButtonGetX2.gameObject.SetActive(true);
        //     ButtonNoThank.gameObject.SetActive(false);
        //     ButtonGetSkin.gameObject.SetActive(false);
        //     StartCoroutine(IEDelayNothank());
        // }
        // IEnumerator IEDelayNothank()
        // {
        //     yield return new WaitForSeconds(2f);
        //     ButtonNoThank.gameObject.SetActive(true);
        // }
        // void SetButtonGetSkin()
        // {
        //     ButtonGetX2.gameObject.SetActive(false);
        //     ButtonNoThank.gameObject.SetActive(false);
        //     ButtonGetSkin.gameObject.SetActive(true);
        // }

        // void ClickButtonGetX2()
        // {
        //     SoundManager.Instance.playSoundFx(SoundManager.Instance.effClickUI);
        //     int res = AdsHelper.Instance.showGift(GameRes.Level, "x2_daily_reward", state =>
        //     {
        //         if (state == AD_State.AD_SHOW)
        //         {
        //             SoundManager.Instance.enableSoundInAds(false);
        //         }
        //         else if (state == AD_State.AD_CLOSE || state == AD_State.AD_SHOW_FAIL)
        //         {
        //             SoundManager.Instance.enableSoundInAds(true);
        //         }
        //         else if (state == AD_State.AD_REWARD_OK)
        //         {
        //             FIRhelper.logEvent("show_gift_x2_daily_reward");
        //             GetReward(today, 2);
        //             Hide();
        //         }
        //     });
        // }
        // void ClickButtonNothank()
        // {
        //     SoundManager.Instance.playSoundFx(SoundManager.Instance.effClickUI);
        //     GetReward(today, 1);
        //     Hide();
        // }
        // void ClickButtonGetSkin()
        // {

        //     SoundManager.Instance.playSoundFx(SoundManager.Instance.effClickUI);
        //     DailyRewardPopupCtl.Instance.skinGet = -1;
        //     GetReward(today, 1);
        //     Hide();
        // }
        // void GetReward(int _id, int x)
        // {
        //     PlayerPrefs.SetInt("isClaim" + _id, 1);
        //     if (reward.skinName != "")
        //     {
        //         FIRhelper.logEvent("unlock_skin_daily_reward");
        //         DataManager.Instance.UnlockSkin(reward.skinName);
        //         DataManager.Instance.currentSkin = reward.skinName;
        //         DailyRewardPopupCtl.Instance.skeletonMecanim.skeleton.SetSkin(reward.skinName);
        //     }
        //     if (reward.coin > 0)
        //     {
        //         DataManager.Instance.AddCoin(reward.coin * x, 0.2f, "daily_reward");
        //     }
        //     if (reward.heart > 0)
        //     {
        //         DataManager.Instance.AddHeart(reward.heart * x, "daily_reward");
        //         SoundManager.Instance.playSoundFx(SoundManager.Instance.effEatingApple);
        //     }
        //     FIRhelper.logEvent("claim_reward_day_" + (_id + 1));
        // }
        
    }
}

