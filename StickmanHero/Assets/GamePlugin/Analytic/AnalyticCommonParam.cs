using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace mygame.sdk
{
    public class AnalyticCommonParam : MonoBehaviour
    {
        public static AnalyticCommonParam Instance = null;

        public int countShowAdsFull
        {
            get { return PlayerPrefs.GetInt("analy_pr_countShowAdsFull", 0); }
            set { PlayerPrefs.SetInt("analy_pr_countShowAdsFull", value); }
        }
        public int countShowAdsGift
        {
            get { return PlayerPrefs.GetInt("analy_pr_countShowAdsGift", 0); }
            set { PlayerPrefs.SetInt("analy_pr_countShowAdsGift", value); }
        }
        public int spendMoneyBuySkin
        {
            get { return PlayerPrefs.GetInt("analy_pr_spendMoneyBuySkin", 0); }
            set { PlayerPrefs.SetInt("analy_pr_spendMoneyBuySkin", value); }
        }
        public int selectCurrentGun
        {
            get { return PlayerPrefs.GetInt("analy_pr_selectCurrentGun", 0); }
            set { PlayerPrefs.SetInt("analy_pr_selectCurrentGun", value); }
        }
        public int spendMoneyBuyGun
        {
            get { return PlayerPrefs.GetInt("analy_pr_spendMoneyBuyGun", 0); }
            set { PlayerPrefs.SetInt("analy_pr_spendMoneyBuyGun", value); }
        }
        public int totalSpendMoney
        {
            get { return (spendMoneyBuyGun); }
        }
        public int totalEarnMoney
        {
            get { return PlayerPrefs.GetInt("analy_pr_totalEarnMoney", 0); }
            set { PlayerPrefs.SetInt("analy_pr_totalEarnMoney", value); }
        }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                LoadData();
            }
        }

        public void LoadData()
        {
        }

        public void saveData()
        {
        }
    }
}
