using TMPro;
using UnityEngine.UI;
using UnityEngine;
using Spine.Unity;
namespace SuperFight
{
    public class DailyDay : MonoBehaviour
    {
        public int day;
        public DailyRewardItem reward;
        public Image backGround;
        public TextMeshProUGUI textDay;
        public TextMeshProUGUI textCoin;
        public SkeletonAnimation skeletonAnimation;
        public Sprite[] bgSprs;
        public GameObject checkMark;
        DailyReward dailyReward;
        public void Initialize(DailyReward dailyReward)
        {
            this.dailyReward = dailyReward;
            textDay.text = "DAY " + (day + 1).ToString();
            checkMark.SetActive(false);
            if (dailyReward.currentGift == day)
            {
                backGround.sprite = bgSprs[0];
            }
            if (dailyReward.currentGift < day)
            {
                backGround.sprite = bgSprs[1];
            }
            if (dailyReward.currentGift > day)
            {
                checkMark.SetActive(true);
                backGround.sprite = bgSprs[2];
            }
            if (skeletonAnimation != null)
            {
                skeletonAnimation.gameObject.SetActive(false);
            }
            if (reward.coin > 0)
            {
                textCoin.gameObject.SetActive(true);
                textCoin.text = "+ " + reward.coin.ToString();
            }
            if (reward.skin != null)
            {
                skeletonAnimation.gameObject.SetActive(true);
                textCoin.gameObject.SetActive(false);
                 //skeletonAnimation.skeleton.SetSkin(reward.skin.skinName);
                // skeletonAnimation.skeleton.SetToSetupPose();
            }
        }
        public void ClaimGift()
        {
            // dailyReward.OnClaim(reward);
        }
    }

}
