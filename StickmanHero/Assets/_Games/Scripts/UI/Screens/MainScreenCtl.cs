using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamePlugins;
using UnityEngine.UI;
using mygame.sdk;
using TMPro;

namespace SuperFight
{

    public class MainScreenCtl : ScreenUI
    {
        [Header("Button")]
        [SerializeField] Button ButtonPlay;
        [SerializeField] Button ButtonLevelPanel;
        [SerializeField] Button ButtonSkin;
        [SerializeField] Button ButtonLuckyWheel;
        [SerializeField] Button ButtonOtherGame;

        [SerializeField] Button ButtonDailyReward;

        [Header("Text")]
        [SerializeField] TextMeshProUGUI txtLevel;
        [SerializeField] TextMeshProUGUI txtLevel2;
 
        [SerializeField] LevelPickPopupCtl levelPickPopup;
        [SerializeField] SkinPopupCtl skinPopupCtl;

        public override void Initialize(ScreenUIManager screenManager)
        {
            base.Initialize(screenManager);
            screenName = ScreenName.MAINSCREEN;
            ButtonPlay.onClick.AddListener(ClickButtonPlayGame);
            ButtonLevelPanel.onClick.AddListener(ClickButtonLevelPanel);
            ButtonSkin.onClick.AddListener(ClickButtonSkin);
            ButtonLuckyWheel.onClick.AddListener(ClickButtonLuckyWheel);
          
            ButtonDailyReward.onClick.AddListener(ClickButtonDailyReward);
            skinPopupCtl.Initialize();
        }
        public override void Show()
        {
            gameObject.SetActive(true);
            txtLevel.text = "Level" + GameRes.GetLevel();
            txtLevel2.text = "Level" + GameRes.GetLevel();
            Invoke("showPromoGame", .5f);
        }
       
        void ClickButtonPlayGame()
        {
            SoundManager.Instance.playSoundFx(SoundManager.Instance.effClickUI);
            GameManager.Instance.PlayGame(GameRes.GetLevel());
        }
        void ClickButtonLevelPanel()
        {
            SoundManager.Instance.playSoundFx(SoundManager.Instance.effClickUI);
            levelPickPopup.Open();
        }
        void ClickButtonSkin()
        {
            SoundManager.Instance.playSoundFx(SoundManager.Instance.effClickUI);
            skinPopupCtl.Show();
        }
        void ClickButtonLuckyWheel()
        {
            SoundManager.Instance.playSoundFx(SoundManager.Instance.effClickUI);
            GameManager.Instance.luckWheelPopup.gameObject.SetActive(true);
        }

        void ClickButtonDailyReward()
        {
            SoundManager.Instance.playSoundFx(SoundManager.Instance.effClickUI);
            PopupManager.Instance.ShowPopup(PopupName.DAILYREWARD);
        }
       
        public override void Hide()
        {
            gameObject.SetActive(false);
        }
        //===============================================================

    }
}