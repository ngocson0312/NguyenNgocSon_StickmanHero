using Spine.Unity.Examples;

using SuperFight;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yasuo : Character
{
    public Tornado slashRiven;
    public SkeletonGhost skeletonGhost;

    public ParticleSystem[] slashFx;

    public Transform shootPosition;
    public ParticleSystem muzzle;
    private void Update()
    {
        if (PlayerManager.Instance.playerController.moveAmount != 0 || currentIndexMoveSet == 1)
        {
            skeletonGhost.enabled = true;
        }
        else
        {
            skeletonGhost.enabled = false;
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
    public void Missle()
    {
        //TriggerSkill();
        Tornado fb = PathologicalGames.PoolManager.Pools["Projectile"].Spawn(slashRiven.transform).GetComponent<Tornado>();
        fb.transform.position = transform.position;
        DamageInfo damageInfo = new DamageInfo();
        int damageBonus = controller.stats.damage * currentWeapon.damage / 100;
        damageInfo.damage = controller.stats.damage + damageBonus;
        damageInfo.idSender = controller.core.combat.getColliderInstanceID;
        damageInfo.characterType = controller.characterType;
        if (PlayerManager.Instance.transform.localScale.x == 1)
        {
            fb.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            fb.Initialize(Vector2.right, damageInfo);
        }
        else
        {
            fb.transform.localScale = new Vector3(-0.1f, 0.1f, 0.1f);
            fb.Initialize(-Vector2.right, damageInfo);
        }
    }
    
}
