using System.Collections.Generic;
using UnityEngine;
using mygame.sdk;
using System.Linq;
using System;
using SuperFight;
using SuperFightData;

public class DataManager : Singleton<DataManager>
{
    public static event System.Action<int, int, float, bool> OnAddCoin = delegate { };
    public static event System.Action<int, int, float> OnAddGem = delegate { };
    public static event System.Action onAddPack;
    public PlayerData data;
    private string userPrefData
    {
        get { return PlayerPrefs.GetString("sf_user_data", ""); }
        set { PlayerPrefs.SetString("sf_user_data", value); }
    }
    [Header("Player Stats")]
    [SerializeField] private int baseCoin = 0;
    [SerializeField] private int baseGem = 0;
    [SerializeField] public int baseHp = 50;
    [SerializeField] public int baseDamage = 10;
    [SerializeField] private int baseUpgradePrice = 300;

    [Header("Shop Skin Stats")]
    [SerializeField] public int skinPrice = 3000;
    public SkinContainer skinContainer;
    public SkinData skinDataPrestige;
    public SkinData skinDataCoin;
    public SkinData skinDataRescue;
    public SkinData SkinDataDaily;
    public SkinData SkinDataCard;
    private void Start()
    {
        UnlockSkin("Base");
    }
    public int coin
    {
        get
        {
            return PlayerPrefs.GetInt("coin", baseCoin);
        }
        set
        {
            PlayerPrefs.SetInt("coin", value);
        }
    }

    public int gem
    {
        get
        {
            return PlayerPrefs.GetInt("gem", baseGem);
        }
        set
        {
            PlayerPrefs.SetInt("gem", value);
        }
    }

    public int spineTicket
    {
        get
        {
            return PlayerPrefs.GetInt("spineTicket", baseGem);
        }
        set
        {
            PlayerPrefs.SetInt("spineTicket", value);
        }
    }

    public int luckyChest
    {
        get
        {
            return PlayerPrefs.GetInt("luckyChest", baseGem);
        }
        set
        {
            PlayerPrefs.SetInt("luckyChest", value);
        }
    }

    public int itemSword
    {
        get
        {
            return PlayerPrefs.GetInt("itemSword", baseGem);
        }
        set
        {
            PlayerPrefs.SetInt("itemSword", value);
        }
    }

    public int itemBow
    {
        get
        {
            return PlayerPrefs.GetInt("itemBow", baseGem);
        }
        set
        {
            PlayerPrefs.SetInt("itemBow", value);
        }
    }

    public int playerLevel
    {
        get
        {
            return PlayerPrefs.GetInt("playerLevel", 1);
        }
        set
        {
            PlayerPrefs.SetInt("playerLevel", value);
        }
    }
    public int playerMaxHp
    {
        get
        {
            return PlayerPrefs.GetInt("playerMaxHp", baseHp);
        }
        set
        {
            PlayerPrefs.SetInt("playerMaxHp", value);
        }
    }

    public int playerUpgradePrice
    {
        get
        {
            return PlayerPrefs.GetInt("playerUpgradePrice", baseUpgradePrice);
        }
        set
        {
            PlayerPrefs.SetInt("playerUpgradePrice", value);
        }
    }

    public int playerDamage
    {
        get
        {
            return PlayerPrefs.GetInt("playerDamage", baseDamage);
        }
        set
        {
            PlayerPrefs.SetInt("playerDamage", value);
        }
    }

    public string currentSkin
    {
        get
        {
            return PlayerPrefs.GetString("currentSkin", "Base");
        }
        set
        {
            //SkinManager.Instance.ReloadSkin(value);
            PlayerPrefs.SetString("currentSkin", value);
        }
    }

