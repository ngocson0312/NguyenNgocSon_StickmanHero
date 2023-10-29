using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using GamePlugins;
using UnityEngine.UI;
using SuperFight;

public class CoinFx : Singleton<CoinFx>
{
    public float range;
    public float moveTime;
    public float delayTime;
    public Transform endPos;
    public void PlayFx(System.Action callBack)
    {
        StartCoroutine(DelayPlayFx(callBack));
    }

    IEnumerator DelayPlayFx(System.Action callBack)
    {
        yield return new WaitForSeconds(0.5f);
        SoundManager.Instance.playSoundFx(SoundManager.Instance.effCoinUI);
        GetComponent<Image>().DOFade(0.7f, moveTime / 2).OnComplete(() =>
        {
            GetComponent<Image>().DOFade(0, moveTime / 2).SetDelay(moveTime / 2);
        });

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform curChild = transform.GetChild(i);
            curChild.gameObject.SetActive(true);
            float ranNumX = Random.Range(-range, range);
            float ranNumY = Random.Range(-range, range);
            curChild.localPosition = new Vector3(ranNumX, ranNumY);
            curChild.localScale = Vector3.zero;
            curChild.DOScale(1, moveTime).SetEase(Ease.OutElastic).SetDelay(Random.Range(0, 0.3f)).OnComplete(() =>
            {
                curChild.DOMove(endPos.position, moveTime).SetEase(Ease.InOutQuad).SetDelay(delayTime).OnComplete(() =>
                {
                    curChild.gameObject.SetActive(false);
                    SoundManager.Instance.playSoundFx(SoundManager.Instance.effCollectCoin);
                    callBack.Invoke();
                });
            });
        }
    }

}
