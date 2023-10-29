using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SuperFight
{
    public class EndermanLaser : State
    {
        Enderman enderman;
        public EndermanLaser(Enderman controller, string stateName) : base(controller, stateName)
        {
            enderman = controller;
        }

        // public State chaseState;
        // public LaserBeam laserBeam;
        bool isPerfomingAction;
        float attackTimer;
        float delayAttack;
        // public override void EnterState(AICharacter aiCharacter)
        // {
        //     Debug.Log("Enter");
        //     base.EnterState(aiCharacter);
        //     delayAttack = 0.5f;
        //     aiCharacter.rigidBody2D.velocity = Vector2.zero;
        //     isPerfomingAction = false;
        //     attackTimer = 0.3f;
        // }
        // public override void UpdatePhysics()
        // {
        // }
        void ActiveLaser()
        {
            if (!controller.isActive) return;
            isPerfomingAction = true;
            if (!controller.isInteracting)
            {
                controller.animatorHandle.PlayAnimation("EndermanLaser", 0.1f, 1, true);
            }
            if (attackTimer > 0)
            {
                attackTimer -= Time.deltaTime;
                if (attackTimer <= 0)
                {
                    DamageInfo damageInfo = new DamageInfo();
                    damageInfo.damage = controller.stats.damage;
                    enderman.laserBeam.Active(controller.core.movement.facingDirection, damageInfo);
                    attackTimer = -1.5f;
                }

            }
            if (attackTimer < 0)
            {
                attackTimer += Time.deltaTime;
                if (attackTimer >= 0)
                {
                    attackTimer = 0.3f;
                    delayAttack = 1.25f;
                    controller.animatorHandle.PlayAnimation("EndermanLaserEnd", 0.1f, 1, true);
                    enderman.laserBeam.Deactive();
                    isPerfomingAction = false;
                }
            }
        }
        // public override void UpdateState()
        // {
        //     if (aiCharacter.isDead)
        //     {
        //         laserBeam.Deactive();
        //         return;
        //     }
        //     if (!isPerfomingAction && !aiCharacter.isInteracting)
        //     {
        //         Bounds playerPostion = PlayerManager.Instance.GetBoundPlayer();
        //         Bounds viewBound = aiCharacter.GetViewBound();
        //         if (!viewBound.Intersects(playerPostion))
        //         {
        //             aiCharacter.SwitchState(chaseState);
        //         }
        //     }

        //     if (!aiCharacter.isInteracting)
        //     {
        //         Vector2 direction = PlayerManager.Instance.transform.position - aiCharacter.transform.position;
        //         direction.y = 0;
        //         Transform targetRotation = aiCharacter.character.transform;
        //         targetRotation.localRotation = Quaternion.LookRotation(direction);
        //         if (direction.x > 0)
        //         {
        //             aiCharacter.SetFacingDirection(1);
        //         }
        //         else if (direction.x < 0)
        //         {
        //             aiCharacter.SetFacingDirection(-1);
        //         }
        //     }
        //     if (delayAttack == 0)
        //     {
        //         ActiveLaser();
        //     }
        //     if (delayAttack > 0)
        //     {
        //         delayAttack -= Time.deltaTime;
        //         if (delayAttack <= 0)
        //         {
        //             delayAttack = 0;
        //         }
        //     }

        // }
        // public override void ExitState()
        // {
        //     aiCharacter.character.PlayAnimation("EndermanLaserEnd", 1, true);
        //     laserBeam.Deactive();
        //     base.ExitState();
        // }
        public override void EnterState()
        {
            delayAttack = 0.5f;
            controller.core.movement.SetVelocityX(0);
            isPerfomingAction = false;
            attackTimer = 0.3f;
        }

        public override void ExitState()
        {
            controller.animatorHandle.PlayAnimation("EndermanLaserEnd", 0.1f, 1, true);
            enderman.laserBeam.Deactive();
        }

        public override void UpdateLogic()
        {

            Controller target = enderman.GetTargetInView();
            if (target == null)
            {
                enderman.SwitchState(enderman.chaseState);
                return;
            }
            if (!controller.isInteracting)
            {
                Vector2 direction = target.transform.position - controller.transform.position;
                direction.y = 0;
                Transform targetRotation = controller.animatorHandle.transform;
                targetRotation.localRotation = Quaternion.LookRotation(direction);
                if (direction.x > 0 && controller.core.movement.facingDirection < 0)
                {
                    controller.core.movement.Flip();
                }
                else if (direction.x < 0 && controller.core.movement.facingDirection > 0)
                {
                    controller.core.movement.Flip();
                }
            }
            if (delayAttack == 0)
            {
                ActiveLaser();
            }
            if (delayAttack > 0)
            {
                delayAttack -= Time.deltaTime;
                if (delayAttack <= 0)
                {
                    delayAttack = 0;
                }
            }
        }

        public override void UpdatePhysic()
        {

        }
    }
}

