using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SuperFight
{
    public class PigletAttackState : State
    {
        Piglet piglet;
        int moveSet;
        float attackTimer;
        float recoverTimer;
        int attackState;
        int currentDirection;
        List<Controller> listDamaged;
        public PigletAttackState(Piglet controller, string stateName) : base(controller, stateName)
        {
            piglet = controller;
            listDamaged = new List<Controller>();
        }

        public override void EnterState()
        {
            ResetAttack();
        }

        public override void ExitState()
        {
            piglet.animatorHandle.PlayAnimation("PigSlideEnd", 0.1f, 1, false);
        }

        public override void UpdateLogic()
        {

        }

        void ResetAttack()
        {
            attackTimer = 0;
            attackState = 0;
            currentDirection = piglet.core.movement.facingDirection;
            piglet.core.movement.SetVelocityZero();
            listDamaged.Clear();
            moveSet = Random.Range(1, 3);
            recoverTimer = 0.5f;
        }

        public override void UpdatePhysic()
        {
            Controller target = piglet.GetTargetInView();
            if (target == null)
            {
                piglet.SwitchState(piglet.patrol);
                return;
            }
            recoverTimer -= Time.fixedDeltaTime;
            if (recoverTimer > 0) return;

            if (moveSet == 1)
            {
                HandleRollingAttack(target);
            }
            else
            {
                HandleSlideAttack(target);
            }
        }
        void HandleRollingAttack(Controller target)
        {
            attackTimer += Time.fixedDeltaTime;
            Vector2 dir = target.transform.position - piglet.transform.position;
            if (dir.x < 0)
            {
                dir.x = -1;
            }
            else if (dir.x > 0)
            {
                dir.x = 1;
            }
            if (!controller.isInteracting)
            {
                if (currentDirection != piglet.core.movement.facingDirection)
                {
                    piglet.core.movement.Flip();
                }
            }

            if (attackTimer >= 0.2f && attackState == 0)
            {
                piglet.animatorHandle.PlayAnimation("PigRoll", 0.1f, 1, true);
                attackState = 1;
            }

            if (attackTimer >= 0.5f && attackTimer < 1f)
            {
                if (attackState == 1)
                {
                    controller.core.movement.SetVelocityY(10);
                    attackState = 2;
                }
                OpenCollider(target);
                controller.core.movement.SetVelocityX(10 * controller.core.movement.facingDirection);
            }
            else if (attackTimer > 1)
            {
                ResetAttack();
            }
        }
        void HandleSlideAttack(Controller target)
        {
            attackTimer += Time.fixedDeltaTime;
            Vector2 dir = target.transform.position - piglet.transform.position;
            if (dir.x < 0)
            {
                dir.x = -1;
            }
            else if (dir.x > 0)
            {
                dir.x = 1;
            }
            if (!controller.isInteracting)
            {
                if (currentDirection != piglet.core.movement.facingDirection)
                {
                    piglet.core.movement.Flip();
                }
            }

            if (attackTimer >= 0.3f && attackState == 0)
            {
                piglet.animatorHandle.PlayAnimation("PigSlide", 0.1f, 1, true);
                attackState = 1;
            }
            if (attackTimer >= 0.5f && attackTimer < 1f)
            {
                OpenCollider(target);
                controller.core.movement.SetVelocityX(10 * controller.core.movement.facingDirection);
            }
            else if (attackTimer > 1)
            {
                piglet.animatorHandle.PlayAnimation("PigSlideEnd", 0.1f, 1, false);
                ResetAttack();
            }
        }

        void OpenCollider(Controller target)
        {
            DamageInfo damageInfo = new DamageInfo();
            damageInfo.damage = piglet.stats.damage;
            damageInfo.hitDirection = controller.core.movement.facingDirection;
            damageInfo.characterType = CharacterType.Mob;
            damageInfo.owner = controller;
            if (Vector2.Distance(target.transform.position, piglet.transform.position) <= 1 && !listDamaged.Contains(target))
            {
                target.core.combat.GetComponent<IDamage>().TakeDamage(damageInfo);
                listDamaged.Add(target);
            }
        }
    }
}

