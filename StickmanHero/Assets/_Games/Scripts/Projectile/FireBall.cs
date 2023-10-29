using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
namespace SuperFight
{
    //test for the future change
    [RequireComponent(typeof(Collider2D))]
    public class FireBall : Projectile
    {
        public float radius = 1f;
        public Vector3 offset;
        public float deactiveTime = 5;
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
                display.SetActive(false);
                impact.SetActive(true);
                GetComponent<Collider2D>().enabled = false;
            }
            var colls = new Collider2D[3];
            Physics2D.OverlapCircleNonAlloc(transform.position + offset, radius, colls, layerContact);
            for (int i = 0; i < colls.Length; i++)
            {
                if(colls[i] == null) continue;
                if (!colls[i].GetInstanceID().Equals(damageInfo.idSender))
                {
                    colls[i].GetComponent<IDamage>()?.TakeDamage(damageInfo);
                }
            }
        }
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position + offset, radius);
        }
    }
}

