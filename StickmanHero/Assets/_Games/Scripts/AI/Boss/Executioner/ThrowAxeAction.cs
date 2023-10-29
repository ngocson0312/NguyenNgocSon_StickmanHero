using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SuperFight
{
    public class ThrowAxeAction : AIAction
    {
        Executioner executioner;
        private float actionTimer;
        private int actionState;
        Transform target;
        public ThrowAxeAction(Executioner controller, Transform target,float timeDelayAction) : base(controller)
        {
            this.target = target;
            executioner = controller;
        }

        public override void ExitAction()
        {
            executioner.FinishAction();
        }

        public override void OnActionEnter()
        {
            Vector3 direction = (target.position - executioner.transform.position).normalized;
            Vector3 targetPostion = target.position;
            int currentDirection = executioner.core.movement.facingDirection;
            if (direction.x < 0)
            {
                currentDirection = -1;
                targetPostion.x += 3;
            }
            else if (direction.x > 0)
            {
                currentDirection = 1;
                targetPostion.x -= 3;
            }
            else
            {
                if (executioner.core.movement.facingDirection < 0)
                {
                    targetPostion.x += 3;
                }
                else
                {
                    targetPostion.x -= 3;
                }
            }

            if (currentDirection != executioner.core.movement.facingDirection)
            {
                executioner.core.movement.Flip();
            }
            executioner.animatorHandle.PlayAnimation("ThrowAxes", 0.1f, 1, true);
            actionTimer = 1.3f;
        }

        public override void UpdateLogic(float deltaTime)
        {
            actionTimer -= Time.deltaTime;
            if (actionTimer <= 0)
            {
                ExitAction();
            }
        }

        public override void UpdatePhysic(float fixedDeltaTime)
        {

        }
    }
}

