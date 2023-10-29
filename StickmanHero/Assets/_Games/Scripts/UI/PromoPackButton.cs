using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
namespace SuperFight
{
    public class PromoPackButton : MonoBehaviour
    {
        [SerializeField] InappData promoPack;
        [SerializeField] TextMeshProUGUI text;
        string timeExpire
        {
            get { return PlayerPrefs.GetString("promo_btn" + GetInstanceID(), ""); }
            set { PlayerPrefs.SetString("promo_btn" + GetInstanceID(), value); }
        }
        int hasTime
        {
            get { return PlayerPrefs.GetInt("promo_btn_expire" + GetInstanceID(), 0); }
            set { PlayerPrefs.GetInt("promo_btn_expire" + GetInstanceID(), value); }
        }
        private void Start()
        {
            if (timeExpire.Length == 0)
            {
                DateTime dateTime = DateTime.Now.AddDays(2);
                timeExpire = dateTime.ToString();
                hasTime = 1;
            }
            if (hasTime == 0)
            {
                DateTime dateTime = DateTime.Parse(timeExpire);
                TimeSpan timeSpan = dateTime - DateTime.Now;
                if (timeSpan.TotalDays < -4)
                {
                    dateTime = DateTime.Now.AddDays(2);
                    timeExpire = dateTime.ToString();
                    hasTime = 1;
                }
            }
            DataManager.onAddPack += Reload;
            Reload();
        }
        private void Update()
        {
            DateTime dateTime = DateTime.Parse(timeExpire);
            TimeSpan timeSpan = dateTime - DateTime.Now;
            int hours = (int)timeSpan.TotalHours;
            text.text = hours.ToString() + ":" + timeSpan.ToString(@"mm\:ss"); ;
        }
        public Button button
        {
            get { return GetComponent<Button>(); }
            private set { }
        }
        public void Reload()
        {
            DateTime dateTime = DateTime.Parse(timeExpire);
            TimeSpan timeSpan = dateTime - DateTime.Now;
            if (timeSpan.TotalHours <= 0)
            {
                hasTime = 0;
                gameObject.SetActive(false);
            }
            if (promoPack == null) return;
            if (DataManager.Instance.SoldOut(promoPack.packageName, promoPack.packType))
            {
                DataManager.onAddPack -= Reload;
                gameObject.SetActive(false);
            }
        }
    }
}
