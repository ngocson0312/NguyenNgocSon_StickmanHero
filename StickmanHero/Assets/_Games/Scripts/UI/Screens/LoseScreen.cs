using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamePlugins;
using UnityEngine.UI;
using mygame.sdk;

namespace SuperFight
{
    public class LoseScreen : ScreenUI
    {
        [SerializeField] Button ButtonHome;
        [SerializeField] Button ButtonRevine;
        [SerializeField] UpgradePopupCtrl upgradePopup;
        public override void Initialize(ScreenUIManager manager)
        {
            base.Initialize(manager);
            screenName = ScreenName.LOSESCREEN;
            ButtonHome.onClick.AddListener(ClickButtonHome);
            ButtonRevine.onClick.AddListener(ClickButtonRevive);
        }

        void ClickButtonHome()
        {
            FIRhelper.logEvent("click_home_in_lose_popup_level" + GameRes.GetLevel());
            bool isshow = AdsHelper.Instance.showFull(false, GameRes.LevelCommon() - 1, false, false, "click_home_in_lose_popup", false, true, (satead) =>
            {
                if (satead == AD_State.AD_CLOSE || satead == AD_State.AD_SHOW_FAIL)
                {
                    SoundManager.Instance.playSoundFx(SoundManager.Instance.effClickUI);
                    Hide();
                    GameManager.Instance.BackMenu();
                }
            });
            if (!isshow)
            {
                SoundManager.Instance.playSoundFx(SoundManager.Instance.effClickUI);
                Hide();
                GameManager.Instance.BackMenu();
            }

        }
        void ClickButtonRevive()
        {
            FIRhelper.logEvent("click_restart_in_lose_popup_level" + GameRes.GetLevel());
            bool isshow = AdsHelper.Instance.showFull(false, GameRes.LevelCommon() - 1, false, false, "click_restart_in_lose_popup", false, true, (satead) =>
            {
                if (satead == AD_State.AD_CLOSE || satead == AD_State.AD_SHOW_FAIL)
                {
                    SoundManager.Instance.playSoundFx(SoundManager.Instance.effClickUI);
                    Hide();
                    GameManager.Instance.Restart();
                }
            });
            if (!isshow)
            {
                SoundManager.Instance.playSoundFx(SoundManager.Instance.effClickUI);
                Hide();
                GameManager.Instance.Restart();
            }

        }
        public override void Show()
        {
            gameObject.SetActive(true);
            DataManager.Instance.currentTrySkin = "";
            SoundManager.Instance.playSoundFx(SoundManager.Instance.effLoseUI);
            CameraController.Instance.SetTargetFollow(PlayerManager.Instance.transform);
        }

        public override void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}