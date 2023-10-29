using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using SuperFight;
public class RewardPopup : PopupUI
{
    public GameObject itemBow;
    public GameObject itemSword;
    public GameObject itemCoin;
    public GameObject itemSkin;
    public GameObject itemTicket;
    public Button claimBtn;
    List<GameObject> tempObj = new List<GameObject>();
    private bool isActive;
    private InappData total;
    public override void Initialize(PopupManager popupManager)
    {
        base.Initialize(popupManager);
        popupName = PopupName.REWARDPOPUP;
        claimBtn.onClick.AddListener(OnClickClaim);
        isActive = false;
    }

    public void ShowReward(InappData inappData)
    {
        if (!isActive)
        {
            if (total != null)
            {
                Destroy(total);
            }
            total = Instantiate(inappData);
            isActive = true;
            tempObj = new List<GameObject>();
            itemCoin.SetActive(false);
            itemBow.SetActive(false);
            itemSword.SetActive(false);
            itemTicket.SetActive(false);
            if (inappData.coinAdd > 0)
            {
                itemCoin.SetActive(true);
                if (inappData.coinAdd < 1000)
                {
                    itemCoin.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = inappData.coinAdd.ToString();
                }
                else
                {
                    itemCoin.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = (inappData.coinAdd / 1000).ToString() + "K";
                }
            }
            if (inappData.bowAdd > 0)
            {
                itemBow.SetActive(true);
                itemBow.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = inappData.bowAdd.ToString();
            }
            if (inappData.swordAdd > 0)
            {
                itemSword.SetActive(true);
                itemSword.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = inappData.swordAdd.ToString();
            }
            if (inappData.spineTicketAdd > 0)
            {
                itemTicket.SetActive(true);
                itemTicket.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = inappData.spineTicketAdd.ToString();
            }
        }
        else
        {
            total.bowAdd += inappData.bowAdd;
            total.chestAdd += inappData.chestAdd;
            total.coinAdd += inappData.coinAdd;
            total.spineTicketAdd += inappData.spineTicketAdd;
            total.swordAdd += inappData.swordAdd;
            if (total.coinAdd > 0)
            {
                itemCoin.SetActive(true);
                if (total.coinAdd < 1000)
                {
                    itemCoin.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = total.coinAdd.ToString();
                }
                else
                {
                    itemCoin.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = (total.coinAdd / 1000).ToString() + "K";
                }
            }
            if (total.bowAdd > 0)
            {
                itemBow.SetActive(true);
                itemBow.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = total.bowAdd.ToString();
            }
            if (total.swordAdd > 0)
            {
                itemSword.SetActive(true);
                itemSword.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = total.swordAdd.ToString();
            }
            if (total.spineTicketAdd > 0)
            {
                itemTicket.SetActive(true);
                itemTicket.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = total.spineTicketAdd.ToString();
            }
        }
        for (int i = 0; i < inappData.skinAdd.Length; i++)
        {
            GameObject skin = Instantiate(itemSkin, transform.GetChild(0));
            skin.SetActive(true);
            skin.transform.GetChild(1).GetComponent<Image>().sprite = inappData.skinAdd[i].avatar;
            tempObj.Add(skin);
        }

    }
   
    void OnClickClaim()
    {
        for (int i = 0; i < tempObj.Count; i++)
        {
            Destroy(tempObj[i]);
        }
        isActive = false;
        Hide();
    }
}
