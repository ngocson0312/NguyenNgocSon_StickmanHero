//#define ENABLE_Gadsme
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using mygame.sdk;
#if ENABLE_Gadsme
using Gadsme;
#endif

public class GadsmeHelper : BaseAdCanvas
{
    private Dictionary<AdCanvasSize, GadsmeObjectWithType> listAdsFree;
    private Dictionary<AdCanvasSize, GadsmeObjectWithType> listAdsUse;
    private List<Vector3> listMem4Check;

    Camera mainCamera;

    public GadsmeObject[] AdsPrefabs;
    public List<GadsmePlacementIdWithType> listPlacementsWithType = new List<GadsmePlacementIdWithType>();


    private Vector3 lastPos;
    private Vector3 currPos;

    private string _gdprString;


    public bool Gadsme_clicking = false;
    int stateClick = 0;

    Vector2 posClick = Vector2.zero;
#if ENABLE_Gadsme
    GadsmePlacement placementClick = null;
    long tClickDown = 0;
#endif

    public override void onAwake()
    {
        listAdsFree = new Dictionary<AdCanvasSize, GadsmeObjectWithType>();
        listAdsUse = new Dictionary<AdCanvasSize, GadsmeObjectWithType>();
        listMem4Check = new List<Vector3>();
        for (int i = 0; i < AdsPrefabs.Length; i++)
        {
            AdsPrefabs[i].initAds(this, AdCanvasHelper.Instance.textureDefault);
            GadsmeObjectWithType awtfree = new GadsmeObjectWithType();
            awtfree.listAds.Add(AdsPrefabs[i]);
            listAdsFree.Add(AdsPrefabs[i].adType, awtfree);
            AdsPrefabs[i].gameObject.SetActive(false);
            GadsmeObjectWithType awtuse = new GadsmeObjectWithType();
            listAdsUse.Add(AdsPrefabs[i].adType, awtuse);
        }
    }
    public override void onStart()
    {
#if ENABLE_Gadsme
        onChangeCamera(AdCanvasHelper.Instance.mainCam);
        GadsmeSDK.Init();
        string iABTCv2String = PlayerPrefs.GetString("mem_iab_tcv2", "");
        if (iABTCv2String != null && iABTCv2String.Length > 10)
        {
            GadsmeSDK.SetGdprConsentString(iABTCv2String);
        }
        GadsmeSDK.SetInteractionsEnabled(false);
        if (PlayerPrefs.GetInt("cf_adcanvas_enable", 1) == 1 && PlayerPrefs.GetInt("cf_adcanvas_CMP", 1) == 1 && PlayerPrefs.GetInt("mem_show_CMP", 0) <= 0)
        {
            Debug.Log($"mysdk: adcv gadsme Start");
#if UNITY_IOS || UNITY_IPHONE

#if !UNITY_EDITOR
            GadsmeSDK.SetAllowConsentDialog(false);
            //mygame.sdk.GameHelper.showCMP();
#endif

#elif UNITY_ANDROID

#if !UNITY_EDITOR
            mygame.sdk.GameHelper.showCMP();
#endif

#endif
        }
#endif
    }

    public override void onChangeCamera(Camera newCamera)
    {
        mainCamera = newCamera;
#if ENABLE_Gadsme
        GadsmeSDK.SetMainCamera(newCamera);
#endif
    }


    public override void onUpdate()
    {
#if ENABLE_Gadsme
        if (listAdsUse.Count > 0 && AdCanvasHelper.Instance.isEnableClick)
        {
            if (Input.GetMouseButtonDown(0))
            {
                placementClick = null;
                tClickDown = 0;
                posClick = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
                eventDataCurrentPosition.position = new Vector2(posClick.x, posClick.y);

                //check ui elements
                bool isIgnoreClick = false;
                if (EventSystem.current != null)
                {
                    List<RaycastResult> results = new List<RaycastResult>();
                    EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
                    foreach (var re in results)
                    {
                        Debug.Log($"mysdk: adcv gadsme click 000 ui=" + re.gameObject.name);
                        if (!AdCanvasHelper.Instance.nameUiObIgnore.Contains(re.gameObject.name))
                        {
                            isIgnoreClick = true;
                            break;
                        }
                    }
                }
                if (!isIgnoreClick)
                {
                    Ray ray;
                    if (mainCamera != null)
                    {
                        ray = mainCamera.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
                    }
                    else
                    {
                        if (Camera.main != null)
                        {
                            ray = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
                        }
                        else
                        {
                            return;
                        }
                    }
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, AdCanvasHelper.Instance.lenghtRayClick, LayerMask.GetMask("Default", "AdCanvasLayer")))
                    {
                        placementClick = hit.collider.GetComponent<GadsmePlacement>();
                        tClickDown = SdkUtil.systemCurrentMiliseconds();
                    }
                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                if (placementClick != null)
                {
                    Vector2 dclick = new Vector2(Input.mousePosition.x, Input.mousePosition.y) - posClick;
                    long tup = SdkUtil.systemCurrentMiliseconds();
                    if ((tup - tClickDown) <= 2000 && Mathf.Abs(dclick.x) <= 10 && Mathf.Abs(dclick.y) <= 10)
                    {
                        placementClick.Interact();
                        onclickGadsme();
                    }
                }
            }
        }
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

