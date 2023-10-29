using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SuperFight
{
    public class Creeper : GroundEnemy
    {
        // private void Awake()
        // {
        //     TypeEnemy = TYPE_ENEMY.E_CREEPER;
        // }
        public Renderer rendererCharacter;
        public override void Initialize()
        {
            base.Initialize();
            attackState = new CreeperExplodeState(this, "explode");
        }
        public override void ResetStatEnemy(CharacterStats overrideStats)
        {
            base.ResetStatEnemy(overrideStats);
            rendererCharacter.material.color = Color.white;
        }
        public override void Die(bool deactiveCharacter)
        {
            if (!isActive) return;
            base.Die(deactiveCharacter);
            healthBar.Deactive();
            GetComponent<Collider2D>().enabled = false;
      
        }

        public override void DetectPlayer()
        {
            base.DetectPlayer();
            if (delayTimePlaySoundFx <= 0)
            {
                delayTimePlaySoundFx = 5f;
                SoundManager.Instance.playSoundFx(SoundManager.Instance.effCreeper);
            }
        }
    }
}

