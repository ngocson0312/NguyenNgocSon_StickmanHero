using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperFight
{
    public class ItemLava : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D col)
        {
            DamageInfo damageInfo = new DamageInfo();
            damageInfo.damage = 999999;
            damageInfo.characterType = CharacterType.Boss;
            col.GetComponent<IDamage>()?.TakeDamage(damageInfo);
            col.GetComponent<PlayerManager>()?.PlayerDie(true);
        }
    }
}

