using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;

namespace SuperFight
{
    public class NewSkinPopup : MonoBehaviour
    {
        [SerializeField] Button ButtonClaim;
        public SkeletonAnimation skeletonAnimation;
        private void Awake()
        {
            ButtonClaim.onClick.AddListener(Hide);
        }
        public void SetSkinClaim(string _skinName)
        {
            mygame.sdk.FIRhelper.logEvent("unlock_skin_lucky_wheel");
            skeletonAnimation.skeleton.SetSkin(_skinName);
        }
        void Hide()
        {
            SoundManager.Instance.playSoundFx(SoundManager.Instance.effClickUI);
            ResetUI();
            gameObject.SetActive(false);
        }
        void ResetUI()
        {
            Spin _spin = GameManager.Instance.luckWheelPopup.spin;
            int _boxCount = _spin.box.Length;

            for (int i = 0; i < _boxCount; i++)
            {
                _spin.box[i].DisplayUI();
            }
        }
    }
}


