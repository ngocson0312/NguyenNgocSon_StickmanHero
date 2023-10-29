using Spine.Unity.Examples;

using SuperFight;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Qiyana : Character
{
    public FireBall slashRiven;
    public ParticleSystem[] slashFx;

    public Transform shootPosition;
    public ParticleSystem muzzle;
    public FireBall fireBall_Qiyana;
    public FireBall fireBall_Qiyana2;
    public Transform direc;
    private void Update()
    {
        
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
        TriggerSkill();
    }

    public void Shoot()
    {
        muzzle.Play();
        FireBall fb = PathologicalGames.PoolManager.Pools["Projectile"].Spawn(fireBall_Qiyana.transform).GetComponent<FireBall>();
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
    public void Missle()
    {
        muzzle.Play();
        SpawnMissLe(Direction(direc.position - Vector3.right * 2));
        SpawnMissLe(Direction(direc.position));
        SpawnMissLe(Direction(direc.position + Vector3.right * 2));
    }
    Vector3 Direction(Vector3 _target)
    {
        return (_target - shootPosition.position).normalized;
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
    public void SpawnMissLe(Vector2 dir)
    {
        FireBall fb = PathologicalGames.PoolManager.Pools["Projectile"].Spawn(fireBall_Qiyana2.transform).GetComponent<FireBall>();
        fb.transform.position = shootPosition.position;
        DamageInfo damageInfo = new DamageInfo();
        damageInfo.idSender = controller.core.combat.getColliderInstanceID;
        int damageBonus = controller.stats.damage * currentWeapon.damage / 100;
        damageInfo.damage = controller.stats.damage + damageBonus;
        damageInfo.characterType = controller.characterType;
        fb.transform.rotation = Quaternion.LookRotation(dir);
        fb.Initialize(dir, damageInfo);
    }
}
