using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SuperFight
{
    public class Kaisa : Character
    {
        public Transform shootPosition;
        public ParticleSystem muzzle;
        public FireBall fireBallKaisa;
        public override void Initialize(Controller controller)
        {
            base.Initialize(controller);
        }
        public void Shoot()
        {
            muzzle.Play();
            FireBall fb = PathologicalGames.PoolManager.Pools["Projectile"].Spawn(fireBallKaisa.transform).GetComponent<FireBall>();
            fb.transform.position = shootPosition.position;
            DamageInfo damageInfo = new DamageInfo();
            damageInfo.characterType = controller.characterType;
            int damageBonus = controller.stats.damage * currentWeapon.damage / 100;
            damageInfo.damage = controller.stats.damage + damageBonus;
            damageInfo.idSender = controller.core.combat.getColliderInstanceID;
            if (PlayerManager.Instance.transform.localScale.x == 1)
            {
                fb.Initialize(Vector2.right, damageInfo);
            }
            else
            {
                fb.Initialize(-Vector2.right, damageInfo);
            }
        }

        public void Missle()
        {
            muzzle.Play();
            SpawnMissLe(Vector2.up * 0.1f);
            SpawnMissLe(Vector2.up * 0.2f);
            SpawnMissLe(Vector2.zero);
        }

        public void SpawnMissLe(Vector2 dir)
        {
            FireBall fb = PathologicalGames.PoolManager.Pools["Projectile"].Spawn(fireBallKaisa.transform).GetComponent<FireBall>();
            fb.transform.position = shootPosition.position;
            DamageInfo damageInfo = new DamageInfo();
            damageInfo.idSender = controller.core.combat.getColliderInstanceID;
            int damageBonus = controller.stats.damage * currentWeapon.damage / 100;
            damageInfo.damage = controller.stats.damage + damageBonus;
            damageInfo.characterType = controller.characterType;
            if (PlayerManager.Instance.transform.localScale.x == 1)
            {
                fb.Initialize(Vector2.right + dir, damageInfo);
            }
            else
            {
                fb.Initialize(-Vector2.right + dir, damageInfo);
            }
        }
    }
}

