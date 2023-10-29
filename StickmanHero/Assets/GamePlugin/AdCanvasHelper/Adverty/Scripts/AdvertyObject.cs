using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using mygame.sdk;
#if ENABLE_Adverty
using Adverty;
#endif

public class AdvertyObject : BaseAdCanvasObject
{
    public List<BaseAdInfo> listAdInfo;
    public List<GameObject> listObBoard;
    Dictionary<GameObject, BaseAdInfo> dicAds = null;
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
            dicAds = new Dictionary<GameObject, BaseAdInfo>();
            for (int i = 0; i < listAdInfo.Count; i++)
            {
                dicAds.Add(listAdInfo[i].mesh.gameObject, listAdInfo[i]);
            }
            hideAds();
        }
#if ENABLE_Adverty
        if (ttdf != null)
        {
            for (int i = 0; i < listAdInfo.Count; i++)
            {
                if (listAdInfo[i].mesh.material.mainTexture != null)
                {
                    //listAdInfo[i].mesh.material.mainTexture = ttdf;//vvv
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
    //     Debug.Log($"mysdk: adcv adverty OnEnable isAdLoaded={isAdLoaded}");
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

    void setTextDefault()
    {
#if ENABLE_Adverty
        for (int i = 0; i < listAdInfo.Count; i++)
        {
            int idx = i;
            if (!listAdInfo[i].isLoadDefault)
            {
                obTTDefault = AdCanvasHelper.Instance.getTTDefault(adCanvasHelper, adType, (obad, tt) =>
                {
                    showAds();
                    if (listAdInfo[idx].mesh.material.mainTexture != null)
                    {
                        listAdInfo[idx].isLoadDefault = true;
                        if (listAdInfo[idx].mesh.material.mainTexture.name.CompareTo("adcanvasttdefault") == 0)
                        {
                            //listAdInfo[idx].mesh.material.mainTexture = tt;//vvv
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
    }

    void hideAds()
    {
        // Debug.Log($"mysdk: adcv adverty hideAds {gameObject.name}");
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

#if ENABLE_Adverty

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
                if (listAdInfo[i].mesh.material.mainTexture != null)
                {
                    listAdInfo[i].isLoadDefault = true;
                    if (listAdInfo[i].mesh.material.mainTexture.name.CompareTo("adcanvasttdefault") == 0)
                    {
                        //listAdInfo[i].mesh.material.mainTexture = obTTDefault.textures[idxAniDf];//vvv
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

    public void AdDelivered(GameObject ad)
    {
        Debug.Log($"mysdk: adcv adverty ob AdDelivered = {name}");
        // isAdLoaded = true;
        // if (dicAds.ContainsKey(ad))
        // {
        //     dicAds[ad].isAdLoaded = true;
        // }
        // showAds();
    }
    public void UnitActivated(GameObject ad)
    {
        Debug.Log($"mysdk: adcv adverty ob UnitActivated = {name}");
        isAdLoaded = true;
        if (dicAds.ContainsKey(ad))
        {
            dicAds[ad].isAdLoaded = true;
        }
        showAds();
    }
    public void UnitActivationFailed(GameObject ad)
    {
        Debug.Log($"mysdk: adcv adverty ob UnitActivationFailed = {name}");
        // isAdLoaded = false;
        // foreach (var item in dicAds)
        // {
        //     if (item.Value.isAdLoaded)
        //     {
        //         isAdLoaded = true;
        //         break;
        //     }
        // }
        // if (isAdLoaded)
        // {
        //     showAds();
        // }
        // else
        // {
        //     // hideAds();
        // }
    }
    public void UnitDeactivated(GameObject ad)
    {
        Debug.Log($"mysdk: adcv adverty ob UnitDeactivated = {name}");
    }
    public void UnitViewed(GameObject ad)
    {
        Debug.Log($"mysdk: adcv adverty ob UnitViewed = {name}");
    }
    public void AdCompleted(GameObject ad)
    {
        Debug.Log($"mysdk: adcv adverty ob AdCompleted = {name}");
    }
#endif

}