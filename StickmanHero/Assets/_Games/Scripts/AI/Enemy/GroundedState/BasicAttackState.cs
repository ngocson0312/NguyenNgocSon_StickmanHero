using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SuperFight
{
    public class BasicAttackState : State
    {
        GroundEnemy enemy;
        float attackTimer = 0;
        int currentDirection;
        public BasicAttackState(GroundEnemy controller, string stateName) : base(controller, stateName)
        {
            this.enemy = controller;
        }
        public override void EnterState()
        {
            
            currentDirection = enemy.core.movement.facingDirection;
            attackTimer = Random.Range(0, 0.3f);
        }
        public override void ExitState()
        {

        }
        public override void UpdateLogic()
        {
            Controller target = enemy.GetTargetInView();
            enemy.core.movement.SetVelocityX(0);
            if (target == null)
            {
                enemy.SwitchState(enemy.patrol);
                return;
            }
            if (Vector2.SqrMagnitude(target.transform.position - controller.transform.position) > enemy.attackRange)
            {
                enemy.SwitchState(enemy.chaseState);
                return;
            }

            if (!controller.isInteracting)
            {
                Vector2 dir = target.transform.position - controller.transform.position;
                if (dir.x < 0)
                {
                    currentDirection = -1;
                }
                else if (dir.x > 0)
                {
                    currentDirection = 1;
                }
                if (!controller.isInteracting)
                {
                    if (currentDirection != controller.core.movement.facingDirection)
                    {
                        controller.core.movement.Flip();
                    }
                }
            }

            if (attackTimer >= 0)
            {
                attackTimer -= Time.deltaTime;
                if (attackTimer <= 0)
                {
                    enemy.animator.HandleCombo();
                    attackTimer = 1 / enemy.attackSpeed;
                }
            }
        }

        public override void UpdatePhysic()
        {

        }
    }
}

