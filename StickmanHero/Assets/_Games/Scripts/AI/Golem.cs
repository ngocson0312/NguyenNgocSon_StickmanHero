using SuperFight;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem : GroundEnemy
{
    [SerializeField] GameObject warning;
    // private void Awake()
    // {
    //     TypeEnemy = TYPE_ENEMY.E_GOLEM;
    // }

    public override void Initialize()
    {
        base.Initialize();
        DisableWarning();
    }
    public override void Die(bool deactiveCharacter)
    {
        base.Die(deactiveCharacter);
        SoundManager.Instance.playSoundFx(SoundManager.Instance.effZombieDie);
        warning.SetActive(false);
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

    public void ActiveWarning()
    {
        warning.SetActive(true);
    }

    public void DisableWarning()
    {
        warning.SetActive(false);
        CameraController.Instance.ShakeCamera(.35f, 0.7f, 10, 90, true);
    }

}
