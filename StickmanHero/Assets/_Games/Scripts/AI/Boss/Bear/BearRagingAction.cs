using UnityEngine;
using DG.Tweening;
namespace SuperFight
{
    public class BearRagingAction : AIAction
    {
        private Bear bear;
        private float actionTimer;
        private int stateAction;
        Transform target;
        public BearRagingAction(Bear controller, Transform target) : base(controller)
        {
            bear = controller;
            this.target = target;
        }

        public override void ExitAction()
        {
            bear.FinishAction();
        }

        public override void OnActionEnter()
        {
            stateAction = 0;
            actionTimer = 0;
            bear.animatorHandle.PlayAnimation("BearRagingStart", 0f, 1, true);
        }

        public override void UpdateLogic(float deltaTime)
        {
            // actionTimer += deltaTime;
            // if (stateAction == 0)
            // {
            //     if (actionTimer >= 2.35f)
            //     {
            //         stateAction = 1;
            //         bear.core.Pause();
            //         Jump();
            //     }
            // }
            // if (stateAction == 2)
            // {
            //     if (actionTimer >= 1f)
            //     {
            //         ExitAction();
            //     }
            // }
        }

        void Jump()
        {
            Vector3 position = target.position;
            position.y = -7.5f;
            bear.transform.DOJump(position, 5f, 1, 0.5f).SetEase(Ease.Linear).OnComplete(FallDown);
        }
        void FallDown()
        {
            bear.core.Resume();
            stateAction = 2;
            actionTimer = 0;
            bear.animatorHandle.PlayAnimation("BearRagingEnd", 0f, 1, true);
        }
        public override void UpdatePhysic(float fixedDeltaTime)
        {
        }
    }
}

