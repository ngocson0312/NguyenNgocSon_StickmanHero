using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
using mygame.sdk;

namespace SuperFight
{
    public class CardSkin : MonoBehaviour
    {
        [SerializeField] int id;
        [SerializeField] Sprite[] SprFlip;
        [SerializeField] Image ImgCard;
        [SerializeField] Button ButtonCard;
        [SerializeField] Button ButtonGetSkin;
        [SerializeField] Button ButtonUnlocked;
        [SerializeField] Button ButtonSelected;
        [SerializeField] TextMeshProUGUI txtProcess;
        [SerializeField] GameObject ObjProcess;
        public string skinName;
        public Spine.Unity.SkeletonAnimation skeletonAnimation;
        public int AdsCount = 10;
        public SkinData skinCard;

        bool isFlip
        {
            get => PlayerPrefs.GetInt("isFlip" + id, 0) == 1;
            set => PlayerPrefs.SetInt("isFlip" + id, value ? 1 : 0);
        }
        public int processSkin
        {
            get => PlayerPrefs.GetInt("processSkin" + id , 0);
            set => PlayerPrefs.SetInt("processSkin" + id , value);
        }
        private void Awake()
        {
            ButtonCard.onClick.AddListener(OnFlipCard);
            ButtonGetSkin.onClick.AddListener(ClickButtonGetSkin);
            ButtonUnlocked.onClick.AddListener(ClickButtonUnlockedSkin);
            skinName = skinCard.listSkin[id].skinName;
        }
        private void OnEnable()
        {
            if (!isFlip)
            {
                DefaultCard();
            }
            else
            {
                txtProcess.text = processSkin + "/" + AdsCount;
                FlipCard();
            }
            
        }
        private void Start()
        {
            /*if (processSkin == AdsCount)
            {
                UnLockedSkin();
            }*/
            
        }
        void DefaultCard()
        {
            ImgCard.sprite = SprFlip[0];
            ObjProcess.SetActive(false);
            ButtonGetSkin.gameObject.SetActive(false);
            ButtonSelected.gameObject.SetActive(false);
            ButtonUnlocked.gameObject.SetActive(false);
            skeletonAnimation.gameObject.SetActive(false);
        }
        void FlipCard()
        {
            ImgCard.sprite = SprFlip[1];
            ImgCard.transform.localEulerAngles = Vector3.zero;
            ObjProcess.SetActive(true);
            txtProcess.text = processSkin + "/" + AdsCount;
            ButtonGetSkin.gameObject.SetActive(true);
            ButtonSelected.gameObject.SetActive(false);
            ButtonUnlocked.gameObject.SetActive(false);
            skeletonAnimation.gameObject.SetActive(true);
            Invoke("LoadSkin",.05f);
            if (processSkin == AdsCount)
            {
                UnLockedSkin();
                if (DataManager.Instance.currentSkin == skinName)
                {
                    SelectedSkin();
                }
            }
        }
        void OnFlipCard()
        {
            if (isFlip) return;
            isFlip = true;
            SoundManager.Instance.playSoundFx(SoundManager.Instance.effFlipCard);
            ImgCard.transform.DOLocalRotate(Vector3.up * 180, .5f, RotateMode.LocalAxisAdd).SetEase(Ease.Linear).OnComplete(()=>{
                FlipCard();
                //partical
            });
        }
        public void LoadSkin()
        {
            skeletonAnimation.skeleton.SetSkin(skinName);
            skeletonAnimation.LateUpdate();
        }
        public void UnlockSkinData(string _skinName)
        {
            FIRhelper.logEvent("unlock_skin_card");
            DataManager.Instance.UnlockSkin(_skinName);
            PlayerManager.Instance.SetSkin(_skinName);
        }
        public void SelectSkinData(string _skinName)
        {
            DataManager.Instance.currentSkin = _skinName;
            PlayerManager.Instance.SetSkin(_skinName);
        }
        void ClickButtonGetSkin()
        {
            int res = AdsHelper.Instance.showGift(GameRes.LevelCommon(), "x2_daily_reward", state =>
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
                    FIRhelper.logEvent("show_gift_x2_daily_reward");
                    GetSkin();
                }
            });
        }
        void GetSkin()
        {
            if (processSkin < AdsCount)
            {
                processSkin++;
                txtProcess.text = processSkin + "/" + AdsCount;
                if (processSkin == AdsCount)
                {
                    SoundManager.Instance.playSoundFx(SoundManager.Instance.effGetNewSkin);
                    UnlockSkinData(skinName);
                    UnLockedSkin();
                }
            }
        }


        void ClickButtonUnlockedSkin()
        {
            SelectedSkin();
        }
        public void UnLockedSkin()
        {
            ObjProcess.SetActive(false);
            ButtonGetSkin.gameObject.SetActive(false);
            ButtonSelected.gameObject.SetActive(false);
            ButtonUnlocked.gameObject.SetActive(true);
        }
        void SelectedSkin()
        {
            GiftCardPopupCtl.Instance.RefreshCard();
            ObjProcess.SetActive(false);
            ButtonGetSkin.gameObject.SetActive(false);
            ButtonSelected.gameObject.SetActive(true);
            ButtonUnlocked.gameObject.SetActive(false);
            SelectSkinData(skinName);
        }
    }
}

