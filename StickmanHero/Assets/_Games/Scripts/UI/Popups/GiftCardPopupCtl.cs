using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;

namespace SuperFight
{
    public class GiftCardPopupCtl : MonoBehaviour
    {
        public static GiftCardPopupCtl Instance;
        [SerializeField] CardSkin[] cardSkins;
        [SerializeField] Button ButtonBack;
        public SkeletonMecanim skeletonMecanim;
        public ParticleSystem FXFirework;
        private void Awake()
        {
            Instance = this;
            ButtonBack.onClick.AddListener(Hide);
        }
        
        public void RefreshCard()
        {
            for (int i = 0; i < cardSkins.Length; i++)
            {
                if (cardSkins[i].AdsCount == cardSkins[i].processSkin)
                {
                    cardSkins[i].UnLockedSkin();
                }
            }
        }
        void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}

