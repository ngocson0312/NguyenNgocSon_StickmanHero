using Spine.Unity;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadSkinData : MonoBehaviour
{
    public SkeletonAnimation skeletonAnimation;
    private void OnEnable()
    {
        LoadSkin("");
    }
    public void LoadSkin(string skinName)
    {
        skeletonAnimation.skeleton.SetSkin(skinName);
        skeletonAnimation.skeleton.SetBonesToSetupPose();
        skeletonAnimation.skeleton.SetToSetupPose();
    }
}
