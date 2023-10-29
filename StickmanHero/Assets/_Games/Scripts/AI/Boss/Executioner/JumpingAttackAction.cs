using DG.Tweening;
using UnityEngine;
namespace SuperFight
{
    public class JumpingAttackAction : AIAction
    {
        Transform target;
        Executioner executioner;
        float timer;
        int state;
        float originY;
        public JumpingAttackAction(Executioner controller, Transform target, float timeDelayAction) : base(controller)
        {
            this.target = target;
            timer = timeDelayAction;
            executioner = controller;
            originY = controller.transform.position.y;
        }

        public override void ExitAction()
        {
            executioner.FinishAction();
        }

        public override void OnActionEnter()
        {
            state = 0;
        }
        void FallDown()
        {
            int currentDirection = 0;
            Vector3 direction = (target.position - executioner.transform.position).normalized;
            Vector3 targetPostion = target.position;
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
            if (targetPostion.x <= executioner.xMin)
            {
                targetPostion.x = executioner.xMin;
            }
            if (targetPostion.x >= executioner.xMax)
            {
                targetPostion.x = executioner.xMax;
            }
            if (currentDirection != executioner.core.movement.facingDirection)
            {
                executioner.core.movement.Flip();
            }

            controller.transform.DOMove(new Vector3(targetPostion.x, originY, 0), 0.25f).SetEase(Ease.InQuart).OnComplete(() =>
            {
                controller.animatorHandle.PlayAnimation("AttackLanding", 0.1f, 1, true);
                executioner.core.Resume();
                state = 2;
                timer = 0.5f;
            });
        }

        void StartAction()
        {
            executioner.core.Pause();
            controller.animatorHandle.PlayAnimation("JumpingAttack", 0.1f, 1, true);
            Vector3 newPos = controller.transform.position + new Vector3(3 * controller.core.movement.facingDirection, 5);
            controller.transform.DOMove(newPos, 0.75f).SetEase(Ease.OutQuart).SetDelay(0.5f).OnComplete(FallDown);
        }

        public override void UpdateLogic(float deltaTime)
        {
            timer -= deltaTime;
            if (state == 0)
            {
                if (timer <= 0)
                {
                    StartAction();
                    state = 1;
                }
            }
            if (state == 2)
            {
                if (timer <= 0)
                {
                    ExitAction();
                    state = 3;
                }
            }
        }

        public override void UpdatePhysic(float fixedDeltaTime)
        {
        }
    }

}
