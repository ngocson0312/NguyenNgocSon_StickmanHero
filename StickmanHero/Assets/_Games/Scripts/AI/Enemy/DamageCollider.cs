using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SuperFight
{
    public class DamageCollider : MonoBehaviour
    {
        public SpiderHanging spiderHanging;
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.GetComponent<IDamage>() != null && !spiderHanging.core.combat.IsSelfCollider(other))
            {
                spiderHanging.OnTouchPlayer(other.GetComponent<IDamage>());
            }
        }
    }
}

