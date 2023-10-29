using Spine.Unity.Examples;

using SuperFight;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vi : Character
{
    public FireBall slashRiven;
    public SkeletonGhost skeletonGhost;

    public ParticleSystem[] slashFx;

    public Transform shootPosition;
    public ParticleSystem muzzle;
    public FireBall fireBall_Vi;

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
            if(slashFx[currentIndexMoveSet] == slashFx[2])
            {
                slashFx[currentIndexMoveSet].transform.localScale = new Vector3(17, 17, 17);
            }
        }
        else
        {
            slashFx[currentIndexMoveSet].transform.localScale = new Vector3(-10, 10, 10);
            if (slashFx[currentIndexMoveSet] == slashFx[2])
            {
                slashFx[currentIndexMoveSet].transform.localScale = new Vector3(-17, 17, 17);
            }
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
        TriggerSkill();
        FireBall fb = PathologicalGames.PoolManager.Pools["Projectile"].Spawn(slashRiven.transform).GetComponent<FireBall>();
        fb.transform.position = transform.position + Vector3.up;
        DamageInfo damageInfo = new DamageInfo();
        int damageBonus = controller.stats.damage * currentWeapon.damage / 100;
        damageInfo.damage = controller.stats.damage + damageBonus;
        damageInfo.idSender = controller.core.combat.getColliderInstanceID;
        damageInfo.characterType = controller.characterType;
        if (PlayerManager.Instance.transform.localScale.x == 1)
        {
            fb.transform.localScale = new Vector3(1, 1, 1);
            fb.Initialize(Vector2.right, damageInfo);
        }
        else
        {
            fb.transform.localScale = new Vector3(-1, 1, 1);
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
        FireBall fb = PathologicalGames.PoolManager.Pools["Projectile"].Spawn(fireBall_Vi.transform).GetComponent<FireBall>();
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
