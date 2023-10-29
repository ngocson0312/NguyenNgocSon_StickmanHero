using PathologicalGames;
using System.Collections.Generic;
using UnityEngine;
namespace SuperFight
{
    public class GroundEnemy : Enemy
    {
        public Vector2 offsetView;
        public Vector2 viewBounds;
        public State patrol;
        public State chaseState;
        public State attackState;
        public float attackSpeed = 1;

        // public bool isDead = false;
        [HideInInspector]
        public float attackRange
        {
            get;
            private set;
        }
        public override void Initialize()
        {
            if (isInit) return;
            base.Initialize();
            attackRange = Mathf.Pow(animator.currentWeapon.attackRange, 2);
            animator = (EnemyAnimator)animatorHandle;
            healthBar.UpdateBar(1);
            patrol = new BasicPatrolState(this, "patrol");
            chaseState = new BasicChaseState(this, "chase");
            attackState = new BasicAttackState(this, "attack");
            core.Active();
        }
        public override Controller GetTargetInView()
        {
            Vector2 origin = (Vector2)transform.position + new Vector2(offsetView.x * core.movement.facingDirection, offsetView.y);
            Bounds bounds = new Bounds(origin, viewBounds);
            Bounds playerBound = PlayerManager.Instance.GetBoundPlayer();
            if (bounds.Intersects(playerBound))
            {
                return PlayerManager.Instance.playerController;
            }
            return null;
        }
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            int f = core.movement.facingDirection;
            if (f == 0)
            {
                f = 1;
            }
            Vector2 os = new Vector2(offsetView.x * f, offsetView.y);
            Gizmos.DrawWireCube(transform.position + (Vector3)os, viewBounds);
            // Gizmos.DrawWireSphere(transform.position, sensorRange);
        }
        protected override void LogicUpdate()
        {
            base.LogicUpdate();
            HandleRotate();
        }
        void HandleRotate()
        {
            if (core.movement.facingDirection > 0)
            {
                animatorHandle.transform.localRotation = Quaternion.Lerp(animatorHandle.transform.localRotation, Quaternion.Euler(0, 110, 0), 0.2f);
            }
            else if (core.movement.facingDirection < 0)
            {
                animatorHandle.transform.localRotation = Quaternion.Lerp(animatorHandle.transform.localRotation, Quaternion.Euler(0, 240, 0), 0.2f);
            }
        }
       
        public override void OnTakeDamage(DamageInfo damageInfo)
        {
            if (!isActive) return;
            base.OnTakeDamage(damageInfo);
            stats.ApplyDamage(damageInfo.damage);
            float normalizedHealth = stats.normalizedHealth;
            healthBar.UpdateBar(normalizedHealth);
            if (normalizedHealth <= 0)
            {
                animatorHandle.PlayAnimation("Dead", 0.1f, 1, true);
                core.movement.SetVelocity(new Vector2(damageInfo.hitDirection, 1), 15);
                Die(false);
            }
        }
        public override void ResetStatEnemy(CharacterStats overrideStats)
        {
            base.ResetStatEnemy(overrideStats);
            SwitchState(patrol);
        }
        public override void DetectPlayer()
        {

        }
    }
}

