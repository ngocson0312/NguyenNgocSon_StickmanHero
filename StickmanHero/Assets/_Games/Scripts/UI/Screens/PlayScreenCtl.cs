using System.Collections.Generic;
using UnityEngine;
using GamePlugins;
using UnityEngine.UI;
using TMPro;
using mygame.sdk;

namespace SuperFight
{
    public class PlayScreenCtl : ScreenUI
    {
        [SerializeField] Button ButtonSetting;
        [SerializeField] Button ButtonBack;
        [SerializeField] TouchButton jumpButton;
        [SerializeField] TouchButton comboButton;
        [SerializeField] TouchButton skillButton;
        [SerializeField] TouchButton dashButton;
        [SerializeField] StaticThumb staticThumb;
        [SerializeField] Joystick thumbStick;
        [SerializeField] TextMeshProUGUI txtHeart;
        [SerializeField] TextMeshProUGUI txtEnemyCount;
        [SerializeField] Slider cameraSizeSlider;
        [SerializeField] GameObject blocker;
        [SerializeField] GameObject controlPanel;
        public FillBar healthBar;
        public FillBar bossHealthBar;

        [Header("Test")]
        [SerializeField] GameObject TestGame;
        private bool isOnSettingPanel;
        private void Start()
        {
#if Test_game
            TestGame.SetActive(true);
            Button ButtonTestBackLevel = TestGame.transform.GetChild(0).GetComponent<Button>();
            Button ButtonTestNextLevel = TestGame.transform.GetChild(1).GetComponent<Button>();
            Button ButtonTestAddSword = TestGame.transform.GetChild(2).GetComponent<Button>();
            Button ButtonTestNextBow = TestGame.transform.GetChild(3).GetComponent<Button>();
            ButtonTestNextLevel.onClick.AddListener(GameplayCtrl.Instance.TestNextLevel);
            ButtonTestBackLevel.onClick.AddListener(GameplayCtrl.Instance.TestBackLevel);
            ButtonTestAddSword.onClick.AddListener(TestAddWeaponSword);
            ButtonTestNextBow.onClick.AddListener(TestAddWeaponBow);
#else
            TestGame.SetActive(false);
#endif
#if UNITY_STANDALONE || UNITY_EDITOR
            controlPanel.SetActive(false);
#endif
        }
        public override void Initialize(ScreenUIManager manager)
        {
            base.Initialize(manager);
            ButtonSetting.onClick.AddListener(ClickButtonSetting);
            ButtonBack.onClick.AddListener(OnClickButtonBack);
            InitEventButton();
            screenName = ScreenName.PLAYSCREEN;
            cameraSizeSlider.value = CameraController.Instance.cameraSizeMultiply;
            cameraSizeSlider.onValueChanged.AddListener(SetNewSize);
        }
        void SetNewSize(float value)
        {
            CameraController.Instance.SetCamMultySize(value);
            
        }
        public void BlockInput(bool status)
        {
            blocker.SetActive(status);
        }
        
        public void ClickButtonSetting()
        {
            if (GameplayCtrl.Instance.gameState != GAME_STATE.GS_PLAYING) return;
            SoundManager.Instance.playSoundFx(SoundManager.Instance.effClickUI);
            GameManager.Instance.PauseGame();
            PlayerManager.Instance.input.ClearInputs();
            PopupManager.Instance.ShowPopup(PopupName.SETTING);
        }

        void OnClickButtonBack()
        {
            SoundManager.Instance.playSoundFx(SoundManager.Instance.effClickUI);
            GameManager.Instance.BackMenu();
            PlayerManager.Instance.SetToDefaultSkin();
            gameObject.SetActive(false);
        }

        public void ResetUI()
        {
            skillButton.SetActive(false);
            ButtonSetting.gameObject.SetActive(true);
            ButtonBack.gameObject.SetActive(false);
        }

        public void DeActiveHealthBar()
        {
            bossHealthBar.Deactive();
        }

        public void SetTestUI()
        {
            bossHealthBar.Deactive();
            skillButton.SetActive(false);
            ButtonSetting.gameObject.SetActive(false);
            ButtonBack.gameObject.SetActive(true);
        }

        public void SetHeart(int value)
        {
            txtHeart.text = "" + (value + 1);
        }
        public void SetEnemyCount(int _enemyCount, int _enemyKill)
        {
            txtEnemyCount.text = _enemyKill + "/" + _enemyCount;
        }
        void InitEventButton()
        {
            PlayerManager.Instance.input.jumpButton = jumpButton;
            PlayerManager.Instance.input.comboButton = comboButton;
            PlayerManager.Instance.input.staticThumb = staticThumb;
            PlayerManager.Instance.input.thumbStick = thumbStick;
            PlayerManager.Instance.input.skillButton = skillButton;
            PlayerManager.Instance.input.dashButton = dashButton;
        }
        public void SetActiveCameraSlider(bool status)
        {
            if (PlayerPrefsUtil.enableZoom != 1)
            {
                cameraSizeSlider.gameObject.SetActive(false);
                return;
            }
            cameraSizeSlider.gameObject.SetActive(status);
        }

        public override void Show()
        {
            gameObject.SetActive(true);
            isOnSettingPanel = false;
            BlockInput(false);
            PopupManager.Instance.ClearQueue();
            GameManager.Instance.ResumeGame();
            GameplayCtrl.Instance.SwitchStateGame(GAME_STATE.GS_PLAYING);
        }

        public override void Hide()
        {
            gameObject.SetActive(false);
        }

#if Test_game
        void TestAddWeaponSword()
        {
            DataManager.Instance.itemSword += 3;
        }
        void TestAddWeaponBow()
        {
            DataManager.Instance.itemBow += 3;
        }
#endif
    }
}