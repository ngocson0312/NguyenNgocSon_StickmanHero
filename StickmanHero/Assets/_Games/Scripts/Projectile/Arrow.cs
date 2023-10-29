using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SuperFight
{
    public class Arrow : Projectile
    {
        public override void Initialize(Vector2 direction, DamageInfo damageInfo)
        {
            base.Initialize(direction, damageInfo);
            display.transform.rotation = Quaternion.LookRotation(direction);
            Deactive(5);
        }
        protected override void OnCollision(Collider2D other)
        {
            if (!isActive) return;
            if ((layerContact & (1 << other.gameObject.layer)) == 0) return;
            if (other.GetInstanceID().Equals(damageInfo.idSender)) return;
            isActive = false;
            if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                GetComponent<Collider2D>().enabled = false;
            }
            else
            {
                GetComponent<Collider2D>().enabled = false;
                display.gameObject.SetActive(false);
                other.GetComponent<IDamage>()?.TakeDamage(damageInfo);
            }
        }
    }
}

