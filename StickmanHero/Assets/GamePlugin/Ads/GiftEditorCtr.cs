using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

namespace mygame.sdk
{
    public class GiftEditorCtr : MonoBehaviour
    {
        public Text txt;

#if UNITY_EDITOR
        private float timeclose = 3;
        bool flagrw = false;

        private void OnEnable()
        {
            timeclose = 0;
            flagrw = false;
            AdsHelper.Instance.adsEditorCtr.onGifShow();
        }

        private void OnDisable()
        {
            AdsHelper.Instance.adsEditorCtr.onGifClose(flagrw);
            flagrw = false;
        }

        private void Update()
        {
            timeclose += Time.unscaledDeltaTime;
            txt.text = Mathf.RoundToInt(Mathf.Clamp(timeclose, 0, int.MaxValue)).ToString();
            if (timeclose >= 0 && !flagrw)
            {
                flagrw = true;
                AdsHelper.Instance.adsEditorCtr.onGiftReward();
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
                AdsHelper.Instance.adsEditorCtr.onGifLoadFail();
            else
                AdsHelper.Instance.adsEditorCtr.onGifLoaded();
        }
#endif
    }
}