    public string currentTrySkin
    {
        get
        {
            return PlayerPrefs.GetString("currentTrySkin", "");
        }
        set
        {
            PlayerPrefs.SetString("currentTrySkin", value);
        }
    }
    public int currentBossLevel
    {
        get { return PlayerPrefs.GetInt("hell_mode_level", 2); }
        set { PlayerPrefs.SetInt("hell_mode_level", value); }
    }
    public int currentBossLevelSelect
    {
        get { return PlayerPrefs.GetInt("hell_mode_level_select", 1); }
        set { PlayerPrefs.SetInt("hell_mode_level_select", value); }
    }
    public void UpgradePlayer(int hp, int damage)
    {
        playerLevel++;
        playerMaxHp += hp;
        playerDamage += damage;
        if (playerLevel <= 5)
        {
            playerUpgradePrice += 35;
        }
        else if (playerLevel <= 10)
        {
            playerUpgradePrice += 170;
        }
        else if (playerLevel <= 15)
        {
            playerUpgradePrice += 400;
        }
        else
        {
            playerUpgradePrice += 600;
        }
       
    }
    public SkinObject GetSkin(string skinName)
    {
        SkinObject s = Array.Find(skinContainer.container, s => s.skinName == skinName);
        return s;
    }
    public void AddCoin(int value, float delayTime, string where, bool showanim = true)
    {
        OnAddCoin(coin, coin + value, delayTime, showanim);
        GameRes.AddRes(RES_type.GOLD, value, where, true, 0);
        coin += value;
    }
    public void AddSpineTicket(int value)
    {
        spineTicket += value;
    }

    public void AddLuckyChest(int value)
    {
        luckyChest += value;
    }

    public void AddGem(int value, float delayTime, string where)
    {
        OnAddGem(gem, gem + value, delayTime);
        gem += value;
    }
    public void AddHeart(int value, string where)
    {
        GameRes.AddRes(RES_type.HEART, value, where);
    }
    public void UnlockSkin(string skinName)
    {
        string pref = "Skin_" + skinName;
        PlayerPrefs.SetInt(pref, 1);
    }
    public bool IsUnlockSkin(string skinName)
    {
        if (skinName == "Base") return true;
        string pref = "Skin_" + skinName;
        if (PlayerPrefs.GetInt(pref, 0) == 0)
        {
            return false;
        }
        else return true;
    }
    public void LoadData()
    {
        string pref = userPrefData;
        if (!string.IsNullOrEmpty(pref))
        {
            data = JsonUtility.FromJson<PlayerData>(userPrefData);
        }
        else
        {
            data = new PlayerData();
        }
        Debug.Log("Load:" + pref);
    }
    public void SaveData()
    {
        string dataJson = JsonUtility.ToJson(data);
        userPrefData = dataJson;
        Debug.Log("Save:" + dataJson);
    }
    public void OnPurchasePack(InappData inappData)
    {
        if (data.IsContainPack(inappData.packageName))
        {
            data.GetPack(inappData.packageName);
        }
        data.AddPack(new PackInfo(inappData.packageName, inappData.packType));
        onAddPack?.Invoke();
        SaveData();
    }
    public bool SoldOut(string packName, PackType packType)
    {
        if (packType == PackType.CONSUM || packType == PackType.SUBSCRIPTION)
        {
            return false;
        }

        return data.IsContainPack(packName);
    }
}
namespace SuperFightData
{
    [System.Serializable]
    public class PlayerData
    {
        public PlayerData()
        {
            skinOwners = new List<string>();
            packOwner = new List<PackInfo>();
        }
        public void AddPack(PackInfo packInfo)
        {
            packOwner.Add(packInfo);
        }
        public bool IsContainPack(string packName)
        {
            for (int i = 0; i < packOwner.Count; i++)
            {
                if (packOwner[i].packName == packName)
                {
                    return true;
                }
            }
            return false;
        }
        public PackInfo GetPack(string packName)
        {
            for (int i = 0; i < packOwner.Count; i++)
            {
                if (packOwner[i].packName == packName)
                {
                    return packOwner[i];
                }
            }
            return null;
        }
        public List<string> skinOwners;
        public List<PackInfo> packOwner;
    }
}
