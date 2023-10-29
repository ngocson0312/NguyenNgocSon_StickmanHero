using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;
using TMPro;
using mygame.sdk;

namespace SuperFight
{
    public class SkinPopupCtl : MonoBehaviour
    {
        [Header("Button")]
        [SerializeField] Button ButtonTry;
        [SerializeField] Button ButtonClose;
        [SerializeField] Button coinButton;
        [SerializeField] Button levelLockButton;
        [SerializeField] Button adsButton;
        [SerializeField] Button specialButton;
        [SerializeField] Button iapButton;
        [Header("Button Tab")]
        [SerializeField] Button[] tabButton;
        [SerializeField] Transform[] packSkin;
        [SerializeField] SkinData[] skinData;
        [SerializeField] Sprite[] previewBorder;
        [SerializeField] GameObject[] previewBg;
        [SerializeField] Image previewBorderImage;
        private List<Pack> allPackInShop = new List<Pack>();
        [Header("Sprite")]
        public Sprite[] tabSprite;
        public SkeletonAnimation[] skeletonAnimation;
        //public SkeletonMecanim skeletonMecanim;
        private Pack curPack;
        Coroutine cor_combo;

        public ParticleSystem buyFx;

        int stateShowAds = 0;
        public void Initialize()
        {
            ButtonClose.onClick.AddListener(ClickButtonClose);
            tabButton[0].onClick.AddListener(() => GoTabSkin(0));
            tabButton[1].onClick.AddListener(() => GoTabSkin(1));
            tabButton[2].onClick.AddListener(() => GoTabSkin(2));
            tabButton[3].onClick.AddListener(() => GoTabSkin(3));
            coinButton.onClick.AddListener(OnClickBtnCoin);
            adsButton.onClick.AddListener(OnCkickBtnAds);
            iapButton.onClick.AddListener(OnClickBtnIap);
            ButtonTry.onClick.AddListener(OnClickBtnTryPrestige);
            InstantiatePack();
        }

        private void Start()
        {
            GoTabSkin(0);
            //coinButton.GetComponentInChildren<TextMeshProUGUI>().text = DataManager.Instance.skinPrice.ToString();
        }

        void InstantiatePack()
        {
            for (int i = 0; i < skinData.Length; i++)
            {
                Transform content = packSkin[i].GetChild(0).GetChild(0).GetChild(0);
                Pack _pack = content.GetChild(0).GetComponent<Pack>();
                for (int j = 0; j < skinData[i].listSkin.Length; j++)
                {
                    Pack _curPack = Instantiate(_pack, content);
                    _curPack.gameObject.SetActive(true);
                    _curPack.LoadPackData(skinData[i].listSkin[j], this);
                    allPackInShop.Add(_curPack);
                }
            }
        }
        public void Show()
        {
            stateShowAds = 0;
            gameObject.SetActive(true);
            StartCoroutine(CheckCurSkin());
        }
        IEnumerator CheckCurSkin()
        {
            yield return new WaitForSeconds(0.05f);
            if (curPack != null) curPack.SetToNormal();
            for (int i = 0; i < allPackInShop.Count; i++)
            {
                if (allPackInShop[i].thisPackSkinName == DataManager.Instance.currentSkin)
                {
                    curPack = allPackInShop[i];
                    break;
                }
            }
            LoadPreviewSkin(curPack, false);
        }