    public GadsmePlacementChannel getPlacementId(GadsmePlacementType type)
    {
        GadsmePlacementChannel re = null;
        foreach (var item in listPlacementsWithType)
        {
            if (item.type == type)
            {
                if (item.idx < item.listPlacements.Count)
                {
                    re = item.listPlacements[item.idx];
                    item.idx++;
                    if (item.idx >= item.listPlacements.Count)
                    {
                        item.idx = 0;
                    }
                }
                break;
            }
        }
        return re;
    }

    public override void initClick(bool isClick)
    {
#if ENABLE_Gadsme

#endif
    }

    void onclickGadsme()
    {
        mygame.sdk.FIRhelper.logEvent("Gadsme_onclick");
        int flagclick = PlayerPrefs.GetInt("cf_adcanvas_click", 0xFF);
        if ((flagclick & 1) > 0)
        {
            if (GameHelper.Instance != null)
            {
                Gadsme_clicking = true;
                stateClick = 1;
                GameHelper.Instance.setAllowShowOpenAd(false);
                GameHelper.Instance.isAlowShowOpen = false;
                StartCoroutine(waitResetFlagShowOpenAds());
                Debug.Log($"mysdk: adcv gadsme onclickGadsme Gadsme_clicking={Gadsme_clicking}");
            }
        }
    }

    IEnumerator waitResetFlagShowOpenAds()
    {
        yield return new WaitForSeconds(3.5f);
        if (stateClick == 1)
        {
            stateClick = 0;
            Gadsme_clicking = false;
            GameHelper.Instance.setAllowShowOpenAd(true);
            GameHelper.Instance.isAlowShowOpen = true;
            Debug.Log($"mysdk: adcv gadsme click but not openLink");
        }
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (!pauseStatus)
        {
            if (Gadsme_clicking)
            {
                Debug.Log($"mysdk: adcv gadsme OnApplicationPause Gadsme_clicking={Gadsme_clicking}");
                Gadsme_clicking = false;
                stateClick = 0;
                StartCoroutine(waitResetFlagShowOpenAdsAfterOpenLink());
            }
        }
        else
        {
            if (stateClick == 1 && Gadsme_clicking)
            {
                Debug.Log($"mysdk: adcv gadsme OnApplicationPause Gadsme openLink");
                stateClick = 0;
            }
        }
    }

    IEnumerator waitResetFlagShowOpenAdsAfterOpenLink()
    {
        yield return new WaitForSeconds(0.35f);
        GameHelper.Instance.setAllowShowOpenAd(true);
        GameHelper.Instance.isAlowShowOpen = true;
        Debug.Log($"mysdk: adcv gadsme setFlag ads open");
    }

    public override BaseAdCanvasObject genAd(AdCanvasSize type, Vector3 pos, Vector3 forward, Transform _target = null, int stateLookat = 0, bool isFloowY = false)
    {
#if ENABLE_Gadsme
        BaseAdCanvasObject ad = getAdsWithType(type, pos, forward, _target, stateLookat, isFloowY);
        Debug.Log($"mysdk: adcv gadsme genAd count={listAdsUse.Count}");
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
                    for (int i = 0; i < re.listAdInfo.Count; i++)
                    {
                        re.listAdInfo[i].isLoadDefault = false;
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
            }
        }
        return null;
    }

    GadsmeObject getPrefab(AdCanvasSize type)
    {
        GadsmeObject re = null;
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
        Debug.Log($"mysdk: adcv gadsme onShowCmpiOS");
    }

    public override void onCMPOK(string iABTCv2String)
    {
        Debug.Log($"mysdk: adcv gadsme onCMPOK=" + iABTCv2String);
#if ENABLE_Gadsme && !UNITY_EDITOR
        GadsmeSDK.SetGdprConsentString(iABTCv2String);
#endif
    }
}

[Serializable]
public class GadsmePlacementIdWithType
{
    public GadsmePlacementType type = GadsmePlacementType.P_300x250;
    public List<GadsmePlacementChannel> listPlacements = new List<GadsmePlacementChannel>();
    public int idx = 0;
}

[Serializable]
public class GadsmePlacementChannel
{
    public string placementId;
    public int channel;
}

public class GadsmeObjectWithType
{
    public List<BaseAdCanvasObject> listAds = new List<BaseAdCanvasObject>();
}
