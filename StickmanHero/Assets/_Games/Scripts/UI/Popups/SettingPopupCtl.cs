using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamePlugins;
using UnityEngine.UI;
using mygame.sdk;

namespace SuperFight
{

    public class SettingPopupCtl : PopupUI
    {
        public Button buttonClose;
        public Button buttonMusic;
        public Button buttonSoundFx;
        public Button buttonVibration;
        public Button buttonHome;
        public Button buttonRestart;
        public GameObject checkMusic;
        public GameObject checkSoundFx;
        public GameObject checkVibration;

        public override void Initialize(PopupManager popupManager)
        {
            base.Initialize(popupManager);
            popupName = PopupName.SETTING;
            buttonClose.onClick.AddListener(() => ClickButtonClose());
            buttonMusic.onClick.AddListener(() => ClickButtonMusic());
            buttonSoundFx.onClick.AddListener(() => ClickButtonSoundFx());
            buttonVibration.onClick.AddListener(() => ClickButtonVibration());
            buttonHome.onClick.AddListener(() => ClickButtonHome());
            buttonRestart.onClick.AddListener(() => ClickButtonRestart());
            CheckStatus(checkMusic, SoundManager.Instance.IsEnableMusic);
            CheckStatus(checkSoundFx, SoundManager.Instance.IsEnableEffect);
            CheckStatus(checkVibration, PlayerPrefsUtil.VibrateSetting < 1 ? false : true);
        }
        void CheckStatus(GameObject _ObjCheck, bool _status = true)
        {
            _ObjCheck.SetActive(_status);
        }

        public void ClickButtonClose()
        {
            SoundManager.Instance.playSoundFx(SoundManager.Instance.effClickUI);
            GameManager.Instance.ResumeGame();
            Hide();
        }

        public void ClickButtonMusic()
        {
            SoundManager.Instance.playSoundFx(SoundManager.Instance.effClickUI);
            SoundManager.Instance.musicOn_Off();
            CheckStatus(checkMusic, SoundManager.Instance.IsEnableMusic);
        }

        public void ClickButtonSoundFx()
        {
            SoundManager.Instance.playSoundFx(SoundManager.Instance.effClickUI);
            SoundManager.Instance.effectOn_Off();
            CheckStatus(checkSoundFx, SoundManager.Instance.IsEnableEffect);
        }

        public void ClickButtonVibration()
        {
            SoundManager.Instance.playSoundFx(SoundManager.Instance.effClickUI);
            PlayerPrefsUtil.VibrateSetting = PlayerPrefsUtil.VibrateSetting < 1 ? 1 : 0;
            CheckStatus(checkVibration, PlayerPrefsUtil.VibrateSetting < 1 ? false : true);
        }

        public void ClickButtonHome()
        {
            SoundManager.Instance.playSoundFx(SoundManager.Instance.effClickUI);
            GameManager.Instance.BackMenu();
            Hide();
        }

        public void ClickButtonRestart()
        {
            SoundManager.Instance.playSoundFx(SoundManager.Instance.effClickUI);
            GameManager.Instance.Restart();
            Hide();
        }

       
    }
}