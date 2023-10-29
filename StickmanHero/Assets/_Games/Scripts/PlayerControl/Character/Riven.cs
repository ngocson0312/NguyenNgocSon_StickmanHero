using Spine.Unity.Examples;

using SuperFight;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Riven : Character
{
    public FireBall slashRiven;
    public SkeletonGhost skeletonGhost;

    public ParticleSystem[] slashFx;


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
            slashFx[currentIndexMoveSet].transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            slashFx[currentIndexMoveSet].transform.localScale = new Vector3(-1, 1, 1);
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
}
