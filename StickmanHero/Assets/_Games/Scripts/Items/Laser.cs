using SuperFight;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public float damageTime = 0.5f;
    public int damageValue = 50;
    float time = 0;

    DamageInfo damage = new DamageInfo();
    public void SetDamage(int _damage)
    {
        damage.stunTime = 0.05f;
        damage.damage = _damage;
    }
    private void OnTriggerStay2D(Collider2D col)
    {
        if (time <= 0)
        {
            time = damageTime;
            col.GetComponent<IDamage>()?.TakeDamage(damage);
        }
        else
        {
            time -= Time.deltaTime;
        }

    }
}
