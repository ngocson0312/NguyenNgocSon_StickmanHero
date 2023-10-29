using SuperFight;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderHanging : Enemy
{
    public float moveTime;
    public LineRenderer silk;
    public Transform firePoint;
    public Arrow arrowPrefab;
    public float moveRange;
    public float firstDelay;
    public MoveState moveState;
    
    public override void Initialize()
    {
        base.Initialize();
        moveState = new MoveState(this, "attack");
        SwitchState(moveState);
    }
   
    public override void OnTakeDamage(DamageInfo damageInfo)
    {
        base.OnTakeDamage(damageInfo);
        stats.ApplyDamage(damageInfo.damage);
        float normalizedHealth = stats.normalizedHealth;
        healthBar.UpdateBar(normalizedHealth);
        if (normalizedHealth <= 0)
        {
            moveState.ExitState();
            animatorHandle.PlayAnimation("Dead", 0.1f, 1, true);
            core.movement.SetVelocity(new Vector2(damageInfo.hitDirection, 1), 10);
            Die(false);
        }
    }

    public void OnTouchPlayer(IDamage idamage)
    {
        DamageInfo damageInfo = new DamageInfo();
        damageInfo.damage = stats.damage;
        idamage.TakeDamage(damageInfo);
    }

    public override void DetectPlayer()
    {

    }
    public override Controller GetTargetInView()
    {
        return null;
    }
}
