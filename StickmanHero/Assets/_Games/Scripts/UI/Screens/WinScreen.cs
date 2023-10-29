using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamePlugins;
using UnityEngine.UI;
using TMPro;
using mygame.sdk;

namespace SuperFight
{
    public class WinScreen : ScreenUI
    {
        int stateShowAds = 0;
        [SerializeField] Button ButtonHome;
        [SerializeField] Button ButtonRestart;
        [SerializeField] Button ButtonNextLevel;
        [SerializeField] Button ButtonX2;
        [SerializeField] TextMeshProUGUI txtCoinEarn;
        [SerializeField] TextMeshProUGUI nextLvTxt;
        [SerializeField] RescuePopup RescuePopup;
        [SerializeField] Button continueBtn;
        int coinAdd = 0;
        public override void Initialize(ScreenUIManager manager)
        {
            base.Initialize(manager);
            screenName = ScreenName.WINSCREEN;
            ButtonHome.onClick.AddListener(ClickButtonHome);
            ButtonRestart.onClick.AddListener(ClickButtonRestart);
            ButtonNextLevel.onClick.AddListener(ClickButtonNextLevel);
            ButtonX2.onClick.AddListener(OnClickX2);
            continueBtn.onClick.AddListener(ClickButtonHome);
        }
        void ClickButtonHome()
        {
            FIRhelper.logEvent("click_home_in_win_popup_level" + GameRes.GetLevel());
            bool isshow = AdsHelper.Instance.showFull(false, GameRes.LevelCommon() - 1, false, false, "click_home_in_win_popup", false, true, (satead) =>
            {
                if (satead == AD_State.AD_CLOSE || satead == AD_State.AD_SHOW_FAIL)
                {
                    SoundManager.Instance.playSoundFx(SoundManager.Instance.effClickUI);
                    GameManager.Instance.BackMenu();
                    Hide();
                }
            });
            if (!isshow)
            {
                SoundManager.Instance.playSoundFx(SoundManager.Instance.effClickUI);
                GameManager.Instance.BackMenu();
                Hide();
            }
        }
        void ClickButtonRestart()
        {
            FIRhelper.logEvent("click_restart_in_win_popup_level" + GameRes.GetLevel());
            bool isshow = AdsHelper.Instance.showFull(false, GameRes.LevelCommon() - 1, false, false, "click_restart_in_win_popup", false, true, (satead) =>
            {
                if (satead == AD_State.AD_CLOSE || satead == AD_State.AD_SHOW_FAIL)
                {
                    SoundManager.Instance.playSoundFx(SoundManager.Instance.effClickUI);
                    GameManager.Instance.Restart();
                    Hide();
                }
            });
            if (!isshow)
            {
                SoundManager.Instance.playSoundFx(SoundManager.Instance.effClickUI);
                GameManager.Instance.Restart();
                Hide();
            }
        }
        void ClickButtonNextLevel()
        {
            if (!SDKManager.Instance.checkConnection()) return;
            FIRhelper.logEvent("click_next_level_in_win_popup_level" + GameRes.GetLevel());
            bool isshow = AdsHelper.Instance.showFull(false, GameRes.LevelCommon() - 1, false, false, "click_next_level_in_win_popup", false, true, (satead) =>
            {
                if (satead == AD_State.AD_CLOSE || satead == AD_State.AD_SHOW_FAIL)
                {
                    SoundManager.Instance.playSoundFx(SoundManager.Instance.effClickUI);
                    GameManager.Instance.NextLevel(GameManager.Instance.CurrLevel);
                    Hide();
                }
            });
            if (!isshow)
            {
                SoundManager.Instance.playSoundFx(SoundManager.Instance.effClickUI);
                GameManager.Instance.NextLevel(GameManager.Instance.CurrLevel);
                Hide();
            }
        }

        void OnClickX2()
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
                        DataManager.Instance.AddCoin(coinAdd * 2, 0, "x2button");
                        ButtonX2.gameObject.SetActive(false);
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
        public void SetCoinEarn(int _coinEarn)
        {
            txtCoinEarn.text = "+" + _coinEarn;
            ButtonX2.GetComponentInChildren<TextMeshProUGUI>().text = "+" + (_coinEarn * 2).ToString();
            coinAdd = _coinEarn;
        }

        void CheckShowRescuePopup()
        {
            int levelUnLock = 0;
            if (GameManager.Instance.CurrLevel == GameRes.GetLevel())
            {
                levelUnLock = GameManager.Instance.CurrLevel - 1;
            }
            else
            {
                levelUnLock = GameManager.Instance.CurrLevel;
            }
            for (int i = 0; i < DataManager.Instance.skinDataRescue.listSkin.Length; i++)
            {
                if ((levelUnLock == DataManager.Instance.skinDataRescue.listSkin[i].levelUnlock && !DataManager.Instance.IsUnlockSkin(DataManager.Instance.skinDataRescue.listSkin[i].skinName)))
                {
                    RescuePopup.skinName = DataManager.Instance.skinDataRescue.listSkin[i].skinName;
                    RescuePopup.gameObject.SetActive(true);
                    break;
                }
            }
        }

        public override void Show()
        {
            // SetCoinEarn(_coinEarn);
            gameObject.SetActive(true);
            CheckShowRescuePopup();
            if (GameManager.Instance.gameMode == GAMEMODE.CAMPAIGN)
            {
                continueBtn.gameObject.SetActive(false);
                ButtonNextLevel.gameObject.SetActive(true);
            }
            else
            {
                continueBtn.gameObject.SetActive(true);
                ButtonNextLevel.gameObject.SetActive(false);
            }
            stateShowAds = 0;
            if (GameRes.GetLevel() == GameManager.Instance.CurrLevel)
            {
                nextLvTxt.text = "LEVEL " + (GameManager.Instance.CurrLevel).ToString();
            }
            else
            {
                nextLvTxt.text = "LEVEL " + (GameManager.Instance.CurrLevel + 1).ToString();
            }
            SoundManager.Instance.playSoundFx(SoundManager.Instance.effWinUI);
            ButtonX2.gameObject.SetActive(true);

            if (DataManager.Instance.currentTrySkin != "")
            {
                PopupManager.Instance.ShowPopup<BuySkinPopup>(PopupName.BUYSKIN).LoadSkinData(DataManager.Instance.currentTrySkin);
                DataManager.Instance.currentTrySkin = "";
            }
           
           
        }

        public override void Hide()
        {
            gameObject.SetActive(false);
        }
    }

}