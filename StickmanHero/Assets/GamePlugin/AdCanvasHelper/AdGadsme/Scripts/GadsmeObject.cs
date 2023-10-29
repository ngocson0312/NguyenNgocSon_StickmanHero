using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using mygame.sdk;
#if ENABLE_Gadsme
using Gadsme;
#endif

public enum GadsmePlacementType
{
    P_300x250 = 0,
    P_300x50,
    P_V4x3
}

public class GadsmeObject : BaseAdCanvasObject
{
    public GadsmePlacementType placementType;
    public List<GadsmeInfo> listAdInfo;
    public List<GameObject> listObBoard;
    Dictionary<GameObject, GadsmeInfo> dicAds = null;
    bool isAdLoaded = false;
    Vector3 postouch;
    TTAdCanvas obTTDefault = null;
    bool isgetAllImg = false;
    int idxAniDf = 0;
    bool isFollowY = false;
    float yTagetOrigin = 0;
    float preYtaget = 0;
    float yWillSet = 0;
    float tLookAt = 2000;
    static bool isSetTouch = false;

    public override void initAds(BaseAdCanvas adhelper, Texture2D ttdf)
    {
        base.initAds(adhelper, ttdf);
        if (dicAds == null)
        {
            isgetAllImg = false;
            isFollowY = false;
            dicAds = new Dictionary<GameObject, GadsmeInfo>();
            GadsmePlacementChannel pidc = ((GadsmeHelper)adCanvasHelper).getPlacementId(placementType);
            for (int i = 0; i < listAdInfo.Count; i++)
            {
                dicAds.Add(listAdInfo[i].mesh.gameObject, listAdInfo[i]);
#if ENABLE_Gadsme
                //listAdInfo[i].placement = listAdInfo[i].mesh.GetComponent<GadsmePlacement>();
                if (pidc != null)
                {
                    listAdInfo[i].placement.placementId = pidc.placementId;
                    listAdInfo[i].placement.adChannelId = pidc.placementId;
                    listAdInfo[i].placement.adChannelNumber = pidc.channel;
                }
                listAdInfo[i].placement.RenderChangeEvent += onRenderChangeEvent;
                listAdInfo[i].placement.InteractEvent += onInteractEvent;
                listAdInfo[i].placement.EnableChangeEvent += onEnableChangeEvent;
                listAdInfo[i].placement.ContentFailedEvent += onContentFailedEvent;
                listAdInfo[i].placement.ContentLoadedEvent += onContentLoadedEvent;
#endif
            }
            hideAds();
        }
        
#if ENABLE_Gadsme
        if (ttdf != null)
        {
            for (int i = 0; i < listAdInfo.Count; i++)
            {
                GadsmePlacement pp = listAdInfo[i].placement;
                if (pp != null)
                {
                    pp.fallbackTexture = null;
                }
                pp.fallbackTextureVisibleWhenLoading = false;
                if (listAdInfo[i].mesh.material.mainTexture != null)
                {
                    //vvvlistAdInfo[i].mesh.material.mainTexture = ttdf;
                }
            }
        }
#endif
        isAdLoaded = false;
        foreach (var item in dicAds)
        {
            if (item.Value.isAdLoaded)
            {
                isAdLoaded = true;
                break;
            }
        }
        if (AdCanvasHelper.cf_adcanvas_alshow == 1)
        {
            isAdLoaded = true;
        }
        if (isAdLoaded)
        {
            showAds();
        }
        else
        {
            hideAds();
        }
    }

    // private void OnEnable()
    // {
    //     Invoke("setTextDefault", 1);
    //     Debug.Log($"mysdk: adcv gadsme OnEnable isAdLoaded={isAdLoaded}");
    // }

    public override void enableMesh()
    {
        // if (listMesh != null)
        // {
        //     for (int i = 0; i < listMesh.Count; i++)
        //     {
        //         listMesh[i].enabled = true;
        //     }
        // }

        yWillSet = 0;
    }

