using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SuperFight
{
    public class SpiderAnimator : EnemyAnimator
    {
        public Transform arrowPrefab;
        public Transform firePoint;
        public override void ResetAnimator()
        {
            gameObject.SetActive(true);
            ResumeAnimator();
            animator.Rebind();
        }
        void ThrowSilk()
        {
            Arrow arrow = PathologicalGames.PoolManager.Pools["Projectile"].Spawn(arrowPrefab).GetComponent<Arrow>();
            arrow.transform.position = firePoint.position;
            Vector3 d = Vector3.down;
            DamageInfo damageInfo = new DamageInfo();
            damageInfo.idSender = controller.core.combat.getColliderInstanceID;
            damageInfo.damage = controller.stats.damage;
            arrow.Initialize(d.normalized, damageInfo);
        }
    }
}
