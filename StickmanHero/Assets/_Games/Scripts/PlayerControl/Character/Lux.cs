using Spine.Unity.Examples;

using SuperFight;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lux : Character
{
    public SkeletonGhost skeletonGhost;
    public Transform shootPosition;
    public ParticleSystem muzzle;
    public FireBall fireBall_Lux;
    public UltimateRainbow ultimateRainbow;
    public Transform Skill2Pos;
    public Transform UltimatePos;
    public LuxExplode FireBall_Skill2;
    private void Update()
    {
        if (PlayerManager.Instance.playerController.moveAmount == 0)
        {
            skeletonGhost.enabled = false;
        }
        else
        {
            skeletonGhost.enabled = true;
        }
    }
    public void Shoot()
    {
        muzzle.Play();
        FireBall fb = PathologicalGames.PoolManager.Pools["Projectile"].Spawn(fireBall_Lux.transform).GetComponent<FireBall>();
        fb.transform.position = shootPosition.position;
        DamageInfo damageInfo = new DamageInfo();
        damageInfo.characterType = controller.characterType;
        int damageBonus = controller.stats.damage * currentWeapon.damage / 100;
        damageInfo.damage = controller.stats.damage + damageBonus;
        damageInfo.idSender = controller.core.combat.getColliderInstanceID;
        if (PlayerManager.Instance.transform.localScale.x == 1)
        {
            fb.Initialize(Vector2.right, damageInfo);
        }
        else
        {
            fb.Initialize(-Vector2.right, damageInfo);
        }
    }
    public void Explosion()
    {
        var StartPos = Skill2Pos.position;
        var direc = (Vector2.right * PlayerManager.Instance.transform.localScale.x).normalized;
        for (int i = 0; i < 3; i++)
        {
            Vector3 _pos = StartPos + (i * 1.5f * direc.x * Vector3.right) ;
            StartCoroutine(IE_SpawnExplode(_pos, i*.15f));
        }
    }
    IEnumerator IE_SpawnExplode(Vector3 _pos,float _delay)
    {
        yield return new WaitForSeconds(_delay);
        SpawnExpolode(_pos);
    }
    void SpawnExpolode(Vector3 _pos)
    {
        LuxExplode fb = PathologicalGames.PoolManager.Pools["Projectile"].Spawn(FireBall_Skill2.transform).GetComponent<LuxExplode>();
        fb.transform.position = _pos;
        DamageInfo damageInfo = new DamageInfo();
        damageInfo.idSender = controller.core.combat.getColliderInstanceID;
        int damageBonus = controller.stats.damage * currentWeapon.damage / 100;
        damageInfo.damage = controller.stats.damage + damageBonus;
        damageInfo.characterType = controller.characterType;
        fb.transform.localScale = new Vector3(PlayerManager.Instance.transform.localScale.x, 1f, 1f);
        fb.Initialize(Vector3.zero, damageInfo);

    }

    public void Missle()
    {
        RainBow();
    }
    void RainBow()
    {
        var direc = (Vector2.right * PlayerManager.Instance.transform.localScale.x).normalized;
        Ultimate(direc);
    }
    void Ultimate(Vector2 dir)
    {
        UltimateRainbow fb = PathologicalGames.PoolManager.Pools["Projectile"].Spawn(ultimateRainbow.transform).GetComponent<UltimateRainbow>();
        fb.transform.position = UltimatePos.position;
        DamageInfo damageInfo = new DamageInfo();
        damageInfo.idSender = controller.core.combat.getColliderInstanceID;
        int damageBonus = controller.stats.damage * currentWeapon.damage / 100;
        damageInfo.damage = controller.stats.damage + damageBonus;
        damageInfo.characterType = controller.characterType;
        fb.transform.localScale = new Vector3(PlayerManager.Instance.transform.localScale.x, 1f, 1f);
        fb.Initialize(Vector3.zero, damageInfo);
    }
    public void SpawnMissLe(Vector2 dir)
    {
        FireBall fb = PathologicalGames.PoolManager.Pools["Projectile"].Spawn(fireBall_Lux.transform).GetComponent<FireBall>();
        fb.transform.position = shootPosition.position;
        DamageInfo damageInfo = new DamageInfo();
        damageInfo.idSender = controller.core.combat.getColliderInstanceID;
        int damageBonus = controller.stats.damage * currentWeapon.damage / 100;
        damageInfo.damage = controller.stats.damage + damageBonus;
        damageInfo.characterType = controller.characterType;
        if (PlayerManager.Instance.transform.localScale.x == 1)
        {
            fb.Initialize(Vector2.right + dir, damageInfo);
        }
        else
        {
            fb.Initialize(-Vector2.right + dir, damageInfo);
        }
    }
}