    public override void setFollowY(bool isfl, float yTaget = -10000)
    {
        isFollowY = isfl;
        if (target != null)
        {
            if (yTaget <= -10000)
            {
                yTagetOrigin = target.transform.position.y;
            }
            else
            {
                yTagetOrigin = yTaget;
            }

            preYtaget = yTagetOrigin;
        }
    }

    void setTextDefault()
    {
#if ENABLE_Gadsme
        for (int i = 0; i < listAdInfo.Count; i++)
        {
            int idx = i;
            if (!listAdInfo[i].isLoadDefault)
            {
                obTTDefault = AdCanvasHelper.Instance.getTTDefault(adCanvasHelper, adType, (obad, tt) =>
                {
                    showAds();
                    if (listAdInfo[idx].placement != null)
                    {
                        //vvvlistAdInfo[idx].placement.fallbackTexture = tt;
                    }
                    if (listAdInfo[idx].mesh.material.mainTexture != null)
                    {
                        listAdInfo[idx].isLoadDefault = true;
                        if (listAdInfo[idx].mesh.material.mainTexture.name.CompareTo("adcanvasttdefault") == 0)
                        {
                            //vvvlistAdInfo[idx].mesh.material.mainTexture = tt;
                        }
                    }
                    if (obad != null && obad.urlImgs.Count > 1)
                    {
                        for (int p = 1; p < obad.urlImgs.Count; p++)
                        {
                            int mp = p;
                            obad.textures.Add(null);
                            ImageLoader.loadImageTexture(obad.urlImgs[p], 100, 100, (ntt) =>
                            {
                                ntt.name = "adcanvasttdefault";
                                obad.textures[mp] = ntt;
                                bool isall = true;
                                for (int ll = 0; ll < obad.textures.Count; ll++)
                                {
                                    if (obad.textures[ll] == null)
                                    {
                                        isall = false;
                                        break;
                                    }
                                }
                                isgetAllImg = isall;
                                if (isall)
                                {
                                    StartCoroutine(AniDefault());
                                }
                            });
                        }
                    }
                });
            }
        }
#endif
    }

    void showAds()
    {
        if (!disableObj)
        {
            foreach (var item in listObBoard)
            {
                item.SetActive(true);
            }
        }
        foreach (var item in dicAds)
        {
            item.Value.mesh.transform.parent.GetComponent<MeshRenderer>().enabled = true;
            item.Value.mesh.enabled = true;
            if (item.Value.defaultMesh != null)
            {
                if (AdCanvasHelper.Instance.useMyDefault)
                {
                    item.Value.defaultMesh.enabled = true;
                }
                else
                {
                    item.Value.defaultMesh.enabled = false;
                }
            }
        }
#if ENABLE_Gadsme
        for (int i = 0; i < listAdInfo.Count; i++)
        {
            GadsmePlacement pp = listAdInfo[i].placement;
            if (pp != null)
            {
                pp.fallbackTexture = AdCanvasHelper.Instance.textureDefault;
            }
        }
#endif
    }

    void hideAds()
    {
        // Debug.Log($"mysdk: adcv gadsme hideAds {gameObject.name}");
        foreach (var item in dicAds)
        {
            item.Value.mesh.transform.parent.GetComponent<MeshRenderer>().enabled = false;
            item.Value.mesh.enabled = false;
            if (item.Value.defaultMesh != null)
            {
                item.Value.defaultMesh.enabled = false;
            }
        }
        foreach (var item in listObBoard)
        {
            item.SetActive(false);
        }
    }

    public override bool isLoaded()
    {
        isAdLoaded = false;
        foreach (var item in dicAds)
        {
            if (item.Value.isAdLoaded)
            {
                isAdLoaded = true;
                break;
            }
        }
        return isAdLoaded;
    }

