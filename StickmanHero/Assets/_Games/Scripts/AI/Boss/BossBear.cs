using DG.Tweening;

using SuperFight;

using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class BossBear : Boss
{
    public ParticleSystem smokeDust;
    public ParticleSystem growlFx;
    public ParticleSystem slashFx;
    public ParticleSystem flameFx;
    public ParticleSystem fxDeath;
    public BoxCollider2D boxSlash;

    public GameObject GroundFake;
    public GameObject GroundFake2;

    public Transform[] spikePos;
    public GameObject spikeUp;

    private List<Transform> curSpike = new List<Transform>();

    void Action1()
    {
        animatorHandle.PlayAnimation("Action3", 0.1f, 1, true);
    }

    IEnumerator AttackAcction()
    {
        yield return new WaitForSeconds(2.5f);
        Action1();
        yield return new WaitForSeconds(2.5f);
        Action1();
        yield return new WaitForSeconds(5);
        StartCoroutine(AttackAcction());
    }

    public void JumpToPlayer()
    {
        CheckRotate();
        float time = 0.5f;
        float playerPos = PlayerManager.Instance.transform.position.x;
        transform.DOMoveY(transform.position.y + 5, time).SetEase(Ease.OutQuart).SetLoops(2, LoopType.Yoyo);
        transform.DOMoveX(playerPos, time * 2).SetEase(Ease.Linear).OnComplete(() =>
        {
            Landing();
        });
    }

    public void Landing()
    {
        smokeDust.Play();
        CheckRotate();
        CameraController.Instance.ShakeCamera(.5f, 1f, 10, 90, true);
        SpawnSpike();
        boxSlash.enabled = true;
        StartCoroutine(DisableSlash());
    }
    public override void Resume()
    {
        base.Resume();
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
    }
    public void Growl()
    {
        growlFx.Play();
        CameraController.Instance.ShakeCamera(1.5f, 0.5f, 10, 90, true);
        for (int i = 0; i < 5; i++)
        {
            ItemSpikeFallTrap[] a = curSpike[i].GetChild(0).GetComponentsInChildren<ItemSpikeFallTrap>();
            for (int j = 0; j < a.Length; j++)
            {
                a[j].Active();
            }
        }
    }

    public void Slash()
    {
        slashFx.Play();
        boxSlash.enabled = true;
        StartCoroutine(DisableSlash());
    }

    IEnumerator DisableSlash()
    {
        yield return new WaitForSeconds(0.2f);
        boxSlash.enabled = false;
    }

    public void Flame()
    {
        flameFx.Play();
        flameFx.GetComponent<BoxCollider2D>().enabled = true;
        StartCoroutine(DisableFlame());
    }

    IEnumerator DisableFlame()
    {
        yield return new WaitForSeconds(1);
        flameFx.GetComponent<BoxCollider2D>().enabled = false;
    }

    void SpawnSpike()
    {
        for (int i = 0; i < curSpike.Count; i++)
        {
            Destroy(curSpike[i].GetChild(0).gameObject);
        }
        curSpike.Clear();

        List<Transform> a = spikePos.ToList();
        for (int i = 0; i < 5; i++)
        {
            int RanNum = Random.Range(0, a.Count);
            curSpike.Add(a[RanNum]);
            GameObject b = Instantiate(spikeUp, a[RanNum]);
            b.transform.localPosition = Vector3.zero + Vector3.up * 2;
            b.transform.DOLocalMove(Vector3.zero, 0.3f);
            a.RemoveAt(RanNum);
        }
    }

    void CheckRotate()
    {
        float playerPos = PlayerManager.Instance.transform.position.x;
        if (playerPos <= transform.position.x)
        {
            transform.GetChild(0).localRotation = Quaternion.Euler(0, 220, 0);
        }
        else
        {
            transform.GetChild(0).localRotation = Quaternion.Euler(0, 140, 0);
        }
    }

    public override void OnTakeDamage(DamageInfo damageInfo)
    {
        if(!isActive)return;
        base.OnTakeDamage(damageInfo);
        if (stats.currHealth <= 0)
        {
            if (GroundFake != null)
            {
                GroundFake.SetActive(false);
            }
            isActive = false;
            fxDeath.Play();
            GameplayCtrl.Instance.CreateCoinBoss(transform.position + new Vector3(0, 0, -1));
            SoundManager.Instance.playSoundFx(SoundManager.Instance.effBossDie);
            animatorHandle.PlayAnimation("Die", 0.1f, 1, true);
            Die(true);
        }
        else
        {
            SoundManager.Instance.playRandFx(TYPE_RAND_FX.FX_TAKE_DAMAGE);
            animatorHandle.PlayAnimation("Hit", 0.1f, 0, true);
        }
    }

    public override void Active()
    {
        isActive = true;
        boxSlash.GetComponent<Electric>().SetDamage(stats.damage);
        flameFx.GetComponent<Laser>().SetDamage(stats.damage / 3);
        if (GroundFake2 != null)
        {
            GroundFake2.SetActive(true);
        }
        StartCoroutine(AttackAcction());
        SpawnSpike();
        CheckRotate();
    }

    protected override void UpdateLogic()
    {
    }

    protected override void UpdatePhysic()
    {
    }
}
