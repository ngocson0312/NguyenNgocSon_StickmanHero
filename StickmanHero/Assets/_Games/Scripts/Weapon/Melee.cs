using UnityEngine;
namespace SuperFight
{
    public class Melee : Weapon
    {
        public override void TriggerSkill(Controller controller, int indexSkill)
        {
            var colls = new Collider2D[3];
            Physics2D.OverlapCircleNonAlloc(controller.transform.position, attackRange, colls, layerContact);
            int hitDirection = controller.core.movement.facingDirection;
            DamageInfo damageInfo = new DamageInfo();
            damageInfo.characterType = controller.characterType;
            damageInfo.hitDirection = hitDirection;
            int bonus = damage * controller.stats.damage / 100;
            damageInfo.damage = (int)controller.stats.damage + bonus;
            damageInfo.stunTime = moveSets[indexSkill].stunTime;
            damageInfo.impactSound = moveSets[indexSkill].impactSound;
            damageInfo.idSender = controller.GetInstanceID();
            damageInfo.owner = controller;
            for (int i = 0; i < colls.Length; i++)
            {
                if(colls[i] == null) continue;
                Vector3 controllerPos = controller.transform.position;
                Vector3 targetPos = colls[i].transform.position;
                if (targetPos.y >= controllerPos.y - attackRange && (Vector2.Dot(Vector2.right * hitDirection, (targetPos - controllerPos).normalized) >= -0.2f || Mathf.Abs(targetPos.x - controllerPos.x) < .85f))
                {
                    colls[i].GetComponent<IDamage>()?.TakeDamage(damageInfo);
                }
            }
        }
    }

}
