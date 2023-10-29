using DG.Tweening;
using mygame.sdk;
using UnityEngine;
namespace SuperFight
{
    public class CreeperExplodeState : State
    {
        Creeper creeper;
        float explodeTimer;
        bool animExplosion;
        public CreeperExplodeState(Creeper controller, string stateName) : base(controller, stateName)
        {
            creeper = controller;
        }

        public override void EnterState()
        {
            controller.core.movement.SetVelocityX(0);
            //AudioManager.Instance.PlayOneShot("countdown", 1, controller.transform);
            explodeTimer = 1.5f;
        }

        public override void ExitState()
        {

        }

        public override void UpdateLogic()
        {
            if(!creeper.isActive)return;
            if (explodeTimer > 0)
            {
                if (!animExplosion)
                {
                    animExplosion = true;
                    creeper.animatorHandle.PlayAnimation("Attack", 0.1f, 1, false);
                    creeper.rendererCharacter.material.DOColor(Color.red, 1.5f);
                }

                explodeTimer -= Time.deltaTime;
                if (explodeTimer <= 0)
                {
                    animExplosion = false;
                    SoundManager.Instance.playSoundFx(SoundManager.Instance.effExplode);
                    GameplayCtrl.Instance.setFxExplode(creeper.transform);
                    GameHelper.Instance.Vibrate(Type_vibreate.Vib_Light);
                    DamageInfo damageInfo = new DamageInfo();
                    damageInfo.characterType = CharacterType.Mob;
                    damageInfo.damage = controller.stats.damage;
                    Controller target = creeper.GetTargetInView();
                    if (target != null && Vector2.SqrMagnitude(target.transform.position - creeper.transform.position) <= 4)
                    {
                        target.core.combat.GetComponent<IDamage>().TakeDamage(damageInfo);
                    }
                    creeper.Die(true);
                    CameraController.Instance.ShakeCamera(.5f, 1f, 10, 90, true);
                }
            }

        }

        public override void UpdatePhysic()
        {
        }
    }

}
