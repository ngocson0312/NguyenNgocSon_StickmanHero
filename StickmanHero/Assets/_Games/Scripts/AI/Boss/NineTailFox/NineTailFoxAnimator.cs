using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SuperFight
{
    public class NineTailFoxAnimator : AnimatorHandle
    {
        [Header("Combo")]
        public Transform rightFeet;
        NineTailFox nineTailFox;
        [Header("Laser")]
        public Transform mouth;
        public Beam beam;
        [Header("FireOrb")]
        public FireOrb foxFireOrb;
        public override void Initialize(Controller controller)
        {
            base.Initialize(controller);
            nineTailFox = (NineTailFox)controller;
        }
        public override void ResetAnimator()
        {

        }
        void SendDamage()
        {
            Collider2D[] colls = new Collider2D[3];
            Physics2D.OverlapCircleNonAlloc(rightFeet.position, 1f, colls, nineTailFox.layerTarget);
            for (int i = 0; i < colls.Length; i++)
            {
                if (colls[i] != null && colls[i].GetInstanceID() != controller.core.combat.getColliderInstanceID)
                {
                    DamageInfo damageInfo = new DamageInfo();
                    damageInfo.damage = nineTailFox.stats.damage;
                    colls[i].GetComponent<IDamage>()?.TakeDamage(damageInfo);
                }
            }
        }
        void ActiveLaser()
        {
            DamageInfo damageInfo = new DamageInfo();
            damageInfo.damage = nineTailFox.stats.damage;
            beam.transform.position = mouth.position;
            beam.ActiveBeam(Vector3.right * controller.core.movement.facingDirection, damageInfo);
        }
        void DeactiveLaser()
        {
            beam.Deactive();
        }
        void PrepareLaser()
        {
            beam.transform.position = mouth.position;
            beam.Prepare();
        }
        void ActiveFireOrb()
        {
            foxFireOrb.transform.position = mouth.position;
            DamageInfo damageInfo = new DamageInfo();
            damageInfo.damage = controller.stats.damage / 5;
            damageInfo.idSender = controller.core.combat.getColliderInstanceID;
            damageInfo.owner = controller;
            foxFireOrb.Active(controller.core.movement.facingDirection, damageInfo);
        }
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(rightFeet.position, 1f);
        }
    }
}

