using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

namespace mygame.sdk
{
    public class FullEditorCtr : MonoBehaviour
    {
        public Button bt;
        private Image img;
        private Text txt;

#if UNITY_EDITOR
        private float timeclose;

        private void OnEnable()
        {
            timeclose = 0;
            AdsHelper.Instance.adsEditorCtr.onFullShow();
            txt = bt.transform.GetChild(1).GetComponent<Text>();
            img = bt.transform.GetChild(0).GetComponent<Image>();
            img.gameObject.SetActive(false);
            bt.interactable = false;
        }

        private void OnDisable()
        {
            AdsHelper.Instance.adsEditorCtr.onFullClose();
        }

        private void Update()
        {
            if (timeclose < 15)
            {
                if (Time.timeScale == 0)
                {
                    timeclose += 0.02f;
                }
                else
                {
                    timeclose += Time.deltaTime;
                }
                if (timeclose > 15) timeclose = 15;
                var n = (int)timeclose;
                if (n < 0) n = 0;
                txt.text = n.ToString();
                if (!img.gameObject.activeInHierarchy)
                    if (timeclose >= 0)
                    {
                        img.gameObject.SetActive(true);
                        bt.interactable = true;
                    }
            }
        }

        public void loadAds()
        {
            var rr = new Random();
            var tl = rr.Next(10, 100);
            float ftl = tl;
            ftl = ftl / 10.0f;
            AdsProcessCB.Instance().Enqueue(() =>
            {
                onloadads();
            }, ftl);
        }

        private void onloadads()
        {
            var rr = new Random();
            var tl = rr.Next(0, 100);
            if (tl >= 97)
                AdsHelper.Instance.adsEditorCtr.onFullLoadFail();
            else
                AdsHelper.Instance.adsEditorCtr.onFullLoaded();
        }
#endif
    }
}