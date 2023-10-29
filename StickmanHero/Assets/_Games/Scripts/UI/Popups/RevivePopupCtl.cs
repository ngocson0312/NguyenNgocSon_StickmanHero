using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using mygame.sdk;

namespace SuperFight
{
    public class RevivePopupCtl : MonoBehaviour
    {
        [SerializeField] Button ButtonRevive;
        [SerializeField] Button NoThanks;
        [SerializeField] Image ImgCircle;
        [SerializeField] TextMeshProUGUI txtTimer;
        float currentTime;
        int stateShowAds = 0;

        private void Awake()
        {
            ButtonRevive.onClick.AddListener(ClickButtonRevive);
            NoThanks.onClick.AddListener(OnClickNoThanks);
        }
        private void OnEnable()
        {
            stateShowAds = 0;
            currentTime = Constant.timeCountdownRevive;
            ImgCircle.fillAmount = 1;
            txtTimer.text = "" + currentTime;
            StartCoroutine(countdownTime());
            StartCoroutine(IEWait());
            // ShowTimer();
        }

        private IEnumerator countdownTime()
        {
            while (currentTime > 0)
            {
                if (stateShowAds <= 0)
                {
                    currentTime -= Time.deltaTime;
                    ImgCircle.fillAmount = currentTime / Constant.timeCountdownRevive;
                    txtTimer.text = ((int)currentTime).ToString();
                }
                yield return new WaitForEndOfFrame();
            }
            OnTimeOut();
        }

        private void OnTimeOut()
        {
            gameObject.SetActive(false);
            GameManager.Instance.TimeoutRevive();
        }
        IEnumerator IEWait()
        {
            NoThanks.gameObject.SetActive(false);
            yield return new WaitForSeconds(3f);
            NoThanks.gameObject.SetActive(true);
        }

        public void ClickButtonRevive()
        {
            SoundManager.Instance.playSoundFx(SoundManager.Instance.effClickUI);
            stateShowAds = 0;
            int res = AdsHelper.Instance.showGift(GameRes.LevelCommon(), "second_change", state =>
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
                        FIRhelper.logEvent("show_gift_revive");
                        GameManager.Instance.OnRevival();
                        gameObject.SetActive(false);
                    }
                    else
                    {
                        OnTimeOut();
                    }
                }
                else if (state == AD_State.AD_REWARD_FAIL)
                {
                    stateShowAds = 0;
                    OnTimeOut();
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

        void OnClickNoThanks()
        {
            OnTimeOut();
        }

    }
}

