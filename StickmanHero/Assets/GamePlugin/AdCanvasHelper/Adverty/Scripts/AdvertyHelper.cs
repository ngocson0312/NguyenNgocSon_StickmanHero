//#define ENABLE_Adverty
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using mygame.sdk;
#if ENABLE_Adverty
using Adverty;
#endif

public class AdvertyHelper : BaseAdCanvas
{
    private const string ANDROID_API_KEY = "ZWRjOWI3YTUtMjdmOC00ZTliLWE4NWEtNDNlYTM4YmFiOTM2JGh0dHBzOi8vYWRzZXJ2ZXIuYWR2ZXJ0eS5jb20=";
    private const string IOS_API_KEY = "Your_iOS_API_Key"; //put your iOS API key here
    private Dictionary<AdCanvasSize, AdvertyObjectWithType> listAdsFree;
    private Dictionary<AdCanvasSize, AdvertyObjectWithType> listAdsUse;
    private List<Vector3> listMem4Check;

    Camera mainCamera;

    public AdvertyObject[] AdsPrefabs;


    private Vector3 lastPos;
    private Vector3 currPos;

    private string _gdprString;


    public bool Adverty_clicking = false;
    int stateClick = 0;

    Vector2 posClick = Vector2.zero;
#if ENABLE_Adverty
    long tClickDown = 0;
#endif

    public override void onAwake()
    {
        listAdsFree = new Dictionary<AdCanvasSize, AdvertyObjectWithType>();
        listAdsUse = new Dictionary<AdCanvasSize, AdvertyObjectWithType>();
        listMem4Check = new List<Vector3>();
        for (int i = 0; i < AdsPrefabs.Length; i++)
        {
            AdsPrefabs[i].initAds(this, AdCanvasHelper.Instance.textureDefault);
            AdvertyObjectWithType awtfree = new AdvertyObjectWithType();
            awtfree.listAds.Add(AdsPrefabs[i]);
            listAdsFree.Add(AdsPrefabs[i].adType, awtfree);
            AdsPrefabs[i].gameObject.SetActive(false);
            AdvertyObjectWithType awtuse = new AdvertyObjectWithType();
            listAdsUse.Add(AdsPrefabs[i].adType, awtuse);
        }
    }
    public override void onStart()
    {
#if ENABLE_Adverty
        onChangeCamera(AdCanvasHelper.Instance.mainCam);

        //Define data and initialize Adverty SDK
        AdvertySettings.SandboxMode = false; //For production we turn off sandbox mode
        AdvertySettings.Platform = AdvertySettings.Mode.Mobile; //define target platform (Mobile, VR, AR)
        AdvertySettings.RestrictUserData = false; //do you disallow collect extra user data?

        if(SDKManager.Instance.isAdCanvasSandbox)
        {
            AdvertySettings.SandboxMode = true; //Sandbox mode enabled if we are using Development build or we are in editor
        }
        UserData userData;
        string iABTCv2String = PlayerPrefs.GetString("mem_iab_tcv2", "");
        if (iABTCv2String != null && iABTCv2String.Length > 10)
        {
            userData = new UserData(AgeSegment.Adult, Gender.Male, iABTCv2String);
        }
        else
        {
            userData = new UserData(AgeSegment.Adult, Gender.Male);
        }

        //Depends on mobile platform we set correspondent API key
        //Assume that we are developing for android on Windows and for iOS on Mac
#if UNITY_ANDROID || UNITY_EDITOR_WIN
        AdvertySettings.APIKey = ANDROID_API_KEY;
#elif UNITY_IOS || UNITY_EDITOR_OSX
        AdvertySettings.APIKey = IOS_API_KEY;
#endif

        AdvertySDK.Init(userData);

        AdvertyEvents.AdvertySessionActivationFailed += AdvertySessionActivationFailed;
        AdvertyEvents.AdvertySessionTerminated += AdvertySessionTerminated;
        AdvertyEvents.AdvertySessionActivated += AdvertySessionActivated;

        AdvertyEvents.UnitActivated += UnitActivated;
        AdvertyEvents.UnitActivationFailed += UnitActivationFailed;
        AdvertyEvents.UnitDeactivated += UnitDeactivated;
        AdvertyEvents.UnitViewed += UnitViewed;
        AdvertyEvents.AdDelivered += AdDelivered;
        AdvertyEvents.AdCompleted += AdCompleted;
        if (PlayerPrefs.GetInt("cf_adcanvas_enable", 1) == 1 && PlayerPrefs.GetInt("cf_adcanvas_CMP", 1) == 1 && PlayerPrefs.GetInt("mem_show_CMP", 0) <= 0)
        {
            Debug.Log($"mysdk: adcv adverty Start");
#if UNITY_IOS || UNITY_IPHONE

#if !UNITY_EDITOR
            //mygame.sdk.GameHelper.showCMP();
#endif

#elif UNITY_ANDROID

#if !UNITY_EDITOR
            // mygame.sdk.GameHelper.showCMP();
#endif

#endif
        }
#endif
    }

