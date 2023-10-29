using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace mygame.sdk
{
    public class AdsEditorCtr : MonoBehaviour
    {
        public Transform adsEditor;
        public FullEditorCtr fullEditor;
        public GiftEditorCtr giftEditor;
        public LoadingAdsCtr loadingAdsCtr;
        public bool isFullEditorLoaded { get; private set; }
        public bool isFullEditorLoading { get; private set; }
        public bool isGiftEditorLoaded { get; private set; }
        public bool isGiftEditorLoading { get; private set; }

        private void Start()
        {
            var list = FindObjectsOfType<EventSystem>();
            if (list == null || list.Count() == 0)
            {
                transform.Find("EventSystem").gameObject.SetActive(true);
            }
        }

        public void loadFullEditor()
        {
#if UNITY_EDITOR
            fullEditor.loadAds();
#endif
        }
        public void showFullEditor()
        {
            adsEditor.gameObject.SetActive(true);
            adsEditor.GetChild(0).gameObject.SetActive(true);
            adsEditor.GetChild(1).gameObject.SetActive(false);
        }
        public void loadGiftEditor()
        {
            SdkUtil.logd("ads helper RequestRewardBasedVideo editor");
#if UNITY_EDITOR
            giftEditor.loadAds();
#endif
        }
        public void showGiftEditor()
        {
            adsEditor.gameObject.SetActive(true);
            adsEditor.GetChild(0).gameObject.SetActive(false);
            adsEditor.GetChild(1).gameObject.SetActive(true);
        }

#if UNITY_EDITOR
        public void onFullLoaded()
        {
            SdkUtil.logd("ads helper onFullLoaded");
            isFullEditorLoading = false;
            isFullEditorLoaded = true;
        }
        public void onFullLoadFail()
        {
            SdkUtil.logd("ads helper onFullLoadFail");
            isFullEditorLoading = false;
            isFullEditorLoaded = false;
        }
        public void onFullShow()
        {
            SdkUtil.logd("ads helper onFullShow");
        }
        public void onFullClose()
        {
            SdkUtil.logd("ads helper onFullClose");
            isFullEditorLoading = false;
            isFullEditorLoaded = false;
            AdsHelper.Instance.EditorOnFullClose();
        }
        public void onGifLoaded()
        {
            SdkUtil.logd("ads helper rw onGifLoaded");
            isGiftEditorLoading = false;
            isGiftEditorLoaded = true;
        }
        public void onGifLoadFail()
        {
            SdkUtil.logd("ads helper rw onGifLoadFail");
            isGiftEditorLoaded = false;
            isGiftEditorLoading = false;
        }
        public void onGifShow()
        {
            SdkUtil.logd("ads helper rw onGifShow");
        }
        public void onGiftReward()
        {
            SdkUtil.logd("ads helper rw onGiftReward");
        }
        public void onGifClose(bool isrw)
        {
            SdkUtil.logd("ads helper rw onGifClose");
            isGiftEditorLoaded = false;
            isGiftEditorLoading = false;
            AdsHelper.Instance.EditorOnGiftClose(isrw);
        }
#endif
    }
}
