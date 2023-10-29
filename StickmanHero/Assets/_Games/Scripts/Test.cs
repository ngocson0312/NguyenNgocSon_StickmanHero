using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
public class Test : MonoBehaviour
{
    public delegate void Hahaha(int ve);
    public static Hahaha sss;
    private void Start()
    {
        sss += ZZ;
    }
    void ZZ(int a)
    {
        Debug.Log(a);
    }
}
