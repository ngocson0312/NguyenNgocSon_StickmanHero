using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SuperFight
{
    public class NineTailFox : Boss
    {
        public NineTailFoxAnimator animator { get; set; }
        public float speed = 4f;
        public NineTailFoxIdleState idleState;
        public NineTailFoxChaseState chaseState;
        public NineTailFoxAttackState attackState;
        public LayerMask layerTarget;
        public Vector2 centerView;
        public Vector2 viewSize;
        public Transform sensorPoint;
        public ParticleSystem bossDead;
        public override void Initialize(BossFightArena arena)
        {
            idleState = new NineTailFoxIdleState(this, "Idle");
            chaseState = new NineTailFoxChaseState(this, "ChaseState");
            attackState = new NineTailFoxAttackState(this, "Attack");
            base.Initialize(arena);
            animator = (NineTailFoxAnimator)base.animatorHandle;
        }
        public override void Active()
        {
            if (Random.Range(0, 100) <= 50)
            {
                SwitchState(idleState);
            }
            else
            {
                SwitchState(chaseState);
            }
            isActive = true;
        }
        protected override void UpdateLogic()
        {
            animator.SetFloat("MoveAmount", Mathf.Abs(core.movement.currentVelecity.x));
            isInteracting = animator.animator.GetBool("IsInteracting");
            HandleRotate();
        }
        void HandleRotate()
        {
            // if (isInteracting) return;
            if (core.movement.facingDirection < 0)
            {
                animator.transform.localRotation = Quaternion.Lerp(animator.transform.localRotation, Quaternion.Euler(0, 270, 0), 0.5f);
            }
            if (core.movement.facingDirection > 0)
            {
                animator.transform.localRotation = Quaternion.Lerp(animator.transform.localRotation, Quaternion.Euler(0, 100, 0), 0.5f);
            }
        }
        protected override void UpdatePhysic()
        {

        }
        public Collider2D[] GetTargetsInView()
        {
            var colls = new Collider2D[3];
            Physics2D.OverlapBoxNonAlloc(sensorPoint.position + (Vector3)centerView, viewSize, 0, colls, layerTarget);

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
                bossDead.Play();
                animator.beam.Deactive();
                animator.PlayAnimation("Die", 0.1f, 1, false);
                Die(true);
            }
            else
            {
                animator.PlayAnimation("Hit", 0.1f, 0, false);
            }
        }
        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireCube(sensorPoint.position + (Vector3)centerView, viewSize);
            Gizmos.DrawWireSphere(sensorPoint.position, 3f);
        }

    }
}