        void ClickButtonClose()
        {
            SoundManager.Instance.playSoundFx(SoundManager.Instance.effClickUI);
            gameObject.SetActive(false);
        }
        void GoTabSkin(int _tab)
        {
            for (int i = 0; i < tabButton.Length; i++)
            {
                if (_tab == i)
                {
                    previewBg[i].SetActive(true);
                    packSkin[i].gameObject.SetActive(true);
                    tabButton[i].image.sprite = tabSprite[0];
                }
                else
                {
                    previewBg[i].SetActive(false);
                    packSkin[i].gameObject.SetActive(false);
                    tabButton[i].image.sprite = tabSprite[1];
                }
            }
            previewBorderImage.sprite = previewBorder[_tab];
        }
        public void LoadPreviewSkin(Pack _pack, bool canReload)
        {
            //if (curPack == null) return;
            curPack.SetToNormal();
            curPack = _pack;
            int idx = 0;
            if (curPack.thisPackSkinName.Contains("Diana") || curPack.thisPackSkinName.Contains("DrStrange"))
            {
                skeletonAnimation[0].gameObject.SetActive(false);
                skeletonAnimation[1].gameObject.SetActive(true);
                idx = 1;
            }
            else
            {
                skeletonAnimation[0].gameObject.SetActive(true);
                skeletonAnimation[1].gameObject.SetActive(false);
                idx = 0;
            }

            skeletonAnimation[idx].skeleton.SetSkin(curPack.thisPackSkinName);
            skeletonAnimation[idx].Skeleton.SetSlotsToSetupPose();
            skeletonAnimation[idx].skeleton.SetToSetupPose();
            if (curPack.skinData.character != null)
            {
                skeletonAnimation[idx].GetComponent<LoadSkinWeapon>().LoadWeapon(curPack.skinData.character.defaultWeapon);
            }
            else
            {
                skeletonAnimation[idx].GetComponent<LoadSkinWeapon>().LoadWeapon(null);
            }
            if (idx == 0)
            {
                ChangeIdle(curPack.thisPackSkinName, curPack.skinType);
            }
            SetButton();
            if (canReload)
            {
                if (DataManager.Instance.IsUnlockSkin(curPack.thisPackSkinName))
                {
                    DataManager.Instance.currentSkin = curPack.thisPackSkinName;
                    PlayerManager.Instance.SetSkin(curPack.thisPackSkinName);
                }
            }
            curPack.SetSelected();
        }

        void SetButton()
        {
            coinButton.gameObject.SetActive(false);
            levelLockButton.gameObject.SetActive(false);
            adsButton.gameObject.SetActive(false);
            specialButton.gameObject.SetActive(false);
            iapButton.gameObject.SetActive(false);
            //if (curPack == null) return;
            if (DataManager.Instance.IsUnlockSkin(curPack.thisPackSkinName))
            {
                //DataManager.Instance.currentSkin = curPack.thisPackSkinName;
                //PlayerManager.Instance.SetSkin(curPack.thisPackSkinName);
            }
            else
            {
                if (curPack.skinType == SkinType.NORMAL)
                {
                    if (curPack.levelUnlock > 0)
                    {
                        if (curPack.levelUnlock <= GameRes.GetLevel())
                        {
                            adsButton.gameObject.SetActive(true);
                        }
                        else
                        {
                            levelLockButton.gameObject.SetActive(true);
                            levelLockButton.GetComponentInChildren<TextMeshProUGUI>().text = "Unlock at level: " + curPack.levelUnlock;
                        }
                    }
                    else
                    {
                        //ButtonTry.gameObject.SetActive(true);
                        coinButton.gameObject.SetActive(true);
                        SetPriceSkinNormal(curPack.skinData);
                    }
                }
                else if (curPack.skinType == SkinType.PESTIGE)
                {
                    iapButton.gameObject.SetActive(true);
                    string id = curPack.thisPackSkinName.Remove(0, 9).ToLower();
                    var _skinName = id.Replace(" ", "");
                    string price = InappHelper.Instance.getPrice("skin" + _skinName);
                    iapButton.GetComponentInChildren<TextMeshProUGUI>().text = price;

                }
                else
                {
                    specialButton.gameObject.SetActive(true);
                    switch (curPack.skinType)
                    {
                        case SkinType.CARD:
                            {
                                specialButton.GetComponentInChildren<TextMeshProUGUI>().text = "Unlock In Mystery Card";
                                break;
                            }
                        case SkinType.DAILY_GIFT:
                            {
                                specialButton.GetComponentInChildren<TextMeshProUGUI>().text = "Unlock In DailyGift";
                                break;
                            }
                    }
                }
            }
        }

        void UnlockSkin()
        {
            DataManager.Instance.UnlockSkin(curPack.thisPackSkinName);
            DataManager.Instance.currentSkin = curPack.thisPackSkinName;
            curPack.Unlock();
            SoundManager.Instance.playSoundFx(SoundManager.Instance.effClickUI);
            PlayerManager.Instance.SetSkin(curPack.thisPackSkinName);
            SetButton();
            buyFx.Play();
            curPack.SetSelected();
        }

        void OnClickBtnCoin()
        {
            SoundManager.Instance.playSoundFx(SoundManager.Instance.effClickUI);
            if (DataManager.Instance.coin >= DataManager.Instance.skinPrice)
            {
                FIRhelper.logEvent("unlock_skin_coin");
                UnlockSkin();
                DataManager.Instance.AddCoin(-DataManager.Instance.skinPrice, 0, "buy_skin");
            }
            else
            {
                PopupManager.Instance.ShowPopup(PopupName.SHOPCOIN);
            }
        }

