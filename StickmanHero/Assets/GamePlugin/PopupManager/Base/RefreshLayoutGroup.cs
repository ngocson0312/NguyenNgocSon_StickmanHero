using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RefreshLayoutGroup : MonoBehaviour
{
    public bool refreshOnEnable;

    public float delay;
    private LayoutGroup group;

    private void Awake()
    {
        group = GetComponent<LayoutGroup>();
    }

    private void OnEnable()
    {
        if(!refreshOnEnable) return;
        StartCoroutine(Refresh(delay));
    }

    private IEnumerator Refresh(float wait = 0)
    {
        yield return new WaitForSeconds(wait);
        group.CalculateLayoutInputHorizontal();
        group.CalculateLayoutInputVertical();
        group.SetLayoutVertical();
        group.SetLayoutHorizontal();
    }
}
