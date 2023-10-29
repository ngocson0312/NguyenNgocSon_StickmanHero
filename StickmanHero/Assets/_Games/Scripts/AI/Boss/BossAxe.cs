using SuperFight;

using System.Collections;
using PathologicalGames;
using UnityEngine;

public class BossAxe : Boss
{
    public GameObject GroundFake;
    public GroundBoss groundBoss;
    public FireBall acidProjectile;
    public ParticleSystem fxDeath;

    IEnumerator AttackAcction()
    {
        yield return new WaitForSeconds(1.5f);
        Action1();
        yield return new WaitForSeconds(3.2f);
        Action1();
        yield return new WaitForSeconds(6.5f);
        StartCoroutine(AttackAcction());
    }

    public void SpawnSlash()
    {
        FireBall fb = PoolManager.Pools["Projectile"].Spawn(acidProjectile.transform).GetComponent<FireBall>();
        fb.transform.position = transform.position + Vector3.up;
        fb.transform.localScale = new Vector3(-1, 1, 1);
        fb.speed = 18;
        DamageInfo damageInfo = new DamageInfo();
        damageInfo.damage = stats.damage / 3;
        damageInfo.idSender = core.combat.getColliderInstanceID;
        fb.Initialize(Vector3.left, damageInfo);
    }

    void Action1()
    {
        animatorHandle.PlayAnimation("Boss2Riu_Attack2", 0.1f, 1, true);
    }

    public void ActiveGroundBoss()
    {
        groundBoss.Active();
    }

    public override void OnTakeDamage(DamageInfo damageInfo)
    {
        if (!isActive) return;
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
        StartCoroutine(AttackAcction());
    }

    protected override void UpdateLogic()
    {
    }

    protected override void UpdatePhysic()
    {
    }
}
