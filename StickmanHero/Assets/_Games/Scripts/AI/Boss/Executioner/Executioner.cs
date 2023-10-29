using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SuperFight
{
    public class Executioner : Boss
    {
        public float speed = 4f;
        public LayerMask layerTarget;
        public Vector2 centerView;
        public Vector2 viewSize;
        public float moveAmount;
        public float xMax;
        public float xMin;
        public ExecutionerIdleState idleState;
        public ExecutionerAttackState attackState;
        public ParticleSystem deathFX;
        public override void Initialize(BossFightArena arena)
        {
            base.Initialize(arena);
            idleState = new ExecutionerIdleState(this, "Idle");
            attackState = new ExecutionerAttackState(this, "Attack");
        }
        public override void Active()
        {
            isActive = true;
            SwitchState(idleState);
        }

        protected override void UpdateLogic()
        {
            isInteracting = animatorHandle.animator.GetBool("IsInteracting");
            animatorHandle.SetFloat("MoveAmount", moveAmount);
            HandleRotate();
        }
        void HandleRotate()
        {
            if (core.movement.facingDirection < 0)
            {
                animatorHandle.transform.localRotation = Quaternion.Lerp(animatorHandle.transform.localRotation, Quaternion.Euler(0, 270, 0), 0.5f);
            }
            if (core.movement.facingDirection > 0)
            {
                animatorHandle.transform.localRotation = Quaternion.Lerp(animatorHandle.transform.localRotation, Quaternion.Euler(0, 90, 0), 0.5f);
            }
        }
        protected override void UpdatePhysic()
        {

        }
        public Collider2D[] GetTargetsInView()
        {
            var colls = new Collider2D[3];
            Physics2D.OverlapBoxNonAlloc(transform.position + (Vector3)centerView, viewSize, 0, colls, layerTarget);

            List<Collider2D> listColl = new List<Collider2D>();
            for (int i = 0; i < colls.Length; i++)
            {
                if (colls[i] != null && !colls[i].Equals(core.combat.GetComponent<Collider2D>()) && colls[i].GetComponent<Combat>().getType != characterType)
                {
                    listColl.Add(colls[i]);
                }
            }
            return listColl.ToArray();
        }
        public override void OnTakeDamage(DamageInfo damageInfo)
        {
            if (!isActive) return;
            base.OnTakeDamage(damageInfo);
            if (stats.currHealth <= 0)
            {
                isActive = false;
                deathFX.Play();
                Die(true);
                animatorHandle.PlayAnimation("Die", 0.1f, 1, false);
                SoundManager.Instance.playSoundFx(SoundManager.Instance.effBossDie);
                GameplayCtrl.Instance.CreateCoinOnKillBoss(transform.position, 20, 40);
            }
            else
            {
                animatorHandle.PlayAnimation("Hit", 0.1f, 0, false);
            }
        }
        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireCube(transform.position + (Vector3)centerView, viewSize);
            Gizmos.DrawWireSphere(transform.position, 3f);
        }
    }

}
