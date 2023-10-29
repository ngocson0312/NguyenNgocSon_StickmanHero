using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace SuperFight
{
    public class DailyReward : PopupUI
    {
        public Button close;
        public Button claim;
        public DailyDay[] gifts;
        public int currentGift
        {
            get { return PlayerPrefs.GetInt("daily_current_day", 0); }
            set { PlayerPrefs.SetInt("daily_current_day", value); }
        }
        public int canGetGift
        {
            get { return PlayerPrefs.GetInt("can_get_daily", 0); }
            set { PlayerPrefs.SetInt("can_get_daily", value); }
        }
        public override void Initialize(PopupManager popupManager)
        {
            base.Initialize(popupManager);
            popupName = PopupName.DAILYREWARD;
            for (int i = 0; i < gifts.Length; i++)
            {
                gifts[i].day = i;
                gifts[i].Initialize(this);
            }
            close.onClick.AddListener(Hide);
            claim.onClick.AddListener(OnClaim);
            if (currentGift >= gifts.Length || canGetGift == 0)
            {
                claim.gameObject.SetActive(false);
            }
            GameManager.resetDay += ResetDay;
        }
        public override void Show()
        {
            base.Show();
        }
        public void OnClaim()
        {
            DailyRewardItem item = gifts[currentGift].reward;
            if (item.skin != null)
            {
                DataManager.Instance.UnlockSkin(item.skin.skinName);
                PlayerManager.Instance.SetSkin(item.skin.skinName);
                DataManager.Instance.currentSkin = item.skin.skinName;
            }
            if (item.coin > 0)
            {
                DataManager.Instance.AddCoin(item.coin, 0.5f, "Daily");
            }
            if (item.heart > 0)
            {
                DataManager.Instance.AddHeart(item.heart, "daily_reward");
            }
            currentGift++;
            canGetGift = 0;
            for (int i = 0; i < gifts.Length; i++)
            {
                gifts[i].Initialize(this);
            }
            claim.gameObject.SetActive(false);
            AudioManager.Instance.PlayOneShot("Special_Powerup2", 1f);
            Hide();
        }
        void ResetDay()
        {
            canGetGift = 1;
            if (currentGift < gifts.Length)
            {
                claim.gameObject.SetActive(true);
            }
        }
    }
    [System.Serializable]
    public class DailyRewardItem
    {
        public int coin;
        public int heart;
        public SkinObject skin;
    }
}

