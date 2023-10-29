using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class TopUI : MonoBehaviour
{
    public Button plusCoin;
    public TextMeshProUGUI coinText;
    public ParticleSystem coinFx;

    public Button plusGem;
    public TextMeshProUGUI gemText;
    public ParticleSystem gemFx;

    public void Start()
    {
        plusCoin.onClick.AddListener(() => OnClickPlus(1));
        DataManager.OnAddCoin += SetCoinText;
        coinText.text = DataManager.Instance.coin.ToString();
    }

    void OnClickPlus(int id)
    {

    }

    public void SetCoinText(int startValue, int endValue, float delayTime = 0, bool showAnim = true)
    {
        int num = startValue;
        DOTween.To(() => num, x => num = x, endValue, 1).SetDelay(0.2f).OnUpdate(() =>
        {
            coinText.text = ConvertMoneyToString(num);
        });
        if (endValue > startValue)
        {
            if (showAnim)
            {
                CoinFx.Instance.PlayFx(() => coinFx.Play());
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            DataManager.Instance.AddCoin(10000, 0, "test");
            DataManager.Instance.AddSpineTicket(5);
        }
    }

    #region ConvertMoney
    public static string ConvertMoneyToString(long money)
    {
        if (money >= 1000000000)
        {
            return (money / 1000000000).ToString() + "B";
        }
        if (money >= 1000000)
        {
            return (money / 1000000).ToString() + "M";
        }
        if (money >= 100000)
        {
            return (money / 1000).ToString() + "K";
        }
        return money.ToString();
    }
    #endregion
}
