using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
public class Test1 : MonoBehaviour
{
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1f);
        Test.sss.Invoke(20);
    }
}
