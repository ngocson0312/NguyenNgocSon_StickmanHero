using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SuperFight
{

    public class BombPack : Weapon
    {
        public Bomb bombPrefab;
        public override void TriggerSkill(Controller controller, int indexSkill)
        {
            Bomb bomb = Instantiate(bombPrefab);
            bomb.transform.position = transform.position;
            DamageInfo damageInfo = new DamageInfo();
            damageInfo.damage = controller.stats.damage;
            damageInfo.characterType = controller.characterType;
            damageInfo.idSender = controller.core.combat.getColliderInstanceID;
            damageInfo.owner = controller;
            bomb.ActiveBomb(PlayerManager.Instance.transform.position, damageInfo);
        }
    }
}
