using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SuperFight
{
    public class MiniSpider : GroundEnemy
    {
        public override void Die(bool deactiveCharacter)
        {
            SoundManager.Instance.playSoundFx(SoundManager.Instance.effSpiderDie);
            healthBar.Deactive();
            animator.DeactiveCharacter();
            GetComponent<Collider2D>().enabled = false;
            GameplayCtrl.Instance.freeEnemyBeKill(this, 2f);
            //GameplayCtrl.Instance.createCoin(transform.position + Vector3.up);
        }
    }

}
