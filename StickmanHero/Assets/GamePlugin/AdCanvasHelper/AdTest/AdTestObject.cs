using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using mygame.sdk;


public class AdTestObject : BaseAdCanvasObject
{
    public List<AdTestInfo> listAdInfo;
    public List<GameObject> listObBoard;
    Dictionary<GameObject, AdTestInfo> dicAds = null;
    bool isAdLoaded = false;
    TTAdCanvas obTTDefault = null;
    bool isgetAllImg = false;
    int idxAniDf = 0;
    bool isFollowY = false;
    float yTagetOrigin = 0;
    float preYtaget = 0;
    float yWillSet = 0;
    float tLookAt = 2000;

    public override void initAds(BaseAdCanvas adhelper, Texture2D ttdf)
    {
        base.initAds(adhelper, ttdf);
        if (dicAds == null)
        {
            isgetAllImg = false;
            isFollowY = false;
            dicAds = new Dictionary<GameObject, AdTestInfo>();
            for (int i = 0; i < listAdInfo.Count; i++)
            {
                dicAds.Add(listAdInfo[i].mesh.gameObject, listAdInfo[i]);
            }
            hideAds();
        }
        if (ttdf != null)
        {
            for (int i = 0; i < listAdInfo.Count; i++)
            {
                if (listAdInfo[i].mesh.material.mainTexture != null)
                {
                    listAdInfo[i].mesh.material.mainTexture = ttdf;
                }
            }
        }
        isAdLoaded = true;
        if (isAdLoaded)
        {
            showAds();
        }
        else
        {
            hideAds();
        }
    }

#if ENABLE_ADCANVASTest
    private void Update() {
        onUpdate();
    }
#endif

    public override void enableMesh()
    {
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

    void showAds()
    {
        foreach (var item in listObBoard)
        {
            item.SetActive(true);
        }
        foreach (var item in dicAds)
        {
            item.Value.mesh.transform.parent.GetComponent<MeshRenderer>().enabled = true;
            item.Value.mesh.enabled = true;
        }
    }

    void hideAds()
    {
        foreach (var item in dicAds)
        {
            item.Value.mesh.transform.parent.GetComponent<MeshRenderer>().enabled = false;
            item.Value.mesh.enabled = false;
        }
        foreach (var item in listObBoard)
        {
            item.SetActive(false);
        }
    }

    public override bool isLoaded()
    {
        isAdLoaded = true;
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
        foreach (var item in dicAds)
        {
            item.Value.mesh = null;
        }
        dicAds.Clear();
        dicAds = null;
    }
}

[Serializable]
public class AdTestInfo
{
    public MeshRenderer mesh;
}