        void OnCkickBtnAds()
        {
            SoundManager.Instance.playSoundFx(SoundManager.Instance.effClickUI);
            stateShowAds = 0;
            int res = AdsHelper.Instance.showGift(GameRes.LevelCommon(), "get_skin_free_inshop", state =>
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
                        FIRhelper.logEvent("unlock_skin_ads");
                        UnlockSkin();
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

        void OnClickBtnIap()
        {
            string id = curPack.thisPackSkinName.Remove(0, 9).ToLower();
            string _packname = id.Replace(" ", "");
            string skuid = "skin" + _packname;
            InappHelper.Instance.BuyPackage(skuid, "buy_skin_in_skin_shop", delegate (PurchaseCallback callback)
            {
                if (callback.status == 1)
                {
                    UnlockSkin();
                }
            });
        }

        void OnClickBtnTry()
        {
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
                        DataManager.Instance.currentTrySkin = curPack.thisPackSkinName;
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

        void OnClickBtnTryPrestige()
        {
            Debug.Log("a1");
            SoundManager.Instance.playSoundFx(SoundManager.Instance.effClickUI);
            Debug.Log("a2");
            ScreenUIManager.Instance.ShowScreen(ScreenName.PLAYSCREEN);
            Debug.Log("a3");
            GameManager.Instance.PlayLevelTest();
            Debug.Log("a4");
            PlayerManager.Instance.SetSkin(curPack.thisPackSkinName);
            Debug.Log("a5");
            gameObject.SetActive(false);
        }
        void SetPriceSkinNormal(Skin skin)//skinCoin
        {
            int _skinID = 0;
            for (int i = 0; i < skinData[0].listSkin.Length; i++)
            {
                if (skinData[0].listSkin[i].skinName == skin.skinName)
                {
                    _skinID = i;
                    break;
                }
            }
            DataManager.Instance.skinPrice = 3000 + (_skinID - 1) * 2000;
            coinButton.GetComponentInChildren<TextMeshProUGUI>().text = DataManager.Instance.skinPrice.ToString();
        }
        void ChangeIdle(string _skinName, SkinType _skinType)
        {
            string _keyPose = "1_/idle active";
            if (_skinType == SkinType.PESTIGE)
            {
                string _name = _skinName.Remove(0, 9);
                if (_name == "Riven")
                {
                    _keyPose = "Kratos/Idle";
                }
                else
                {
                    _keyPose = _name + "/Idle";
                }
            }
            else
            {
                _keyPose = "1_/idle active";
            }
            skeletonAnimation[0].AnimationName = _keyPose;
            if (cor_combo != null) StopCoroutine(cor_combo);
            cor_combo = StartCoroutine(IE_PlayCombo(_skinName, _skinType));
        }
        IEnumerator IE_PlayCombo(string _skinName, SkinType _skinType)
        {
            string _keyPose;
            if (_skinType == SkinType.PESTIGE)
            {
                string _name = _skinName.Remove(0, 9);
                if (_name == "Riven" || _name == "Kaisa")
                {
                    int rd = Random.Range(1, 3);
                    if (rd == 1)
                    {
                        _keyPose = _name + "/Attack ";
                    }
                    else
                    {
                        _keyPose = _name + "/Attack " + rd;
                    }
                }
                else
                {
                    if (_name == "Yasuo" || _name == "Vi" || _name == "ViraBot" ||
                        _name == "Akali" || _name == "Lux")
                    {
                        int rd = Random.Range(1, 4);
                        _keyPose = _name + "/Attack " + rd;
                    }
                    else
                    {
                        int rd = Random.Range(1, 3);
                        _keyPose = _name + "/Attack " + rd;
                    }
                }
                Debug.Log("Key pose = " + _keyPose);
                skeletonAnimation[0].AnimationName = _keyPose;
                yield return new WaitForSeconds(3f);
                if (cor_combo != null) StopCoroutine(cor_combo);
                cor_combo = StartCoroutine(IE_PlayCombo(_skinName, _skinType));
            }
        }

        private void OnEnable()
        {
            if (cor_combo != null) StopCoroutine(cor_combo);
        }
    }


}