    public override void onUpdate()
    {
        //transform.position += forward * 2 * Time.deltaTime;
        if (target != null)
        {
            if (isFollowY)
            {
                float dy = target.transform.position.y - yTagetOrigin;
                if (dy > 1f || dy < -1f)
                {
                    dy = target.transform.position.y - preYtaget;
                    preYtaget = target.transform.position.y;
                    yWillSet += dy;
                }
                if (yWillSet > 0.1f || yWillSet < -0.1f)
                {
                    if (yWillSet > 0)
                    {
                        dy = 7.5f * Time.deltaTime;
                        if (yWillSet < dy)
                        {
                            dy = yWillSet;
                            yWillSet = 0;
                        }
                        else
                        {
                            yWillSet -= dy;
                        }
                    }
                    else
                    {
                        dy = -7.5f * Time.deltaTime;
                        if (yWillSet > dy)
                        {
                            dy = yWillSet;
                            yWillSet = 0;
                        }
                        else
                        {
                            yWillSet -= dy;
                        }
                    }
                    transform.position = new Vector3(transform.position.x, transform.position.y + dy, transform.position.z);
                }
            }
            if (stateLoockat > 0)
            {
                transform.LookAt(new Vector3(target.position.x, transform.position.y, target.position.z));
                if (stateLoockat == 2)
                {
                    stateLoockat = 0;
                }
            }
        }
    }

    private void OnDestroy()
    {
        if (dicAds != null)
        {
            foreach (var item in dicAds)
            {
                item.Value.mesh = null;
            }
            dicAds.Clear();
            dicAds = null;
        }
    }

#if ENABLE_Gadsme

    IEnumerator AniDefault()
    {
        yield return new WaitForSeconds(0.1f);
        idxAniDf++;
        bool isc = false;
        if (idxAniDf >= obTTDefault.textures.Count)
        {
            idxAniDf = 0;
            isc = true;
        }
        if (obTTDefault.textures[idxAniDf] != null)
        {
            for (int i = 0; i < listAdInfo.Count; i++)
            {
                if (listAdInfo[i].placement != null)
                {
                    //vvvlistAdInfo[i].placement.fallbackTexture = obTTDefault.textures[idxAniDf];
                }
                if (listAdInfo[i].mesh.material.mainTexture != null)
                {
                    listAdInfo[i].isLoadDefault = true;
                    if (listAdInfo[i].mesh.material.mainTexture.name.CompareTo("adcanvasttdefault") == 0)
                    {
                        //vvvlistAdInfo[i].mesh.material.mainTexture = obTTDefault.textures[idxAniDf];
                    }
                }
            }
        }
        if (isc)
        {
            yield return new WaitForSeconds(3.5f);
        }
        StartCoroutine(AniDefault());
    }

    public void onRenderChangeEvent(GadsmePlacement ad, bool ischange)
    {
        Debug.Log($"mysdk: adcv gadsme onRenderChangeEvent={ad.placementId}, ischange={ischange}");
    }

    public void onInteractEvent(GadsmePlacement ad)
    {
        Debug.Log($"mysdk: adcv gadsme onInteractEvent={ad.placementId}");
    }

    public void onEnableChangeEvent(GadsmePlacement ad, bool ischange)
    {
        Debug.Log($"mysdk: adcv gadsme onEnableChangeEvent={ad.placementId}, ischange={ischange}");
    }

    public void onContentFailedEvent(GadsmePlacement ad)
    {
        Debug.Log($"mysdk: adcv gadsme onContentFailedEvent={ad.placementId}");
        if (dicAds.ContainsKey(ad.gameObject))
        {
            //dicAds[ad.gameObject].isAdLoaded = false;
        }
        isAdLoaded = false;
        foreach (var item in dicAds)
        {
            if (item.Value.isAdLoaded)
            {
                isAdLoaded = true;
                break;
            }
        }
        if (isAdLoaded)
        {
            showAds();
        }
        else
        {
            // hideAds();
        }
    }

    public void onContentLoadedEvent(GadsmePlacement ad)
    {
        Debug.Log($"mysdk: adcv gadsme onContentLoadedEvent={ad.placementId}");
        isAdLoaded = true;
        if (dicAds.ContainsKey(ad.gameObject))
        {
            dicAds[ad.gameObject].isAdLoaded = true;
        }
        showAds();
    }
#endif

}

[Serializable]
public class GadsmeInfo : BaseAdInfo
{
#if ENABLE_Gadsme
    public GadsmePlacement placement;
#endif
}
