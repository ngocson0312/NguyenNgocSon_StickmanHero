#define ENABLE_ADCANVASTest
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using mygame.sdk;

public class AdTestHelper : BaseAdCanvas
{
    private Dictionary<AdCanvasSize, AdTestObjectWithType> listAdsFree;
    private Dictionary<AdCanvasSize, AdTestObjectWithType> listAdsUse;
    private List<Vector3> listMem4Check;

    Camera mainCamera;

    public AdTestObject[] AdsPrefabs;


    private Vector3 lastPos;
    private Vector3 currPos;

    public override void onAwake()
    {
        listAdsFree = new Dictionary<AdCanvasSize, AdTestObjectWithType>();
        listAdsUse = new Dictionary<AdCanvasSize, AdTestObjectWithType>();
        listMem4Check = new List<Vector3>();
        for (int i = 0; i < AdsPrefabs.Length; i++)
        {
            AdsPrefabs[i].initAds(this, AdCanvasHelper.Instance.textureDefault);
            AdTestObjectWithType awtfree = new AdTestObjectWithType();
            awtfree.listAds.Add(AdsPrefabs[i]);
            listAdsFree.Add(AdsPrefabs[i].adType, awtfree);
            AdsPrefabs[i].gameObject.SetActive(false);
            AdTestObjectWithType awtuse = new AdTestObjectWithType();
            listAdsUse.Add(AdsPrefabs[i].adType, awtuse);
        }
    }
    public override void onStart()
    {
    }

    public override void onChangeCamera(Camera newCamera)
    {
        mainCamera = newCamera;
    }


    public override void onUpdate()
    {
#if ENABLE_ADCANVASTest
        foreach (var ad in listAdsUse)
        {
            foreach (var item in ad.Value.listAds)
            {
                item.onUpdate();
            }
        }
#endif
    }
    private void OnDestroy()
    {
        listAdsFree.Clear();
        listAdsUse.Clear();
        listMem4Check = null;
    }

    public override void initClick(bool isClick)
    {
#if ENABLE_ADCANVASTest

#endif
    }

    public override BaseAdCanvasObject genAd(AdCanvasSize type, Vector3 pos, Vector3 forward, Transform _target = null, int stateLookat = 0, bool isFloowY = false)
    {
#if ENABLE_ADCANVASTest
        BaseAdCanvasObject ad = getAdsWithType(type, pos, forward, _target, stateLookat, isFloowY);
        if (ad != null)
        {
            ad.transform.localScale = new Vector3(1, 1, 1);
        }

        return ad;
#else
        return null;
#endif
    }
    private BaseAdCanvasObject getAdsWithType(AdCanvasSize type, Vector3 pos, Vector3 forward, Transform _target, int stateLookat, bool isFloowY)
    {
        if (listAdsUse[type].listAds.Count < 15)
        {
            if (listAdsFree[type].listAds.Count > 0)
            {
                BaseAdCanvasObject re = null;
                for (int i = 0; i < listAdsFree[type].listAds.Count; i++)
                {
                    if (listAdsFree[type].listAds[i].isLoaded())
                    {
                        re = listAdsFree[type].listAds[i];
                        listAdsFree[type].listAds.RemoveAt(i);
                        break;
                    }
                }
                if (re == null)
                {
                    re = listAdsFree[type].listAds[0];
                    listAdsFree[type].listAds.RemoveAt(0);
                }
                re.transform.position = pos;
                re.pos = pos;
                re.forward = forward;
                re.target = _target;
                re.stateLoockat = stateLookat;
                listAdsUse[type].listAds.Add(re);
                re.gameObject.SetActive(true);
                re.setFollowY(isFloowY);
                re.enableMesh();
                return re;
            }
            else
            {
                var prefabsads = getPrefab(type);
                if (prefabsads != null)
                {
                    var re = Instantiate(prefabsads, pos, Quaternion.identity, prefabsads.transform.parent);
                    re.initAds(this, AdCanvasHelper.Instance.textureDefault);
                    re.transform.position = pos;
                    re.pos = pos;
                    re.forward = forward;
                    re.target = _target;
                    re.stateLoockat = stateLookat;
                    listAdsUse[type].listAds.Add(re);
                    re.gameObject.SetActive(true);
                    re.setFollowY(isFloowY);
                    re.enableMesh();
                    return re;
                }
            }
        }
        return null;
    }

    AdTestObject getPrefab(AdCanvasSize type)
    {
        AdTestObject re = null;
        for (int i = 0; i < AdsPrefabs.Length; i++)
        {
            if (AdsPrefabs[i].adType == type)
            {
                re = AdsPrefabs[i];
                break;
            }
        }

        return re;
    }
    public override void freeAll()
    {
        foreach (var item in listAdsUse)
        {
            for (int i = (item.Value.listAds.Count - 1); i >= 0; i--)
            {
                listAdsFree[item.Value.listAds[i].adType].listAds.Add(item.Value.listAds[i]);
                item.Value.listAds[i].gameObject.SetActive(false);
                item.Value.listAds.RemoveAt(i);
            }
        }
    }
    public override void freeAd(BaseAdCanvasObject ad)
    {
        if (ad != null)
        {
            if (listAdsUse[ad.adType].listAds.Contains(ad))
            {
                listAdsUse[ad.adType].listAds.Remove(ad);
                listAdsFree[ad.adType].listAds.Add(ad);
                ad.gameObject.SetActive(false);
            }
        }
    }




    public override void onShowCmpNative()
    {
    }

    public override void onCMPOK(string iABTCv2String)
    {
    }
}

public class AdTestObjectWithType
{
    public List<BaseAdCanvasObject> listAds = new List<BaseAdCanvasObject>();
}
