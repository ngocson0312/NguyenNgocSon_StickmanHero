using SuperFight;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossVillageSolider : Boss
{
    public FireBall acidProjectile;
    public ParticleSystem smokeDust;
    public ParticleSystem fxDeath;
    public GameObject GroundFake;
    public GameObject GroundFake2;
    float originY;
    public override void Initialize(BossFightArena bossFightArena)
    {
        base.Initialize(bossFightArena);
        originY = transform.position.y;
    }

    public override void Active()
    {
        StartCoroutine(AttackAcction(true));
        if (GroundFake2 != null)
        {
            GroundFake2.SetActive(true);
        }
    }


    IEnumerator AttackAcction(bool isFirst)
    {
        if (isFirst)
        {
            yield return new WaitForSeconds(1);
        }
        else
        {
            yield return new WaitForSeconds(3);
        }
        JumpToPlayer();
        yield return new WaitForSeconds(3);
        JumpToPlayer();
        yield return new WaitForSeconds(3);
        JumpToPlayer();
        yield return new WaitForSeconds(4);
        StartCoroutine(AttackAcction(false));
    }

    void JumpToPlayer()
    {
        CheckRotate();
        animatorHandle.PlayAnimation("Warrior_Attack_3", 0.1f, 0, true);
        float time = 0.5f * 1.25f;
        float playerPos = PlayerManager.Instance.transform.position.x;
        transform.DOMoveY(transform.position.y + 4, time).SetEase(Ease.OutQuart).SetLoops(2, LoopType.Yoyo).OnComplete(() =>
        {
            transform.position = new Vector3(transform.position.x, originY, 0);
        });
        transform.DOMoveX(playerPos, time * 2);
    }

    public void SpawnSlash()
    {
        smokeDust.Play();
        CheckRotate();
        CameraController.Instance.ShakeCamera(.5f, 1f, 10, 90, true);
        float timeDelay = 0;
        for (int i = 0; i < 3; i++)
        {
            StartCoroutine(_SpawnSlash(timeDelay, Vector3.right, new Vector3(1, 1, 1)));
            timeDelay += 0.3f;
        }
        timeDelay = 0;
        for (int i = 0; i < 3; i++)
        {
            StartCoroutine(_SpawnSlash(timeDelay, -Vector3.right, new Vector3(-1, 1, 1)));
            timeDelay += 0.3f;
        }
    }
    public override void Resume()
    {
        base.Resume();
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
    }
    IEnumerator _SpawnSlash(float delayTime, Vector3 dir, Vector3 scale)
    {
        yield return new WaitForSeconds(delayTime);
        FireBall fb = PathologicalGames.PoolManager.Pools["Projectile"].Spawn(acidProjectile.transform).GetComponent<FireBall>();
        fb.transform.position = transform.position;
        fb.transform.localScale = scale;
        DamageInfo damageInfo = new DamageInfo();
        damageInfo.damage = stats.damage / 3;
        damageInfo.idSender = core.combat.getColliderInstanceID;
        fb.Initialize(dir, damageInfo);
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
        if (!isActive) return;
        base.OnTakeDamage(damageInfo);
        if (stats.currHealth <= 0)
        {
            fxDeath.Play();
            GameplayCtrl.Instance.CreateCoinBoss(transform.position + new Vector3(0, 0, -1));
            SoundManager.Instance.playSoundFx(SoundManager.Instance.effBossDie);
            if (GroundFake != null)
            {
                GroundFake.SetActive(false);
            }
            isActive = false;
            animatorHandle.PlayAnimation("Die", 0.1f, 0, true);
            Die(true);
        }
        else
        {
            SoundManager.Instance.playRandFx(TYPE_RAND_FX.FX_TAKE_DAMAGE);
            animatorHandle.PlayAnimation("Hit", 0.1f, 0, true);
        }
    }



    protected override void UpdateLogic()
    {
    }

    protected override void UpdatePhysic()
    {
    }
}
