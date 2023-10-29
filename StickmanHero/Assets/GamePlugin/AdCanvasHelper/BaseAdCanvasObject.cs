using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum AdCanvasSize
{
    Size6x5 = 0,
    Size_3f_6x5,
    Size32x5,
    Size4x3Video
}

public abstract class BaseAdCanvasObject : MonoBehaviour
{
    public AdCanvasSize adType;
    public BaseAdCanvas adCanvasHelper { get; private set; }
    public bool disableObj = false;

    [HideInInspector] public Vector3 pos;
    [HideInInspector] public Vector3 forward;
    [HideInInspector] public Transform target;
    [HideInInspector] public int stateLoockat = 0;

    public virtual void initAds(BaseAdCanvas adhelper, Texture2D ttdf)
    {
        adCanvasHelper = adhelper;
    }

    public abstract void onUpdate();
    public abstract bool isLoaded();
    public abstract void enableMesh();
    public abstract void setFollowY(bool isfl, float yTaget = -10000);
}

[Serializable]
public class BaseAdInfo
{
    public Renderer mesh;
    public Renderer defaultMesh;
    [HideInInspector] public bool isLoadDefault = false;
    [HideInInspector] public bool isAdLoaded;
}
