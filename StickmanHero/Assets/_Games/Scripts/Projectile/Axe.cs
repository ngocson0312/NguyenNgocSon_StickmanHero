using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SuperFight
{
    [RequireComponent(typeof(Collider2D))]
    public class Axe : Projectile
    {
        [SerializeField] private float timeDeactive = 1f;
        public override void Initialize(Vector2 direction, DamageInfo damageInfo)
        {
            base.Initialize(direction, damageInfo);
            Deactive(timeDeactive);
        }
        protected override void OnCollision(Collider2D other)
        {
            throw new System.NotImplementedException();
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!isActive) return;
            if ((layerContact & (1 << other.gameObject.layer)) == 0) return;
            if (other.GetInstanceID().Equals(damageInfo.idSender)) return;
            //isActive = false;
            //display.SetActive(false);
            impact.SetActive(true);
            if (!other.GetInstanceID().Equals(damageInfo.idSender))
            {
                other.GetComponent<IDamage>()?.TakeDamage(damageInfo);
            }
        }
    }
}

