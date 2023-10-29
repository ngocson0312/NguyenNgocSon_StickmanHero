using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamePlugins;
using UnityEngine.UI;
using mygame.sdk;

public class MyOpenAdsStaticCtrl : SingletonPopup<MyOpenAdsStaticCtrl>
{
    // Start is called before the first frame update
    public bool isClose = true;
    [SerializeField] GameObject btCclose;
    [SerializeField] Image imgBg;
    string _link = "";

    public void initAds(bool close, string imgurl, string linkstore)
    {
        _link = linkstore;
        btCclose.SetActive(false);
        if (close)
        {
            StartCoroutine(waitShowClose());
        }
        ImageLoader.LoadImageSprite(imgurl, 720, 1280, (sprite) => {
            if (sprite) {
                imgBg.sprite = sprite;
            }
        });
    }

    IEnumerator waitShowClose()
    {
        yield return new WaitForSeconds(3);
        btCclose.SetActive(true);
    }

    public void onClickAds()
    {
        if (isClose)
        {
            OnCloseClick();
        }
        Application.OpenURL(_link);
    }
}
