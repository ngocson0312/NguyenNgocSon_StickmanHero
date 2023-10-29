using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamePlugins;
using UnityEngine.UI;
using mygame.sdk;

namespace SuperFight
{
    public class LosePopupCtl : MonoBehaviour
    {
        [SerializeField] Button ButtonHome;
        [SerializeField] Button ButtonRevine;
        [SerializeField] UpgradePopupCtrl upgradePopup;
        private void Awake()
        {
            ButtonHome.onClick.AddListener(ClickButtonHome);
            ButtonRevine.onClick.AddListener(ClickButtonRevive);
        }
        private void OnEnable()
        {
            SoundManager.Instance.playSoundFx(SoundManager.Instance.effLoseUI);
            CameraController.Instance.SetTargetFollow(PlayerManager.Instance.transform);
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
        void Hide()
        {
            gameObject.SetActive(false);
        }
        public void Show()
        {
            gameObject.SetActive(true);
            DataManager.Instance.currentTrySkin = "";
        }
    }
}