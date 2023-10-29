using SuperFight;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InappData", menuName = "InappData")]
public class InappData : ScriptableObject
{
    public string packName;
    public string packageName;
    public PackType packType;
    public int salePercent;
    public int coinAdd;
    public int chestAdd;
    public int swordAdd;
    public int bowAdd;
    public int spineTicketAdd;
    public int lifeTime;
    public SkinObject[] skinAdd;
    public void ReceiveReward()
    {
        DataManager.Instance.AddCoin(coinAdd, 0, "buy_promopack_" + packName);
        DataManager.Instance.AddSpineTicket(spineTicketAdd);
        DataManager.Instance.AddLuckyChest(chestAdd);
        DataManager.Instance.itemBow += bowAdd;
        DataManager.Instance.itemSword += swordAdd;
        for (int i = 0; i < skinAdd.Length; i++)
        {
            DataManager.Instance.UnlockSkin(skinAdd[i].skinName);
            DataManager.Instance.currentSkin = skinAdd[i].skinName;
            PlayerManager.Instance.SetSkin(skinAdd[i].skinName);
        }
    }
}
