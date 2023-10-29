using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SuperFight
{
    public class ExecutionerAnimator : AnimatorHandle
    {
        public ExecutionerAxe[] axes;
        public GameObject[] realAxes;
        public Vector2 groundSlashSize;
        public Vector2 groundSlashPoint;
        public Vector2 slashPoint;
        public Vector2 slashSize;
        private Executioner executioner;
        public override void Initialize(Controller controller)
        {
            base.Initialize(controller);
            executioner = (Executioner)controller;
            for (int i = 0; i < axes.Length; i++)
            {
                axes[i].CatchAxe();
            }
        }
        void ThrowAxes()
        {
            for (int i = 0; i < realAxes.Length; i++)
            {
                realAxes[i].SetActive(false);
            }
            Vector3 direction = new Vector3(controller.core.movement.facingDirection, 0);
            DamageInfo damageInfo = new DamageInfo();
            damageInfo.damage = controller.stats.damage / 2;
            damageInfo.idSender = controller.core.combat.getColliderInstanceID;
            damageInfo.hitDirection = (int)direction.x;
            for (int i = 0; i < axes.Length; i++)
            {
                axes[i].ThrowAxe(damageInfo);
            }
        }
        void CatchAxes()
        {
            for (int i = 0; i < realAxes.Length; i++)
            {
                realAxes[i].SetActive(true);
            }
            for (int i = 0; i < axes.Length; i++)
            {
                axes[i].CatchAxe();
            }
        }
        void GroundSlash()
        {
            Vector3 direction = new Vector3(controller.core.movement.facingDirection, 0);
            Vector3 point = controller.transform.position + new Vector3(groundSlashPoint.x * direction.x, groundSlashPoint.y, 0);
            var colls = new Collider2D[3];
            Physics2D.OverlapBoxNonAlloc(point, groundSlashSize, 0, colls, executioner.layerTarget);

            DamageInfo damageInfo = new DamageInfo();
            damageInfo.damage = controller.stats.damage;
            damageInfo.idSender = controller.core.combat.getColliderInstanceID;
            damageInfo.hitDirection = (int)direction.x;
            damageInfo.stunTime = 0.3f;
            for (int i = 0; i < colls.Length; i++)
            {
                if (colls[i] != null && !controller.core.combat.IsSelfCollider(colls[i]))
                {
                    colls[i].GetComponent<IDamage>()?.TakeDamage(damageInfo);
                }
            }
            CameraController.Instance.ShakeCamera(.5f, 1f, 10, 90, true);
        }

        void Slash()
        {
            Vector3 direction = new Vector3(controller.core.movement.facingDirection, 0);
            Vector3 point = controller.transform.position + new Vector3(slashPoint.x * direction.x, slashPoint.y, 0);
            var colls = new Collider2D[3];
            Physics2D.OverlapBoxNonAlloc(point, slashSize, 0, colls, executioner.layerTarget);

            DamageInfo damageInfo = new DamageInfo();
            damageInfo.damage = controller.stats.damage / 2;
            damageInfo.idSender = controller.core.combat.getColliderInstanceID;
            damageInfo.hitDirection = (int)direction.x;
            damageInfo.stunTime = 0.3f;
            for (int i = 0; i < colls.Length; i++)
            {
                if (colls[i] != null && !controller.core.combat.IsSelfCollider(colls[i]))
                {
                    colls[i].GetComponent<IDamage>()?.TakeDamage(damageInfo);
                }
            }
        }

        public override void ResetAnimator()
        {
            gameObject.SetActive(true);
            ResumeAnimator();
            animator.Rebind();
        }
        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(transform.position + (Vector3)groundSlashPoint, groundSlashSize);
            Gizmos.DrawWireCube(transform.position + (Vector3)slashPoint, slashSize);
        }
    }
}

