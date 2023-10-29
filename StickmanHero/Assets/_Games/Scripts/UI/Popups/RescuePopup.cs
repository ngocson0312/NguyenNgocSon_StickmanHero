using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Spine.Unity;
using SuperFight;
using mygame.sdk;

public class RescuePopup : MonoBehaviour
{
    [SerializeField] float timeWait = 3f;
    [SerializeField] Button ButtonAds;
    [SerializeField] Button ButtonNoThanks;
    [SerializeField] SkeletonAnimation skeletonAnimation;
    [SerializeField] SkeletonAnimation skeletonAnimationWin;
    [SerializeField] SkeletonMecanim skeletonAnimationMain;

    public string skinName;
    int stateShowAds = 0;

    private void Awake()
    {
        ButtonAds.onClick.AddListener(ClickButtonAds);
        ButtonNoThanks.onClick.AddListener(ClickNothanks);
    }
    private void OnEnable()
    {
        StartCoroutine(IEWait());
        skeletonAnimation.skeleton.SetSkin(skinName);
        stateShowAds = 0;
    }

    IEnumerator IEWait()
    {
        ButtonNoThanks.gameObject.SetActive(false);
        yield return new WaitForSeconds(timeWait);
        ButtonNoThanks.gameObject.SetActive(true);
    }
    void ClickButtonAds()
    {
        SoundManager.Instance.playSoundFx(SoundManager.Instance.effClickUI);
        stateShowAds = 0;
        int res = AdsHelper.Instance.showGift(GameRes.LevelCommon(), "get_skin_free", state =>
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
                    DataManager.Instance.UnlockSkin(skinName);
                    DataManager.Instance.currentSkin = skinName;
                    skeletonAnimationWin.skeleton.SetSkin(skinName);
                    PlayerManager.Instance.SetSkin(skinName);
                    gameObject.SetActive(false);
                }
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
    void ClickNothanks()
    {
        gameObject.SetActive(false);
    }
}
