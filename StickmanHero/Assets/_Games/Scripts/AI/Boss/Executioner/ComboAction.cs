using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SuperFight
{
    public class ComboAction : AIAction
    {
        Executioner executioner;
        private float timer;
        private int state;
        Transform target;
        public ComboAction(Executioner controller, Transform target, float timeDelay) : base(controller)
        {
            this.target = target;
            executioner = controller;
            timer = timeDelay;
        }

        public override void ExitAction()
        {
            executioner.FinishAction();
        }

        public override void OnActionEnter()
        {
            state = 0;
        }

        public override void UpdateLogic(float deltaTime)
        {
            timer -= deltaTime;
            if (state == 0)
            {
                int currentDirection = 0;
                Vector3 direction = (target.position - executioner.transform.position).normalized;
                Vector3 targetPostion = target.position;
                if (direction.x < 0)
                {
                    currentDirection = -1;
                    targetPostion.x += 2;
                }
                else if (direction.x > 0)
                {
                    currentDirection = 1;
                    targetPostion.x -= 2;
                }
                else
                {
                    if (executioner.core.movement.facingDirection < 0)
                    {
                        targetPostion.x += 2;
                    }
                    else
                    {
                        targetPostion.x -= 2;
                    }
                }
                if (currentDirection != executioner.core.movement.facingDirection)
                {
                    executioner.core.movement.Flip();
                }
                if (timer <= 0)
                {
                    controller.animatorHandle.PlayAnimation("Combo", 0.1f, 1, true);
                    state = 1;
                    timer = 3f;
                }
            }
            if (state == 1)
            {
                if (timer <= 0)
                {
                    ExitAction();
                }
            }
        }

        public override void UpdatePhysic(float fixedDeltaTime)
        {

        }
    }
}

