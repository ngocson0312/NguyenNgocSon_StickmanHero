using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
namespace SuperFight
{
    [RequireComponent(typeof(Collider2D))]
    public class Tornado : Projectile
    {
        public override void Initialize(Vector2 direction, DamageInfo damageInfo)
        {
            base.Initialize(direction, damageInfo);
            gameObject.transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.85f).SetEase(Ease.InOutQuad);
            Deactive(1f);
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

