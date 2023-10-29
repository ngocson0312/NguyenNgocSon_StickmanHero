using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using mygame.sdk;

namespace SuperFight
{
    public class Pack : MonoBehaviour
    {
        [SerializeField] Sprite sprLock;
        [SerializeField] Sprite sprUnlock;
        [SerializeField] Sprite sprSelected;
        [SerializeField] Sprite sprPreview;

        [SerializeField] GameObject _lock;
        [SerializeField] GameObject levelLockText;

        public Image iconSkin;

        [SerializeField] Button ButtonPack;

        public string thisPackSkinName;
        public int levelUnlock;
        public SkinType skinType;
        private SkinPopupCtl skinPopupCtl;
        public Skin skinData;
        private void Awake()
        {
            ButtonPack.onClick.AddListener(ClickButtonPack);
        }

        public void LoadPackData(Skin _skin, SkinPopupCtl skinPopupCtl)
        {
            this.skinPopupCtl = skinPopupCtl;
            skinData = _skin;
            iconSkin.sprite = _skin.avatar;
            thisPackSkinName = _skin.skinName;
            levelUnlock = _skin.levelUnlock;
            skinType = _skin.skinType;
            levelLockText.GetComponent<TextMeshProUGUI>().text = "level " + _skin.levelUnlock.ToString();
        }

        private void OnEnable()
        {
            StartCoroutine(DelayCheckUnlock());
        }

        IEnumerator DelayCheckUnlock()
        {
            yield return new WaitForSeconds(0.02f);
            if (DataManager.Instance.IsUnlockSkin(thisPackSkinName))
            {
                _lock.SetActive(false);
                levelLockText.gameObject.SetActive(false);
                if (DataManager.Instance.currentSkin == thisPackSkinName) GetComponent<Image>().sprite = sprSelected;
                else GetComponent<Image>().sprite = sprUnlock;
            }
            else
            {
                _lock.SetActive(true);
                if (GameRes.GetLevel() < levelUnlock)
                {
                    levelLockText.gameObject.SetActive(true);
                }
                else
                {
                    levelLockText.gameObject.SetActive(false);
                }
                GetComponent<Image>().sprite = sprLock;
            }
            if (levelUnlock <= 0) levelLockText.gameObject.SetActive(false);
        }

        public void SetToNormal()
        {
            GetComponent<Image>().sprite = sprUnlock;
        }

        public void SetSelected()
        {
            GetComponent<Image>().sprite = sprSelected;
        }

        public void Unlock()
        {
            _lock.SetActive(false);
            levelLockText.gameObject.SetActive(false);
        }

        void ClickButtonPack()
        {
            skinPopupCtl.LoadPreviewSkin(this, true);
            if (DataManager.Instance.IsUnlockSkin(thisPackSkinName))
            {
                GetComponent<Image>().sprite = sprSelected;
                DataManager.Instance.currentSkin = thisPackSkinName;
            }
            else
            {
                GetComponent<Image>().sprite = sprPreview;
            }
        }

    }
}

