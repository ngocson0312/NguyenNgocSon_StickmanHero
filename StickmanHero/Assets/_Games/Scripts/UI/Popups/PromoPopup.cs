using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using mygame.sdk;
using Spine.Unity;

public class PromoPopup : MonoBehaviour
{
    public PromoPack[] promoPack;
    public Button buyBtn;
    public Button closeBtn;

    public TextMeshProUGUI coinText;
    public TextMeshProUGUI packHeader;

    [SerializeField] SkeletonAnimation[] skeletonAnimation;

    [HideInInspector] public int curId = 0;

    private void Awake()
    {
        buyBtn.onClick.AddListener(OnClickBuy);
        closeBtn.onClick.AddListener(OnClickClose);
    }

    private void OnEnable()
    {
        bool canShow = false;
        for (int i = 0; i < promoPack.Length; i++)
        {
            if (!promoPack[i].isPurchased)
            {
                canShow = true;
                curId = i;
                break;
            }
        }
        if (!canShow) gameObject.SetActive(false);
        buyBtn.GetComponentInChildren<TextMeshProUGUI>().text = InappHelper.Instance.getPrice(promoPack[curId].packName);
        coinText.text = "+" + InappHelper.Instance.getMoneyRcv(promoPack[curId].packName, "gold");
        packHeader.text = promoPack[curId].packHeader;
        StartCoroutine(LoadSkin());
    }

    IEnumerator LoadSkin()
    {
        yield return new WaitForSeconds(0.05f);
        for (int i = 0; i < promoPack[curId].skinAdd.Length; i++)
        {
            skeletonAnimation[i].skeleton.SetSkin(promoPack[curId].skinAdd[i]);
        }
    }

    void OnClickBuy()
    {
        InappHelper.Instance.BuyPackage(promoPack[curId].packName, "promo_popup", delegate (PurchaseCallback callback) {
            if (callback.status == 1)
            {
                int coinAdd = InappHelper.Instance.getMoneyRcv(promoPack[curId].packName, "gold");
                DataManager.Instance.AddCoin(coinAdd, 0, "buy_promo_1");
                for (int i = 0; i < promoPack[curId].skinAdd.Length; i++)
                {
                    DataManager.Instance.UnlockSkin(promoPack[curId].skinAdd[i]);
                }
                promoPack[curId].isPurchased = true;
                OnClickClose();
            }
        });
    }

    void OnClickClose()
    {
        gameObject.SetActive(false);
    }
}

[System.Serializable]
public class PromoPack
{
    public string packHeader;
    public string packName;
    public bool isFull = false;
    public string[] skinAdd;
    public bool isPurchased
    {
        get
        {
            if(PlayerPrefs.GetInt("isPurchase " + packName,0) == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        set
        {
            if(value == true)
            {
                PlayerPrefs.SetInt("isPurchase " + packName, 1);
            }
        }
    }

}
