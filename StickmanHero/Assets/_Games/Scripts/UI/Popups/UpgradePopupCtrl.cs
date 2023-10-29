using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using mygame.sdk;
using SuperFight;
using DG.Tweening;

public class UpgradePopupCtrl : MonoBehaviour
{
    int stateShowAds = 0;
    [Header("Text")]
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] TextMeshProUGUI maxHpText;
    [SerializeField] TextMeshProUGUI maxHpAddText;
    [SerializeField] TextMeshProUGUI atkText;
    [SerializeField] TextMeshProUGUI atkAddText;
    [SerializeField] TextMeshProUGUI upgradePriceText;

    [Header("Button")]
    [SerializeField] Button upgradeCoin;
    [SerializeField] Button adsButton;
    [SerializeField] Button goToShopButton;

    [Header("VFx")]
    [SerializeField] ParticleSystem upgradeFx;

    private int curUpgradeHp, curUpgradeDamage;

    public int canWatchAd
    {
        get
        {
            return PlayerPrefs.GetInt("canWatchAd", 1);
        }
        set
        {
            PlayerPrefs.SetInt("canWatchAd", value);
        }
    }

    private void Awake()
    {
        upgradeCoin.onClick.AddListener(OnClickCoin);
    }

    private void OnEnable()
    {
        stateShowAds = 0;
        LoadStats(false);
    }

    private void TextAnim()
    {
        int hp1 = DataManager.Instance.playerMaxHp;
        int hp2 = (int)Mathf.Round(DataManager.Instance.playerMaxHp * 0.15f);
        //DOTween.To(() => hp1, x => hp1 = x, hp1 + hp2, 0.4f).OnUpdate(() =>
        //{
        //    maxHpText.text = "HP: " + hp1;
        //});

        int atk1 = DataManager.Instance.playerDamage;
        int atk2 = (int)Mathf.Round(DataManager.Instance.playerDamage * 0.15f);
        //DOTween.To(() => atk1, x => atk1 = x, atk1 + atk2, 0.4f).OnUpdate(() =>
        //{
        //    atkText.text = "ATK: " + atk1;
        //});
        maxHpText.text = "HP: " + DataManager.Instance.playerMaxHp;
        atkText.text = "ATK: " + DataManager.Instance.playerDamage;
    }

    private void LoadStats(bool canAnim = true)
    {
        levelText.text = DataManager.Instance.playerLevel.ToString();


        float randomNum = 0;
        if (DataManager.Instance.playerLevel <= 5)
        {
            randomNum = 0.15f;
        }
        else if (DataManager.Instance.playerLevel <= 10)
        {
            randomNum = 0.2f;
        }
        else if (DataManager.Instance.playerLevel <= 15)
        {
            randomNum = 0.25f;
        }
        else
        {
            randomNum = 0.3f;
        }
        curUpgradeHp = (int)(DataManager.Instance.baseHp * randomNum);
        curUpgradeDamage = (int)(DataManager.Instance.baseDamage * randomNum);

        if (canAnim)
        {
            TextAnim();
        }
        else
        {
            maxHpText.text = "HP: " + DataManager.Instance.playerMaxHp;
            atkText.text = "ATK: " + DataManager.Instance.playerDamage;
        }

        upgradePriceText.text = DataManager.Instance.playerUpgradePrice.ToString();
        maxHpAddText.text = curUpgradeHp.ToString();
        atkAddText.text = curUpgradeDamage.ToString();
        upgradeCoin.gameObject.SetActive(true);
    }

    public void UpgradePlayer()
    {
        upgradeFx.Play();
        DataManager.Instance.UpgradePlayer(curUpgradeHp, curUpgradeDamage);
        LoadStats();
        SoundManager.Instance.playSoundFx(SoundManager.Instance.effUpgrade);
    }

    void OnClickCoin()
    {
        SoundManager.Instance.playSoundFx(SoundManager.Instance.effClickUI);
        if (DataManager.Instance.coin >= DataManager.Instance.playerUpgradePrice)
        {
            DataManager.Instance.AddCoin(-DataManager.Instance.playerUpgradePrice, 0, "upgrade player");
            UpgradePlayer();
            canWatchAd = 1;
        }
        else
        {
            GameManager.Instance.ShowNotEnough(this);
        }
    }

    void OnClickAd()
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
                    UpgradePlayer();
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
}