    public override void onChangeCamera(Camera newCamera)
    {
        if(newCamera == null)
        {
            newCamera = Camera.main;
        }
        mainCamera = newCamera;
#if ENABLE_Adverty
        AdvertySettings.SetMainCamera(mainCamera);
#endif
    }


    public override void onUpdate()
    {
#if ENABLE_Adverty
        // if (listAdsUse.Count > 0 && AdCanvasHelper.Instance.isEnableClick)
        // {
        //     if (Input.GetMouseButtonDown(0))
        //     {
        //         placementClick = null;
        //         tClickDown = 0;
        //         posClick = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        //         PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        //         eventDataCurrentPosition.position = new Vector2(posClick.x, posClick.y);

        //         //check ui elements
        //         bool isIgnoreClick = false;
        //         if (EventSystem.current != null)
        //         {
        //             List<RaycastResult> results = new List<RaycastResult>();
        //             EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        //             foreach (var re in results)
        //             {
        //                 Debug.Log($"mysdk: adcv adverty click 000 ui=" + re.gameObject.name);
        //                 if (!AdCanvasHelper.Instance.nameUiObIgnore.Contains(re.gameObject.name))
        //                 {
        //                     isIgnoreClick = true;
        //                     break;
        //                 }
        //             }
        //         }
        //         if (!isIgnoreClick)
        //         {
        //             Ray ray;
        //             if (mainCamera != null)
        //             {
        //                 ray = mainCamera.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
        //             }
        //             else
        //             {
        //                 if (Camera.main != null)
        //                 {
        //                     ray = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
        //                 }
        //                 else
        //                 {
        //                     return;
        //                 }
        //             }
        //             RaycastHit hit;
        //             if (Physics.Raycast(ray, out hit, AdCanvasHelper.Instance.lenghtRayClick, LayerMask.GetMask("AdCanvasLayer")))
        //             {
        //                 placementClick = hit.collider.GetComponent<AdvertyPlacement>();
        //                 tClickDown = SdkUtil.systemCurrentMiliseconds();
        //             }
        //         }
        //     }
        //     if (Input.GetMouseButtonUp(0))
        //     {
        //         if (placementClick != null)
        //         {
        //             Vector2 dclick = new Vector2(Input.mousePosition.x, Input.mousePosition.y) - posClick;
        //             long tup = SdkUtil.systemCurrentMiliseconds();
        //             if ((tup - tClickDown) <= 2000 && Mathf.Abs(dclick.x) <= 10 && Mathf.Abs(dclick.y) <= 10)
        //             {
        //                 placementClick.Interact();
        //                 onclickAdverty();
        //             }
        //         }
        //     }
        // }
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
#if ENABLE_Adverty

#endif
    }

    void onclickAdverty()
    {
        // mygame.sdk.FIRhelper.logEvent("Adverty_click_ui");
        // if (PlayerPrefs.GetInt("cf_Adverty_click", 1) > 0)
        // {
        //     if (GameHelper.Instance != null)
        //     {
        //         Adverty_clicking = true;
        //         stateClick = 1;
        //         GameHelper.Instance.setAllowShowOpenAd(false);
        //         GameHelper.Instance.isAlowShowOpen = false;
        //         StartCoroutine(waitResetFlagShowOpenAds());
        //         Debug.Log($"mysdk: adcv adverty onclickAdverty Adverty_clicking={Adverty_clicking}");
        //     }
        // }
    }

