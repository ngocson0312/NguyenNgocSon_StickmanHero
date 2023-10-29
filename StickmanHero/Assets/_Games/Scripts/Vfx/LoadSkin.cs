using Spine.Unity;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadSkin : MonoBehaviour
{
    [SerializeField] bool canPlayAnim = false;
    [SerializeField] string[] listAnimName;
    [SerializeField] SkeletonAnimation[] skeletonAnimation;
    int idx = 0;
    private void OnEnable()
    {
        StartCoroutine(DelaySetSkin());
        if (canPlayAnim)
        {
            StartCoroutine(playAnim());
        }
    }
    
    IEnumerator DelaySetSkin()
    {
        yield return new WaitForSeconds(0.03f);
        string skinName = DataManager.Instance.currentSkin;
        if (skinName.Contains("Diana") || skinName.Contains("DrStrange"))
        {
            skeletonAnimation[0].gameObject.SetActive(false);
            skeletonAnimation[1].gameObject.SetActive(true);
            idx = 1;
        }
        else
        {
            skeletonAnimation[0].gameObject.SetActive(true);
            skeletonAnimation[1].gameObject.SetActive(false);
            idx = 0;
        }
        skeletonAnimation[idx].skeleton.SetSkin(skinName);
        skeletonAnimation[idx].Skeleton.SetSlotsToSetupPose();
        skeletonAnimation[idx].LateUpdate();
    }

    IEnumerator playAnim()
    {
        yield return new WaitForSeconds(0.04f);
        int ranNum = Random.Range(0, listAnimName.Length);
        skeletonAnimation[idx].state.SetAnimation(0, listAnimName[ranNum], true);
    }
}
