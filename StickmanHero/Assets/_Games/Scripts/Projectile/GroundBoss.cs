using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundBoss : MonoBehaviour
{
    public GameObject[] listFx;
    public float delayTime = 0.5f;
    public void SetUp()
    {
        for (int i = 0; i < listFx.Length; i++)
        {
            listFx[i].SetActive(false);
        }
    }

    public void Active()
    {
        float time = 0;
        for (int i = 0; i < listFx.Length; i++)
        {
            StartCoroutine(ActiveFx(time, i));
            time += delayTime;
        }
    }

    IEnumerator ActiveFx(float time, int id)
    {
        yield return new WaitForSeconds(time);
        listFx[id].SetActive(true);
    }
}
