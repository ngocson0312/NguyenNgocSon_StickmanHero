using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SuperFight
{
    public class Piglet : GroundEnemy
    {
        // private void Awake()
        // {
        //     TypeEnemy = TYPE_ENEMY.E_PIG;
        // }
        public bool superPiglet;
        public override void Initialize()
        {
            base.Initialize();
            if (superPiglet)
            {
                chaseState = new PigletAttackState(this, "attack");
            }
        }
        public override void Die(bool deactiveCharacter)
        {
            base.Die(deactiveCharacter);
            SoundManager.Instance.playSoundFx(SoundManager.Instance.effPigDie);
        }
        public override void DetectPlayer()
        {
            base.DetectPlayer();
            if (delayTimePlaySoundFx <= 0)
            {
                delayTimePlaySoundFx = 5f;
                SoundManager.Instance.playSoundFx(SoundManager.Instance.effPig);
            }
        }

    }
}