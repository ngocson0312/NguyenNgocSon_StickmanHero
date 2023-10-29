using System.Collections.Generic;
using PathologicalGames;
using UnityEngine;
namespace SuperFight
{
    public class Bow : Weapon
    {
        public Projectile arrow;
        public Transform posSpawn;
        public bool isTargetBow;
        public override void TriggerSkill(Controller controller, int indexSkill)
        {
            Projectile p = PoolManager.Pools["Projectile"].Spawn(arrow.transform).GetComponent<Projectile>();
            p.transform.position = posSpawn.position;
            int hitDirection = controller.core.movement.facingDirection;
            DamageInfo damageInfo = new DamageInfo();
            damageInfo.characterType = controller.characterType;
            damageInfo.hitDirection = hitDirection;
            int damageAdd = (int)controller.stats.damage * damage / 100;
            damageInfo.damage = (int)controller.stats.damage + damageAdd;
            damageInfo.stunTime = moveSets[indexSkill].stunTime;
            damageInfo.impactSound = moveSets[indexSkill].impactSound;
            damageInfo.idSender = controller.core.combat.GetComponent<Collider2D>().GetInstanceID();
            damageInfo.owner = controller;
            Vector2 direction = new Vector2(hitDirection, 0);
            if (isTargetBow)
            {
                var colls = new Collider2D[3];
                Physics2D.OverlapCircleNonAlloc(controller.transform.position, attackRange, colls, layerContact);
                List<Collider2D> coll = new List<Collider2D>();
                for (int i = 0; i < colls.Length; i++)
                {
                    if(colls[i] != null && colls[i].GetInstanceID() != damageInfo.idSender)
                    {
                        coll.Add(colls[i]);
                    }
                }
                if (coll.Count > 0)
                {
                    if (Vector2.Dot(Vector2.right * controller.core.movement.facingDirection, (coll[0].transform.position - controller.transform.position).normalized) > 0)
                    {
                        direction = coll[0].transform.position - controller.transform.position;
                        direction.Normalize();
                    }
                }
            }
            p.Initialize(direction, damageInfo);

        }

    }

}