    IEnumerator waitResetFlagShowOpenAds()
    {
        yield return new WaitForSeconds(3.5f);
        if (stateClick == 1)
        {
            stateClick = 0;
            Adverty_clicking = false;
            GameHelper.Instance.setAllowShowOpenAd(true);
            GameHelper.Instance.isAlowShowOpen = true;
            Debug.Log($"mysdk: adcv adverty click but not openLink");
        }
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (!pauseStatus)
        {
            if (Adverty_clicking)
            {
                Debug.Log($"mysdk: adcv adverty OnApplicationPause Adverty_clicking={Adverty_clicking}");
                Adverty_clicking = false;
                stateClick = 0;
                StartCoroutine(waitResetFlagShowOpenAdsAfterOpenLink());
            }
        }
        else
        {
            if (stateClick == 1 && Adverty_clicking)
            {
                Debug.Log($"mysdk: adcv adverty OnApplicationPause Adverty openLink");
                stateClick = 0;
            }
        }
    }

    IEnumerator waitResetFlagShowOpenAdsAfterOpenLink()
    {
        yield return new WaitForSeconds(0.35f);
        GameHelper.Instance.setAllowShowOpenAd(true);
        GameHelper.Instance.isAlowShowOpen = true;
        Debug.Log($"mysdk: adcv adverty setFlag ads open");
    }

    public override BaseAdCanvasObject genAd(AdCanvasSize type, Vector3 pos, Vector3 forward, Transform _target = null, int stateLookat = 0, bool isFloowY = false)
    {
#if ENABLE_Adverty
        BaseAdCanvasObject ad = getAdsWithType(type, pos, forward, _target, stateLookat, isFloowY);
        Debug.Log($"mysdk: adcv adverty genAd count={listAdsUse.Count}");
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

    AdvertyObject getPrefab(AdCanvasSize type)
    {
        AdvertyObject re = null;
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

#if ENABLE_Adverty
    public void AdvertySessionActivationFailed()
    {
        Debug.Log($"mysdk: adcv adverty AdvertySessionActivationFailed");
    }
    public void AdvertySessionTerminated()
    {
        Debug.Log($"mysdk: adcv adverty AdvertySessionTerminated");
    }
    public void AdvertySessionActivated()
    {
        Debug.Log($"mysdk: adcv adverty AdvertySessionActivated");
    }

    public void UnitActivated(BaseUnit unit)
    {
        AdvertyObject ad = unit.transform.parent.parent.GetComponent<AdvertyObject>();
        if (ad != null)
        {
            ad.UnitActivated(unit.gameObject);
        }
    }
    public void UnitActivationFailed(BaseUnit unit)
    {
        AdvertyObject ad = unit.transform.parent.parent.GetComponent<AdvertyObject>();
        if (ad != null)
        {
            ad.UnitActivationFailed(unit.gameObject);
        }
    }
    public void UnitDeactivated(BaseUnit unit)
    {
        AdvertyObject ad = unit.transform.parent.parent.GetComponent<AdvertyObject>();
        if (ad != null)
        {
            ad.UnitDeactivated(unit.gameObject);
        }
    }
    public void UnitViewed(BaseUnit unit)
    {
        AdvertyObject ad = unit.transform.parent.parent.GetComponent<AdvertyObject>();
        if (ad != null)
        {
            ad.UnitViewed(unit.gameObject);
        }
    }
    public void AdDelivered(BaseUnit unit)
    {
        AdvertyObject ad = unit.transform.parent.parent.GetComponent<AdvertyObject>();
        if (ad != null)
        {
            ad.AdDelivered(unit.gameObject);
        }
    }
    public void AdCompleted(BaseUnit unit)
    {
        AdvertyObject ad = unit.transform.parent.parent.GetComponent<AdvertyObject>();
        if (ad != null)
        {
            ad.AdCompleted(unit.gameObject);
        }
    }
#endif

    public override void onShowCmpNative()
    {
        Debug.Log($"mysdk: adcv adverty onShowCmpiOS");
    }

    public override void onCMPOK(string iABTCv2String)
    {
        Debug.Log($"mysdk: adcv adverty onCMPOK=" + iABTCv2String);
#if ENABLE_Adverty && !UNITY_EDITOR
        UserData userData = new UserData(AgeSegment.Unknown, Gender.Unknown, iABTCv2String);
        AdvertySDK.UpdateUserData(userData);
#endif
    }
}

public class AdvertyObjectWithType
{
    public List<BaseAdCanvasObject> listAds = new List<BaseAdCanvasObject>();
}
