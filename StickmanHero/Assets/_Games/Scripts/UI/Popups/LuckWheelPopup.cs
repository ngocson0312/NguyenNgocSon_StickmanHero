using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using mygame.sdk;
using Spine.Unity;
using TMPro;

namespace SuperFight
{
    public class LuckWheelPopup : MonoBehaviour
    {
        [Header("Buttons")]
        [SerializeField] Button ButtonSpin;
        [SerializeField] Button ButtonAds;
        [SerializeField] Button ButtonBack;
        [Header("Components")]
        public SkeletonMecanim skeletonAnimationMain;
        public Spin spin;
        public DataManager dataManager;
        public SkinData skinDataCoin;
        public NewSkinPopup newSkinPopup;
        [SerializeField] GameObject TimeremaningObj;
        [SerializeField] Text textTime;
        [SerializeField] TextMeshProUGUI spineTicketText;
        [Header("=====")]
        [Range(0f, 24)]
        [SerializeField] float hourRemainPerSpin = 24;
        public int spinCount
        {
            get => PlayerPrefs.GetInt("spinCount", 0);
            set => PlayerPrefs.SetInt("spinCount", value);
        }
        public int idBox;
        public string DataTimeReward
        {
            get => PlayerPrefs.GetString("DataTimeReward", DateTime.UtcNow.Ticks.ToString());
            set => PlayerPrefs.SetString("DataTimeReward", value);
        }
        public int timeRemain
        {
            get => PlayerPrefs.GetInt("timeRemain", 0);
            set => PlayerPrefs.SetInt("timeRemain", value);
        }
        float _timer = 1;
        bool isFirst = true;
        private void Awake()
        {
            ButtonSpin.onClick.AddListener(ClickButtonSpin);
            ButtonAds.onClick.AddListener(ClickButtonAds);
            ButtonBack.onClick.AddListener(Hide);
        }
        private void OnEnable()
        {
            timeRemain -= CurrentSeconds();
            spineTicketText.text = DataManager.Instance.spineTicket.ToString();
            if (timeRemain < 0 || DataManager.Instance.spineTicket > 0)
            {
                timeRemain = 0;
                SetButton(true);
            }
            else
            {
                SetButton(false);
                textTime.text = DisPlayTime(timeRemain);
            }
        }
        private void Start()
        {
            dataManager = DataManager.Instance;
        }
        private void Update()
        {
            if (timeRemain > 0)
            {
                SetButton(false);
                TimeremaningObj.SetActive(true);
                if (_timer > 0)
                {
                    _timer -= Time.deltaTime;
                    if (_timer <= 0)
                    {
                        textTime.text = DisPlayTime(timeRemain);
                        timeRemain--;
                        _timer = 1;
                    }
                }
            }
            else
            {
                SetButton(true);
                TimeremaningObj.SetActive(false);
            }
        }

        public int CurrentSeconds()
        {
            DateTime epochStart = new DateTime(long.Parse(DataTimeReward));
            int currentEpochTime = (int)(DateTime.UtcNow - epochStart).TotalSeconds;
            return currentEpochTime;
        }
        void ClickButtonSpin()
        {
            if (spin._isSpinning) return;
            FIRhelper.logEvent("free_lucky_wheel");
            if (isFirst)
            {
                isFirst = false;
                DataTimeReward = DateTime.UtcNow.Ticks.ToString();
            }
            if (DataManager.Instance.spineTicket > 0)
            {
                DataManager.Instance.AddSpineTicket(-1);
                spineTicketText.text = DataManager.Instance.spineTicket.ToString();
            }
            else
            {
                timeRemain = (int)(3600 * hourRemainPerSpin);
                textTime.text = DisPlayTime(timeRemain);
                SetButton(false);
            }
            spin.OnSpin();
        }
        void ClickButtonAds()
        {
            if (spin._isSpinning) return;
            int res = AdsHelper.Instance.showGift(GameRes.LevelCommon(), "lucky_wheel", state =>
            {
                if (state == AD_State.AD_SHOW)
                {
                    SoundManager.Instance.enableSoundInAds(false);
                }
                else if (state == AD_State.AD_CLOSE || state == AD_State.AD_SHOW_FAIL)
                {
                    SoundManager.Instance.enableSoundInAds(true);
                }
                else if (state == AD_State.AD_REWARD_OK)
                {
                    FIRhelper.logEvent("show_gift_lucky_wheel");
                    spin.OnSpin();
                }
            });
        }
        void Hide()
        {
            if (spin._isSpinning) return;
            gameObject.SetActive(false);
        }
        public void ShowNewSkinPopup(string _skinName)
        {
            newSkinPopup.gameObject.SetActive(true);
            SoundManager.Instance.playSoundFx(SoundManager.Instance.effGetNewSkin);
            newSkinPopup.SetSkinClaim(_skinName);
        }
        void SetButton(bool _active = true)
        {
            ButtonSpin.gameObject.SetActive(_active);
            ButtonAds.gameObject.SetActive(!_active);
            if (DataManager.Instance.spineTicket > 0)
            {
                ButtonSpin.transform.GetChild(0).gameObject.SetActive(false);
                ButtonSpin.transform.GetChild(1).gameObject.SetActive(true);
            }
            else
            {
                ButtonSpin.transform.GetChild(0).gameObject.SetActive(true);
                ButtonSpin.transform.GetChild(1).gameObject.SetActive(false);
            }
        }
        string DisPlayTime(int _second)
        {
            int hour = TimeSpan.FromSeconds(_second).Hours;
            int min = TimeSpan.FromSeconds(_second).Minutes;
            int sec = TimeSpan.FromSeconds(_second).Seconds;
            return string.Format("{0:00}:{1:00}:{2:00}", hour, min, sec);
        }
    }
}

