using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SuperFight
{
    public enum TypeGift
    {
        Skin,
        Coin,
        Heart
    }
    public class GiftBox : MonoBehaviour
    {
        [SerializeField] Text txtGift;
        [SerializeField] Image imgGift;
        [SerializeField] Sprite sprCoin;
        public int id;
        public TypeGift type;
        public int amount;
        List<int> ListSkinLocked;
        Skin skin;
        int idSkin
        {
            get => PlayerPrefs.GetInt("idSkin" + id, 0);
            set => PlayerPrefs.SetInt("idSkin" + id, value);
        }
        private void Start()
        {
            DisplayUI();
        }
        public void DisplayUI()
        {
            switch (type)
            {
                case TypeGift.Skin:
                    SetSkinClaim();
                    break;
                case TypeGift.Coin:
                    txtGift.text = "+" + amount;
                    break;
                case TypeGift.Heart:
                    txtGift.text = "+" + amount;
                    break;
                default:
                    break;
            }
        }
        public void ClaimGift()
        {
            switch (type)
            {
                case TypeGift.Skin:
                    ClaimSkin();
                    break;
                case TypeGift.Coin:
                    ClaimCoin();
                    break;
                case TypeGift.Heart:
                    ClaimHeart();
                    break;
                default:
                    break;
            }
            mygame.sdk.FIRhelper.logEvent("claim_reward_lucky_wheel");
        }
        void ClaimCoin()
        {
            DataManager.Instance.AddCoin(amount, 0, "Spin");
        }
        void ClaimHeart()
        {
            DataManager.Instance.AddCoin(amount, 0, "Spin");
        }
        void SetSkinClaim()
        {
            DataManager _dataManager = DataManager.Instance;
            SkinData _skinDataCoin = DataManager.Instance.skinDataCoin;
            
            if (!_dataManager.IsUnlockSkin(_skinDataCoin.listSkin[idSkin].skinName)) 
            {
                imgGift.sprite = _skinDataCoin.listSkin[idSkin].avatar;
                return;
            }
            ListSkinLocked = new List<int>();
            for (int i = 0; i < _skinDataCoin.listSkin.Length; i++)
            {
                if (!_dataManager.IsUnlockSkin(_skinDataCoin.listSkin[i].skinName))
                {
                    ListSkinLocked.Add(i);
                }
            }
            if (ListSkinLocked.Count > 0)
            {
                var rd = Random.Range(0, ListSkinLocked.Count);
                idSkin = ListSkinLocked[rd];
                imgGift.sprite = _skinDataCoin.listSkin[idSkin].avatar;
                skin = _skinDataCoin.listSkin[idSkin];
                txtGift.text = "Skin";
            }
            else
            {
                //Full skin
                type = TypeGift.Coin;
                amount = 5000;
                txtGift.text = "+" + amount;
                imgGift.sprite = sprCoin;
            }
        }
        
        void ClaimSkin()
        {
            
            if (ListSkinLocked.Count > 0)
            {
                SkinData _skinDataCoin = GameManager.Instance.luckWheelPopup.skinDataCoin;
                GameManager.Instance.luckWheelPopup.dataManager.UnlockSkin(_skinDataCoin.listSkin[idSkin].skinName);
                DataManager.Instance.currentSkin = skin.skinName;
                PlayerManager.Instance.SetSkin(skin.skinName);
                GameManager.Instance.luckWheelPopup.ShowNewSkinPopup(skin.skinName);
                idSkin = 0;
            }
        }
    }
}

