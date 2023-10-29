using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SuperFight
{
    public class Spider : GroundEnemy
    {
        public Enemy miniSpider;
        public ParticleSystem miniSpiderFx;
        // private void Awake()
        // {
        //     TypeEnemy = TYPE_ENEMY.E_SPIDER;
        // }
        public override void Initialize()
        {
            base.Initialize();
        }
        public override void Die(bool deactiveCharacter)
        {
            base.Die(deactiveCharacter);
            healthBar.Deactive();
            animator.DeactiveCharacter();

            SoundManager.Instance.playSoundFx(SoundManager.Instance.effSpiderDie);
            miniSpiderFx.Play();

            Enemy s = PoolingObject.GetObjectFree<Enemy>(miniSpider);
            GameplayCtrl.Instance.objManager.addEnemy(s);
            s.transform.SetParent(transform.parent);
            s.transform.position = transform.position + Vector3.left * 1.2f + Vector3.up * 4;
            s.Initialize();
            s.ResetStatEnemy(new CharacterStats((int)stats.health / 2, stats.damage / 2));

            s = PoolingObject.GetObjectFree<Enemy>(miniSpider);
            GameplayCtrl.Instance.objManager.addEnemy(s);
            s.transform.SetParent(transform.parent);
            s.transform.position = transform.position + Vector3.right * 1.2f + Vector3.up * 4;
            s.Initialize();
            s.ResetStatEnemy(new CharacterStats((int)stats.health / 2, stats.damage / 2));
        }

        public override void DetectPlayer()
        {
            base.DetectPlayer();
            if (delayTimePlaySoundFx <= 0)
            {
                delayTimePlaySoundFx = 5f;
                SoundManager.Instance.playSoundFx(SoundManager.Instance.effSpider);
            }
        }
    }
}