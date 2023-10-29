using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperFight
{
    public class Spike : MonoBehaviour
    {
        public int damage = 30;
        private void OnTriggerEnter2D(Collider2D col)
        {
            DamageInfo damageInfo = new DamageInfo();
            damageInfo.damage = damage;
            damageInfo.characterType = CharacterType.Boss;
            col.GetComponent<IDamage>()?.TakeDamage(damageInfo);
        }
    }
}

