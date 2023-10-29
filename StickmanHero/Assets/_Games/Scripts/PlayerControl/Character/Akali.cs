using Spine.Unity.Examples;

using SuperFight;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Akali : Character
{
    public FireBall slashRiven;
    public SkeletonGhost skeletonGhost;

    public ParticleSystem[] slashFx;

    public Transform shootPosition;
    public ParticleSystem muzzle;
    public FireBall fireBallKaisa;
    public Transform direc;
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
        //SoundManager.Instance.playSoundFx(currentWeapon.moveSets[currentIndexMoveSet].activeSound);
    }
    public override void TriggerSkill()
    {
        base.TriggerSkill();
        TriggerFx();
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(shootPosition.position, direc.position - Vector3.right * 2);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(shootPosition.position, direc.position);
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(shootPosition.position, direc.position + Vector3.right * 2);
    }
    public void Shoot()
    {
        //muzzle.Play();
        SpawnMissLe(Direction(direc.position - Vector3.right * 2));
        SpawnMissLe(Direction(direc.position));
        SpawnMissLe(Direction(direc.position + Vector3.right * 2));
    }
    Vector3 Direction(Vector3 _target)
    {
        return (_target - shootPosition.position).normalized;
    }
    public void SpawnMissLe(Vector2 dir)
    {
        FireBall fb = PathologicalGames.PoolManager.Pools["Projectile"].Spawn(fireBallKaisa.transform).GetComponent<FireBall>();
        fb.transform.position = shootPosition.position;
        DamageInfo damageInfo = new DamageInfo();
        damageInfo.idSender = controller.core.combat.getColliderInstanceID;
        int damageBonus = controller.stats.damage * currentWeapon.damage / 100;
        damageInfo.damage = controller.stats.damage + damageBonus;
        damageInfo.characterType = controller.characterType;
        fb.transform.rotation = Quaternion.LookRotation(dir);
        fb.Initialize(dir, damageInfo);
        /*if (PlayerManager.Instance.transform.localScale.x == 1)
        {
            fb.Initialize(dir, damageInfo);
        }
        else
        {
            fb.Initialize(-dir, damageInfo);
        }*/
    }
}
