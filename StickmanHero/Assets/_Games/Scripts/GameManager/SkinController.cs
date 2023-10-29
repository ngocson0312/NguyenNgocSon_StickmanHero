using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SuperFight
{
    public class SkinController : MonoBehaviour
    {
        public static SkinController Instance;
        private int idTry = -1;


        public int GetTypeSkinSelect()
        {
            return PlayerPrefs.GetInt("idTypeSkinSelect");
        }

        public int idTypeSkinSelect
        {
            get { return PlayerPrefs.GetInt("idTypeSkinSelect", 0); }
            set { PlayerPrefs.SetInt("idTypeSkinSelect", value); }
        }

        //public SkeletonDataAsset[] ObjectSkin;
        [SerializeField] SkeletonDataAsset[] SkeletonSkinNormal;
        [SerializeField] SkeletonDataAsset[] SkeletonSkinPremium;


        [SerializeField] Sprite[] iconSkinNormal;
        [SerializeField] Sprite[] iconSkinPremium;


        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            //ObjectSkin[0].AnimationState.SetAnimation(0, "run", true);
        }

    }
}

