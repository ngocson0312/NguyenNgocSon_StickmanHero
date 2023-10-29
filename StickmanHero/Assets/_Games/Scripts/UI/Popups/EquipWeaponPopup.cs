using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SuperFight
{
    public class EquipWeaponPopup : MonoBehaviour
    {
        [Header("Button")]
        [SerializeField] Button ButtonEquipSword; 
        [SerializeField] Button ButtonEquipBow; 
        [Header("Text")]
        [SerializeField] TextMeshProUGUI textMeshSwordCount;
        [SerializeField] TextMeshProUGUI textMeshBowCount;
        [Header("Sprite")]
        [SerializeField] Sprite sprButtonActive;
        [SerializeField] Sprite sprButtonDeactive;
        [Header("Weapon")]
        [SerializeField] Weapon weaponSword;
        [SerializeField] Weapon weaponBow;
        private void OnEnable()
        {
            Initialize();
        }
        private void Start()
        {
            ButtonEquipSword.onClick.AddListener(EquipSword);
            ButtonEquipBow.onClick.AddListener(EquipBow);
        }
        public void Initialize()
        {
            UpdateText(textMeshSwordCount, DataManager.Instance.itemSword);
            UpdateText(textMeshBowCount, DataManager.Instance.itemBow);
            
        }
        void EquipSword()
        {
            if (DataManager.Instance.itemSword > 0 && PlayerManager.Instance.character.currentWeapon != weaponSword)
            {
                EquipWeapon(weaponSword, DataManager.Instance.itemSword, textMeshSwordCount);
                DataManager.Instance.itemSword--;
            }
        }
        void EquipBow()
        {
            if (DataManager.Instance.itemBow > 0 && PlayerManager.Instance.character.currentWeapon != weaponBow)
            {
                EquipWeapon(weaponBow, DataManager.Instance.itemBow, textMeshBowCount);
                DataManager.Instance.itemBow--;
            }
        }
        void EquipWeapon(Weapon _weapon,int _weaponCount, TextMeshProUGUI _textMesh)
        {
            _weaponCount--;
            UpdateText(_textMesh, _weaponCount);
            PlayerManager.Instance.character.LoadWeapon(_weapon);
            
        }
        void UpdateText(TextMeshProUGUI _textMesh,int _weaponCount)
        {
            _textMesh.text = _weaponCount + "";
        }
        void UpdateButton()
        {

        }
    }
}

