using Spine.Unity.Examples;

using SuperFight;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeeSin : Character
{
    public FireBall slashRiven;
    public SkeletonGhost skeletonGhost;

    public ParticleSystem[] slashFx;

    public Transform shootPosition;
    public ParticleSystem muzzle;
    public FireBall fireBall_Leesin;

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
    public void TriggerFx()
    {
        if (!currentWeapon.hasFx) return;
        if (PlayerManager.Instance.transform.localScale.x == 1)
        {
            slashFx[currentIndexMoveSet].transform.localScale = new Vector3(10, 10, 10);
        }
        else
        {
            slashFx[currentIndexMoveSet].transform.localScale = new Vector3(-10, 10, 10);
        }
        slashFx[currentIndexMoveSet].Play();
    }
    public override void TriggerSkill()
    {
        base.TriggerSkill();
        TriggerFx();
    }
    public void Shoot()
    {
        muzzle.Play();
        FireBall fb = PathologicalGames.PoolManager.Pools["Projectile"].Spawn(fireBall_Leesin.transform).GetComponent<FireBall>();
        fb.transform.position = shootPosition.position;
        DamageInfo damageInfo = new DamageInfo();
        damageInfo.characterType = controller.characterType;
        int damageBonus = controller.stats.damage * currentWeapon.damage / 100;
        damageInfo.damage = controller.stats.damage + damageBonus;
        damageInfo.idSender = controller.core.combat.getColliderInstanceID;
        fb.transform.localScale = Vector3.one;
        if (PlayerManager.Instance.transform.localScale.x == 1)
        {
            fb.Initialize(Vector2.right, damageInfo);
        }
        else
        {
            fb.Initialize(-Vector2.right, damageInfo);
        }
    }

    public void Missle()
    {
        muzzle.Play();
        SpawnMissLe(Vector2.up * 0.1f);
        SpawnMissLe(Vector2.up * 0.2f);
        SpawnMissLe(Vector2.zero);
    }

    public void SpawnMissLe(Vector2 dir)
    {
        FireBall fb = PathologicalGames.PoolManager.Pools["Projectile"].Spawn(fireBall_Leesin.transform).GetComponent<FireBall>();
        fb.transform.position = shootPosition.position;
        DamageInfo damageInfo = new DamageInfo();
        damageInfo.idSender = controller.core.combat.getColliderInstanceID;
        int damageBonus = controller.stats.damage * currentWeapon.damage / 100;
        damageInfo.damage = controller.stats.damage + damageBonus;
        damageInfo.characterType = controller.characterType;
        fb.transform.localScale = Vector3.one * .5f;
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
