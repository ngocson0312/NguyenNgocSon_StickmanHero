using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperFight
{
    public class Electric : MonoBehaviour
    {
        DamageInfo damage = new DamageInfo();
        public Collider2D myColl;
        public void SetDamage(int _damage = 50)
        {
            damage.damage = _damage;
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (myColl != null && myColl.Equals(col)) return;
            col.GetComponent<IDamage>()?.TakeDamage(damage);
        }
    }
}

