using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperFight
{
    public class Enderman : GroundEnemy
    {
        // private void Awake()
        // {
        //     TypeEnemy = TYPE_ENEMY.E_ENDERMAN;
        // }
        public bool isLaserMan;
        public LaserBeam laserBeam;
        public override void Initialize()
        {
            base.Initialize();
            if(isLaserMan)
            {
                attackState = new EndermanLaser(this,"Laser");
            }
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
                SoundManager.Instance.playSoundFx(SoundManager.Instance.effEnderman);
            }
        }

    }
}