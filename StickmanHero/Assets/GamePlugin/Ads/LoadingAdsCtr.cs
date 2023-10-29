using System.Collections;
using UnityEngine;

namespace mygame.sdk
{
    public delegate void onSHowPgr(int state);

    public class LoadingAdsCtr : MonoBehaviour
    {
        private onSHowPgr _cb;

        private void OnEnable()
        {
            StartCoroutine(closeMy());
        }

        public void showPgr(onSHowPgr cb)
        {
            _cb = cb;
            gameObject.SetActive(true);
            transform.parent.gameObject.SetActive(true);
        }

        private IEnumerator closeMy()
        {
            yield return new WaitForSeconds(3);
            if (_cb != null) _cb(0);
            _cb = null;
            gameObject.SetActive(false);
            transform.parent.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (_cb != null && AdsHelper.Instance.isGift4Show(true))
            {
                if (_cb != null)
                {
                    _cb(1);
                    _cb = null;
                }

                gameObject.SetActive(false);
                transform.parent.gameObject.SetActive(false);
            }
        }
    }
}