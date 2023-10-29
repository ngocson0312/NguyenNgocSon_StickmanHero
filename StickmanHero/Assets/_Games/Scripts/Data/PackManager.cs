using System.Collections;
using System.Collections.Generic;
using mygame.sdk;
using UnityEngine;
namespace SuperFight
{
    public class PackManager : Singleton<PackManager>
    {

        public PopupPack[] superLevelUp;

        public Transform popupHolder;
        private int groupType;
        private int currentSuperLevelUpPack
        {
            get { return PlayerPrefs.GetInt("current_splevel_index", 0); }
            set { PlayerPrefs.SetInt("current_splevel_index", value); }
        }

        protected override void Awake()
        {
            base.Awake();

        }

     

    }
    [System.Serializable]
    public class PackInfo
    {
        public PackInfo()
        {

        }
        public PackInfo(string packName, PackType packType)
        {
            this.packName = packName;
            this.packType = packType;
        }
        public PackType packType;
        public string packName;
    }
    [System.Serializable]
    public class PopupPack
    {
        public InappData packInfo;
        public GameObject popupPrefab;
    }
    public enum PackType
    {
        CONSUM, PACK, SUBSCRIPTION
    }
}
