using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AdCanvasType
{
    GadsMe = 0,
    AdVerty
}

public abstract class  BaseAdCanvas : MonoBehaviour
{
    public AdCanvasType TypeAdCanvas;

    public abstract void onAwake();

    public abstract void onStart();

    public abstract void onChangeCamera(Camera newCamera);

    public abstract void onShowCmpNative();

    public abstract void onCMPOK(string iABTCv2String);

    public abstract void onUpdate();

    public abstract BaseAdCanvasObject genAd(AdCanvasSize type, Vector3 pos, Vector3 forward, Transform _target = null, int stateLookat = 0, bool isFloowY = false);
    
    public abstract void initClick(bool isClick);

    public abstract void freeAd(BaseAdCanvasObject ad);
    
    public abstract void freeAll();
}
