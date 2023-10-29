using Spine.Unity;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
namespace SuperFight
{
    public class BuySkinPopup : PopupUI
    {
        public Button buyButton;
        public Button closeButton;
        private string skinName;
        [SerializeField] SkeletonAnimation skeletonAnimation;
        [SerializeField] SkeletonAnimation skeletonAnimationWin;
        public override void Initialize(PopupManager popupManager)
        {
            base.Initialize(popupManager);
            popupName = PopupName.BUYSKIN;
            buyButton.onClick.AddListener(OnClickBuy);
            closeButton.onClick.AddListener(Hide);
        }
        public void LoadSkinData(string _skinName)
        {
            skinName = _skinName;
            StartCoroutine(_LoadSkinData(_skinName));
        }

        IEnumerator _LoadSkinData(string _skinName)
        {
            yield return new WaitForEndOfFrame();
            skeletonAnimation.skeleton.SetSkin(skinName);
        }

        void OnClickBuy()
        {
            SoundManager.Instance.playSoundFx(SoundManager.Instance.effClickUI);
            if (DataManager.Instance.coin >= 3000)
            {
                DataManager.Instance.UnlockSkin(skinName);
                DataManager.Instance.AddCoin(-3000, 0, "buy_skin");
                DataManager.Instance.currentSkin = skinName;
                skeletonAnimationWin.skeleton.SetSkin(skinName);
                PlayerManager.Instance.SetSkin(skinName);
                gameObject.SetActive(false);
            }
            else
            {
                PopupManager.Instance.ShowPopup(PopupName.SHOPCOIN);
                Hide();
            }
        }


    }
}

