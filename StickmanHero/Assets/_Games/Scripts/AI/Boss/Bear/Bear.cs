using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SuperFight
{
    public class Bear : Boss
    {
        public float speed = 4f;
        public LayerMask layerTarget;
        public Vector2 centerView;
        public Vector2 viewSize;
        public BearIdleState idleState;
        public BearChaseState chaseState;
        public BearAttackState attackState;
        public override void Initialize(BossFightArena arena)
        {
            base.Initialize(arena);
            idleState = new BearIdleState(this, "Idle");
            chaseState = new BearChaseState(this, "Chase");
            attackState = new BearAttackState(this, "Attack");
            Active();
        }
        public override void Active()
        {
            isActive = true;
            SwitchState(idleState);
        }

        protected override void UpdateLogic()
        {
            isInteracting = animatorHandle.animator.GetBool("IsInteracting");
            animatorHandle.SetFloat("MoveAmount", Mathf.Abs(core.movement.currentVelecity.x));
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
        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireCube(transform.position + (Vector3)centerView, viewSize);
            Gizmos.DrawWireSphere(transform.position, 3f);
        }
        protected override void UpdatePhysic()
        {
        }
    }
}

