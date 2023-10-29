using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SuperFight
{
    public class Zombie : GroundEnemy
    {
        public override void Initialize()
        {
            base.Initialize();
        }
        public override void Die(bool deactiveCharacter)
        {
            base.Die(deactiveCharacter);
            SoundManager.Instance.playSoundFx(SoundManager.Instance.effZombieDie);
        }

        public override void DetectPlayer()
        {
            base.DetectPlayer();
            if (delayTimePlaySoundFx <= 0)
            {
                delayTimePlaySoundFx = 5f;
                SoundManager.Instance.playRandFx(new AudioClip[] { SoundManager.Instance.effZombie1, SoundManager.Instance.effZombie2, SoundManager.Instance.effZombie3 });
            }
        }
    }
}