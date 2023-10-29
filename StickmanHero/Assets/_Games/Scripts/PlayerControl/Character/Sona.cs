using Spine.Unity.Examples;

using SuperFight;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sona : Character
{
    public SkeletonGhost skeletonGhost;

    public FireBall FireBall_Sona;
    public SonaUlti ultiSona;
    public Transform shootPosition;
    public ParticleSystem muzzle;
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
        SpawShoot(.3f);
        SpawShoot(-.3f);
    }
    
    void SpawShoot(float offsetY)
    {
        muzzle.Play();
        FireBall fb = PathologicalGames.PoolManager.Pools["Projectile"].Spawn(FireBall_Sona.transform).GetComponent<FireBall>();
        fb.transform.position = shootPosition.position;
        fb.transform.DOLocalMoveY(shootPosition.position.y + offsetY, 0.18f).SetEase(Ease.InOutQuad).OnComplete(() =>
        {
            fb.transform.DOLocalMoveY(shootPosition.position.y - offsetY, 0.18f).SetEase(Ease.InOutQuad).SetLoops(20, LoopType.Yoyo);
        });
        DamageInfo damageInfo = new DamageInfo();
        damageInfo.characterType = controller.characterType;
        int damageBonus = controller.stats.damage * currentWeapon.damage / 100;
        damageInfo.damage = controller.stats.damage/2 + damageBonus/2;
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
        //TriggerSkill();
        SonaUlti fb = PathologicalGames.PoolManager.Pools["Projectile"].Spawn(ultiSona.transform).GetComponent<SonaUlti>();
        fb.transform.position = shootPosition.position + new Vector3(-0.25f *PlayerManager.Instance.transform.localScale.x, 0.2f, 0);
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
