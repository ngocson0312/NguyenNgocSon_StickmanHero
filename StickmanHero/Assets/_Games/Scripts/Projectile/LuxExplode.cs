using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperFight
{
    [RequireComponent(typeof(Collider2D))]
    public class LuxExplode : Projectile
    {
        public Vector3 Size;
        public Vector3 offset;
        public float deactiveTime = 5f;
        public bool canDeactive = false;
        public override void Initialize(Vector2 direction, DamageInfo damageInfo)
        {
            base.Initialize(direction, damageInfo);
            Deactive(deactiveTime);
        }
        protected override void OnCollision(Collider2D other)
        {
            if (!isActive) return;
            if ((layerContact & (1 << other.gameObject.layer)) == 0) return;
            if (other.GetInstanceID().Equals(damageInfo.idSender)) return;
            if (!canDeactive)
            {
                isActive = false;
                //display.SetActive(false);
                impact.SetActive(true);
                GetComponent<Collider2D>().enabled = false;
            }
            var colls = new Collider2D[3];
            Physics2D.OverlapBoxNonAlloc(transform.position + offset, Size, 0, colls, layerContact);

            DamageInfo _DamageInfo = damageInfo;
            _DamageInfo.damage = damageInfo.damage / 2;
            for (int i = 0; i < colls.Length; i++)
            {
                if (colls[i] != null && !colls[i].GetInstanceID().Equals(damageInfo.idSender))
                {
                    colls[i].GetComponent<IDamage>()?.TakeDamage(_DamageInfo);
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireCube(transform.position + offset, Size);
        }
    }
}

