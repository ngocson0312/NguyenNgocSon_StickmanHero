using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using PathologicalGames;
using SuperFight;

public class Bomb : MonoBehaviour
{
    public float speed;
    public DamageInfo damageInfo;
    public ParticleSystem explosionFx;
    public GameObject display;
    public void ActiveBomb(Vector3 target, DamageInfo _damageInfo)
    {
        damageInfo = _damageInfo;
        float distance = Vector3.Distance(transform.position, target);
        if (distance >= 3.2f)
        {
            float time = distance / speed;
            transform.DOMoveY(transform.position.y + 2, time).SetEase(Ease.OutQuad).OnComplete(() =>
            {
                transform.DOMoveY(target.y, time).SetEase(Ease.InQuad).OnComplete(() =>
                {
                    Explosion();
                });
            });
            transform.DOMoveX(target.x, time * 2).SetEase(Ease.Linear);
        }
        else
        {
            transform.DOMove(target + Vector3.up, 0.2f).SetEase(Ease.Linear).OnComplete(() =>
            {
                Explosion();
            });
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.GetInstanceID() != damageInfo.idSender && other.GetComponent<Combat>() != null && other.GetComponent<Combat>().getType != damageInfo.characterType)
        {
            other.GetComponent<IDamage>()?.TakeDamage(damageInfo);
            Explosion();
        }
    }

    void Explosion()
    {
        display.SetActive(false);
        explosionFx.Play();
        transform.DOKill();
        GetComponent<Collider2D>().enabled = false;
        SoundManager.Instance.playSoundFx(SoundManager.Instance.effExplode);
        Destroy(gameObject, 2f);
    }
}
